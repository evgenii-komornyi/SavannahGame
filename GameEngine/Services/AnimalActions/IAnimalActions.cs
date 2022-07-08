using GameEngine.Entities;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains all common logic of the animals' behaviour.
    /// </summary>
    public interface IAnimalActions
    {
        /// <summary>
        /// Adds new animal to the list of the animals on the current board. 
        /// </summary>
        /// <param name="newAnimal">New animal.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="board">Board.</param>
        void AddAnimal(Animal newAnimal, List<Animal> animals, Board board);

        /// <summary>
        /// Adds borned childs to common animals' list.
        /// </summary>
        /// <param name="animals">Animals.</param>
        /// <param name="children">Children.</param>
        void AddChildren(List<Animal> animals, List<Animal> children);

        /// <summary>
        /// Moves animals on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        void Move(Board board, List<Animal> animals);

        /// <summary>
        /// Removes all dead animals from board. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        void RemoveDeadAnimals(List<Animal> animals);
    }
}