
using FileManager;
namespace BotConfig
{
    class Config
    {
        #region Methods
        public static void Load()
        {
            _config = FileManager<Config>.Load(@"Config.json");
        }
        public static void Save()
        {
            FileManager<Config>.Save(_config, @"Config.json");
        }
        public static Config GetConfig()
        {
            if (_config != null)
                return _config;
            Load();
            return _config;
        }
        #endregion



        private static Config _config;
        public string BotToken { get; set; }
        public long ReviewsGroupId { get; set; }
        public int SuperAdminId { get; set; }
        private Config()
        {
            //Closed constructor
        }
    }
}