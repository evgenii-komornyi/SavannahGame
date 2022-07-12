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
        /// Fills the board with the game objects
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        public void FillBoardWithObjects(List<IItem> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                PutObjectOnBoard(gameObject);
            }
        }

        /// <summary>
        /// Prepares clean board for new iteration.
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        public void PrepareBoard(List<IItem> gameObjects)
        {
            BoardItem boardItem = new BoardItem
            {
                Letter = ConstantsRepository.EmptyCell,
                Color = ConsoleColor.Gray
            };

            foreach (var gameObject in gameObjects)
            {
                GameBoard[gameObject.CoordinateX, gameObject.CoordinateY] = boardItem;
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
        /// Fills the board with the object key. 
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="objectKey">Object key.</param>
        private void PutObjectOnBoard(IItem gameObject)
        {
            BoardItem boardItem = new BoardItem
            {
                Letter = gameObject.Letter.ToString(),
                Color = gameObject.Color
            };

            GameBoard[gameObject.CoordinateX, gameObject.CoordinateY] = boardItem;
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