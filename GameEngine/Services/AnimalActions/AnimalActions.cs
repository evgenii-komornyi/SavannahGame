using GameEngine.Entities;
using Repository;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains all common logic of the animals' behaviour.
    /// </summary>
    public class AnimalActions : IAnimalActions
    {
        private Random _random = new Random();

        /// <summary>
        /// Adds new animal to the list of the animals on the current board. 
        /// </summary>
        /// <param name="newAnimal">New animal.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="board">Board.</param>
        public void AddAnimal(IAnimal newAnimal, List<IAnimal> animals, Board board)
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
        /// <param name="newXCoordinate">New X coordinate.</param>
        /// <param name="newYCoordinate">New Y coordinate.</param>
        /// <param name="animals">Animals.</param>
        private void AddAnimalToList(IAnimal newAnimal,List<NewAnimalCoordinates> freeCells, List<IAnimal> animals)
        {
            NewAnimalCoordinates freeCoordinates = freeCells[GenerateRandomCoordinates(0, freeCells.Count)];
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
        private List<NewAnimalCoordinates> CalculateFreeCellsToAddAnimal(List<IAnimal> animals, Board board)
        {
            List<NewAnimalCoordinates> freeCellsToAdd = new List<NewAnimalCoordinates>();
            NewAnimalCoordinates newCoordinates = new NewAnimalCoordinates();

            for (int newXCoordinate = 0; newXCoordinate < board.GameBoard.GetLength(0); newXCoordinate++)
            {
                for (int newYCoordinate = 0; newYCoordinate < board.GameBoard.GetLength(1); newYCoordinate++)
                {
                    if (!board.IsOffBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !IsNotFreePlace(newXCoordinate, newYCoordinate, animals))
                    {
                        newCoordinates = new NewAnimalCoordinates();
                        newCoordinates.NewXCoordinate = newXCoordinate;
                        newCoordinates.NewYCoordinate = newYCoordinate;
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
        public void Move(Board board, List<IAnimal> animals)
        {
            foreach (IAnimal animal in animals)
            {
                if (!IsAnimalDead(animal))
                {
                    if (animal.Type == AnimalType.Lion)
                    {
                        Lion lion = (Lion)animal;
                        LionActions lionActions = new LionActions();

                        List<IAnimal> antilopesAround = FindAnimalsAroundByType(lion, animals, AnimalType.Antilope);

                        IAnimal? antilopeToHunt = lionActions.FindNearestAntilope(antilopesAround, lion);
                        
                        List<NewAnimalCoordinates> freeCellsToMove = CalculateCorrectPosition(board, animals, lion);

                        if (antilopeToHunt == null)
                        {
                            MoveToNewRandomPosition(lion, freeCellsToMove);
                        }
                        else
                        {
                            if (IsAntilopeFar(lion, antilopeToHunt))
                            {
                                lionActions.HuntAntilope(antilopeToHunt, lion, freeCellsToMove);
                            }
                            else
                            {
                                lionActions.EatAntilope(antilopeToHunt, lion);
                            }
                        }
                    }

                    if (animal.Type == AnimalType.Antilope)
                    {
                        Antilope antilope = (Antilope)animal;
                        AntilopeActions antilopeActions = new AntilopeActions();

                        List<IAnimal> lionsAround = FindAnimalsAroundByType(antilope, animals, AnimalType.Lion);

                        IAnimal? nearestLion = antilopeActions.FindNearestLion(lionsAround, antilope);

                        List<NewAnimalCoordinates> freeCellsToMove = CalculateCorrectPosition(board, animals, antilope);

                        if (nearestLion == null)
                        {
                            MoveToNewRandomPosition(antilope, freeCellsToMove);
                        }
                        else
                        {
                            if (IsLionNear(antilope, nearestLion))
                            {
                                antilopeActions.TryToRunAway(nearestLion, antilope, freeCellsToMove);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks the antilope far the lion.
        /// </summary>
        /// <param name="lion">Lion.</param>
        /// <param name="antilopeToHunt">Far antilope to hunt.</param>
        /// <returns>Is antilope far.</returns>
        private bool IsAntilopeFar(Lion lion, IAnimal antilopeToHunt)
        {
            return Math.Abs(lion.CoordinateX - antilopeToHunt.CoordinateX) > 1 ||
                   Math.Abs(lion.CoordinateY - antilopeToHunt.CoordinateY) > 1;
        }

        /// <summary>
        /// Checks the nearest lion to antilope.
        /// </summary>
        /// <param name="antilope">Antilope.</param>
        /// <param name="nearestLion">Nearest lion.</param>
        /// <returns>Is lion near.</returns>
        private bool IsLionNear(Antilope antilope, IAnimal nearestLion)
        {
            return Math.Abs(antilope.CoordinateX - nearestLion.CoordinateX) < 2 ||
                            Math.Abs(antilope.CoordinateY - nearestLion.CoordinateY) < 2;
        }

        /// <summary>
        /// Moves animal to a new random position.
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        private void MoveToNewRandomPosition(IAnimal animal, List<NewAnimalCoordinates> freeCellsToMove)
        {
            NewAnimalCoordinates nextCoordinatesToMove = freeCellsToMove[GenerateRandomCoordinates(0, freeCellsToMove.Count)];
            animal.CoordinateX = nextCoordinatesToMove.NewXCoordinate;
            animal.CoordinateY = nextCoordinatesToMove.NewYCoordinate;
        }

        /// <summary>
        /// Calculates a new position based on the free cells around animal.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="animal">Animal.</param>
        /// <returns>Free cells to move.</returns>
        private List<NewAnimalCoordinates> CalculateCorrectPosition(Board board, List<IAnimal> animals, IAnimal animal)
        {
            List<NewAnimalCoordinates> freeCellsToMove = new List<NewAnimalCoordinates>();
            NewAnimalCoordinates newCoordinates = new NewAnimalCoordinates();

            for (int newXCoordinate = animal.CoordinateX - 1; newXCoordinate <= animal.CoordinateX + 1; newXCoordinate++) 
            {
                for (int newYCoordinate = animal.CoordinateY - 1; newYCoordinate <= animal.CoordinateY + 1; newYCoordinate++)
                {
                    if (!board.IsOffBoard(newXCoordinate, newYCoordinate, board.GameBoard) && 
                        !IsNotFreePlace(newXCoordinate, newYCoordinate, animals))
                    {
                        newCoordinates = new NewAnimalCoordinates();
                        newCoordinates.NewXCoordinate = newXCoordinate;
                        newCoordinates.NewYCoordinate = newYCoordinate;
                        freeCellsToMove.Add(newCoordinates);
                    }
                }
            }
            newCoordinates = new NewAnimalCoordinates();
            newCoordinates.NewXCoordinate = animal.CoordinateX;
            newCoordinates.NewYCoordinate = animal.CoordinateY;
            freeCellsToMove.Add(newCoordinates);

            return freeCellsToMove;
        }

        /// <summary>
        /// Checks non-free place for new animal's step.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>The first non-free place for animal.</returns>
        private bool IsNotFreePlace(int newXPosition, int newYPosition, List<IAnimal> animals)
        {
            return (from animal in animals
                   where animal.CoordinateX.Equals(newXPosition) &&
                         animal.CoordinateY.Equals(newYPosition)
                    select animal).Any();
        }

        /// <summary>
        /// Looks around board from animal based on it vision.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <returns>Enumerable list with nearby animals.</returns>
        protected IEnumerable<IAnimal> LookAround(IAnimal currentAnimal, IEnumerable<IAnimal> allAnimals)
        {
            return from animalForSearch in allAnimals
                   where AnimalsAreInRange(currentAnimal, animalForSearch)
                   select animalForSearch;
        }

        /// <summary>
        /// Generates a random number in the interval.
        /// </summary>
        /// <param name="minLimit">Min limit.</param>
        /// <param name="maxLimit">Max limit.</param>
        /// <returns>Random number.</returns>
        protected int GenerateRandomCoordinates(int minLimit, int maxLimit)
        {
            return _random.Next(minLimit, maxLimit);
        }

        /// <summary>
        /// Returns all animals in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="animalForSearch">Animal for search.</param>
        /// <returns>Are the animals in the range of vision, excluding itself.</returns>
        private bool AnimalsAreInRange(IAnimal currentAnimal, IAnimal animalForSearch)
        {
            bool isOwnPosition = currentAnimal.CoordinateX == animalForSearch.CoordinateX &&
                                 currentAnimal.CoordinateY == animalForSearch.CoordinateY;
            return !isOwnPosition &&
                    animalForSearch.CoordinateX >= currentAnimal.CoordinateX - currentAnimal.Vision &&
                    animalForSearch.CoordinateX <= currentAnimal.CoordinateX + currentAnimal.Vision &&
                    animalForSearch.CoordinateY >= currentAnimal.CoordinateY - currentAnimal.Vision &&
                    animalForSearch.CoordinateY <= currentAnimal.CoordinateY + currentAnimal.Vision;
        }

        /// <summary>
        /// Removes all dead animals. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        public void Die(List<IAnimal> animals)
        {
            IEnumerable<IAnimal> deadAnimals = CheckForDead(animals);
            animals.RemoveAll(currentAnimal => deadAnimals.Contains(currentAnimal));
        }

        /// <summary>
        /// Checks the dead animals.
        /// </summary>
        /// <param name="animals">Animals.</param>
        /// <returns>Enumerable list of the dead animals.</returns>
        private IEnumerable<IAnimal> CheckForDead(List<IAnimal> animals)
        {
            return from currentAnimal in animals
                   where IsAnimalDead(currentAnimal)
                   select currentAnimal;
        }

        /// <summary>
        /// Finds animals around by type.
        /// </summary>
        /// <param name="animalTarget">Animal target.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <param name="animalType">Animal type.</param>
        /// <returns>Animals around by type.</returns>
        private List<IAnimal> FindAnimalsAroundByType(IAnimal animalTarget, List<IAnimal> allAnimals, AnimalType animalType)
        {
            IEnumerable<IAnimal> animalsAround = LookAround(animalTarget, allAnimals);

            return animalsAround.Where(animal => animal.Type == animalType && !IsAnimalDead(animal)).ToList();
        }

        /// <summary>
        /// Checks dead animal.  
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <returns>Is animal dead.</returns>
        private bool IsAnimalDead(IAnimal animal)
        {
            return animal.IsDead;
        }

        /// <summary>
        /// Calculates square distance by Pythagorian theorem.
        /// </summary>
        /// <param name="pointsCoordinates">Points coordinates.</param>
        /// <returns>Square distance.</returns>
        protected double CalculateSquareDistanceByPythagoras(PointsCoordinates pointsCoordinates)
        {
            return Math.Pow(pointsCoordinates.FirstXPoint - pointsCoordinates.SecondXPoint, 2) + 
                Math.Pow(pointsCoordinates.FirstYPoint - pointsCoordinates.SecondYPoint, 2);
        }
    }
}