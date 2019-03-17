namespace Mallos.Networking.User
{
    using System;
    using System.Threading.Tasks;

    public class UserManager
    {
        /// <summary>
        /// Gets the <see cref="IUserStorage"/>.
        /// </summary>
        public IUserStorage UserStorage { get; }

        /// <summary>
        /// Gets the <see cref="PasswordHasher"/>.
        /// </summary>
        public PasswordHasher PasswordHasher { get; }

        /// <summary>
        /// Initialize a new <see cref="UserManager"/>.
        /// </summary>
        /// <param name="storage"></param>
        public UserManager(IUserStorage storage, PasswordHasher passwordHasher = null)
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
        public virtual async Task<UserResult> AddLoginAsync(string username, string password)
        {
            var user = await UserStorage.FindByNameAsync(username);
            var passwordHashed = PasswordHasher.HashPassword(password);

            var valid = PasswordHasher.VerifyHashedPassword(user.PasswordHash, passwordHashed);
            var success = valid == VerifyHashedPasswordResult.Success ||
                          valid == VerifyHashedPasswordResult.SuccessRehashNeeded;

            return new UserResult(success);
        }

        /// <summary>
        /// Asynchronously associates a login with a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="password">The user login information.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<UserResult> AddLoginAsync(Guid id, string password)
        {
            var user = await UserStorage.FindByIdAsync(id);
            var passwordHashed = PasswordHasher.HashPassword(password);

            var valid = PasswordHasher.VerifyHashedPassword(user.PasswordHash, passwordHashed);
            var success = valid == VerifyHashedPasswordResult.Success ||
                          valid == VerifyHashedPasswordResult.SuccessRehashNeeded;

            return new UserResult(success);
        }

        /// <summary>
        /// Asynchronously creates a user with no password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual Task<UserResult> CreateAsync(User user) => CreateAsync(user, null);

        /// <summary>
        /// Asynchronously creates a user with the given password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<UserResult> CreateAsync(User user, string password)
        {
            if (password != null)
            {
                user.PasswordHash = PasswordHasher.HashPassword(password);
            }

            var createTask = UserStorage.CreateAsync(user);

            try
            {
                return new UserResult(await createTask);
            }
            catch (Exception exception)
            {
                return new UserResult(exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously deletes a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<UserResult> DeleteAsync(User user)
        {
            return new UserResult(await UserStorage.DeleteAsync(user));
        }

        /// <summary>
        /// Asynchronously updates a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<UserResult> UpdateAsync(User user)
        {
            return new UserResult(await UserStorage.UpdateAsync(user));
        }

        /// <summary>
        /// Asynchronously updates a users password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        public virtual async Task<UserResult> UpdatePassword(User user, string newPassword)
        {
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            return new UserResult(await UserStorage.UpdateAsync(user));
        }
    }
}
