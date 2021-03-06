﻿namespace Mallos.Networking.User.Abstractions
{
    using System;
    using System.Threading.Tasks;

    public interface IUserManager {}

    public class UserManager<TUser> : IUserManager
        where TUser : IdentityUser
    {
        /// <summary>
        /// Gets the <see cref="IUserStorage{TUser}"/>.
        /// </summary>
        public IUserStorage<TUser> UserStorage { get; }

        /// <summary>
        /// Gets the <see cref="PasswordHasher"/>.
        /// </summary>
        public PasswordHasher PasswordHasher { get; }

        /// <summary>
        /// Initialize a new <see cref="UserManager{TUser}"/>.
        /// </summary>
        /// <param name="storage"></param>
        public UserManager(IUserStorage<TUser> storage, PasswordHasher passwordHasher = null)
        {
            this.UserStorage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.PasswordHasher = passwordHasher ?? new PasswordHasher();
        }

        /// <summary>
        /// Asynchronously associates a login with a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The user login information.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<IdentityResult> AddLoginAsync(string username, string password)
        {
            var user = await UserStorage.FindByNameAsync(username);
            if (user == null)
            {
                return new IdentityResult("Wrong username or password.");
            }

            var passwordHashed = PasswordHasher.HashPassword(password);

            var valid = PasswordHasher.VerifyHashedPassword(user.PasswordHash, passwordHashed);
            if (valid == VerifyHashedPasswordResult.Success ||
                valid == VerifyHashedPasswordResult.SuccessRehashNeeded)
            {
                return new IdentityResult(true);
            }
            else
            {
                return new IdentityResult("Wrong username or password.");
            }
        }

        /// <summary>
        /// Asynchronously associates a login with a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="password">The user login information.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<IdentityResult> AddLoginAsync(Guid id, string password)
        {
            var user = await UserStorage.FindByIdAsync(id);
            var passwordHashed = PasswordHasher.HashPassword(password);

            var valid = PasswordHasher.VerifyHashedPassword(user.PasswordHash, passwordHashed);
            var success = valid == VerifyHashedPasswordResult.Success ||
                          valid == VerifyHashedPasswordResult.SuccessRehashNeeded;

            return new IdentityResult(success);
        }

        /// <summary>
        /// Asynchronously creates a user with no password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual Task<IdentityResult> CreateAsync(TUser user) => CreateAsync(user, null);

        /// <summary>
        /// Asynchronously creates a user with the given password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            if (password != null)
            {
                user.PasswordHash = PasswordHasher.HashPassword(password);
            }

            var createTask = UserStorage.CreateAsync(user);

            try
            {
                return new IdentityResult(await createTask);
            }
            catch (Exception exception)
            {
                return new IdentityResult(exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously deletes a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<IdentityResult> DeleteAsync(TUser user)
        {
            return new IdentityResult(await UserStorage.DeleteAsync(user));
        }

        /// <summary>
        /// Asynchronously updates a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<IdentityResult> UpdateAsync(TUser user)
        {
            return new IdentityResult(await UserStorage.UpdateAsync(user));
        }

        /// <summary>
        /// Asynchronously updates a users password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<IdentityResult> UpdatePassword(TUser user, string newPassword)
        {
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            return new IdentityResult(await UserStorage.UpdateAsync(user));
        }
    }
}
