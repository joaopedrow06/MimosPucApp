using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record PetsVacciness
    {
        public long Id { get; set; }
        public long VaccineId { get; set; }
        public Vaccines Vaccine { get; set; } = default!;
        public long PetId { get; set; }
        public Pets Pet { get; set; } = default!;
    }
}
