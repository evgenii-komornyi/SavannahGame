using GameEngine;

namespace UI
{
    /// <summary>
    /// The class contains user interaction logic.
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        /// Gets response in the main menu from user. 
        /// </summary>
        /// <returns>User prompt.</returns>
        string? GetResponseFromMenu();

/*        /// <summary>
        /// Shows buttons and corresponding commands.
        /// </summary>
        void ShowMenuButtons();*/

        /// <summary>
        /// Shows message to user.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="useWriteLine">Use new line.</param>
        void ShowMessage(string message, bool useWriteLine = true);

        /// <summary>
        /// Draws borders of a board.
        /// </summary>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="borderHeight">Border height.</param>
        void DrawBorders(int borderWidth, int borderHeight);

        /// <summary>
        /// Draws a game board.
        /// </summary>
        /// <param name="board">Game board.</param>
        void DrawBoard(Board board);

        /// <summary>
        /// Gets input key. 
        /// </summary>
        /// <returns>Pressed button value.</returns>
        public ConsoleKey? GetInputKey();
    }
}