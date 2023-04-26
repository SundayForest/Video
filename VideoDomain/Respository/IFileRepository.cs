namespace VideoDomain
{
    public interface IFileRepository
    {
        Task<TheFile?> FindFileAsync(string hash);
        Task<TheFile?> FindFileWithTagAsync(string hash);
        Task<List<Tag>> GetAllTag();
        Task<Tag?> CreateTagAsync(string name);
        Task<Tag?> FindTagWithFilesOrderAsync(int tagId);
        Task<Uri> SaveAsync(string key,Stream stream);

        Task<List<TheFile>> PageFileOrderAsync(int page,int pageSize,int tagId,DayType dayType);
        Task<List<TheFile>> FindAuthWorksAsync(long id);
    }
}
