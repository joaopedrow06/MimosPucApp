using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record ClientsPets
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long PetId { get; set; }

        [NotMapped]
        public List<Pets> Pets { get; set; } = new();
        [NotMapped]
        public Clients Client { get; set; } = default!;

        [NotMapped]
        public Pets Pet { get; set; } = default!;

    }
}
