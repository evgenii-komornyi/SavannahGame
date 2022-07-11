using GameEngine.Interfaces;
using Repository;

namespace GameEngine
{
    /// <summary>
    /// The class contains all logic of the game board.
    /// </summary>
    public class Board
    {
        public string[,] GameBoard;

        /// <summary>
        /// The class contains all logic of the game board.
        /// </summary>
        public Board()
        {
            GameBoard = new string[ConstantsRepository.ColumnsCount, ConstantsRepository.RowsCount];
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
                PutObjectOnBoard(gameObject, gameObject.Letter.ToString());
            }
        }

        /// <summary>
        /// Removes game objects from board.
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        public void RemoveGameObjectFromBoard(List<IItem> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                GameBoard[gameObject.CoordinateX, gameObject.CoordinateY] = ConstantsRepository.EmptyCell;
            }
        }

        /// <summary>
        /// Checks whether cell is on board.
        /// </summary>
        /// <param name="newXCoordinate">New X coordinate.</param>
        /// <param name="newYCoordinate">New Y coordinate.</param>
        /// <param name="currentBoard">Current board.</param>
        /// <returns>Is cell on board.</returns>
        public bool IsCellOnBoard(int newXCoordinate, int newYCoordinate, string[,] currentBoard)
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
        private void PutObjectOnBoard(IItem gameObject, string objectKey)
        {
            GameBoard[gameObject.CoordinateX, gameObject.CoordinateY] = objectKey;
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        private void ClearBoard()
        {
            for (int currentColumn = 0; currentColumn < GameBoard.GetLength(0); currentColumn++)
            {
                for (int currentRow = 0; currentRow < GameBoard.GetLength(1); currentRow++)
                {     
                    GameBoard[currentColumn, currentRow] = ConstantsRepository.EmptyCell;
                }
            }
        }
    }
}