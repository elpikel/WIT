using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Wit
{
    public interface IDownloader
    {
        void DownloadPage();
        void DownloadImage();
    }

    public class Downloader : IDownloader
    {
        private string _url;
        private string _page;
        private BitmapImage _image;

        public delegate void OnPageDownloaded(HtmlDocument page);
        public delegate void ImageDownloaded(Uri imagePath);

        public event OnPageDownloaded DownloadedPage;
        public event ImageDownloaded OnDownloadedImage;

        public Downloader(string url, OnPageDownloaded onDownloadedPage, ImageDownloaded onImageDownloaded)
        {
            _url = url;
            DownloadedPage += onDownloadedPage;
            OnDownloadedImage += onImageDownloaded;
        }

        public async void DownloadPage()
        {
            var webGet = new HtmlWeb();
            var document = await webGet.LoadFromWebAsync(_url);
            DownloadedPage(document);
        }

        public async void DownloadImage()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            handler.AllowAutoRedirect = true;
            HttpClient client = new HttpClient(handler);
            HttpResponseMessage response = await client.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var uniqueName = Guid.NewGuid().ToString().Replace("-", "");
                var desiredName = string.Format("{0}.jpg", uniqueName);
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(desiredName, CreationCollisionOption.ReplaceExisting);

                using (var filestream = await file.OpenStreamForWriteAsync())
                {
                    await stream.CopyToAsync(filestream);
                    OnDownloadedImage(new Uri(string.Format("ms-appdata:///local/{0}.jpg", uniqueName), UriKind.Absolute));
                }
            }
        }
    }
}
