namespace Repository
{
    /// <summary>
    /// The class contains constants.
    /// </summary>
    public class ConstantsRepository
    {
        #region Titles
        public const string GameTitle = "Savannah - Antilopes vs Lions";
        #endregion

        #region MainMenuCommands 
        public const string ExitButtonDescription = "Q, or Escape - Exit game;";

        public const string AddAntilopeDescription = "A Button - Add an antilope;";

        public const string AddLionDescription = "L Button - Add a lion;";
        #endregion

        #region Numbers
        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;
        public const int Three = 3;
        public const int Four = 4;
        #endregion

        #region AnimalSettings
        public const int LionVision = 2;
        public const int AntilopeVision = 2;
        #endregion

        #region BoardSettings
        public const int ColumnsCount = 15;
        public const int RowsCount = 10;

        public const int BorderWidth = ColumnsCount + Three;
        public const int BorderHeight = RowsCount + One;

        public const string UpLeftCorner = "╔";
        public const string UpRightCorner = "╗";
        public const string DownLeftCorner = "╚";
        public const string DownRightCorner = "╝";
        public const string UpDownWall = "═";
        public const string LeftRightWall = "║";

        public const int OffsetX = Three;
        public const int OffsetY = One;

        public const string EmptyCell = ".";

        public const string AntilopeLetter = "A";
        public const string LionLetter = "L";

        public const double HalfOfBoard = 0.5;
        #endregion

        #region WindowSettings
        public const int MaximizedWindowSize = Three;
        public const int Disabled = Zero;
        public const int ConsoleSize = 61440;
        #endregion

        #region Other
        public const int ThreadSleep = 1000;

        public const string Space = " ";
        public const string EmptyString = "";
        #endregion
    }
}