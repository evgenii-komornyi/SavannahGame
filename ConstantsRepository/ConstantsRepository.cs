namespace Repository
{
    /// <summary>
    /// The class contains constants.
    /// </summary>
    public class ConstantsRepository
    {
        #region Titles
        public const string GameTitle = "Savannah - Herbivores and Carnivores";
        #endregion

        #region MainMenuCommands 
        public const string ExitButtonDescription = "Press Q, or Escape - Exit game;";
        #endregion

        #region AnimalSettings
        public const string Antilope = "Antilope";
        public const string Lion = "Lion";
        public const string Rabbit = "Rabbit";
        public const string Fox = "Fox";
        public const int LionVision = 2;
        public const double MaxHealth = 100;
        public const int FoxVision = 2;
        public const int AntilopeVision = 2;
        public const int RabbitVision = 2;
        public const double HealthDecreaser = 0.5;
        public const int RelationshipDuration = 2;
        #endregion

        #region BoardSettings
        public const int ColumnsCount = 15;
        public const int RowsCount = 10;
        public const int BorderWidth = ColumnsCount + 3;
        public const int BorderHeight = RowsCount + 1;
        public const string UpLeftCorner = "╔";
        public const string UpRightCorner = "╗";
        public const string DownLeftCorner = "╚";
        public const string DownRightCorner = "╝";
        public const string UpDownWall = "═";
        public const string LeftRightWall = "║";
        public const int OffsetX = 3;
        public const int OffsetY = 1;
        public const string EmptyCell = " ";
        public const double HalfOfBoard = 0.5;
        #endregion

        #region WindowSettings
        public const int MaximizedWindowSize = 3;
        public const int Disabled = 0;
        public const int ConsoleSize = 61440;
        #endregion

        #region Other
        public const int TimeInterval = 1000;
        public const string PluginsFolder = "Plugins";
        #endregion
    }
}