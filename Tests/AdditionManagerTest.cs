using GameEngine;
using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Services.Managers;
using Tests.Animals;
using Xunit;

namespace Tests
{
    public class AdditionManagerTest
    {
        private List<IItem> _gameItems;
        private List<IItem> _childrenItems;
        private Dictionary<ConsoleKey, IItem> _gameItemsInfo;
        private Board _board;
        private AdditionManager _additionManager;

        public AdditionManagerTest()
        {
            _gameItems = new List<IItem>();
            _childrenItems = new List<IItem>
            {
                CreateCarnivore(),
                CreateCarnivore()
            };
            _gameItemsInfo = new Dictionary<ConsoleKey, IItem>();
            IItem carnivore = CreateCarnivore();

            _gameItemsInfo.Add(ConsoleKey.C, new TestCarnivore());

            _board = new Board();
            _additionManager = new AdditionManager();
        }

        [Fact]
        public void AddItemToList_CheckItemsListCount_ReturnsOne()
        {
            // Act
            _additionManager.ProcessAddNewItem(ConsoleKey.C, _gameItemsInfo, _gameItems, _board);

            // Assert
            Assert.Single(_gameItems);
        }

        [Fact]
        public void AddChildrenToList_CheckItemsListCount_ReturnsOne()
        {
            // Arrange
            const int ExpectedListCount = 3;

            // Act
            _additionManager.ProcessAddNewItem(ConsoleKey.C, _gameItemsInfo, _gameItems, _board);
            _additionManager.ProcessChildrenItems(_gameItems, _childrenItems);
            
            // Assert
            Assert.Equal(ExpectedListCount, _gameItems.Count);
        }

        private TestCarnivore CreateCarnivore()
        {
            return new TestCarnivore
            {
                Id = 0,
                Sex = AnimalSex.Female,
                IsPaired = false,
                IsVisible = true,
                Vision = 2,
                Health = 100,
                IsActive = true,
                Species = "Carnivore",
                Letter = ConsoleKey.C,
                Color = ConsoleColor.White
            };
        }
    }
}