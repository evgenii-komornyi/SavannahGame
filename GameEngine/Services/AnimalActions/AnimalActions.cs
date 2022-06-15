using GameEngine.Entities;
using Repository;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains all common logic of the animals' behaviour.
    /// </summary>
    public class AnimalActions : IAnimalActions
    {
        /// <summary>
        /// Adds new animal to the list of the animals on the current board. 
        /// </summary>
        /// <param name="newAnimal">New animal.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="board">Board.</param>
        public void AddAnimal(Animal newAnimal, List<Animal> animals, Board board)
        {
            List<NewAnimalCoordinates> freeCells = CalculateFreeCellsToAddAnimal(animals, board);
            if (board.GameBoard.Length > board.GameBoard.Length * ConstantsRepository.HalfOfBoard)
            {
                AddAnimalToList(newAnimal, freeCells, animals);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Adds animal to list with the new coordinates.
        /// </summary>
        /// <param name="newAnimal">New animal.</param>
        /// <param name="freeCells">Free cells.</param>
        /// <param name="animals">Animals.</param>
        private void AddAnimalToList(Animal newAnimal, List<NewAnimalCoordinates> freeCells, List<Animal> animals)
        {
            NewAnimalCoordinates freeCoordinates = freeCells[Helper.GenerateRandomCoordinates(0, freeCells.Count)];
            newAnimal.CoordinateX = freeCoordinates.NewXCoordinate;
            newAnimal.CoordinateY = freeCoordinates.NewYCoordinate;
            animals.Add(newAnimal);
        }

        /// <summary>
        /// Calculates the free cells on board to add a new animal. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        /// <param name="board">Board.</param>
        /// <returns>Free cells.</returns>
        private List<NewAnimalCoordinates> CalculateFreeCellsToAddAnimal(List<Animal> animals, Board board)
        {
            List<NewAnimalCoordinates> freeCellsToAdd = new List<NewAnimalCoordinates>();

            for (int newXCoordinate = 0; newXCoordinate < board.GameBoard.GetLength(0); newXCoordinate++)
            {
                for (int newYCoordinate = 0; newYCoordinate < board.GameBoard.GetLength(1); newYCoordinate++)
                {
                    if (!board.IsOffBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsNotFreePlace(newXCoordinate, newYCoordinate, animals))
                    {
                        NewAnimalCoordinates newCoordinates = new NewAnimalCoordinates
                        {
                            NewXCoordinate = newXCoordinate,
                            NewYCoordinate = newYCoordinate
                        };

                        freeCellsToAdd.Add(newCoordinates);
                    }
                }
            }

            return freeCellsToAdd;
        }
        /// <summary>
        /// Moves animals on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        public void Move(Board board, List<Animal> animals)
        {
            foreach (Animal animal in animals)
            { 
                if (!IsAnimalDead(animal))
                {
                    animal.MoveAnimal(board, animal, animals);
                }
            }
        }

        /// <summary>
        /// Removes all dead animals from board. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        public void ClearBoard(List<Animal> animals)
        {
            IEnumerable<Animal> deadAnimals = CheckForDead(animals);
            animals.RemoveAll(currentAnimal => deadAnimals.Contains(currentAnimal));
        }

        /// <summary>
        /// Checks the dead animals.
        /// </summary>
        /// <param name="animals">Animals.</param>
        /// <returns>Enumerable list of the dead animals.</returns>
        private IEnumerable<Animal> CheckForDead(List<Animal> animals)
        {
            return from currentAnimal in animals
                   where IsAnimalDead(currentAnimal)
                   select currentAnimal;
        }

        /// <summary>
        /// Checks dead animal.  
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <returns>Is animal dead.</returns>
        public bool IsAnimalDead(Animal animal)
        {
            return animal.IsDead;
        }
    }
}