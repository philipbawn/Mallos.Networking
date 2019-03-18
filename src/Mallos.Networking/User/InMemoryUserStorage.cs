namespace Mallos.Networking.User
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InMemoryUserStorage<TUser> : IUserStorage<TUser>
        where TUser : IdentityUser
    {
        protected readonly List<TUser> Users = new List<TUser>();

        public async Task<bool> CreateAsync(TUser user)
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
            Users.Add(user);

            return true;
        }

        public async Task<bool> UpdateAsync(TUser user)
        {
            (int index, TUser otherUser) = await FindUserIndexByUniqueIdAsync(user.Guid);
            if (otherUser == null || otherUser.Guid != user.Guid)
            {
                return false;
            }

            Users[index] = user;
            return true;
        }

        public async Task<bool> DeleteAsync(TUser user)
        {
            (int index, TUser otherUser) = await FindUserIndexByUniqueIdAsync(user.Guid);
            if (otherUser == null || otherUser.Guid != user.Guid)
            {
                return false;
            }

            Users.RemoveAt(index);
            return true;
        }

        public Task<TUser> FindByNameAsync(string username)
        {
            var usernameLower = username.ToLower();
            foreach (var user in Users)
            {
                if (user.Username.ToLower() == usernameLower)
                {
                    return Task.FromResult(user);
                }
            }
            return Task.FromResult<TUser>(null);
        }

        public async Task<TUser> FindByIdAsync(Guid guid)
        {
            (int _, TUser user) = await FindUserIndexByUniqueIdAsync(guid);
            return user;
        }

        private Task<(int, TUser)> FindUserIndexByUniqueIdAsync(Guid guid)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Guid == guid)
                {
                    return Task.FromResult((i, Users[i]));
                }
            }
            return null;
        }
    }
}
