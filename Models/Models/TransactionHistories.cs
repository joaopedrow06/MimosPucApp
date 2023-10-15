using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record TransactionHistories
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; } = default!;
        public long AppointmentId { get; set; } = default!;
        public bool WasCanceled { get; set; } = default!;
        [NotMapped]
        public Appointments Appointment { get; set; } = default!;

    }
}
