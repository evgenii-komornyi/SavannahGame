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
        /// Creates new pair for current pairable item, searching opposite free pairable item near by this item. 
        /// </summary>
        /// <param name="pairableItems">Pairable items.</param>
        /// <returns>New pair.</returns>
        public Pair CreatePair(List<IItem> pairableItems)
        {
            Pair newPair = null;
            foreach (var currentAnimal in pairableItems.Cast<Animal>())
            {
                if (!currentAnimal.IsPaired)
                {
                    List<Animal> animalsAround = Helper.LookAround(currentAnimal, pairableItems).Cast<Animal>().ToList();

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
        /// Added pair to list.
        /// </summary>
        /// <param name="newPair">New pair.</param>
        /// <param name="pairs">Pairs.</param>
        public void AddPairToList(Pair newPair, List<Pair> pairs)
        {
            if (newPair != null)
            {
                pairs.Add(newPair);
            }
        }

        /// <summary>
        /// Checks pair for existance, and if it is true, then item can reproduce new item after 3 consecutive rounds. 
        /// </summary>
        /// <param name="pairsToDestroy">Pairs to destroy.</param>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="reproducedItems">Reproduced items.</param>
        public void CheckPairForExistence(List<Pair> pairsToDestroy, Board board, List<IItem> gameItems, List<IItem> reproducedItems)
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
                            IItem? newItem = pair.Reproduce(board, gameItems, pairsToDestroy);
                            if (newItem != null)
                            {
                                reproducedItems.Add(newItem);
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
