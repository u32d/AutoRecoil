using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using autorecoil.Sources.Interop;

namespace autorecoil.Sources.Libraries
{
    /// <summary>
    /// 输入模拟库：对键盘 / 鼠标相关的 Win32 API 做高层封装，供上层直接调用。
    /// </summary>
    internal static class InputLibrary
    {
        /// <summary>
        /// 全局相对移动鼠标（SendInput，游戏最易接收）。
        /// </summary>
        public static void MoveMouseRelative(int dx, int dy)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = NativeConstants.INPUT_MOUSE;
            inputs[0].u.mi.dx = dx;
            inputs[0].u.mi.dy = dy;
            inputs[0].u.mi.dwFlags = NativeConstants.MOUSEEVENTF_MOVE;
            POINT before;
            NativeMethods.GetCursorPos(out before);
            uint sent = NativeMethods.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
            POINT after;
            NativeMethods.GetCursorPos(out after);
            Diag("dx={0} dy={1} sent={2} cursor {3},{4}->{5},{6}", dx, dy, sent, before.X, before.Y, after.X, after.Y);
        }

        // TEMP-DIAG: lightweight file logger to trace the recoil chain (remove when done)
        internal static readonly object DiagLock = new object();
        internal static void Diag(string format, params object[] args)
        {
            try {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recoil-diag.log");
                lock (DiagLock) {
                    System.IO.File.AppendAllText(path,
                        DateTime.Now.ToString("HH:mm:ss.fff ") + string.Format(format, args) + Environment.NewLine);
                }
            } catch { }
        }

        /// <summary>
        /// 仅发送一次鼠标左/右键的按下(down=true)或抬起(down=false)，用于 SendInput 模式的按住 / 连射管理。
        /// </summary>
        public static void SetMouseButton(int vKey, bool down)
        {
            bool right = vKey == NativeConstants.VK_RBUTTON;
            uint flag = down
                ? (right ? NativeConstants.MOUSEEVENTF_RIGHTDOWN : NativeConstants.MOUSEEVENTF_LEFTDOWN)
                : (right ? NativeConstants.MOUSEEVENTF_RIGHTUP : NativeConstants.MOUSEEVENTF_LEFTUP);

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = NativeConstants.INPUT_MOUSE;
            inputs[0].u.mi.dwFlags = flag;
            NativeMethods.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 通过 SendInput 发送一次键盘按键（down 后 up），全局生效。
        /// </summary>
        public static void KeyClick(int vKey)
        {
            INPUT[] inputs = new INPUT[2];
            inputs[0].type = NativeConstants.INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = (ushort)vKey;
            inputs[0].u.ki.dwFlags = 0; // key down
            inputs[1].type = NativeConstants.INPUT_KEYBOARD;
            inputs[1].u.ki.wVk = (ushort)vKey;
            inputs[1].u.ki.dwFlags = NativeConstants.KEYEVENTF_KEYUP; // key up
            NativeMethods.SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 通过 SendMessage/PostMessage 向指定窗口发送一次鼠标左/右键点击（down 后 up）。
        /// 注意：多数游戏读 raw input 时无效，主要用于普通窗口程序；异常由调用方捕获处理。
        /// </summary>
        public static void ClickMouseToWindow(IntPtr hWnd, int vKey, bool post)
        {
            bool right = vKey == NativeConstants.VK_RBUTTON;
            uint downMsg = right ? NativeConstants.WM_RBUTTONDOWN : NativeConstants.WM_LBUTTONDOWN;
            uint upMsg = right ? NativeConstants.WM_RBUTTONUP : NativeConstants.WM_LBUTTONUP;
            IntPtr wDown = right ? new IntPtr(NativeConstants.MK_RBUTTON) : new IntPtr(NativeConstants.MK_LBUTTON);

            POINT p;
            NativeMethods.GetCursorPos(out p);
            IntPtr lParam = (IntPtr)((p.X & 0xFFFF) | ((p.Y & 0xFFFF) << 16));

            if (post)
            {
                NativeMethods.PostMessage(hWnd, downMsg, wDown, lParam);
                NativeMethods.PostMessage(hWnd, upMsg, IntPtr.Zero, lParam);
            }
            else
            {
                NativeMethods.SendMessage(hWnd, downMsg, wDown, lParam);
                NativeMethods.SendMessage(hWnd, upMsg, IntPtr.Zero, lParam);
            }
        }

        /// <summary>
        /// 通过 SendInput 逐字符、以"虚拟键（VK）"方式输入文本（k-i-l-l…），全局生效。
        /// 与之前能出 'f' 的 KeyClick 同一条路：用 VK 注入等价于真实按键，文本框与 raw input 游戏都能收到；
        /// 而 Unicode 字符事件会被多数 raw input 程序忽略，故此处改用 VK。
        /// </summary>
        public static void TypeText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var inputs = new List<INPUT>();
            foreach (char c in text)
            {
                ushort vkScan = (ushort)NativeMethods.VkKeyScan(c);
                if (vkScan == 0xFFFF) continue; // 该字符无法映射为虚拟键，跳过
                byte vk = (byte)(vkScan & 0xFF);
                bool needShift = (vkScan & 0x0100) != 0; // 高字节 bit0 = 需要 Shift

                if (needShift) inputs.Add(MakeKey(NativeConstants.VK_SHIFT, 0));
                inputs.Add(MakeKey(vk, 0));                                // 按下
                inputs.Add(MakeKey(vk, NativeConstants.KEYEVENTF_KEYUP));  // 抬起
                if (needShift) inputs.Add(MakeKey(NativeConstants.VK_SHIFT, NativeConstants.KEYEVENTF_KEYUP));
            }
            NativeMethods.SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
        }

        // 构造一个键盘 INPUT（wVk 方式，wScan 留 0）
        private static INPUT MakeKey(ushort vk, uint flags)
        {
            INPUT input = new INPUT();
            input.type = NativeConstants.INPUT_KEYBOARD;
            input.u.ki.wVk = vk;
            input.u.ki.wScan = 0;
            input.u.ki.dwFlags = flags;
            return input;
        }

        /// <summary>
        /// 查询指定虚拟键当前是否按住（高位为 1 表示按下）。
        /// </summary>
        public static bool IsKeyDown(int vKey)
        {
            return (NativeMethods.GetAsyncKeyState(vKey) & 0x8000) != 0;
        }
    }
}
