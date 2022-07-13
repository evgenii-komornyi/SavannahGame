using GameEngine.Helpers;
using GameEngine.Interfaces;

namespace GameEngine.Entities
{
    /// <summary>
    /// Pair entity.
    /// </summary>
    public class Pair : IReproducable
    {
        /// <summary>
        /// First animal in pair.
        /// </summary>
        public Animal? FirstAnimal { get; set; }

        /// <summary>
        /// Second animal in pair.
        /// </summary>
        public Animal? SecondAnimal { get; set; }

        /// <summary>
        /// Relationship duration. 
        /// Indicates how many moves animals in pair do together for birth child.
        /// </summary>
        public int RelationshipDuration { get; set; } = 0;

        /// <summary>
        /// Indicates is pair exists.
        /// </summary>
        public bool IsPairExist { get; set; } = false;

        /// <summary>
        /// Reproduces new child.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="pair">Pair.</param>
        /// <returns>New game item by type.</returns>
        public IItem? Reproduce(Board board, List<IItem> gameItems, List<Pair> pair)
        {
            Animal? female = FindFemale(pair);
            Type femaleType = female.GetType();
            NewItemCoordinates? birthCoordinates = CalculateFreeCellsToBirth(board, gameItems, female);
            
            IItem child = CreateInstanceOfType(femaleType); 
            if (child != null && birthCoordinates != null)
            {
                child.CoordinateX = birthCoordinates.NewXCoordinate;
                child.CoordinateY = birthCoordinates.NewYCoordinate;
            }

            return child;
        }

        /// <summary>
        /// Finds female in the pair.
        /// </summary>
        /// <param name="pair">Pair.</param>
        /// <returns>Female.</returns>
        private Animal FindFemale(List<Pair> pair)
        {
            Animal female = null;

            foreach (var animal in pair)
            {
                if (animal.FirstAnimal.Sex.Equals(AnimalSex.Female))
                {
                    female = animal.FirstAnimal;
                }
                else
                {
                    female = animal.SecondAnimal;
                }
            }

            return female;
        }

        /// <summary>
        /// Creates new instance of animal by type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>New instance (child).</returns>
        private IItem CreateInstanceOfType(Type type) => (IItem)Activator.CreateInstance(type);

        /// <summary>
        /// Calculates first free cell for birth child near female animal.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        /// <returns>New item coordinates for child.</returns>
        private NewItemCoordinates? CalculateFreeCellsToBirth(Board board, List<IItem> gameItems, Animal female)
        {
            NewItemCoordinates? newCoordinates = null;

            for (int newXCoordinate = female.CoordinateX - 1; newXCoordinate <= female.CoordinateX + 1; newXCoordinate++)
            {
                for (int newYCoordinate = female.CoordinateY - 1; newYCoordinate <= female.CoordinateY + 1; newYCoordinate++)
                {
                    if (!board.IsCellOnBoard(newXCoordinate, newYCoordinate, board.GameBoard) &&
                        !Helper.IsCellOccupied(newXCoordinate, newYCoordinate, gameItems))
                    {
                        newCoordinates = new NewItemCoordinates
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
    }
}
