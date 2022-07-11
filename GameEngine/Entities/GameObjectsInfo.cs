namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains properties for creating instants in the user inteface.
    /// </summary>
    public class GameObjectsInfo
    {
        /// <summary>
        /// Item's specie.
        /// </summary>
        public string Specie { get; set; }

        /// <summary>
        /// Symbol's color.
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Item's type.
        /// </summary>
        public Type Type { get; set; }
    }
}
