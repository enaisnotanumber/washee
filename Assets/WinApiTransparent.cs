// !WARNING!
// make sure the camera is set to "solid color" with all values at 0 (red, green, blue, alpha),
// otherwise the window will not be transparent!

// credit to code monkey for the transparent window tutorial

// also uhhh alpha values don't work because i'm lazy

using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinApiTransparent : MonoBehaviour
{
    // user32.dll
    // +==============+
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    public static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);
    // +==============+

    // Dwmapi.dll
    // +==============+
    [DllImport("Dwmapi.dll")]
    private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
    // +==============+

    private struct MARGINS // window margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    // these are so you don't have to use magic numbers when calling a function itself
    // +==============+
    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    const uint LWA_COLORKEY = 0x00000001;
    // +==============+

    private void Start()
    {
#if !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();

        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        // comment these two lines out to disable interactions with gameobjects
        // +==============+
        SetWindowLong(hWnd, GWL_EXSTYLE, (int)(WS_EX_LAYERED)); // make the window support transparency i think

        SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY); // make the window transparent
        // +==============+
        // SetWindowLong(hWnd, GWL_EXSTYLE, (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT)); // window transparency without gameobject interaction

        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0); // set on top

        Application.runInBackground = true; // make sure to allow for running in the background (even though it's enabled in the player settings anyway)
#else
        MessageBox(IntPtr.Zero, "Don't run this in the Unity Editor!", ":3", 0);
#endif
    }
}
