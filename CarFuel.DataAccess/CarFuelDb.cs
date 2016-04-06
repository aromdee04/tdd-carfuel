using CarFuel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFuel.DataAccess
{
    public class CarFuelDb : DbContext
    {
        public DbSet<Car> Cars { get; set; }
    }
}
