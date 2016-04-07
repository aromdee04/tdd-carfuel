using CNX.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFuel.Models
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual List<FillUp> FillUps { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        public Car()
        {
            FillUps = new List<FillUp>();
        }

        //private List<Car>
        public FillUp AddFillUp(int OdoMeter, double Liters)
        {
            FillUp fu = new FillUp
            {
                OdoMeter = OdoMeter,
                Liters = Liters
            };

            fu.Date = SystemTime.Now();

            FillUps.Add(fu);

            var count = FillUps.Count();

            if(FillUps.Count > 1)
            {
                FillUps[count - 2].NextFillUp = fu;
            }

            return fu;
        }

        public double? AverageKmL
        {
            get
            {
                if (FillUps.Count >= 2)
                {
                    var firstFillUp = FillUps.First();
                    var lastFillup = FillUps.Last();

                    var sumLiters = FillUps.Sum(o => o.Liters);

                    var diffOdoMeter = lastFillup.OdoMeter - firstFillUp.OdoMeter;
                    var diffLiters = sumLiters - firstFillUp.Liters;

                    return Math.Round(diffOdoMeter / diffLiters, 2, MidpointRounding.AwayFromZero);
                }

                return null;
            }
        }

        public Guid OwnerId { get; set; }
    }
}
