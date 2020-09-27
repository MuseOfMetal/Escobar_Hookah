namespace Types
{
    class Texts
    {
        public static void Load()
        {
            texts_ = FileManager.FileManager<Texts>.Load("Texts.json");
            if (texts_ == null)
                texts_ = new Texts();
        }
        public string AboutUs { get; set; } = "null";
        public string BanMessage { get; set; }
        private static Texts texts_;
        public static Texts GetTexts()
        {
            if (texts_ != null)
                return texts_;
            Load();
            return texts_;

        }
    }
}