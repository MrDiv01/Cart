using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VCard.Models;

namespace VCard.Helpers
{
    public class Result
    {
        public Name Name { get; set; }
        public Location Location { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
