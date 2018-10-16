using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABQC.Domain.Entities
{
    public class Factory :BaseEntity
    {
        [Key]
        public Guid FactoryId { get; set; }

        [Required(ErrorMessage = "Factory Name is required")]
        public string FactoryName { get; set; }

        public string FactoryCode { get; set; }

        public Guid CompanyId { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }

    }
}
