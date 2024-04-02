using AqbaServer.Interfaces.Service;
using Microsoft.AspNetCore.StaticFiles;

namespace AqbaServer.Helper
{
    public class ManageImage : IManageImage
    {
        public (FileStream?, string?, string?) DownloadFile(string directoryName, string fileName)
        {
            try
            {
                var filepath = GetFilePath(directoryName, fileName);
                if (!File.Exists(filepath))
                {
                    WriteLog.Info($"[Download file method] File {fileName} not exists");
                    return (null, null, null);
                }

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filepath, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }

                FileStream fileStream = new(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                return (fileStream, _ContentType, fileName);
            }
            catch (Exception ex)
            {
                WriteLog.Error(ex.ToString());
                return (null, null, null);
            }
        }

        public async Task<string?> UploadFile(string directoryName, IFormFile file)
        {
            try
            {
                FileInfo _FileInfo = new(file.FileName);

                var filepath = GetFilePath(directoryName, _FileInfo.Name);

                using (FileStream _FileStream = new(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(_FileStream);
                }

                return _FileInfo.Name;
            }
            catch (Exception ex)
            {
                WriteLog.Error(ex.ToString());
                return null;
            }
        }

        static string GetFilePath(string directoryName, string fileName)
        {
            var staticContentDirectory = System.AppContext.BaseDirectory;

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            var result = Path.Combine(staticContentDirectory, directoryName, fileName);
            return result;
        }
    }
}
