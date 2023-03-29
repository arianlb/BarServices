using BarServices.Services.Interfaces;

namespace BarServices.Services
{
    public class StoreFileImple : IStoreFiles
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public StoreFileImple(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task DeleteFileAsync(string route, string folder)
        {
            if( route is not null)
            {
                var fileName = Path.GetFileName(route);
                string address = Path.Combine(env.WebRootPath, folder, fileName);

                if(File.Exists(address))
                {
                    File.Delete(address);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<string> EditFileAsync(byte[] content, string extension, string folder, string route, string contentType)
        {
            await DeleteFileAsync(route, folder);
            return await SaveFileAsync(content, extension, folder, contentType);
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string folder, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            string address = Path.Combine(env.WebRootPath, folder);

            if(!Directory.Exists(address))
            {
                Directory.CreateDirectory(address);
            }

            string route = Path.Combine(address, fileName);
            await File.WriteAllBytesAsync(route, content);

            var currentUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            return Path.Combine(currentUrl, folder, fileName).Replace("\\", "/");
        }
    }
}
