using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Wit
{
    public class Downloader
    {
        private string _url;
        private string _page;
        private BitmapImage _image;

        public delegate string OnPageDownloaded(string page);

        public event OnPageDownloaded DownloadedPage;

        public Downloader(string url, OnPageDownloaded onDownloadedPage)
        {
            _url = url;
            DownloadedPage += onDownloadedPage;
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
            using (var imageStream = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                var bitmap = new BitmapImage();
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
