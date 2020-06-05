using Bot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Service
{
    public class SaverService
    {
        private static List<UserModel> Users = new List<UserModel>();
        private static string path = Environment.CurrentDirectory + "\\data.json";
        public SaverService()
        {
            ReadJSON();
        }
        private void ReadJSON()
        {
            Users = JsonConvert.DeserializeObject<List<UserModel>>(File.ReadAllText(path));
        }
        public void Save(List<UserModel> Users)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(Users));
            ReadJSON();
        }
        public bool Add(UserModel model)
        {
            try
            {
                Users.Add(model);
                Save(Users);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(UserModel model)
        {
            try
            {
                var data = Users.FirstOrDefault(x => x.Discord == model.Discord);
                Users.Remove(data);
                Users.Add(model);
                Save(Users);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public UserModel Get(ulong id)
        {
            var model = Users.FirstOrDefault(x => x.Discord == id);
            if (model == null)
            {
                return new UserModel
                {
                    Discord = id
                };
            }
            return model;
        }
    }
}
