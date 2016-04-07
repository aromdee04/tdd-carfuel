using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Should;
using CarFuel.Models;
using CarFuel.Services;
using CarFuel.DataAccess;
using Xunit.Abstractions;
using Moq;

namespace CarFuel.Facts.Services
{
    public class SharedService
    {
        public CarService CarService { get; set; }

        public FakeCarDb Db { get; set; }

        public SharedService()
        {
            Db = new FakeCarDb();
            CarService = new CarService(Db);
        }
    }

    [CollectionDefinition("collection1")]
    public class CarServiceFactCollection : ICollectionFixture<SharedService>
    {
        //
    }

    public class CarServiceFacts
    {
        [Collection("collection1")]
        public class AddCarMethod
        {
            private CarService s;
            private FakeCarDb db;

            private ITestOutputHelper output;

            public AddCarMethod(ITestOutputHelper output, SharedService service) 
            {
                
                this.output = output;
                s = service.CarService;
                db = service.Db;

                output.WriteLine("ctor");
            }

            [Fact]
            public void AddSingleCar()
            {
                var mock = new Mock<ICarDb>();

                mock.Setup(db => db.Add(It.IsAny<Car>()))
                    .Returns((Car car) => car);

                var service = new CarService(mock.Object);

                output.WriteLine("AddSingleCar");

                var c = new Car();
                c.Make = "Honda";
                c.Model = "Jazz";
                var userId = Guid.NewGuid();

                //db.AddMethodsHasCalled = false;

                var c2 = service.AddCar(c, userId);

                Assert.NotNull(c2);
                Assert.Equal(c2.Make, c.Make);
                Assert.Equal(c2.Model, c.Model);

                mock.Verify(db => db.Add(It.IsAny<Car>()), Times.Once);

                //Assert.True(db.AddMethodsHasCalled); // เป็นทางที่ดีกว่า เนื่องจากไม่สนใจปัญหาของ GetCarsByMember(userId); 

                //(2)
                //var cars = s.GetCarsByMember(userId);

                //Assert.Equal(1, cars.Count());
                //Assert.Contains(cars, x => x.OwnerId == userId);
            }

            [Fact]
            public void Add3Cars_ThrowsException()
            {
                output.WriteLine("Add3Cars");

                var memberId = Guid.NewGuid();

                s.AddCar(new Car(), memberId);
                s.AddCar(new Car(), memberId);

                var ex = Assert.Throws<OverQuotaException>(() =>
                {
                    s.AddCar(new Car(), memberId);
                });

                ex.Message.ShouldEqual("Cannot add more car.");
            }

        }

        [Collection("collection1")]
        public class GetCarsByMemberMethod
        {
            private CarService s;

            public GetCarsByMemberMethod(SharedService service)
            {
                s = service.CarService;
            }

            [Fact]
            public void MemberCarGetOnlyHisOrHerOwnCars()
            {
                
                var member1_Id = Guid.NewGuid();
                var member2_Id = Guid.NewGuid();
                var member3_Id = Guid.NewGuid();

                s.AddCar(new Car(), member1_Id);
                s.AddCar(new Car(), member1_Id);
                
                s.AddCar(new Car(), member2_Id);
                s.AddCar(new Car(), member2_Id);

                Assert.Equal(2, s.GetCarsByMember(member1_Id).Count());
                Assert.Equal(2, s.GetCarsByMember(member2_Id).Count());
                Assert.Equal(0, s.GetCarsByMember(member3_Id).Count());

            }
        }

        [Collection("collection1")]
        public class CanAddMoreCarsMethod
        {
            private CarService s;

            public CanAddMoreCarsMethod(SharedService service)
            {
                s = service.CarService;
            }

            [Fact]
            public void MemberCanAddNotMoreThanTwoCars()
            {
                var db = new FakeCarDb();
                var s = new CarService(db);

                var member1_Id = Guid.NewGuid();

                Assert.True(s.CanAddMoreCars(member1_Id));

                s.AddCar(new Car(), member1_Id);
                Assert.True(s.CanAddMoreCars(member1_Id));

                s.AddCar(new Car(), member1_Id);
                Assert.False(s.CanAddMoreCars(member1_Id));
            }
        }
    }
}
