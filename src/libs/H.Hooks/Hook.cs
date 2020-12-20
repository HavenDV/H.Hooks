using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;

namespace H.Hooks
{
    public abstract class Hook : IDisposable
    {
        public static Keys FromString(string text) => Enum.TryParse<Keys>(text, true, out var result) ? result : Keys.None;

        #region Properties

        public string Name { get; }
        public bool IsStarted { get; private set; }

        private HookProcedureType Type { get; }
        private IntPtr HookHandle { get; set; }
        private HookProc? HookAction { get; set; }

        #endregion

        #region Events

        public event EventHandler<UnhandledExceptionEventArgs>? UnhandledException;

        #endregion

        #region Constructors

        protected Hook(string name, HookProcedureType type)
        {
            Name = name;
            Type = type;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Start hook process
        /// </summary>
        /// <exception cref="Win32Exception">If SetWindowsHookEx return error code</exception>
        public void Start()
        {
            if (IsStarted)
            {
                return;
            }

            Trace.WriteLine($"Starting hook '{Name}'...", $"Hook.StartHook [{Thread.CurrentThread.Name}]");

            HookAction = Callback;
            var moduleHandle = Kernel32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);

            HookHandle = User32.SetWindowsHookEx(Type, HookAction, moduleHandle, 0);
            if (HookHandle == null || HookHandle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            IsStarted = true;
        }

        /// <summary>
        /// Stop hook process
        /// </summary>
        public void Stop()
        {
            if (!IsStarted)
            {
                return;
            }

            Trace.WriteLine($"Stopping hook '{Name}'...", $"Hook.StartHook [{Thread.CurrentThread.Name}]");

            User32.UnhookWindowsHookEx(HookHandle);

            IsStarted = false;
        }

        /// <summary>
        /// Start(if <paramref name="value"/> is <see langword="true"/>) or 
        /// Stop(if <paramref name="value"/> is <see langword="false"/>) hook process
        /// </summary>
        /// <param name="value"></param>
        public void SetEnabled(bool value)
        {
            if (value)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        #endregion

        #region Protected methods

        protected abstract int InternalCallback(int nCode, int wParam, IntPtr lParam);

        protected static T ToStructure<T>(IntPtr ptr) where T : struct => (T)Marshal.PtrToStructure(ptr, typeof(T));

        #endregion

        #region Private methods

        private int Callback(int nCode, int wParam, IntPtr lParam)
        {
            var result = 0;

            try
            {
                result = InternalCallback(nCode, wParam, lParam);
            }
            catch (Exception exception)
            {
                UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(exception, false));
            }

            return result < 0 ? result : User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        #endregion

        #region IDisposable

        /// <inheritdoc />
        /// <summary>
        /// Dispose internal system hook resources
        /// </summary>
        public void Dispose() => Stop();

        #endregion
    }
}