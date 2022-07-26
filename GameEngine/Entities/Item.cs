using GameEngine.Interfaces;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains properties and behaviour of the game items. 
    /// </summary>
    public abstract class Item : IItem
    {
        /// <summary>
        /// Item id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Coordinate X.
        /// </summary>
        public int CoordinateX { get; set; }

        /// <summary>
        /// Coordinate Y.
        /// </summary>
        public int CoordinateY { get; set; }

        /// <summary>
        /// Indicate if item is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Indicate if item is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Symbol for displaying item on a game field.
        /// </summary>
        public ConsoleKey Letter { get; set; }

        /// <summary>
        /// Symbol's color.
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Item's specie.
        /// </summary>
        public string Species { get; set; }

        private static int id = 0;

        /// <summary>
        /// Generates new id.
        /// </summary>
        /// <returns>Incremented id.</returns>
        protected int GenerateId() => id++;

        /// <summary>
        /// Makes item inactive.
        /// </summary>
        public void Die() => IsActive = false;
    }
}
