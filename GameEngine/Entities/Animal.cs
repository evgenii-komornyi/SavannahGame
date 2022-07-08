using GameEngine.Services;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains all common properties and behaviour of the animals.
    /// </summary>
    public abstract class Animal
    {
        /// <summary>
        /// Animal id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Coordinate X.
        /// </summary>
        public int CoordinateX { get; set; }

        /// <summary>
        /// Coordinate Y.
        /// </summary>
        public int CoordinateY { get; set; }

        /// <summary>
        /// Animal sex.
        /// </summary>
        public AnimalSex Sex { get; set; }

        /// <summary>
        /// Indicates animal is paired. 
        /// If true - animal is paired.
        /// </summary>
        public bool IsPaired { get; set; }
        
        /// <summary>
        /// Animal vision.
        /// </summary>
        public int Vision { get; set; }

        /// <summary>
        /// Animal health.
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        ///  Indicates animal is dead. 
        ///  If true - animal is dead.
        /// </summary>
        public bool IsDead { get; set; }

        /// <summary>
        /// Symbol for displaying animal on a game field.
        /// </summary>
        public string? Letter { get; set; }

        private static int id = 0;

        /// <summary>
        /// Generates new id.
        /// </summary>
        /// <returns>Incremented id.</returns>
        protected int GenerateId() => id++;

        /// <summary>
        /// Moves current animal on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animal">Animal.</param>
        /// <param name="animals">Animals.</param>
        public abstract void MoveAnimal(Board board, Animal animal, List<Animal> animals);

        /// <summary>
        /// Gives birth of a new animal.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>New animal (child).</returns>
        public abstract Animal? GiveBirth(Board board, List<Animal> animals);

        /// <summary>
        /// Makes next move of animal.
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        protected void MakeNextMove(Animal animal, List<NewAnimalCoordinates> freeCellsToMove)
        {
            MoveToNewRandomPosition(animal, freeCellsToMove);
            Die(animal, DecreaseHealth(animal));
        }

        /// <summary>
        /// Moves animal to a new random position.
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        protected void MoveToNewRandomPosition(Animal animal, List<NewAnimalCoordinates> freeCellsToMove)
        {
            NewAnimalCoordinates nextCoordinatesToMove = freeCellsToMove[Helper.random.Next(0, freeCellsToMove.Count)];
            animal.CoordinateX = nextCoordinatesToMove.NewXCoordinate;
            animal.CoordinateY = nextCoordinatesToMove.NewYCoordinate;
        }

        /// <summary>
        /// Decreases animal's health.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <returns>Decreased health.</returns>
        private double DecreaseHealth(Animal currentAnimal) => currentAnimal.Health -= ConstantsRepository.HealthDecreaser;

        /// <summary>
        /// Makes the animal dead.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="currentHealth">Current health.</param>
        private void Die(Animal currentAnimal, double currentHealth)
        {
            if (currentHealth <= 0)
            {
                currentAnimal.IsDead = true;
            }
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
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsCellOccupied(newXCoordinate, newYCoordinate, animals))
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
        /// Calculates first free cell for birth child near female animal.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>New animal coordinates for child.</returns>
        protected NewAnimalCoordinates? CalculateFreeCellsToBirth(Board board, List<Animal> animals)
        {
            NewAnimalCoordinates? newCoordinates = null;

            for (int newXCoordinate = CoordinateX - 1; newXCoordinate <= CoordinateX + 1; newXCoordinate++)
            {
                for (int newYCoordinate = CoordinateY - 1; newYCoordinate <= CoordinateY + 1; newYCoordinate++)
                {
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsCellOccupied(newXCoordinate, newYCoordinate, animals))
                    {
                        newCoordinates = new NewAnimalCoordinates
                        {
                            NewXCoordinate = newXCoordinate,
                            NewYCoordinate = newYCoordinate
                        };
                    
                        break;
                    }
                }
            }
            
            return newCoordinates;
        }

        /// <summary>
        /// Finds animals around by type.
        /// </summary>
        /// <param name="animalTarget">Animal target.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <returns>Animals around by type.</returns>
        protected List<T> FindAnimalsAroundByType<T>(Animal animalTarget, List<Animal> allAnimals) where T : Animal
        {
            IEnumerable<Animal> animalsAround = Helper.LookAround(animalTarget, allAnimals, animalTarget.Vision);

            return animalsAround.Where(animal => typeof(T).IsAssignableTo(animal.GetType()) && !animal.IsDead).Select(animal => (T)animal).ToList();
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
