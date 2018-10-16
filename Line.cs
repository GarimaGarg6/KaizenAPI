using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FABQC.Domain.Entities
{
    public class Line :BaseEntity
    {
        [Key]
        public Guid LineId { get; set; }

        [Required(ErrorMessage = "Line Name is required")]
        public string LineName { get; set; }

        [MaxLength(50, ErrorMessage = "Line Description cannot be longer than 50 characters.")]
        public string LineDesc { get; set; }

        public Guid FactoryId { get; set; }

    }
}
