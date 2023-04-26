

namespace VideoDomain.Entity
{
    public record TheFile
    {
        public Uri Path { get; private set; }
        public Uri PreviousImgUri { get; private set; }
        public string FileName { get;private set; }
        public string Descrition { get; private set; }
        public long Size { get; private set; }
        public string FileHash { get; private set; }
        public List<User> Users { get; private set; }
        public AuthInfo Auth { get; private set; }
        public DateTime CreateTime { get; private set; }
        public HashSet<Tag> Tags { get; private set; }
        public bool IsDeleted { get; private set; }
        public long Hit { get; private set; }
        private TheFile() { }
        public static TheFile InitFile(Uri path,Uri previousImgUri,AuthInfo auth, string FileName,string FileHash,long Size)
        {
            TheFile file = new TheFile();
            file.Path = path;
            file.Size = Size;
            file.Auth = auth;
            file.FileName = FileName;
            file.FileHash = FileHash;
            file.PreviousImgUri = previousImgUri;
            file.Hit = 0;
            file.CreateTime = DateTime.Now;
            file.IsDeleted = false;
            return file;
        }
        public void ChangeHit(long hit) {
            this.Hit += hit;
        }
        public void AddTag(Tag tag) {
            Tags.Add(tag);
        }
        public void Delete()
        {
            this.IsDeleted = true;
        } 
        public void UpdateName(string filename) { 
            this.FileName = filename;
        }
        public void UpdateDescription(string des) { 
            this.Descrition = des;
        } 
    }
}
