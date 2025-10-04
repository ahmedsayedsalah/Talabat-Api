using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(int id, string fName, string lName, string street, string city, string country)
        {
            Id = id;
            FName = fName;
            LName = lName;
            Street = street;
            City = city;
            Country = country;
        }

        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
