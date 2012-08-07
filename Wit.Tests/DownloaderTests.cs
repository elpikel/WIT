using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

            [TestMethod]
            public void ShouldReturnNonEmptyUrlWhenDownloadingImages()
            {
                var downloader = new Downloader("http://25.media.tumblr.com/tumblr_m8ebzkziRA1qzleu4o1_400.jpg", new Downloader.OnPageDownloaded(Downloaded), new Downloader.ImageDownloaded(ImageDownloaded));
                downloader.DownloadImage();
            }

            private void ImageDownloaded(Uri imagePath)
            {
                Assert.AreEqual(imagePath.AbsolutePath, "");
            }

            private void Downloaded(HtmlDocument page)
            {
                System.Diagnostics.Debug.WriteLine(page);
                Assert.AreNotSame(string.Empty, page);
            }
        }
    }
}
