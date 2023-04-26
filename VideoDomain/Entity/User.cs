namespace VideoDomain.Entity
{
    public class User : IdentityUser<long>
    {
        public DateTime CreateTime { get; private set; }
        public UserLoginHistory LoginHistory { get; private set; }
        public AuthInfo AuthInfo { get; private set; }
        public long AuthId { get; private set; }
        public List<AuthInfo> Attentions { get; private set; }

        private User() { }
        public User(string userName) : base(userName)
        {
            this.CreateTime = DateTime.Now;
            this.AuthInfo = AuthInfo.InitAuthInfo(this);
        }
        public void AddAttention(AuthInfo authInfo)
        {
            this.Attentions.Add(authInfo);
        }
    }
}
