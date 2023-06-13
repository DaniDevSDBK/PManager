using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Model
{
    public interface UIRepositable
    {
        bool AuthenticateUser(NetworkCredential credential);

        public void Add(UserModel userModel);
        public void Edit(UserModel userModel);
        public void Remove(int id);
        UserModel GetById(int id);
        UserModel GetByEmail(string email);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetByAll();
        bool UserExists(string email);
    }
}
