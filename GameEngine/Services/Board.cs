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
            FillBoard();
        }

        /// <summary>
        /// Fills the board with animals
        /// </summary>
        /// <param name="animals">Animals to fill the board.</param>
        public void FillBoardWithAnimals(List<IAnimal> animals)
        {
            foreach (var animal in animals)
            {
                PutAnimalOnBoard(animal, animal.Letter);
            }
        }

        /// <summary>
        /// Changes current animal with empty cell on the next move.
        /// </summary>
        /// <param name="animals">Animals.</param>
        public void ChangeAnimalWithEmptyCell(List<IAnimal> animals)
        {
            foreach (IAnimal animal in animals)
            {
                GameBoard[animal.CoordinateX, animal.CoordinateY] = ConstantsRepository.EmptyCell;
            }
        }

        /// <summary>
        /// Checks coordinate X and coordinate Y on the ends of board.
        /// </summary>
        /// <param name="newXCoordinate">New X coordinate.</param>
        /// <param name="newYCoordinate">New Y coordinate.</param>
        /// <param name="currentBoard">Current board.</param>
        /// <returns>Is the coordinates out off board.</returns>
        public bool IsOffBoard(int newXCoordinate, int newYCoordinate, string[,] currentBoard)
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
        private void PutAnimalOnBoard(IAnimal animal, string animalLetter)
        {
            GameBoard[animal.CoordinateX, animal.CoordinateY] = animalLetter;
        }

        /// <summary>
        /// Fills the board with empty cells.
        /// </summary>
        private void FillBoard()
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