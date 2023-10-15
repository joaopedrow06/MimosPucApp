using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record Vaccines
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;

    }
}
