using GameEngine.Entities;
using GameEngine.Interfaces;

namespace GameEngine.Helpers
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
        /// <param name="gameObjects">Game objects.</param>
        /// <returns>Is cell occupied.</returns>
        public static bool IsCellOccupied(int newXPosition, int newYPosition, List<IItem> gameObjects)
        {
            return NearByObjects(newXPosition, newYPosition, gameObjects).Any();
        }

        /// <summary>
        /// Looks around board from object based on it vision.
        /// </summary>
        /// <param name="currentObject">Current object.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="vision">How many cells objects can sense other objects.</param>
        /// <returns>Enumerable list with nearby objects.</returns>
        public static IEnumerable<T> LookAround<T>(T currentObject, List<T> gameObjects, int vision = 1) where T : IItem
        {
            return from objectForSearch in gameObjects
                where ObjectsAreInRange<T>(currentObject, objectForSearch, vision)
                select objectForSearch;
        }

        /// <summary>
        /// Finds objects around by type.
        /// </summary>
        /// <param name="objectsAround">Objects around.</param>
        /// <returns>Objects around by type.</returns>
        public static List<T> FindObjectsAroundByType<T>(List<Animal> objectsAround) where T : Animal
        {
            return objectsAround
                .Where(animal => typeof(T).IsAssignableFrom(animal.GetType()) && animal.IsActive && animal.IsVisible)
                .Select(animal => (T)animal)
                .ToList();
        }

        

        /// <summary>
        /// Calculates square distance by Pythagorian theorem.
        /// </summary>
        /// <param name="pointsCoordinates">Points coordinates.</param>
        /// <returns>Square distance.</returns>
        private static double CalculateSquareDistanceByPythagoras(PointsCoordinates pointsCoordinates) => 
            Math.Pow(pointsCoordinates.FirstXPoint - pointsCoordinates.SecondXPoint, 2) +
            Math.Pow(pointsCoordinates.FirstYPoint - pointsCoordinates.SecondYPoint, 2);
    
        /// <summary>
        /// Finds the nearest object to carnivore's position.
        /// </summary>
        /// <param name="objectsAround">Objects around.</param>
        /// <param name="carnivore">Carnivore.</param>
        /// <returns>Nearest object to hunt.</returns>
        public static T? FindNearestObject<T>(List<T> objectsAround, Carnivore carnivore) where T : IItem
        {
            T? nearestObject = default(T);

            if (objectsAround.Count != 0)
            {
                nearestObject = CalculateMinDistanceToObject(objectsAround, carnivore);
            }

            return nearestObject;
        }

        /// <summary>
        /// Calculates minimal distance to the food by free cells.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        /// <param name="food">Food.</param>
        /// <returns>Index of minimal distance cell.</returns>
        public static int CalculateMinDistanceToFoodByFreeCells(List<NewObjectCoordinates> freeCells, Animal food)
        {
            double distance;
            int counter = 0;
            int nearestFreeCellIndex = 0;
            double minDistance = Double.MaxValue;

            foreach (var cell in freeCells)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = cell.NewXCoordinate,
                    SecondXPoint = food.CoordinateX,
                    FirstYPoint = cell.NewYCoordinate,
                    SecondYPoint = food.CoordinateY
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
        /// Calculates minimal distance to the food among objects in the carnivore's vision by Pythagorian theorem. 
        /// </summary>
        /// <param name="objectsAround">Objects around.</param>
        /// <param name="carnivore">Carnivore.</param>
        /// <returns>Nearest food to carnivore.</returns>
        private static T? CalculateMinDistanceToObject<T>(List<T> objectsAround, Carnivore carnivore) where T : IItem
        {
            double distance;
            int counter = 0;
            int nearestFoodIndex = 0;
            double minFoodDistance = Double.MaxValue;

            foreach (var objectAround in objectsAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = carnivore.CoordinateX,
                    SecondXPoint = objectAround.CoordinateX,
                    FirstYPoint = carnivore.CoordinateY,
                    SecondYPoint = objectAround.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minFoodDistance > distance)
                {
                    minFoodDistance = distance;
                    nearestFoodIndex = counter;
                }
                counter++;
            }

            return objectsAround[nearestFoodIndex];
        }

        /// <summary>
        /// Checks the food is far from the carnivore.
        /// </summary>
        /// <param name="food">Far food.</param>
        /// <param name="carnivore">Carnivore.</param>
        /// <returns>Is food far.</returns>
        public static bool IsFoodFar(Animal food, Carnivore carnivore) => 
            Math.Abs(carnivore.CoordinateX - food.CoordinateX) > 1 ||
            Math.Abs(carnivore.CoordinateY - food.CoordinateY) > 1;

        /// <summary>
        /// Returns all objects in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentObject">Current object.</param>
        /// <param name="objectForSearch">Object for search.</param>
        /// <param name="vision">How many cells object can sense other objects.</param>
        /// <returns>Are the objects in the range of vision, excluding itself.</returns>
        private static bool ObjectsAreInRange<T>(T currentObject, T objectForSearch, int vision) where T : IItem
        {
            bool isOwnPosition = currentObject.CoordinateX == objectForSearch.CoordinateX &&
                                 currentObject.CoordinateY == objectForSearch.CoordinateY;
            return !isOwnPosition &&
                objectForSearch.CoordinateX >= currentObject.CoordinateX - vision &&
                objectForSearch.CoordinateX <= currentObject.CoordinateX + vision &&
                objectForSearch.CoordinateY >= currentObject.CoordinateY - vision &&
                objectForSearch.CoordinateY <= currentObject.CoordinateY + vision;
        }

        /// <summary>
        /// Checks nearby game objects around object.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <returns>Enumerable list of found game objects around.</returns>
        private static IEnumerable<IItem> NearByObjects(int newXPosition, int newYPosition, List<IItem> gameObjects)
        {
            return from gameObject in gameObjects
                   where CheckForNearByObjects(newXPosition, newYPosition, gameObject)
                   select gameObject;
        }

        /// <summary>
        /// Checks for nearby game objects.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="gameObject">Game object.</param>
        /// <returns>Is object found another object around it.</returns>
        private static bool CheckForNearByObjects(int newXPosition, int newYPosition, IItem gameObject)
        {
            return gameObject.CoordinateX.Equals(newXPosition) &&
                         gameObject.CoordinateY.Equals(newYPosition);
        }

        /// <summary>
        /// Finds nearest carnivore to the animal.
        /// </summary>
        /// <param name="carnivoresAround">Carnivores around.</param>
        /// <param name="animal">Animal.</param>
        /// <returns>Nearest carnivore.</returns>
        public static Carnivore? FindNearestCarnivore(List<Carnivore> carnivoresAround, Animal animal
            )
        {
            Carnivore? nearestCarnivore = null;

            if (carnivoresAround.Count != 0)
            {
                nearestCarnivore = CalculateMinDistanceToCarnivore(carnivoresAround, animal);
            }

            return nearestCarnivore;
        }

        /// <summary>
        /// Calculates minimal distance to the carnivore.
        /// </summary>
        /// <param name="carnivoresAround">Carnivores around.</param>
        /// <param name="animal">Animal.</param>
        /// <returns>Nearest carnivore to the animal.</returns>
        private static Carnivore? CalculateMinDistanceToCarnivore(List<Carnivore> carnivoresAround, Animal animal)
        {
            double nearestCarnivoreDistance;
            int counter = 0;
            int nearestCarnivoreIndex = 0;
            double minCarnivoreDistance = Double.MaxValue;

            foreach (var carnivore in carnivoresAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = animal.CoordinateX,
                    SecondXPoint = carnivore.CoordinateX,
                    FirstYPoint = animal.CoordinateY,
                    SecondYPoint = carnivore.CoordinateY
                };

                nearestCarnivoreDistance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minCarnivoreDistance > nearestCarnivoreDistance)
                {
                    minCarnivoreDistance = nearestCarnivoreDistance;
                    nearestCarnivoreIndex = counter;
                }
                counter++;
            }

            return carnivoresAround[nearestCarnivoreIndex];
        }

        /// <summary>
        /// Checks the nearest hunter.
        /// </summary>
        /// <param name="nearestHunter">Nearest hunter.</param>
        /// <param name="animal">Animal.</param>
        /// <returns>Is hunter near.</returns>
        public static bool IsHunterNear(IHunter nearestHunter, Animal animal) =>
            Math.Abs(animal.CoordinateX - nearestHunter.CoordinateX) < 2 ||
            Math.Abs(animal.CoordinateY - nearestHunter.CoordinateY) < 2;

        /// <summary>
        /// Calculates maximal distance to the carnivore by free cells.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        /// <param name="carnivore">Carnivore.</param>
        /// <returns>Index of maximal distance cell.</returns>
        public static int CalculateMaxDistanceFromCarnivoreByFreeCells(List<NewObjectCoordinates> freeCells, Carnivore carnivore)
        {
            double distance;
            int counter = 0;
            int farthestFreeCellIndex = 0;
            double maxDistance = 0;

            foreach (var cell in freeCells)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = cell.NewXCoordinate,
                    SecondXPoint = carnivore.CoordinateX,
                    FirstYPoint = cell.NewYCoordinate,
                    SecondYPoint = carnivore.CoordinateY
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
        /// Checks object is active.  
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <returns>Is object active.</returns>
        public static bool IsObjectActive(IItem gameObject)
        {
            return gameObject.IsActive;
        }

        /// <summary>
        /// Calculates a new position based on the free cells around object.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="gameObject">Game object.</param>
        /// <returns>Free cells.</returns>
        public static List<NewObjectCoordinates> CalculateCorrectPosition(Board board, List<IItem> gameObjects, IItem gameObject)
        {
            List<NewObjectCoordinates> freeCellsToMove = new List<NewObjectCoordinates>();
            NewObjectCoordinates newCoordinates;

            for (int newXCoordinate = gameObject.CoordinateX - 1; newXCoordinate <= gameObject.CoordinateX + 1; newXCoordinate++)
            {
                for (int newYCoordinate = gameObject.CoordinateY - 1; newYCoordinate <= gameObject.CoordinateY + 1; newYCoordinate++)
                {
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !IsCellOccupied(newXCoordinate, newYCoordinate, gameObjects))
                    {
                        newCoordinates = new NewObjectCoordinates
                        {
                            NewXCoordinate = newXCoordinate,
                            NewYCoordinate = newYCoordinate
                        };

                        freeCellsToMove.Add(newCoordinates);
                    }
                }
            }
            newCoordinates = new NewObjectCoordinates
            {
                NewXCoordinate = gameObject.CoordinateX,
                NewYCoordinate = gameObject.CoordinateY
            };
            freeCellsToMove.Add(newCoordinates);

            return freeCellsToMove;
        }
    }
}
