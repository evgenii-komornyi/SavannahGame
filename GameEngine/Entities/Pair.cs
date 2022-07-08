using GameEngine.Entities.Interfaces;

namespace GameEngine.Entities
{
    /// <summary>
    /// Pair entity.
    /// </summary>
    public class Pair
    {
        /// <summary>
        /// First animal in pair.
        /// </summary>
        public IAnimal? FirstAnimal { get; set; }

        /// <summary>
        /// Second animal in pair.
        /// </summary>
        public IAnimal? SecondAnimal { get; set; }

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
        /// Gives birth of child from female animal.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="pair">Pair.</param>
        /// <returns>New animal (child).</returns>
        public Animal GiveBirth(Board board, List<Animal> animals, List<Pair> pair)
        {
            Animal? female = null;

            foreach (var animal in pair)
            {
                if (animal.FirstAnimal.Sex.Equals(AnimalSex.Female))
                {
                    female = (Animal) animal.FirstAnimal;
                }
                else
                {
                    female = (Animal) animal.SecondAnimal;
                }
            }

            return female.GiveBirth(board, animals);
        }
    }
}
