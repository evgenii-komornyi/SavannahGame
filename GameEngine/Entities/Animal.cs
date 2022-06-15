using GameEngine.Services;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains all common properties and behaviour of the animals.
    /// </summary>
    public abstract class Animal
    {
        public int Id { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public AnimalType Type { get; set; }
        public int Vision { get; set; }
        public bool IsDead { get; set; }
        public string? Letter { get; set; }

        private static int id = 0;

        /// <summary>
        /// Generates new id.
        /// </summary>
        /// <returns>Incremented id.</returns>
        protected int GenerateId()
        {
            return id++;
        }

        /// <summary>
        /// Moves current animal on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animal">Animal.</param>
        /// <param name="animals">Animals.</param>
        public abstract void MoveAnimal(Board board, Animal animal, List<Animal> animals);

        /// <summary>
        /// Moves animal to a new random position.
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        protected void MoveToNewRandomPosition(Animal animal, List<NewAnimalCoordinates> freeCellsToMove)
        {
            NewAnimalCoordinates nextCoordinatesToMove = freeCellsToMove[Helper.GenerateRandomCoordinates(0, freeCellsToMove.Count)];
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
        protected List<NewAnimalCoordinates> CalculateCorrectPosition(Board board, List<Animal> animals, Animal animal)
        {
            List<NewAnimalCoordinates> freeCellsToMove = new List<NewAnimalCoordinates>();
            NewAnimalCoordinates newCoordinates;

            for (int newXCoordinate = animal.CoordinateX - 1; newXCoordinate <= animal.CoordinateX + 1; newXCoordinate++)
            {
                for (int newYCoordinate = animal.CoordinateY - 1; newYCoordinate <= animal.CoordinateY + 1; newYCoordinate++)
                {
                    if (!board.IsOffBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsNotFreePlace(newXCoordinate, newYCoordinate, animals))
                    {
                        newCoordinates = new NewAnimalCoordinates
                        {
                            NewXCoordinate = newXCoordinate,
                            NewYCoordinate = newYCoordinate
                        };

                        freeCellsToMove.Add(newCoordinates);
                    }
                }
            }
            newCoordinates = new NewAnimalCoordinates
            {
                NewXCoordinate = animal.CoordinateX,
                NewYCoordinate = animal.CoordinateY
            };

            freeCellsToMove.Add(newCoordinates);

            return freeCellsToMove;
        }

        /// <summary>
        /// Finds animals around by type.
        /// </summary>
        /// <param name="animalTarget">Animal target.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <param name="animalType">Animal type.</param>
        /// <returns>Animals around by type.</returns>
        protected List<Animal> FindAnimalsAroundByType(Animal animalTarget, List<Animal> allAnimals, AnimalType animalType)
        {
            IEnumerable<Animal> animalsAround = LookAround(animalTarget, allAnimals);

            return animalsAround.Where(animal => animal.Type == animalType && !animal.IsDead).ToList();
        }

        /// <summary>
        /// Looks around board from animal based on it vision.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <returns>Enumerable list with nearby animals.</returns>
        private IEnumerable<Animal> LookAround(Animal currentAnimal, IEnumerable<Animal> allAnimals)
        {
            return from animalForSearch in allAnimals
                   where AnimalsAreInRange(currentAnimal, animalForSearch)
                   select animalForSearch;
        }

        /// <summary>
        /// Returns all animals in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="animalForSearch">Animal for search.</param>
        /// <returns>Are the animals in the range of vision, excluding itself.</returns>
        private bool AnimalsAreInRange(Animal currentAnimal, Animal animalForSearch)
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
