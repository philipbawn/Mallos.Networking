namespace Mallos.Networking.User
{
    public enum VerifyHashedPasswordResult
    {
        /// <summary>
        /// Password verification failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Succes.
        /// </summary>
        Success,

        /// <summary>
        /// Success, but should update and rehash the password.
        /// </summary>
        SuccessRehashNeeded
    }

    public class PasswordHasher
    {
        public virtual string HashPassword(string password)
        {
            // TODO: Hash password.
            return password;
        }

        /// <summary>
        /// Verifies that a password matches the hashed password.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="providedPassword">The provided password.</param>
        /// <returns>The result of the verification.</returns>
        public VerifyHashedPasswordResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == providedPassword)
            {
                return VerifyHashedPasswordResult.Success;
            }

            if (hashedPassword == HashPassword(providedPassword))
            {
                return VerifyHashedPasswordResult.SuccessRehashNeeded;
            }

            return VerifyHashedPasswordResult.Failed;
        }
    }
}
