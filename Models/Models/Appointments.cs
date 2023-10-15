using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public record Appointments
    {
        public long Id { get; set; }
        public long ClientPetId { get; set; }
        public DateTime Date { get; set; } = default!;
        public bool AppointmentIsComplete { get; set; } = default!;
        [Column("AppointmentName")]
        public string AppointmentNameEnumString
        {
            get { return AppointmentName.ToString(); }
            private set => AppointmentName = value.ParseEnum<AppointmentNames>();
        }
        [NotMapped]
        public AppointmentNames AppointmentName { get; set; } = 0;
        [NotMapped]
        public bool WasCanceled { get; set; } = default!;
        [NotMapped]
        public ClientsPets ClientPet { get; set; } = default!;

    }
}
