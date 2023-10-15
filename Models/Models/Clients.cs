using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record Clients
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string CellPhone { get; set; } = default!;
        public string CEP { get; set; } = default!;
        public string Adress { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
    }
}
