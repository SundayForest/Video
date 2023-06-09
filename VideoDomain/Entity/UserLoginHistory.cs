﻿namespace VideoDomain.Entity
{
    public record UserLoginHistory
    {
        public long Id { get;  set; }
        public long UserId { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        private UserLoginHistory() { }
        public static UserLoginHistory Init(long UserId)
        {
            UserLoginHistory history = new UserLoginHistory();
            history.UserId = UserId;
            history.CreateDateTime = DateTime.Now;
            return history;
        }

    }
}
