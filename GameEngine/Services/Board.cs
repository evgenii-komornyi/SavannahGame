using GameEngine.Entities;
using Repository;

namespace GameEngine
{
    /// <summary>
    /// The class contains all logic of the game engine.
    /// </summary>
    public class Board
    {
        public string[,] GameBoard;

        /// <summary>
        /// The class contains all logic of the game engine.
        /// </summary>
        public Board()
        {
            GameBoard = new string[ConstantsRepository.ColumnsCount, ConstantsRepository.RowsCount];
            ClearBoard();
        }

        /// <summary>
        /// Fills the board with animals
        /// </summary>
        /// <param name="animals">Animals to fill the board.</param>
        public void FillBoardWithAnimals(List<Animal> animals)
        {
            foreach (var animal in animals)
            {
                PutAnimalOnBoard(animal, animal.Letter);
            }
        }

        /// <summary>
        /// Removes animal from board.
        /// </summary>
        /// <param name="animals">Animals.</param>
        public void RemoveAnimalFromBoard(List<Animal> animals)
        {
            foreach (Animal animal in animals)
            {
                GameBoard[animal.CoordinateX, animal.CoordinateY] = ConstantsRepository.EmptyCell;
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
        /// Fills the board with the first animals' letter. 
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <param name="animalLetter">Animal letter.</param>
        private void PutAnimalOnBoard(Animal animal, string animalLetter)
        {
            GameBoard[animal.CoordinateX, animal.CoordinateY] = animalLetter;
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