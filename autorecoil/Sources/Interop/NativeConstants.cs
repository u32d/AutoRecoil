using System;

namespace autorecoil.Sources.Interop
{
    /// <summary>
    /// Win32 常量集中定义（虚拟键码、窗口消息、输入标志等），方便统一维护。
    /// </summary>
    internal static class NativeConstants
    {
        // 虚拟键码
        public const int VK_LBUTTON = 0x01;
        public const int VK_RBUTTON = 0x02;
        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_MENU = 0x12;    // Alt
        public const int VK_CAPITAL = 0x14; // CapsLock

        // 输入相关
        public const uint INPUT_MOUSE = 0;
        public const uint MOUSEEVENTF_MOVE = 0x0001;

        // 窗口消息
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_RBUTTONUP = 0x0205;

        // SendInput 鼠标按键标志
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        // 鼠标按键状态掩码（WM_xBUTTON 的 wParam 用）
        public const int MK_LBUTTON = 0x0001;
        public const int MK_RBUTTON = 0x0002;

        // 键盘输入相关
        public const uint INPUT_KEYBOARD = 1;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004; // 以 Unicode 方式输入字符（wScan=字符）

        // 键盘消息（窗口消息方式）
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;

        // 虚拟键码：~ / ` 波浪键
        public const int VK_OEM_3 = 0xC0;
        // 虚拟键码：Enter / 回车
        public const int VK_RETURN = 0x0D;
    }
}
