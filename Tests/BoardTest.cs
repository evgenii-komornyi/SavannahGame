using GameEngine;
using GameEngine.Entities;
using GameEngine.Interfaces;
using Tests.Animals;
using Xunit;

namespace Tests
{
    public class BoardTest
    {
        private Board _board;
        private List<IItem> _items;

        public BoardTest()
        {
            _board = new Board();
            _items = new List<IItem>
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
                    Species = "Test",
                    Letter = ConsoleKey.T,
                    Color = ConsoleColor.White,
                    IsActive = true
                }
            };
        }

        [Fact]
        public void CreateArray_CheckArrayLength_ReturnsHundredFifty()
        {
            // Arrange
            var actual = _board.GameBoard.Length;
            const int ExpectedLength = 150;

            // Assert
            Assert.Equal(ExpectedLength, actual);
        }

        [Fact]
        public void FillBoardWithItems_CheckArrayOnNonEmptyCells_ContainsItemLetterOnPosition()
        {
            // Arrange
            const string expectedLetterAtPosition = "T";
            
            // Act
            _board.FillBoardWithItems(_items);

            // Assert
            Assert.Contains(expectedLetterAtPosition, _board.GameBoard[_items[0].CoordinateX, _items[0].CoordinateY].Letter);
        }

        [Fact]
        public void PrepareBoard_CheckArrayOnEmptyCells_ContainsEmptyCells()
        {
            // Arrange
            const string expectedEmptyCellAtPosition = " ";

            // Act
            _board.PrepareBoard(_items);

            // Assert
            Assert.Contains(expectedEmptyCellAtPosition, _board.GameBoard[5, 5].Letter);
        }
    }
}
