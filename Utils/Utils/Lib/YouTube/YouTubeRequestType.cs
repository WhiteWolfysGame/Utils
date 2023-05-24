using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.YouTube
{
    internal class YouTubeRequestType
    {
        public static string Snippet { get { return "snippet"; } }
        public static string SnippetAndStatistics { get { return "snippet, statistics"; } }
        public static string Contentdetails { get { return "contentDetails"; } }
    }
}
