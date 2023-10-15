using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record VaccinesHistories
    {
        public long Id { get; set; }
        public long PetVaccineId { get; set; }
        public PetsVacciness PetsVacciness { get; set; } = default!;
        public DateTime VaccineDate { get; set; } = default!;
    }
}
