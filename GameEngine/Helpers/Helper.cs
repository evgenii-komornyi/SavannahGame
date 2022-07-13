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
        /// <param name="gameItems">Game items.</param>
        /// <returns>Is cell occupied.</returns>
        public static bool IsCellOccupied(int newXPosition, int newYPosition, List<IItem> gameItems)
        {
            return NearByItems(newXPosition, newYPosition, gameItems).Any();
        }

        /// <summary>
        /// Looks around board from item based on it vision.
        /// </summary>
        /// <param name="currentItem">Current item.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="vision">How many cells items can sense other items.</param>
        /// <returns>Enumerable list with nearby items.</returns>
        public static IEnumerable<T> LookAround<T>(T currentItem, List<T> gameItems, int vision = 1) where T : IItem
        {
            return from itemForSearch in gameItems
                where ItemsAreInRange<T>(currentItem, itemForSearch, vision)
                select itemForSearch;
        }

        /// <summary>
        /// Finds items around by type.
        /// </summary>
        /// <param name="animalsAround">Animals around.</param>
        /// <returns>Items around by type.</returns>
        public static List<T> FindItemsAroundByType<T>(List<Animal> animalsAround) where T : Animal
        {
            return animalsAround
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
        /// Finds the nearest item to carnivore's position.
        /// </summary>
        /// <param name="itemsAround">Items around.</param>
        /// <param name="carnivore">Carnivore.</param>
        /// <returns>Nearest item to hunt.</returns>
        public static T? FindNearestItem<T>(List<T> itemsAround, Carnivore carnivore) where T : IItem
        {
            T? nearestItem = default(T);

            if (itemsAround.Count != 0)
            {
                nearestItem = CalculateMinDistanceToItem(itemsAround, carnivore);
            }

            return nearestItem;
        }

        /// <summary>
        /// Calculates minimal distance to the food by free cells.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        /// <param name="food">Food.</param>
        /// <returns>Index of minimal distance cell.</returns>
        public static int CalculateMinDistanceToFoodByFreeCells(List<NewItemCoordinates> freeCells, Animal food)
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
        /// Calculates minimal distance to the food among items in the carnivore's vision by Pythagorian theorem. 
        /// </summary>
        /// <param name="itemsAround">Items around.</param>
        /// <param name="carnivore">Carnivore.</param>
        /// <returns>Nearest food to carnivore.</returns>
        private static T? CalculateMinDistanceToItem<T>(List<T> itemsAround, Carnivore carnivore) where T : IItem
        {
            double distance;
            int counter = 0;
            int nearestFoodIndex = 0;
            double minFoodDistance = Double.MaxValue;

            foreach (var itemAround in itemsAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = carnivore.CoordinateX,
                    SecondXPoint = itemAround.CoordinateX,
                    FirstYPoint = carnivore.CoordinateY,
                    SecondYPoint = itemAround.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minFoodDistance > distance)
                {
                    minFoodDistance = distance;
                    nearestFoodIndex = counter;
                }
                counter++;
            }

            return itemsAround[nearestFoodIndex];
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
        /// Returns all items in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentItem">Current item.</param>
        /// <param name="itemForSearch">Item for search.</param>
        /// <param name="vision">How many cells item can sense other items.</param>
        /// <returns>Are the items in the range of vision, excluding itself.</returns>
        private static bool ItemsAreInRange<T>(T currentItem, T itemForSearch, int vision) where T : IItem
        {
            bool isOwnPosition = currentItem.CoordinateX == itemForSearch.CoordinateX &&
                                 currentItem.CoordinateY == itemForSearch.CoordinateY;
            return !isOwnPosition &&
                itemForSearch.CoordinateX >= currentItem.CoordinateX - vision &&
                itemForSearch.CoordinateX <= currentItem.CoordinateX + vision &&
                itemForSearch.CoordinateY >= currentItem.CoordinateY - vision &&
                itemForSearch.CoordinateY <= currentItem.CoordinateY + vision;
        }

        /// <summary>
        /// Checks nearby game items around item.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="gameItems">Game items.</param>
        /// <returns>Enumerable list of found game items around.</returns>
        private static IEnumerable<IItem> NearByItems(int newXPosition, int newYPosition, List<IItem> gameItems)
        {
            return from gameItem in gameItems
                   where CheckForNearByItems(newXPosition, newYPosition, gameItem)
                   select gameItem;
        }

        /// <summary>
        /// Checks for nearby game items.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="gameItem">Game item.</param>
        /// <returns>Is item found another item around it.</returns>
        private static bool CheckForNearByItems(int newXPosition, int newYPosition, IItem gameItem)
        {
            return gameItem.CoordinateX.Equals(newXPosition) &&
                         gameItem.CoordinateY.Equals(newYPosition);
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
        public static int CalculateMaxDistanceFromCarnivoreByFreeCells(List<NewItemCoordinates> freeCells, Carnivore carnivore)
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
        /// Checks item is active.  
        /// </summary>
        /// <param name="gameItem">Game item.</param>
        /// <returns>Is item active.</returns>
        public static bool IsItemActive(IItem gameItem)
        {
            return gameItem.IsActive;
        }

        /// <summary>
        /// Calculates a new position based on the free cells around item.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="gameItem">Game item.</param>
        /// <returns>Free cells.</returns>
        public static List<NewItemCoordinates> CalculateCorrectPosition(Board board, List<IItem> gameItems, IItem gameItem)
        {
            List<NewItemCoordinates> freeCellsToMove = new List<NewItemCoordinates>();
            NewItemCoordinates newCoordinates;

            for (int newXCoordinate = gameItem.CoordinateX - 1; newXCoordinate <= gameItem.CoordinateX + 1; newXCoordinate++)
            {
                for (int newYCoordinate = gameItem.CoordinateY - 1; newYCoordinate <= gameItem.CoordinateY + 1; newYCoordinate++)
                {
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !IsCellOccupied(newXCoordinate, newYCoordinate, gameItems))
                    {
                        newCoordinates = new NewItemCoordinates
                        {
                            NewXCoordinate = newXCoordinate,
                            NewYCoordinate = newYCoordinate
                        };

                        freeCellsToMove.Add(newCoordinates);
                    }
                }
            }
            newCoordinates = new NewItemCoordinates
            {
                NewXCoordinate = gameItem.CoordinateX,
                NewYCoordinate = gameItem.CoordinateY
            };
            freeCellsToMove.Add(newCoordinates);

            return freeCellsToMove;
        }
    }
}
