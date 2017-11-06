using System;
using System.Collections.Generic;
using System.Text;

namespace Pamux.GameDev.Lib.Utilities
{
    public static class HtmlGen
    {

        public static string GetHTMLDiv(string className, string id, string content)
        {
            return GetHTMLTag("div", className, id, content);
        }

        public static string GetHTMLAnchor(string className, string id, string href, string image, string content)
        {
            string otherAttributes = string.Format(" href=\"{0}\"", href);

            return GetHTMLTag("a", className, id, otherAttributes, content);
        }
        public static string GetHTMLTag(string tag, string className, string id, string content)
        {
            return GetHTMLTag(tag, className, id, "", content);
        }
        public static string GetHTMLTag(string tag, string className, string id, string otherAttributes, string content)
        {
            return string.Format("<{0} class=\"{1}\" id=\"{2}\"{3}>{4}</{0}>", tag, className, id, otherAttributes, content);
        }

        public static string GetHTMLHead()
        {

            return "<meta charset=\"UTF-8\" /><link href=\"local.css\" rel=\"stylesheet\" />";
        }
    }
}
