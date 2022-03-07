using System.Runtime.CompilerServices;
using MinimalRestDemo.DAL.Models;

namespace MinimalRestDemo.DAL
{
    public class UserStorage
    {
        private readonly IDictionary<int, User> _users;

        private int _id;

        public UserStorage()
        {
            _users = new Dictionary<int, User>();
        }

        public bool CreateUser(User user)
        {
            if (_users.Values.Contains(user))
                return false;
            _users.Add(_id++, user);
            return true;
        }

        public ICollection<User> GetAllUsers()
        {
            return _users.Values;
        }

        public User? GetUser(int id)
        {
            if (!_users.Keys.Contains(id))
                return null;
            return _users[id];
        }

        public bool UpdateUser(int id, User user)
        {
            if (!_users.Keys.Contains(id))
            {
                return false;
            }

            _users[id] = user;

            return true;
        }

        public bool UpdateUserName(int id, string name)
        {
            if (!_users.Keys.Contains(id))
            {
                return false;
            }

            _users[id].Name = name;

            return true;
        }

        public bool UpdateUserEmail(int id, string email)
        {
            if (!_users.Keys.Contains(id))
            {
                return false;
            }

            _users[id].Email = email;

            return true;
        }

        public bool DeleteUser(int id)
        {
            if (!_users.Keys.Contains(id))
            {
                return false;
            }

            _users.Remove(id);

            return true;
        }
    }
}
