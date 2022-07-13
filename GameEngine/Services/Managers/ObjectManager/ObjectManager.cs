using GameEngine.Entities;
using GameEngine.Helpers;
using GameEngine.Interfaces;
using Repository;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains controll of the game objects list.
    /// </summary>
    public class ObjectManager : IObjectManager
    {
        /// <summary>
        /// Adds new object to the list of the objects on the current board. 
        /// </summary>
        /// <param name="newGameObject">New game object.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="board">Board.</param>
        public void AddObject(IItem newGameObject, List<IItem> gameObjects, Board board)
        {
            List<NewObjectCoordinates> freeCells = CalculateFreeCellsToAddObject(gameObjects, board);
            if (board.GameBoard.Length > board.GameBoard.Length * ConstantsRepository.HalfOfBoard)
            {
                GenerateObjectCoordinates(newGameObject, freeCells, gameObjects);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Generates the new game object's coordinates.
        /// </summary>
        /// <param name="newGameObject">New game object.</param>
        /// <param name="freeCells">Free cells.</param>
        /// <param name="gameObjects">Game objects.</param>
        private void GenerateObjectCoordinates(IItem newGameObject, List<NewObjectCoordinates> freeCells, List<IItem> gameObjects)
        {
            NewObjectCoordinates freeCoordinates = freeCells[Helper.random.Next(0, freeCells.Count)];
            newGameObject.CoordinateX = freeCoordinates.NewXCoordinate;
            newGameObject.CoordinateY = freeCoordinates.NewYCoordinate;
            AddObjectToList(gameObjects, newGameObject);
        }

        /// <summary>
        /// Adds new object to list.
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="newGameObject">New game object.</param>
        private void AddObjectToList(List<IItem> gameObjects, IItem newGameObject)
        {
            gameObjects.Add(newGameObject);
        }

        /// <summary>
        /// Adds children objects to common game objects' list and clear children list.
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="childrenObjects">Children objects.</param>
        public void ProcessChildrenObjects(List<IItem> gameObjects, List<IItem> childrenObjects)
        {
            gameObjects.AddRange(childrenObjects);
            childrenObjects.Clear();
        } 

        /// <summary>
        /// Calculates the free cells on board to add a new object. 
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="board">Board.</param>
        /// <returns>Free cells.</returns>
        private List<NewObjectCoordinates> CalculateFreeCellsToAddObject(List<IItem> gameObjects, Board board)
        {
            List<NewObjectCoordinates> freeCells = new List<NewObjectCoordinates>();

            for (int newXCoordinate = 0; newXCoordinate < board.GameBoard.GetLength(0); newXCoordinate++)
            {
                for (int newYCoordinate = 0; newYCoordinate < board.GameBoard.GetLength(1); newYCoordinate++)
                {
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsCellOccupied(newXCoordinate, newYCoordinate, gameObjects))
                    {
                        NewObjectCoordinates newCoordinates = new NewObjectCoordinates
                        {
                            NewXCoordinate = newXCoordinate,
                            NewYCoordinate = newYCoordinate
                        };

                        freeCells.Add(newCoordinates);
                    }
                }
            }

            return freeCells;
        }

        /// <summary>
        /// Removes all inactive objects from board. 
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        public void RemoveInactiveObjects(List<IItem> gameObjects)
        {
            IEnumerable<IItem> inactiveObjects = CheckForActive(gameObjects);
            gameObjects.RemoveAll(currentItem => inactiveObjects.Contains(currentItem));
        }

        /// <summary>
        /// Checks the active objects.
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        /// <returns>Enumerable list of the inactive objects.</returns>
        private IEnumerable<IItem> CheckForActive(List<IItem> gameObjects)
        {
            return from currentObject in gameObjects
                   where !Helper.IsObjectActive(currentObject)
                   select currentObject;
        }
    }
}