namespace UI
{
    /// <summary>
    /// The class contains console's window configuration.
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// Sets optimal window size.
        /// </summary>
        void WindowConfiguration();
      
        /// <summary>
        /// Sets console's font color.
        /// </summary>
        /// <param name="color">Font color.</param>
        void SetFontColor(ConsoleColor color);

        /// <summary>
        /// Resets console's font color to default.
        /// </summary>
        void ResetFontColor();
        /// <summary>
        /// Clears console window.
        /// </summary>
        void ClearConsole();
        
        /// <summary>
        /// Sets beginning cursor's position. 
        /// </summary>
        /// <param name="left">Shift from left.</param>
        /// <param name="top">Shift from top.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Sets console window's title.
        /// </summary>
        /// <param name="title">Title.</param>
        void SetTitle(string title);
    }
}