using GameEngine.Entities;
using GameEngine.Entities.Interfaces;
using Repository;

namespace GameEngine.Services.PairManager
{
    /// <summary>
    /// The class contains logic to manage aniimals' pairs.
    /// </summary>
    public class PairManager : IPairManager
    {
        /// <summary>
        /// Pair.
        /// </summary>
        public Pair? Pair { get; private set; }

        /// <summary>
        /// Creates new pair for current animal, searching opposite free animal near by this animal. 
        /// </summary>
        /// <param name="animals">Animals.</param>
        public void CreatePair(List<Animal> animals)
        {
            foreach (var currentAnimal in animals)
            {
                if (!currentAnimal.IsPaired)
                {
                    List<IAnimal> animalsAround = Helper.LookAround(currentAnimal, animals).ToList();

                    List<IAnimal> freeOppositeSexAnimalsAround = animalsAround
                        .Where(animal => !animal.Sex.Equals(currentAnimal.Sex) && !animal.IsPaired && animal.GetType() == currentAnimal.GetType())
                        .ToList();

                    IAnimal? animalToPair = freeOppositeSexAnimalsAround.FirstOrDefault();

                    if (animalToPair == null)
                    {
                        return;
                    }

                    Pair newPair = new Pair
                    {
                        FirstAnimal = currentAnimal,
                        SecondAnimal = animalToPair,
                        IsPairExist = true
                    };

                    newPair.FirstAnimal.IsPaired = true;
                    newPair.SecondAnimal.IsPaired = true;
                    Pair = newPair;
                }
            }
        }

        /// <summary>
        /// Checks pair for existance, and if it is true, then animal can give birth new animal after 3 consecutive rounds. 
        /// </summary>
        /// <param name="pairs">Pairs.</param>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <param name="children">Childs.</param>
        public void CheckPairForExistence(List<Pair> pairs, Board board, List<Animal> animals, List<Animal> children)
        {
            foreach (var pair in pairs)
            {
                if (pair != null)
                {
                    if (!IsAnimalStillNear(pair.FirstAnimal, pair.SecondAnimal))
                    {
                        pair.FirstAnimal.IsPaired = false;
                        pair.SecondAnimal.IsPaired = false;
                        pair.IsPairExist = false;
                    }
                    else
                    {
                        pair.RelationshipDuration++;

                        if (pair.RelationshipDuration == ConstantsRepository.RelationshipDuration)
                        {
                            Animal child = pair.GiveBirth(board, animals, pairs);
                            children.Add(child);
                            pair.RelationshipDuration = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks animal is still nearby current animal. 
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="pairAnimal">Animal in pair.</param>
        /// <returns>True if animal is still nearby.</returns>
        private bool IsAnimalStillNear(IAnimal currentAnimal, IAnimal pairAnimal)
        {
            return Math.Abs(currentAnimal.CoordinateX - pairAnimal.CoordinateX) <= 1 &&
                   Math.Abs(currentAnimal.CoordinateY - pairAnimal.CoordinateY) <= 1;
        }

        /// <summary>
        /// Removes pairs that end their existence. 
        /// </summary>
        public void RemoveNonExistsPairs(List<Pair> pairs)
        {
            if (pairs.Count != 0)
            {
                IEnumerable<Pair> doesNotExistingPairs = CheckForExistingPairs(pairs);
                pairs.RemoveAll(currentAnimal => doesNotExistingPairs.Contains(currentAnimal));
            }
        }

        /// <summary>
        /// Checks the dead animals.
        /// </summary>
        /// <param name="pairs">Animals.</param>
        /// <returns>Enumerable list of the dead animals.</returns>
        private IEnumerable<Pair> CheckForExistingPairs(List<Pair> pairs)
        {
            return from currentPair in pairs
                   where !currentPair.IsPairExist
                   select currentPair;
        }
    }
}
