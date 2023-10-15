using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record Pets
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        [NotMapped]
        public long ClientId { get; set; }
    }
}
