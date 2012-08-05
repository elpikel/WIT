using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Wit
{
    public class Downloader
    {
        private string _url;
        private string _page;
        private BitmapImage _image;

        public delegate string OnPageDownloaded(string page);
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
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            handler.AllowAutoRedirect = true;
            HttpClient client = new HttpClient(handler);
            HttpResponseMessage response = await client.GetAsync(_url);
            response.EnsureSuccessStatusCode();
            _page = await response.Content.ReadAsStringAsync();
            DownloadedPage(_page);
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

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            using (Stream postStream = request.EndGetRequestStream(asynchronousResult))
            {
                using (StreamReader rs = new StreamReader(postStream))
                {
                    _page = rs.ReadToEnd();
                    DownloadedPage(_page);
                }
            }
        }
    }
}
