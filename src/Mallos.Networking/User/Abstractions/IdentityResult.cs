namespace Mallos.Networking.User.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;

    public class IdentityResult
    {
        public bool Success { get; }

        public IEnumerable<string> Errors { get; }

        public IdentityResult(bool success)
        {
            this.Success = success;
            this.Errors = null;
        }

        public IdentityResult(IEnumerable<string> errors)
        {
            this.Success = errors.Count() == 0;
            this.Errors = errors;
        }

        public IdentityResult(params string[] errors)
        {
            this.Success = errors.Count() == 0;
            this.Errors = errors;
        }
    }
}
