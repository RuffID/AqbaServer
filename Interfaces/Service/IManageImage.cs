namespace AqbaServer.Interfaces.Service
{
    public interface IManageImage
    {
        Task<string?> UploadFile(string directoryName, IFormFile _IFormFile);
        (FileStream?, string?, string?) DownloadFile(string directoryName, string fileName);
    }
}