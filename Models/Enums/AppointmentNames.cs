using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum AppointmentNames
    {
        [Description("Atendimento Veterinário")]
        VET,
        [Description("Banho e Tosa")]
        PETSHOP
    }
}
