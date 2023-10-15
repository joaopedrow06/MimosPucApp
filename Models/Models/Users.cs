using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record Users
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
