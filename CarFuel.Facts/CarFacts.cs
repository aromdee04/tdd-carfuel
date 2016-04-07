using CarFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Should;
using CNX.Shared;

namespace CarFuel.Facts
{
    public class CarFacts
    {
        public class General
        {
            [Fact]
            public void BasicUsages()
            {
                Car c = new Car();

                c.Make = "Honda";
                c.Model = "Jazz";

                c.Make.ShouldEqual("Honda");
                c.Model.ShouldEqual("Jazz");
                c.FillUps.Count().ShouldEqual(0);
            }
        }

        public class CarMothods
        {
            [Fact]
            public void NewFillUp_HasCurrentDateTime()
            {
                var c = new Car();
                var now = DateTime.Now;

                SystemTime.SetDateTime(now);
                FillUp f = c.AddFillUp(1000, 4);
                SystemTime.Reset();

                f.Date.ShouldEqual(now);
            }

            [Fact]
            public void AddFirstFillupTest()
            {
                Car c = new Car();
                FillUp f1 = c.AddFillUp(1000, 40d);

                f1.OdoMeter.ShouldEqual(1000);
                f1.Liters.ShouldEqual(40d);
                f1.KmL.ShouldBeNull();
                c.FillUps.Count().ShouldEqual(1);
            }

            [Fact]
            public void AddSecondFillupTest()
            {
                Car c = new Car();
                FillUp f1 = c.AddFillUp(1000, 40d);

                f1.OdoMeter.ShouldEqual(1000);
                f1.Liters.ShouldEqual(40d);
                f1.NextFillUp.ShouldBeNull();
                f1.KmL.ShouldBeNull();

                FillUp f2 = c.AddFillUp(1600, 50d);
                f2.OdoMeter.ShouldEqual(1600);
                f2.Liters.ShouldEqual(50d);
                f2.NextFillUp.ShouldBeNull();
                f2.KmL.ShouldBeNull();

                c.FillUps.Count().ShouldEqual(2);

                f1.KmL.ShouldEqual(12d);

                //Assert.Same(f2, f1.NextFillup);
                f1.NextFillUp.ShouldBeSameAs(f2);
            }

            [Fact]
            public void GetAvgKmLSingleFillup()
            {
                Car c = new Car();

                c.AddFillUp(1000, 40);
                double? avg = c.AverageKmL;

                avg.ShouldBeNull();
            }

            [Fact]
            public void GetAvgKmLTwoFillup()
            {
                Car c = new Car();

                c.AddFillUp(1000, 40);
                c.AddFillUp(1600, 50);
                double? avg = c.AverageKmL;

                avg.ShouldEqual(12);
            }

            [Fact]
            public void GetAvgKmLThreeFillup()
            {
                Car c = new Car();

                c.AddFillUp(1000, 40);
                c.AddFillUp(1600, 50);
                c.AddFillUp(2000, 40);
                double? avg = c.AverageKmL;

                avg.ShouldEqual(11.11);
            }
        }

    }


}
