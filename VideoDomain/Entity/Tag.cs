namespace VideoDomain.Entity
{
    public record Tag
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        //public string Description { get; private set; }
        public long Contain { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime DeletedTime { get; private set; }
        public List<TheFile> TheFiles { get; private set; }
        private Tag() { }
        public static Tag InitTag(string name) { 
            Tag tag = new Tag();    
            tag.Name = name;
            //tag.Description = description;
            tag.Contain = 0;
            return tag;
        }
        public void ChangeContain(long contain)
        {
            this.Contain += contain;
        }
        //public void ChangeDescription(string description)
        //{
        //    this.Description = description;
        //}
        public void Deleted()
        {
            IsDeleted = true;
            DeletedTime = DateTime.Now;
        }

    }
}
