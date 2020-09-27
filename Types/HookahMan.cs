
using System.Collections.Generic;
using FileManager;
namespace Types
{
    class HookahMan
    {
        public static void Load()
        {
            HookahWorkersBase = FileManager<List<HookahMan>>.Load(@"HookahWorkers.json");
            if (HookahWorkersBase == null)
                HookahWorkersBase = new List<HookahMan>();
        }

        public static void Save()
        {
            FileManager<List<HookahMan>>.Save(HookahWorkersBase, @"HookahWorkers.json");
        }

        public static List<HookahMan> HookahWorkersBase;
        public static string PathToImage = @"Images/HookahWorkers/{0}.png";
        public string Id { get; set; }
        public string Name { get; set; }
        public static int FindHookahMan(string Id)
        {
            var Base = HookahWorkersBase;
            for (int i = 0; i < Base.Count; i++)
            {
                if (Base[i].Id == Id)
                    return i;
            }
            return -1;
        }
        public static void AddHookahMan(string Id, string Name)
        {
            HookahWorkersBase.Add(new HookahMan() { Id = Id, Name = Name });
        }
        public static void DeleteHookahMan(string Id)
        {
            HookahWorkersBase.RemoveAt(FindHookahMan(Id));
        }
    }
}