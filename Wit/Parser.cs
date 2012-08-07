using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Wit
{
    public interface IParser
    {
        List<string> Parse(HtmlDocument page);
    }

    public class Parser
    {
        public List<string> Parse(HtmlDocument page)
        {
            var images = new List<string>();

            foreach (var item in page.DocumentNode.Descendants())
            {
                if (item.Attributes["class"] != null && item.Attributes["class"].Value == "photo")
                {
                    images.Add(item.Attributes["src"].Value);
                }
            }

            return images;
        }
    }
}
