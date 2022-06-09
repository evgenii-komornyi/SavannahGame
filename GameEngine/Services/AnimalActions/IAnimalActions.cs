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
        void AddAnimal(IAnimal newAnimal, List<IAnimal> animals, string[,] board);

        /// <summary>
        /// Moves animals on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        void Move(Board board, List<IAnimal> animals);
        
        /// <summary>
        /// Removes all dead animals. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        void Die(List<IAnimal> animals);
    }
}