using System;
using System.Runtime.InteropServices;

namespace autorecoil.Sources.Interop
{
    /// <summary>
    /// Win32 结构体集中定义（RECT / POINT / MOUSEINPUT / INPUT），方便统一维护。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    // INPUT 为鼠标/键盘共用的 union。不能用显式布局 [FieldOffset(4)]:x86 下联合体在偏移 4 正确,
    // 但 x64 下 MOUSEINPUT 含 8 字节指针成员,真实 Win64 INPUT 的联合体必须对齐到偏移 8(总大小 40),
    // 固定偏移 4 会让 SendInput 从错误偏移读 dx/dy → 注入 sent=1 但光标不动。
    // 顺序布局 + 嵌套联合体让 marshaler 按当前位数自动对齐(x86=28 字节,x64=40 字节),双位数均正确。
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT
    {
        public uint type;
        public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
    }
}
