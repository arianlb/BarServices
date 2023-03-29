namespace BarServices.Services.Interfaces
{
    public interface IStoreFiles
    {
        Task<string> SaveFileAsync(byte[] content, string extension, string folder, string contentType);
        Task<string> EditFileAsync(byte[] content, string extension, string folder, string route, string contentType);
        Task DeleteFileAsync(string route, string folder);
    }
}
