using AdditionalLibrary;
using AnimalsBehaviour;

namespace HelperLibrary
{
    /// <summary>
    /// The class contains common methods.
    /// </summary>
    public class Helper
    {
        public static Random random = new Random();

        /// <summary>
        /// Checks whether cell is occupied.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>Is cell occupied.</returns>
        public static bool IsCellOccupied(int newXPosition, int newYPosition, List<Animal> animals)
        {
            return NearByAnimals(newXPosition, newYPosition, animals).Any();
        }

        /// <summary>
        /// Looks around board from animal based on it vision.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <param name="vision">How many cells animal can sense other animals.</param>
        /// <returns>Enumerable list with nearby animals.</returns>
        public static IEnumerable<Animal> LookAround(Animal currentAnimal, IEnumerable<Animal> allAnimals, int vision = 1)
        {
            return from animalForSearch in allAnimals
                   where AnimalsAreInRange(currentAnimal, animalForSearch, vision)
                   select animalForSearch;
        }

        /// <summary>
        /// Finds animals around by type.
        /// </summary>
        /// <param name="animalsAround">Animals around.</param>
        /// <returns>Animals around by type.</returns>
        public static List<T> FindAnimalsAroundByType<T>(List<Animal> animalsAround) where T : Animal
        {
            return animalsAround.Where(animal => typeof(T).IsAssignableFrom(animal.GetType()) && !animal.IsDead).Select(animal => (T)animal).ToList();
        }

        

        /// <summary>
        /// Calculates square distance by Pythagorian theorem.
        /// </summary>
        /// <param name="pointsCoordinates">Points coordinates.</param>
        /// <returns>Square distance.</returns>
        private static double CalculateSquareDistanceByPythagoras(PointsCoordinates pointsCoordinates)
        {
            return Math.Pow(pointsCoordinates.FirstXPoint - pointsCoordinates.SecondXPoint, 2) +
                Math.Pow(pointsCoordinates.FirstYPoint - pointsCoordinates.SecondYPoint, 2);
        }

        /// <summary>
        /// Finds the nearest prey to predator's position.
        /// </summary>
        /// <param name="preysAround">Preys around.</param>
        /// <param name="predator">Predator.</param>
        /// <returns>Nearest prey to hunt.</returns>
        public static IPrey? FindNearestPrey(List<IPrey> preysAround, Predator predator)
        {
            IPrey? nearestPrey = null;

            if (preysAround.Count != 0)
            {
                nearestPrey = CalculateMinDistanceToPrey(preysAround, predator);
            }

            return nearestPrey;
        }

        /// <summary>
        /// Calculates minimal distance to the prey by free cells.
        /// </summary>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        /// <param name="prey">Prey.</param>
        /// <returns>Index of minimal distance cell.</returns>
        public static int CalculateMinDistanceToPreyByFreeCells(List<NewAnimalCoordinates> freeCellsToMove, IPrey prey)
        {
            double distance;
            int counter = 0;
            int nearestFreeCellIndex = 0;
            double minDistance = Double.MaxValue;

            foreach (var cell in freeCellsToMove)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = cell.NewXCoordinate,
                    SecondXPoint = prey.CoordinateX,
                    FirstYPoint = cell.NewYCoordinate,
                    SecondYPoint = prey.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestFreeCellIndex = counter;
                }
                counter++;
            }

            return nearestFreeCellIndex;
        }

        /// <summary>
        /// Calculates minimal distance to the prey among preys in the predator's vision by Pythagoryan theorem. 
        /// </summary>
        /// <param name="preysAround">Preys are around.</param>
        /// <param name="predator">Predator.</param>
        /// <returns>Nearest prey to predator.</returns>
        private static IPrey? CalculateMinDistanceToPrey(List<IPrey> preysAround, Predator predator)
        {
            double distance;
            int counter = 0;
            int nearestPreyIndex = 0;
            double minPreyDistance = Double.MaxValue;

            foreach (var prey in preysAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = predator.CoordinateX,
                    SecondXPoint = prey.CoordinateX,
                    FirstYPoint = predator.CoordinateY,
                    SecondYPoint = prey.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minPreyDistance > distance)
                {
                    minPreyDistance = distance;
                    nearestPreyIndex = counter;
                }
                counter++;
            }

            return preysAround[nearestPreyIndex];
        }

        /// <summary>
        /// Checks the prey far the predator.
        /// </summary>
        /// <param name="preyToHunt">Far prey to hunt.</param>
        /// <param name="predator">Predator.</param>
        /// <returns>Is prey far.</returns>
        public static bool IsPreyFar(IPrey preyToHunt, Predator predator)
        {
            return Math.Abs(predator.CoordinateX - preyToHunt.CoordinateX) > 1 ||
                   Math.Abs(predator.CoordinateY - preyToHunt.CoordinateY) > 1;
        }

        /// <summary>
        /// Returns all animals in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="animalForSearch">Animal for search.</param>
        /// <param name="vision">How many cells animal can sense other animals.</param>
        /// <returns>Are the animals in the range of vision, excluding itself.</returns>
        private static bool AnimalsAreInRange(Animal currentAnimal, Animal animalForSearch, int vision)
        {
            bool isOwnPosition = currentAnimal.CoordinateX == animalForSearch.CoordinateX &&
                                 currentAnimal.CoordinateY == animalForSearch.CoordinateY;
            return !isOwnPosition &&
                    animalForSearch.CoordinateX >= currentAnimal.CoordinateX - vision &&
                    animalForSearch.CoordinateX <= currentAnimal.CoordinateX + vision &&
                    animalForSearch.CoordinateY >= currentAnimal.CoordinateY - vision &&
                    animalForSearch.CoordinateY <= currentAnimal.CoordinateY + vision;
        }

        /// <summary>
        /// Checks nearby animals around animal.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>Enumerable list of found animals around.</returns>
        private static IEnumerable<Animal> NearByAnimals(int newXPosition, int newYPosition, List<Animal> animals)
        {
            return from animal in animals
                   where CheckForNearByAnimals(newXPosition, newYPosition, animal)
                   select animal;
        }

        /// <summary>
        /// Checks for nearby animals.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animal">Animal.</param>
        /// <returns>Is animal found another animal around it.</returns>
        private static bool CheckForNearByAnimals(int newXPosition, int newYPosition, Animal animal)
        {
            return animal.CoordinateX.Equals(newXPosition) &&
                         animal.CoordinateY.Equals(newYPosition);
        }

        /// <summary>
        /// Finds nearest predator to the prey.
        /// </summary>
        /// <param name="predatorsAround">Predators around.</param>
        /// <param name="prey">Prey.</param>
        /// <returns>Nearest predator.</returns>
        public static Predator? FindNearestPredator(List<Predator> predatorsAround, IPrey prey)
        {
            Predator? nearestPredator = null;

            if (predatorsAround.Count != 0)
            {
                nearestPredator = CalculateMinDistanceToPredator(predatorsAround, prey);
            }

            return nearestPredator;
        }

        /// <summary>
        /// Calculates minimal distance to the predator.
        /// </summary>
        /// <param name="predatorsAround">Predators around.</param>
        /// <param name="prey">Prey.</param>
        /// <returns>Nearest predator to the prey.</returns>
        private static Predator? CalculateMinDistanceToPredator(List<Predator> predatorsAround, IPrey prey)
        {
            double nearestPredatorDistance;
            int counter = 0;
            int nearestPredatorIndex = 0;
            double minPredatorDistance = Double.MaxValue;

            foreach (var predator in predatorsAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = prey.CoordinateX,
                    SecondXPoint = predator.CoordinateX,
                    FirstYPoint = prey.CoordinateY,
                    SecondYPoint = predator.CoordinateY
                };

                nearestPredatorDistance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minPredatorDistance > nearestPredatorDistance)
                {
                    minPredatorDistance = nearestPredatorDistance;
                    nearestPredatorIndex = counter;
                }
                counter++;
            }

            return predatorsAround[nearestPredatorIndex];
        }

        /// <summary>
        /// Checks the nearest predator to prey.
        /// </summary>
        /// <param name="nearestPredator">Nearest predator.</param>
        /// <param name="prey">Prey.</param>
        /// <returns>Is predator near.</returns>
        public static bool IsPredatorNear(Predator nearestPredator, Animal prey)
        {
            return Math.Abs(prey.CoordinateX - nearestPredator.CoordinateX) < 2 ||
                   Math.Abs(prey.CoordinateY - nearestPredator.CoordinateY) < 2;
        }

        /// <summary>
        /// Calculates maximal distance to the predator by free cells.
        /// </summary>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        /// <param name="predator">Predator.</param>
        /// <returns>Index of maximal distance cell.</returns>
        public static int CalculateMaxDistanceFromPredatorByFreeCells(List<NewAnimalCoordinates> freeCellsToMove, Predator predator)
        {
            double distance;
            int counter = 0;
            int farthestFreeCellIndex = 0;
            double maxDistance = 0;

            foreach (var cell in freeCellsToMove)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = cell.NewXCoordinate,
                    SecondXPoint = predator.CoordinateX,
                    FirstYPoint = cell.NewYCoordinate,
                    SecondYPoint = predator.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (maxDistance < distance)
                {
                    maxDistance = distance;
                    farthestFreeCellIndex = counter;
                }
                counter++;
            }

            return farthestFreeCellIndex;
        }

        /// <summary>
        /// Checks dead animal.  
        /// </summary>
        /// <param name="animal">Animal.</param>
        /// <returns>Is animal dead.</returns>
        public static bool IsAnimalDead(Animal animal)
        {
            return animal.IsDead;
        }
    }
}
