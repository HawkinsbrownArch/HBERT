using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace CarbonEmissionTool.Services
{
    /// <summary>
    /// Wrapper class for window handles
    /// </summary>
    public class WindowHandleSearch : IWin32Window, System.Windows.Forms.IWin32Window
    {
        /// <summary>
        /// Window handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        // User32.dll calls used to get the Main Window for a Process Id (PID)
        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.DLL")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        #region Constructor
        /// <summary>
        /// Constructor - From WinForms window handle
        /// </summary>
        /// <param name="hwnd">Window handle</param>
        public WindowHandleSearch(IntPtr hwnd)
        {
            // Assert valid window handle
            Debug.Assert(IntPtr.Zero != hwnd, "Null window handle");

            Handle = hwnd;
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Revit main window handle
        /// </summary>
        static public WindowHandleSearch MainWindowHandle
        {
            get
            {
                // Get handle of main window
                var revitProcess = Process.GetCurrentProcess();
                return new WindowHandleSearch(GetMainWindow(revitProcess.Id));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set this window handle as the owner of the given window
        /// </summary>
        /// <param name="childWindow"> Child window whose parent will be set to be this window handle. </param>
        public void SetAsOwner(Window childWindow)
        {
            var helper = new WindowInteropHelper(childWindow) { Owner = Handle };
        }

        /// <summary>
        /// Returns the main Window Handle for the process Id (pid) passed in. If the Main Window is not found then a 
        /// handle value of Zreo is returned, no handle.
        /// </summary>
        public static IntPtr GetMainWindow(int pid)
        {
            IntPtr shellWindow = GetShellWindow();
            List<IntPtr> windowsForPid = new List<IntPtr>();

            try
            {
                EnumWindows(
                    // EnumWindowsProc Function, does the work on each window.
                    delegate (IntPtr hWnd, int lParam)
                    {
                        if (hWnd == shellWindow) return true;
                        if (!IsWindowVisible(hWnd)) return true;

                        uint windowPid = 0;
                        GetWindowThreadProcessId(hWnd, out windowPid);

                        // if window is from Pid of interest, see if it's the Main Window
                        if (windowPid == pid)
                        {
                            // By default Main Window has a Parent Window of Zero, no parent.
                            IntPtr parentHwnd = GetParent(hWnd);
                            if (parentHwnd == IntPtr.Zero)
                            {
                                windowsForPid.Add(hWnd);
                            }
                        }

                        return true;
                    }
                    // lParam, nothing, null...
                    , 0);
            }
            catch (Exception)
            {

            }

            return DetermineMainWindow(windowsForPid);
        }

        /// <summary>
        /// Finds Revit's Main Window from the list of window handles passed in. If the Main Window for Revit is not found
        /// then a Null (IntPtr.Zero) handle is returnd.
        /// </summary>
        private static IntPtr DetermineMainWindow(List<IntPtr> handles)
        {
            // Safty conditions, bail if not met.
            if (handles == null || handles.Count <= 0)
                return IntPtr.Zero;

            // default Null handel
            IntPtr mainWindow = IntPtr.Zero;

            // only one window so return it, must be the Main Window??
            if (handles.Count == 1)
            {
                mainWindow = handles[0];
            }
            // more than one window
            else
            {
                // more than one candidate for Main Window so find the Main Window by its Title, it will contain "Autodesk Revit".
                foreach (var hWnd in handles)
                {
                    int length = GetWindowTextLength(hWnd);
                    if (length == 0) continue;

                    StringBuilder builder = new StringBuilder(length);

                    WindowHandleSearch.GetWindowText(hWnd, builder, length + 1);

                    // Depending on the Title of the Main Window to have "Autodesk Revit" in it.
                    if (builder.ToString().ToLower().Contains("autodesk revit"))
                    {
                        mainWindow = hWnd;
                        break; // found Main Window stop and return it.
                    }
                }
            }
            return mainWindow;
        }
        #endregion
    }
}