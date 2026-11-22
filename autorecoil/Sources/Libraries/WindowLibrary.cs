
using System;
using System.Text;
using autorecoil.Sources.Interop;

namespace autorecoil.Sources.Libraries
{
    /// <summary>
    /// 窗口句柄库：与窗口/句柄相关的纯逻辑（查找、枚举等），与 UI 无关，可在任意线程调用。
    /// 后续进程库、字符串库等同体系扩展时，统一放在这个命名空间下。
    /// </summary>
    internal static class WindowLibrary
    {
        /// <summary>
        /// 枚举所有顶层窗口，返回标题包含指定关键字的第一个窗口句柄；找不到返回 IntPtr.Zero。
        /// </summary>
        /// <param name="title">要匹配的标题关键字（忽略大小写，会自动 Trim）。</param>
        public static IntPtr FindMainWindowByTitle(string title)
        {
            string keyword = (title ?? string.Empty).Trim();
            if (keyword.Length == 0)
            {
                return IntPtr.Zero;
            }

            IntPtr result = IntPtr.Zero;

            NativeMethods.EnumWindows((hWnd, lParam) =>
            {
                int length = NativeMethods.GetWindowTextLengthW(hWnd);
                if (length > 0)
                {
                    StringBuilder sb = new StringBuilder(length + 1);
                    NativeMethods.GetWindowTextW(hWnd, sb, sb.Capacity);
                    if (sb.ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result = hWnd; // 记录匹配到的主窗口句柄
                        return false;  // 找到即停止枚举
                    }
                }
                return true;
            }, IntPtr.Zero);

            return result;
        }
    }
}
