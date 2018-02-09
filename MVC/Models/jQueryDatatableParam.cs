using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class jQueryDatatableParam
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Dictionary<string, string> search { get; set; }
        public List<Dictionary<string, string>> order { get; set; }
        public List<Dictionary<string, string>> columns { get; set; }
    }
}