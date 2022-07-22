using GameEngine;
using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Services.Managers;
using Tests.Animals;
using Xunit;

namespace Tests
{
    public class PairManagerTest
    {
        private const int ColumnsCount = 2;
        private const int RowsCount = 2;

        private PairManager _pairManager;
        private AdditionManager _additionManager;
        private List<IItem> _gameItems;
        private List<Pair> _pairs;
        private Board _board;
        private List<IItem> _children;

        public PairManagerTest()
        {
            _board = new Board(ColumnsCount, RowsCount);
            _pairs = new List<Pair>();
            _children = new List<IItem>();
            _gameItems = GenerateItems();
            _additionManager = new AdditionManager();
            _pairManager = new PairManager();
            _board.FillBoardWithItems(_gameItems);
            _board.PrepareBoard(_gameItems);
        }

        private List<IItem> GenerateItems()
        {
            return new List<IItem>
            {
                new TestCarnivore
                {
                    CoordinateX = 0,
                    CoordinateY = 0,
                    Id = 0,
                    Sex = AnimalSex.Female,
                    IsPaired = false,
                    IsVisible = true,
                    Vision = 2,
                    Health = 100,
                    IsActive = true,
                    Specie = "Carnivore",
                    Letter = ConsoleKey.C,
                    Color = ConsoleColor.DarkRed
                },
                new TestCarnivore
                {
                    CoordinateX = 1,
                    CoordinateY = 1,
                    Id = 1,
                    Sex = AnimalSex.Male,
                    IsPaired = false,
                    IsVisible = true,
                    Vision = 2,
                    Health = 100,
                    IsActive = true,
                    Specie = "Carnivore",
                    Letter = ConsoleKey.C,
                    Color = ConsoleColor.DarkRed
                },
            };
        }

        [Fact]
        public void CreatePairAndAddPairToList_CheckTwoNearbyAnimalsIsInPair_ReturnsPairsListCountEqualsOne()
        {
            // Act
            _pairManager.AddPairToList(_pairManager.CreatePair(_gameItems), _pairs);

            // Assert
            Assert.Single(_pairs);
        }

        [Fact]
        public void GiveBirth_CheckIfPairExistsThreeRounds_ReturnsChildrenListCountEqualsOne()
        {
            // Act
            _pairManager.AddPairToList(_pairManager.CreatePair(_gameItems), _pairs);
            _pairManager.CheckPairForExistence(_pairs, _board, _gameItems, _children);
            _pairManager.CheckPairForExistence(_pairs, _board, _gameItems, _children);

            // Assert
            Assert.Single(_children);
        }

        [Fact]
        public void DestroyPair_CheckPairForExistence_ReturnsFalseIsPairExistFlag()
        {
            // Arrange
            const int NewColumnsCount = 5;
            const int NewRowsCount = 5;
            _board = new Board(NewColumnsCount, NewRowsCount);

            // Act
            Pair currentPair = _pairManager.CreatePair(_gameItems);
            _pairManager.AddPairToList(currentPair, _pairs);

            _gameItems[1].CoordinateX = 5;
            _gameItems[1].CoordinateY = 5;

            _pairManager.CheckPairForExistence(_pairs, _board, _gameItems, _children);

            // Assert
            Assert.False(currentPair.IsPairExist);
        }
    }
}
