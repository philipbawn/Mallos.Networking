namespace Mallos.Networking.User
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InMemoryUserStorage : IUserStorage
    {
        private readonly List<User> users = new List<User>();

        public async Task<bool> CreateAsync(User user)
        {
            var otherUser = await FindByNameAsync(user.Username);
            if (otherUser != null)
            {
                if (otherUser.Guid == user.Guid)
                {
                    return await Task.FromException<bool>(new ArgumentException("User is already created."));
                }
                else
                {
                    return await Task.FromException<bool>(new ArgumentException("Username is taken."));
                }
            }

            user.Guid = Guid.NewGuid();
            users.Add(user);

            return true;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            (int index, User otherUser) = await FindUserIndexByUniqueIdAsync(user.Guid);
            if (otherUser == null || otherUser.Guid != user.Guid)
            {
                return false;
            }

            users[index] = user;
            return true;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            (int index, User otherUser) = await FindUserIndexByUniqueIdAsync(user.Guid);
            if (otherUser == null || otherUser.Guid != user.Guid)
            {
                return false;
            }

            users.RemoveAt(index);
            return true;
        }

        public Task<User> FindByNameAsync(string username)
        {
            var usernameLower = username.ToLower();
            foreach (var user in users)
            {
                if (user.Username.ToLower() == usernameLower)
                {
                    return Task.FromResult(user);
                }
            }
            return Task.FromResult<User>(null);
        }

        public async Task<User> FindByIdAsync(Guid guid)
        {
            (int _, User user) = await FindUserIndexByUniqueIdAsync(guid);
            return user;
        }

        private Task<(int, User)> FindUserIndexByUniqueIdAsync(Guid guid)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Guid == guid)
                {
                    return Task.FromResult((i, users[i]));
                }
            }
            return null;
        }
    }
}
