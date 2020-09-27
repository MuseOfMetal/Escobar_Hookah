using System.Collections.Generic;
using FileManager;
namespace Types
{
    class User
    {

        public static void Load()
        {
            UserBase = FileManager<List<User>>.Load(@"Users.json");
            if (UserBase == null)
                UserBase = new List<User>();
        }

        public static void Save()
        {
            FileManager<List<User>>.Save(UserBase, @"Users.json");
        }
        public static List<User> UserBase;
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UName { get; set; }
        public string Status { get; set; }
        public int LevelPermission { get; set; }
        public void Ban()
        {
            LevelPermission = -1;
        }

        public void SetDefault()
        {
            LevelPermission = 0;
        }

        public void AddToAdmin()
        {
            LevelPermission = 1;
        }
        public void UpdateUser(string FName, string LName, string UName)
        {
            this.FName = FName;
            this.LName = LName;
            this.UName = UName;
        }
        public static void AddUser(int Id, string FName, string LName, string UName, int LevelPermission, out int Index)
        {
            UserBase.Add(new User(Id, FName, LName, UName, LevelPermission));
            Index = FindUser(Id);
        }
        public static int FindUser(int Id)
        {
            var User = UserBase;
            for (int i = 0; i < User.Count; i++)
            {
                if (User[i].Id == Id)
                    return i;
            }
            return -1;
        }
        public User(int Id, string FName, string LName, string UName, int LevelPermission)
        {
            this.Id = Id;
            this.FName = FName;
            this.LName = LName;
            this.UName = UName;
            this.LevelPermission = LevelPermission;
            this.Status = null;
        }
    }
}