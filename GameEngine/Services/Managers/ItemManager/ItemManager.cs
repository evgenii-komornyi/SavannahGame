using GameEngine.Entities;
using GameEngine.Helpers;
using GameEngine.Interfaces;
using Repository;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains controll of the game items list.
    /// </summary>
    public class ItemManager : IItemManager
    {
        /// <summary>
        /// Adds new item to the list of the items on the current board. 
        /// </summary>
        /// <param name="newGameItem">New game item.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="board">Board.</param>
        public void AddItem(IItem newGameItem, List<IItem> gameItems, Board board)
        {
            List<NewItemCoordinates> freeCells = CalculateFreeCellsToAddItem(gameItems, board);
            if (board.GameBoard.Length > board.GameBoard.Length * ConstantsRepository.HalfOfBoard)
            {
                GenerateItemCoordinates(newGameItem, freeCells, gameItems);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Generates the new game item's coordinates.
        /// </summary>
        /// <param name="newGameItem">New game item.</param>
        /// <param name="freeCells">Free cells.</param>
        /// <param name="gameItems">Game items.</param>
        private void GenerateItemCoordinates(IItem newGameItem, List<NewItemCoordinates> freeCells, List<IItem> gameItems)
        {
            NewItemCoordinates freeCoordinates = freeCells[Helper.random.Next(0, freeCells.Count)];
            newGameItem.CoordinateX = freeCoordinates.NewXCoordinate;
            newGameItem.CoordinateY = freeCoordinates.NewYCoordinate;
            AddItemToList(gameItems, newGameItem);
        }

        /// <summary>
        /// Adds new item to list.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <param name="newGameItem">New game item.</param>
        private void AddItemToList(List<IItem> gameItems, IItem newGameItem)
        {
            gameItems.Add(newGameItem);
        }

        /// <summary>
        /// Adds children items to common game items' list and clear children list.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <param name="childrenItems">Children items.</param>
        public void ProcessChildrenItems(List<IItem> gameItems, List<IItem> childrenItems)
        {
            gameItems.AddRange(childrenItems);
            childrenItems.Clear();
        } 

        /// <summary>
        /// Calculates the free cells on board to add a new item. 
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <param name="board">Board.</param>
        /// <returns>Free cells.</returns>
        private List<NewItemCoordinates> CalculateFreeCellsToAddItem(List<IItem> gameItems, Board board)
        {
            List<NewItemCoordinates> freeCells = new List<NewItemCoordinates>();

            for (int newXCoordinate = 0; newXCoordinate < board.GameBoard.GetLength(0); newXCoordinate++)
            {
                for (int newYCoordinate = 0; newYCoordinate < board.GameBoard.GetLength(1); newYCoordinate++)
                {
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsCellOccupied(newXCoordinate, newYCoordinate, gameItems))
                    {
                        NewItemCoordinates newCoordinates = new NewItemCoordinates
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
        /// Removes all inactive items from board. 
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        public void RemoveInactiveItems(List<IItem> gameItems)
        {
            IEnumerable<IItem> inactiveItems = CheckForActive(gameItems);
            gameItems.RemoveAll(currentItem => inactiveItems.Contains(currentItem));
        }

        /// <summary>
        /// Checks the active items.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <returns>Enumerable list of the inactive items.</returns>
        private IEnumerable<IItem> CheckForActive(List<IItem> gameItems)
        {
            return from currentItem in gameItems
                   where !Helper.IsItemActive(currentItem)
                   select currentItem;
        }
    }
}