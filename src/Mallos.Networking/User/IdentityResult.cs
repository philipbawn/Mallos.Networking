namespace Mallos.Networking.User
{
    using System.Collections.Generic;
    using System.Linq;

    public class UserResult
    {
        public bool Success { get; }

        public IEnumerable<string> Errors { get; }

        public UserResult(bool success)
        {
            this.Success = success;
            this.Errors = null;
        }

        public UserResult(IEnumerable<string> errors)
        {
            this.Success = errors.Count() == 0;
            this.Errors = errors;
        }

        public UserResult(params string[] errors)
        {
            this.Success = errors.Count() == 0;
            this.Errors = errors;
        }
    }
}
