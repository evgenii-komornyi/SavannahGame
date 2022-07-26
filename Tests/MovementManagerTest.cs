using GameEngine;
using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Services.Managers;
using Tests.Animals;
using Xunit;

namespace Tests
{
    public class MovementManagerTest
    {
        private const int ColumnsCount = 2;
        private const int RowsCount = 2;

        private Board _board;
        private List<IItem> _gameItems;
        private MovementManager _movementManager;
        private DeletionManager _deletionManager;

        public MovementManagerTest()
        {
            _movementManager = new MovementManager();
            _deletionManager = new DeletionManager();
            _board = new Board(ColumnsCount, RowsCount);
            _gameItems = CreateGameItemsList();
            _board.FillBoardWithItems(_gameItems);
            _board.PrepareBoard(_gameItems);
        }

        private List<IItem> CreateGameItemsList()
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
                    Species = "Carnivore",
                    Letter = ConsoleKey.C,
                    Color = ConsoleColor.DarkRed
                },
                new TestHerbivore
                {
                    CoordinateX = 1,
                    CoordinateY = 1,
                    Id = 1,
                    Sex = AnimalSex.Female,
                    IsPaired = false,
                    IsVisible = true,
                    Vision = 2,
                    Health = 100,
                    IsActive = true,
                    Species = "Herbivore",
                    Letter = ConsoleKey.H,
                    Color = ConsoleColor.Green
                },
            };
        }

        [Fact]
        public void MoveItemOnBoard_ChecksNewItemCoordinates_ReturnsNewCoordinates()
        {
            // Arrange
            int coodrinateXBeforeMove = _gameItems[0].CoordinateX;
            int coodrinateYBeforeMove = _gameItems[0].CoordinateY;
            bool ExpectedDifferenceBetweenCoordinates = Math.Abs(coodrinateXBeforeMove - _gameItems[0].CoordinateX) <= 1 ||
                Math.Abs(coodrinateYBeforeMove - _gameItems[0].CoordinateY) <= 1;

            // Act
            _movementManager.Act(_gameItems, _board);
            
            // Assert
            Assert.True(ExpectedDifferenceBetweenCoordinates);
        }

        [Fact]
        public void EatNearestHerbivoreByCarnivore_ChecksHerbivoresHealthDecrease_ReturnsEightyNineWithAHalf()
        {
            // Arrange
            var animals = _gameItems.Cast<Animal>().ToList();
            const double ExpectedHealthAfterAttack = 89.5;

            // Act
            _movementManager.Act(_gameItems, _board);

            // Assert
            Assert.Equal(ExpectedHealthAfterAttack, animals[1].Health);
        }

        [Fact]
        public void HealCarnivoreAfterKillingHerbivore_ChecksCarnivoreRelocatedToHerbivoresCoordinatesAfterKillingItAndDeleteHerbivoreFromList_ReturnsCarnivoresHealthEqualsHundredAndCoordinatesEqualsOneAndOneAndListCountEqualsOne()
        {
            // Arrange
            var animals = _gameItems.Cast<Animal>().ToList();
            var carnivore = animals[0];
            var herbivore = animals[1];
            herbivore.Health = 5;
            const double ExpectedCarnivoresHealth = 100;
            const double ExpectedCarnivoresCoordinateX = 1;
            const double ExpectedCarnivoresCoordinateY = 1;

            // Act
            _movementManager.Act(_gameItems, _board);
            _deletionManager.RemoveInactiveItems(_gameItems);

            // Assert
            Assert.Equal(ExpectedCarnivoresHealth, carnivore.Health);
            Assert.Equal(ExpectedCarnivoresCoordinateX, carnivore.CoordinateX);
            Assert.Equal(ExpectedCarnivoresCoordinateY, carnivore.CoordinateY);
            Assert.False(herbivore.IsActive);
            Assert.Single(_gameItems);
        }
    }
}
