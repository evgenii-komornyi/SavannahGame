using GameEngine.Entities;

namespace GameEngine.Interfaces
{
    /// <summary>
    /// The class contains properties for pairable animals.
    /// </summary>
    public interface IPairable 
    {
        /// <summary>
        /// Animal's sex.
        /// </summary>
        AnimalSex Sex { get; set; }

        /// <summary>
        /// Indicates if animal is in pair.
        /// </summary>
        bool IsPaired { get; set; }
    }
}
