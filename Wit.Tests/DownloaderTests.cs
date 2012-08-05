using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Wit.Tests
{
    [TestClass]
    public class DownloaderTests
    {
        [TestClass]
        public class DownloadTests
        {
            [TestMethod]
            public void ShouldReturnNonEmptyStringWhenDownloadingMainPage()
            {
                var downloader = new Downloader("http://whereisthecool.com/page/2", new Downloader.OnPageDownloaded(Downloaded), new Downloader.ImageDownloaded(ImageDownloaded));
                downloader.DownloadPage();
            }

            private void ImageDownloaded(Uri imagePath)
            {
                throw new NotImplementedException();
            }

            private string Downloaded(string page)
            {
                System.Diagnostics.Debug.WriteLine(page);
                Assert.AreNotSame(string.Empty, page);
                return page;
            }
        }
    }
}
