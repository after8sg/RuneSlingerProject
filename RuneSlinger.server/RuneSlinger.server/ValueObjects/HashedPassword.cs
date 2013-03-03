namespace RuneSlinger.server.ValueObjects
{
    public class HashedPassword
    {
        public string Hash { get; private set; }
        public string Salt { get; private set; }

        public HashedPassword(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }

        /// <summary>
        /// for nhibernate
        /// </summary>
        private HashedPassword()
        {
        }

    }
}
