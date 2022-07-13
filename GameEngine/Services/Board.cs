using GameEngine.Entities;
using GameEngine.Interfaces;
using Repository;

namespace GameEngine
{
    /// <summary>
    /// The class contains all logic of the game board.
    /// </summary>
    public class Board
    {
        public BoardItem[,] GameBoard;

        /// <summary>
        /// The class contains all logic of the game board.
        /// </summary>
        public Board()
        {
            GameBoard = new BoardItem[ConstantsRepository.ColumnsCount, ConstantsRepository.RowsCount];
            ClearBoard();
        }

        /// <summary>
        /// Fills the board with the game items.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        public void FillBoardWithItems(List<IItem> gameItems)
        {
            foreach (var gameItem in gameItems)
            {
                PutItemOnBoard(gameItem);
            }
        }

        /// <summary>
        /// Prepares clean board for new iteration.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        public void PrepareBoard(List<IItem> gameItems)
        {
            BoardItem boardItem = new BoardItem
            {
                Letter = ConstantsRepository.EmptyCell,
                Color = ConsoleColor.Gray
            };

            foreach (var gameItem in gameItems)
            {
                GameBoard[gameItem.CoordinateX, gameItem.CoordinateY] = boardItem;
            }
        }

        /// <summary>
        /// Checks whether cell is on board.
        /// </summary>
        /// <param name="newXCoordinate">New X coordinate.</param>
        /// <param name="newYCoordinate">New Y coordinate.</param>
        /// <param name="currentBoard">Current board.</param>
        /// <returns>Is cell on board.</returns>
        public bool IsCellOnBoard(int newXCoordinate, int newYCoordinate, BoardItem[,] currentBoard)
        {
            return (newXCoordinate < 0) ||
                   (newXCoordinate >= currentBoard.GetLength(0)) ||
                   (newYCoordinate < 0) ||
                   (newYCoordinate >= currentBoard.GetLength(1));
        }

        /// <summary>
        /// Fills the board with the item key and color. 
        /// </summary>
        /// <param name="gameItem">Game item.</param>
        private void PutItemOnBoard(IItem gameItem)
        {
            BoardItem boardItem = new BoardItem
            {
                Letter = gameItem.Letter.ToString(),
                Color = gameItem.Color
            };

            GameBoard[gameItem.CoordinateX, gameItem.CoordinateY] = boardItem;
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        private void ClearBoard()
        {
            BoardItem boardItem = new BoardItem
            {
                Letter = ConstantsRepository.EmptyCell,
                Color = ConsoleColor.Gray
            };

            for (int currentColumn = 0; currentColumn < GameBoard.GetLength(0); currentColumn++)
            {
                for (int currentRow = 0; currentRow < GameBoard.GetLength(1); currentRow++)
                {     
                    GameBoard[currentColumn, currentRow] = boardItem;
                }
            }
        }
    }
}