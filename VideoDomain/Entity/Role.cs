namespace VideoDomain.Entity
{
    public class Role : IdentityRole<Guid>
    {
        public Role() { 
            this.Id = new Guid();
        }
    }
}
