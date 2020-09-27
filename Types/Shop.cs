using FileManager;
using System.Collections.Generic;

namespace Types
{
    class Shop
    {
        public static void Load()
        {
            ShopsBase = FileManager<List<Shop>>.Load(@"Shops.json");
            if (ShopsBase == null)
                ShopsBase = new List<Shop>();
        }
        public static void Save()
        {
            FileManager<List<Shop>>.Save(ShopsBase, @"Shops.json");
        }
        public static List<Shop> ShopsBase;
        public string Name { get; set; } = "null";
        public string About { get; set; } = "null";
        public string Phone { get; set; } = "null";

        public static void AddShop(string Name, string About, string Contacts)
        {
            ShopsBase.Add(new Shop() {Name = Name, About = About, Phone = Contacts});
        }
        public static void RemoveShop(string Name)
        {
            ShopsBase.RemoveAt(FindShop(Name));
        }
        public static int FindShop(string name)
        {
            var list = ShopsBase;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name)
                    return i;
            }
            return -1;
        }
    }
}