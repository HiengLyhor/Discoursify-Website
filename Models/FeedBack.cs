using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discoursify.Models
{
    public class FeedBack
    {
        public string Owner { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public int Rate { get; set; }
    }
}