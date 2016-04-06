using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFuel.Models
{
    public class FillUp
    {
        public int Id { get; set; }

        public double Liters { get; set; }
        public FillUp NextFillup { get; set; }
        public int OdoMeter { get; set; }

        public double? getKmL()
        {

            if (NextFillup == null) return null;

            return (NextFillup.OdoMeter - OdoMeter) / NextFillup.Liters;
            
            //throw new NotImplementedException();
        }
    }
}
