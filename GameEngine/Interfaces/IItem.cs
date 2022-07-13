namespace GameEngine.Interfaces
{
    /// <summary>
    /// The class contains properties and behaviour of the game items. 
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Item id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Coordinate X.
        /// </summary>
        int CoordinateX { get; set; }

        /// <summary>
        /// Coordinate Y.
        /// </summary>
        int CoordinateY { get; set; }

        /// <summary>
        /// Indicate if item is active.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Indicate if item is visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Symbol for displaying item on a game field.
        /// </summary>
        ConsoleKey Letter { get; set; }

        /// <summary>
        /// Symbol's color.
        /// </summary>
        ConsoleColor Color { get; set; }

        /// <summary>
        /// Item's specie.
        /// </summary>
        string Specie { get; set; }

        /// <summary>
        /// Makes item inactive.
        /// </summary>
        void Die();
    }
}
