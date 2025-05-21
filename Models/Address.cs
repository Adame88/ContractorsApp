using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractorsApp.Models
{
    public class Address
    {
        [Display(Name = "ID")]
        public int AddressID { get; set; }
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Kod")]
        public string PostalCode { get; set; }
        [Display(Name = "Główny adres")]
        public bool IsMainAddress { get; set; } = false;
    }
}
