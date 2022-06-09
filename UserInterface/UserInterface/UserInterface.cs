using GameEngine;
using Repository;

namespace UI
{
    /// <summary>
    /// The class contains user interaction logic.
    /// </summary>
    public class UserInterface : IUserInterface
    {
        private readonly IWindow _window;
        /// <summary>
        /// The class contains user interaction logic.
        /// </summary>
        /// <param name="window">Window.</param>
        public UserInterface(IWindow window)
        {
            _window = window;
        }

        /// <summary>
        /// Gets response in the main menu from user. 
        /// </summary>
        /// <returns>User prompt.</returns>
        public string? GetResponseFromMenu()
        {
            _window.SetTitle(ConstantsRepository.GameTitle);
            
            return Console.ReadLine();
        }

        /// <summary>
        /// Shows buttons and corresponding commands.
        /// </summary>
        public void ShowMenuButtons()
        {
            ShowMessage(ConstantsRepository.AddAntilopeDescription);
            ShowMessage(ConstantsRepository.AddLionDescription);
            ShowMessage(ConstantsRepository.ExitButtonDescription);
        }

        /// <summary>
        /// Shows message to user.
        /// </summary>
        /// <param name="message">Message to show.</param>
        public void ShowMessage(string message, bool useWriteLine = true)
        {
            if (useWriteLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
        }

        /// <summary>
        /// Draws borders of a board.
        /// </summary>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="borderHeight">Border height.</param>
        public void DrawBorders(int borderWidth, int borderHeight)
        {
            _window.SetFontColor(ConsoleColor.Red);

            DrawWalls(borderWidth, borderHeight);
            DrawCorners(borderWidth, borderHeight);

            _window.ResetFontColor();
        }

        /// <summary>
        /// Draws border's walls.
        /// </summary>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="borderHeight">Border height.</param>
        private void DrawWalls(int borderWidth, int borderHeight)
        {
            for (int currentColumn = 0; currentColumn < borderWidth; currentColumn++)
            {
                ShowMessage(ConstantsRepository.UpDownWall, false);
            }

            _window.SetCursorPosition(ConstantsRepository.Zero, borderHeight);
            
            for (int currentColumn = 0; currentColumn < borderWidth; currentColumn++)
            {
                ShowMessage(ConstantsRepository.UpDownWall, false);
            }

            for (int currentRow = 0; currentRow < borderHeight; currentRow++)
            {
                _window.SetCursorPosition(ConstantsRepository.Zero, currentRow);
                ShowMessage(ConstantsRepository.LeftRightWall, false);
            }

            for (int currentRow = 0; currentRow < borderHeight; currentRow++)
            {
                _window.SetCursorPosition(borderWidth, currentRow);
                ShowMessage(ConstantsRepository.LeftRightWall, false);
            }
        }

        /// <summary>
        /// Draws border's corners.
        /// </summary>
        /// <param name="borderWidth">Border width.</param>
        /// <param name="borderHeight">Border height.</param>
        private void DrawCorners(int borderWidth, int borderHeight)
        {
            _window.SetCursorPosition(0, 0);
            ShowMessage(ConstantsRepository.UpLeftCorner, false);
            
            _window.SetCursorPosition(borderWidth, 0);
            ShowMessage(ConstantsRepository.UpRightCorner, false);

            _window.SetCursorPosition(0, borderHeight);
            ShowMessage(ConstantsRepository.DownLeftCorner, false);

            _window.SetCursorPosition(borderWidth, borderHeight);
            ShowMessage(ConstantsRepository.DownRightCorner, false);
        }

        /// <summary>
        /// Draws a game board.
        /// </summary>
        /// <param name="board">Game board.</param>
        public void DrawBoard(Board board)
        {
            for (int currentRow = 0; currentRow < board.GameBoard.GetLength(1); currentRow++)
            {
                _window.SetCursorPosition(ConstantsRepository.OffsetX - ConstantsRepository.One, currentRow + ConstantsRepository.OffsetY);

                for (int currentColumn = 0; currentColumn < board.GameBoard.GetLength(0); currentColumn++)
                {
                    Console.Write(board.GameBoard[currentColumn, currentRow]);
                    _window.SetCursorPosition(ConstantsRepository.OffsetX + currentColumn, currentRow + ConstantsRepository.OffsetY);
                }
            }
        }

        /// <summary>
        /// Gets input key. 
        /// </summary>
        /// <returns>Pressed button value.</returns>
        public ConsoleKey? GetInputKey()
        {
            if (Console.KeyAvailable)
            {
                return Console.ReadKey(true).Key;
            }
            return null;
        }
    }
}