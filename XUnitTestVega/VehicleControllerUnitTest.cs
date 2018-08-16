using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using vega.Controllers;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Models;
using vega.Persistence;
using Xunit;

namespace XUnitTestVega
{
    public class VehicleControllerUnitTest : IDisposable
    {
        public Mock<IVehicleRepository> MockRepository { get; set; }
        public Mock<IMapper> MockMapper { get; set; }
        public Mock<IUnitOfWork> MockUniofWrk { get; set; }
        public VehicleController Controller { get; set; }


        public VehicleControllerUnitTest()
        {
            MockUniofWrk = new Mock<IUnitOfWork>();
            MockRepository = new Mock<IVehicleRepository>();
            MockMapper = new Mock<IMapper>();
            Controller = new VehicleController(MockMapper.Object, MockRepository.Object, MockUniofWrk.Object);
        }


        [Fact]
        public async Task Test_Get_Vehicle_With_Id()
        {

            //Arrange
            var dataToSend = NewVehcile(11);

            var expectedData = VehicleResource(11);

            MockRepository.Setup(x => x.GetVechicle(11, true)).Returns(Task.FromResult(dataToSend));

            MockMapper.Setup(m => m.Map<Vehicle, VehicleResource>(It.IsAny<Vehicle>())).Returns(expectedData);


            //act
            var result = await Controller.GetVehicle(11);

            //Assert
            MockRepository.Verify(x => x.GetVechicle(11, true), Times.Once);
            var viewResult = Assert.IsType<OkObjectResult>(result);

            var expectedModel = Assert.IsAssignableFrom<VehicleResource>(viewResult.Value);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dataToSend.Id, expectedModel.Id);
        }

        [Fact]
        public async Task Test_Get_All_Vehicles()
        {
            //arrange
            var filterResource = new VehicleQueryResource
            {
                Page = 0,
                PageSize = 15
            };


            var filters = MockMapper.Object.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var dataToSend =
                new QueryResult<Vehicle> {Items = new List<Vehicle> {NewVehcile(1), NewVehcile(2), NewVehcile(3)}};

            MockRepository.Setup(x => x.GetVechicles(filters)).Returns(Task.FromResult(dataToSend));

            var expected = new QueryResultResource<VehicleResource>
            {
                Items = new List<VehicleResource> {VehicleResource(1), VehicleResource(2), VehicleResource(3)}
            };

            MockMapper.Setup(x =>
                    x.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(It.IsAny<QueryResult<Vehicle>>()))
                .Returns(expected);

            //Act
            var result = await Controller.GetVehicles(filterResource);

            //Assert

            var viewResult = Assert.IsType<QueryResultResource<VehicleResource>>(result);
            var expectedModel = Assert.IsAssignableFrom<QueryResultResource<VehicleResource>>(viewResult);
            MockRepository.Verify(x => x.GetVechicles(filters), Times.Once);

            Assert.Equal(dataToSend.Items.Count(), expectedModel.Items.Count());
        }


        [Fact]
        public async Task Test_Create_Vehicle()
        {
            //Arrange
            var dataToSend = SaveVehicle();
            
            
            var expectedData = NewVehcile(1);
           
            MockMapper.Setup(m => m.Map<SaveVehicleResource, Vehicle>(It.IsAny<SaveVehicleResource>())).Returns(expectedData);

          //  repository.Setup(x => x.Add(expectedData)).Returns(Task.CompletedTask);

            // mapper.Setup(m => m.Map<Vehicle, VehicleResource>(It.IsAny<Vehicle>())).Returns(expectedData);

            //Act
            var result = await Controller.CreateVehicle(dataToSend);

            //Assert
            
            var viewResult = Assert.IsType<OkObjectResult>(result);

            var expectedModel = Assert.IsAssignableFrom<VehicleResource>(viewResult.Value);

            Assert.Equal(1, expectedModel.Id);

        }

        [Fact]
        public async Task Test_Delete_Vehicle()
        {

            //Arrange
            var dataToSend = NewVehcile(11);

            var expectedData = VehicleResource(11);

            MockRepository.Setup(x => x.GetVechicle(It.IsAny<int>(), true)).Returns(Task.FromResult(dataToSend));
//
//           MockMapper.Setup(m => m.Map<Vehicle, VehicleResource>(It.IsAny<Vehicle>())).Returns(expectedData);


            await Controller.GetVehicle(11);

            //Act
            var result = await Controller.DeleteVehicle(11);

            //Assert

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var expectedModel = Assert.IsAssignableFrom<int>(viewResult.Value);

            Assert.Equal(11, expectedModel);


        }

        private Make NewMake(int id)
        {
            return new Make
            {
                Id = id,
                Name = "Make -" + id
            };
        }

        private Model NewModel(int id)
        {
            return new Model
            {
                Id = id,
                Name = "Model-" + id,
                MakeId = id,
                Make = NewMake(id)
            };
        }

        private VehicleFeature NewVehicleFeature(int id, int vehicleId, int featureId)
        {
            return new VehicleFeature
            {
                FeatureId = id,
                VehicleId = vehicleId,
                Feature = new Feature
                {
                    Id = featureId,
                    Name = "Feature -" + id
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

        private KeyValuePairResource KeyValuePairResource(int id, string name)
        {
            return new KeyValuePairResource
            {
                Id = id,
                Name = name
            };
        }

        private VehicleResource VehicleResource(int id)
        {
            var make = NewMake(id);
            var model = NewModel(id);
            return new VehicleResource
            {
                Id = id,
                Model = KeyValuePairResource(model.Id, model.Name),
                Make = KeyValuePairResource(make.Id, make.Name),
                Features = new List<KeyValuePairResource>
                {
                    new KeyValuePairResource
                    {
                        Id = id,
                        Name = "feaqsdqture-" + id
                    },
                    new KeyValuePairResource
                    {
                        Id = id + 1,
                        Name = "featqsdqsdure-" + id
                    }
                },
                Contact = NewContactResource("azea", "qQSD", "qsd"),
                LastUpdate = new DateTime(),
                IsRegistered = true
            };
        }

        private Vehicle NewVehcile(int id)
        {
            return new Vehicle
            {
                Id = id,
                ModelId = id,
                Model = NewModel(id),
                ContactEmail = "azeaez",
                ContactName = "azeaze",
                ContactPhone = "azeaze",
                LastUpdate = new DateTime(),
                Features = new List<VehicleFeature>
                {
                    NewVehicleFeature(id, id, id),
                },
                IsRegistered = true
            };
        }

        private SaveVehicleResource SaveVehicle()
        {
            return new SaveVehicleResource
            {
                Contact = NewContactResource("azeae", "qedqad", "qsdqsd"),
                Features = new List<int>
                {
                    1,
                    2,
                    3
                },
                IsRegistered = true,
                ModelId = 1
            };
        }

        public void Dispose()
        {
        }
    }
}