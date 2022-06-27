using GameEngine.Entities;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains logic to manage aniimals' pairs.
    /// </summary>
    public interface IPairManager
    {
        /// <summary>
        /// Pair.
        /// </summary>
        Pair? Pair { get; }

        /// <summary>
        /// Creates new pair for current animal, searching opposite free animal near by this animal. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        void CreatePair(List<Animal> animals);

        /// <summary>
        /// Checks pair for existance, and if it is true, then animal can give birth new animal after 3 consecutive rounds. 
        /// </summary>
        /// <param name="pairs">Pairs.</param>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="childs">Childs.</param>
        void CheckPairForExistence(List<Pair> pairToDestroy, Board board, List<Animal> animals, List<Animal> childs);

        /// <summary>
        /// Removes pairs that end their existence. 
        /// </summary>
        void RemoveNonExistsPairs(List<Pair> pairs);
    }
}
