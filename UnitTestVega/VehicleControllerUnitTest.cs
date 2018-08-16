using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vega.Controllers;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Models;

namespace UnitTestVega
{
    [TestClass]
    public class VehicleControllerUnitTest
    {
        public Mock<IVehicleRepository> repository { get; set; }
        public Mock<IMapper> mapper { get; set; }
        public Mock<IUnitOfWork> unitWorkMock { get; set; }

        [TestInitialize]
        public void Intialize()
        {
            repository = new Mock<IVehicleRepository>();
            mapper = new Mock<IMapper>();
            unitWorkMock = new Mock<IUnitOfWork>();
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public async Task Test_GetVehicle_with_id()
        {

            var newVehcile = NewVehcile(1);
            var maped = new VehicleResource();

            repository.Setup(x => x.GetVechicle(1, false)).Returns(Task.FromResult(newVehcile));

            mapper.Setup(m => m.Map<Vehicle, VehicleResource>(newVehcile)).Returns(maped);

            //arrange
            var controller = new VehicleController(mapper.Object, repository.Object, unitWorkMock.Object);

         
            //act
            var result = await controller.GetVehicle(1);


            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            Console.WriteLine(okResult.Value);
            var vehicle = okResult.Value.Should().BeAssignableTo<Vehicle>().Subject;

            vehicle.Id.Should().Be(16);
        }

        [TestMethod]
        public async Task Test_GetVehicles()
        {
           
            var filterResource = new VehicleQueryResource
            {
                Page = 0,
                PageSize = 15
            };

            var filters = mapper.Object.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var listItems =
                new QueryResult<Vehicle> {Items = new List<Vehicle> {NewVehcile(1), NewVehcile(2), NewVehcile(3)}};

            repository.Setup(x => x.GetVechicles(filters)).Returns(Task.FromResult(listItems));

            var maped = new QueryResultResource<VehicleResource>();
            mapper.Setup(m => m.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(listItems))
                .Returns(maped);
            //arrange
            var controller = new VehicleController(mapper.Object, repository.Object, unitWorkMock.Object);

            var result = await controller.GetVehicles(filterResource);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var vehicleSubject = okResult.Value.Should().BeAssignableTo<Task<QueryResultResource<VehicleResource>>>()
                .Subject;

            vehicleSubject.Result.Items.Count().Should().Be(3);
        }


        private Make NewMake(int id)
        {
            return new Make
            {
                Id = id,
                Name = "Make -" + id
            };
        }
        private Model NewModel(int id, string MakeName)
        {
            return new Model
            {
                Id = id,
                Name = MakeName,
                MakeId = id,
                Make = NewMake(id)

            };
        }

        private VehicleFeature NewVehicleFeature(int id, int vehicleId, int featureId, string featuresName)
        {
            return new VehicleFeature
            {
                FeatureId = id,
                VehicleId = vehicleId,
                Feature = new Feature
                {
                    Id = featureId,
                    Name = featuresName
                }
            };
        }

        private SaveVehicleResource.ContactResource NewContactResource(string name, string phone, string email)
        {
            return new SaveVehicleResource.ContactResource
            {
                Name = name,
                Phone = phone,
                Email = email
            };
        }

        private Vehicle NewVehcile(int id)
        {
            return new Vehicle
            {
                Id = id,
                ModelId = id,
                Model = NewModel(id, "Model" + "-1"),
                ContactEmail = "azeaez",
                ContactName = "azeaze",
                ContactPhone = "azeaze",
                LastUpdate = new DateTime(),
                Features = new List<VehicleFeature>
                {
                    NewVehicleFeature(id, id, id, "Feautre" + "-1"),
                },
                IsRegistered = true
            };
        }
    }
}