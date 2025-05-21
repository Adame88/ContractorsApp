using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractorsApp.Models
{
    public class Contractor
    {
        [Display(Name = "Id")]
        public int ContractorId { get; set; }
        [Display(Name = "Nazwa Firmy")]
        public string CompanyName { get; set; }
        [Display(Name = "NIP")]
        public string TaxNumber { get; set; }
        public string REGON { get; set; }

        [Display(Name = "Adresy")]
        public List<Address> Addresses { get; set; } = new();
        [Display(Name = "Główny adres")]
        public Address MainAddress => Addresses.FirstOrDefault(a => a.IsMainAddress);
    }
}
