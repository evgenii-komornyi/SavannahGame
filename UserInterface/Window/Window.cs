using Repository;
using System.Runtime.InteropServices;

namespace UI
{
    /// <summary>
    /// The class contains console's window configuration.
    /// </summary>
    public class Window : IWindow
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Sets optimal window size.
        /// </summary>
        public void WindowConfiguration()
        {
            #pragma warning disable CA1416 // Validate platform compatibility
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            #pragma warning restore CA1416 // Validate platform compatibility;
            
            IntPtr currentConsole = GetConsoleWindow();
            ShowWindow(currentConsole, ConstantsRepository.MaximizedWindowSize);
            IntPtr systemMenu = GetSystemMenu(currentConsole, false);

            if (currentConsole != IntPtr.Zero)
            {
                DeleteMenu(systemMenu, ConstantsRepository.ConsoleSize, ConstantsRepository.Disabled);
            }
        }

        /// <summary>
        /// Sets console's font color.
        /// </summary>
        /// <param name="color">Font color.</param>
        public void SetFontColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Resets console's font color to default.
        /// </summary>
        public void ResetFontColor()
        {
            Console.ResetColor();
        }

        /// <summary>
        /// Clears console window.
        /// </summary>
        public void ClearConsole()
        {
            Console.Clear();
        }

        /// <summary>
        /// Sets beginning cursor's position. 
        /// </summary>
        /// <param name="left">Shift from left.</param>
        /// <param name="top">Shift from top.</param>
        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }
    }
}