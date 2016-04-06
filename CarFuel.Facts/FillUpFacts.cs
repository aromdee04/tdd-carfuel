using CarFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Should;

namespace CarFuel.Facts
{
    public class FillUpFacts
    {
        public class General // inner class / nested class can has all 5 access modifiers
        {
            [Fact]
            public void BasicUsage()
            {
                // arrange -> จัดเตรียมสิ่งที่ต้องการเทส
                FillUp fu = new FillUp();

                fu.OdoMeter = 1000;
                fu.Liters = 40d;

                // act

                // assert ตรวจสอบ
                fu.OdoMeter.ShouldEqual(1000);
                fu.Liters.ShouldEqual(40d);
               
            }

            [Fact]
            public void NewFillup_GetKmL_IsNull()
            {
                FillUp fu = new FillUp();

                fu.OdoMeter = 1600;
                fu.Liters = 50d;

                Assert.Null(fu.getKmL());        
            }


        }

        public class FillUpMothods
        {
            [Fact]
            public void TwoFillUps()
            {
                FillUp f1 = new FillUp();
                f1.OdoMeter = 1000;
                f1.Liters = 40d;

                FillUp f2 = new FillUp();
                f2.OdoMeter = 1600;
                f2.Liters = 50d;

                f1.NextFillup = f2;

                f2.getKmL().ShouldBeNull();
                f1.getKmL().ShouldEqual(12d);
            }
        }
            
    }


}
