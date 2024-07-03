using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCard.Models
{
    public class vCard
    {
        
        public Id Id { get; set; }
        public Name Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
    }
    public class Id
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Name
    {
        public string First { get; set; }
        public string Last { get; set; }
    }
    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }
    }
    public class Result
    {
        public List<vCard> Results { get; set; }
    }
}
