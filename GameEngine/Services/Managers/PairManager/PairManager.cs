using GameEngine.Entities;
using GameEngine.Helpers;
using GameEngine.Interfaces;
using Repository;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains logic to manage animals' pairs.
    /// </summary>
    public class PairManager : IPairManager
    {
        /// <summary>
        /// Creates new pair for current pairable object, searching opposite free pairable object near by this object. 
        /// </summary>
        /// <param name="pairables">Pairables.</param>
        /// <returns>New pair.</returns>
        public Pair CreatePair(List<IItem> pairables)
        {
            Pair newPair = null;
            foreach (var currentAnimal in pairables.Cast<Animal>())
            {
                if (!currentAnimal.IsPaired)
                {
                    List<Animal> animalsAround = Helper.LookAround(currentAnimal, pairables).Cast<Animal>().ToList();

                    List<Animal> freeOppositeSexAnimalsAround = animalsAround
                        .Where(animal => !animal.Sex.Equals(currentAnimal.Sex) && !animal.IsPaired && animal.GetType() == currentAnimal.GetType())
                        .ToList();

                    Animal? animalToPair = freeOppositeSexAnimalsAround.FirstOrDefault();

                    if (animalToPair != null)
                    {
                        newPair = new Pair
                        {
                            FirstAnimal = currentAnimal,
                            SecondAnimal = animalToPair,
                            IsPairExist = true
                        };

                        newPair.FirstAnimal.IsPaired = true;
                        newPair.SecondAnimal.IsPaired = true;
                    }
                }
            }

            return newPair;
        }

        /// <summary>
        /// Checks pair for existance, and if it is true, then object can reproduce new object after 3 consecutive rounds. 
        /// </summary>
        /// <param name="pairsToDestroy">Pairs to destroy.</param>
        /// <param name="board">Board.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="reproducedObjects">Reproduced objects.</param>
        public void CheckPairForExistence(List<Pair> pairsToDestroy, Board board, List<IItem> gameObjects, List<IItem> reproducedObjects)
        {
            foreach (var pair in pairsToDestroy)
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
                            IItem? newItem = pair.Reproduce(board, gameObjects, pairsToDestroy);
                            if (newItem != null)
                            {
                                reproducedObjects.Add(newItem);
                            }
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
        private bool IsAnimalStillNear(Animal currentAnimal, Animal pairAnimal)
        {
            return Math.Abs(currentAnimal.CoordinateX - pairAnimal.CoordinateX) <= 1 &&
                   Math.Abs(currentAnimal.CoordinateY - pairAnimal.CoordinateY) <= 1;
        }

        /// <summary>
        /// Removes pairs that end their existence. 
        /// </summary>
        public void RemoveNotExistingPairs(List<Pair> pairs)
        {
            IEnumerable<Pair> doesNotExistingPairs = CheckForExistingPairs(pairs);
            pairs.RemoveAll(currentAnimal => doesNotExistingPairs.Contains(currentAnimal));
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
