using System.Threading.Tasks;

namespace Foundation.Commerce.Mail
{
    public interface IHtmlDownloader
    {
        Task<string> DownloadAsync(string baseUrl, string relativeUrl);
    }
}