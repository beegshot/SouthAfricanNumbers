using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SouthAfricanNumbers.Server.Controllers;
using SouthAfricanNumbers.Server.Data;
using SouthAfricanNumbers.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SouthAfricanNumbers.UnitTests
{
    public class NumberListControllerTest
    {
        private DbContextOptions<DataContext> contextOptions;

        public NumberListControllerTest()
        {
            //create In Memory Database
            contextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "NumberDataBase").Options;
        }

        [Fact]
        public void GetAllTest()
        {
            //Arrange
            DataContext context = new DataContext(contextOptions);
            context.Numbers.Add(new Number
            {
                Id = Guid.Parse("d3f9a7ec-fc74-424c-b948-45a962cd2736"),
                PhoneNumber = "11111111111"
            });

            context.Numbers.Add(new Number
            {
                Id = Guid.Parse("1aadfac6-3348-425b-8ab8-75b3c767fa07"),
                PhoneNumber = "99999999999"
            });
            context.SaveChanges();
            NumberListController _controller = new NumberListController(context);

            //Act
            var result = _controller.GetAllNumber();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Number>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }


        [Fact]
        public void FormatNumberTest()
        {
            //Arrange
            DataContext context = new DataContext(contextOptions);
            context.Numbers.Add(new Number
            {
                Id = Guid.Parse("a3f9a7ec-fc74-424c-b948-45a962cd2736"),
                PhoneNumber = "12345678978"
            });

            context.Numbers.Add(new Number
            {
                Id = Guid.Parse("9aadfac6-3348-425b-8ab8-75b3c767fa07"),
                PhoneNumber = "98765432112"
            });
            context.SaveChanges();
            NumberListController _controller = new NumberListController(context);

            Number numberNew = new Number { Id = Guid.NewGuid(), PhoneNumber = "12345678999" };

            Number numberDuplicate = new Number { Id = Guid.NewGuid(), PhoneNumber = "12345678978" };

            Number numberUpdate = new Number { Id = Guid.Parse("a3f9a7ec-fc74-424c-b948-45a962cd2736"), PhoneNumber = "12345678979" };

            Number numberFormatA = new Number { Id = Guid.NewGuid(), PhoneNumber = "_DELETED_12345678111" };
            Number numberFormatB = new Number { Id = Guid.NewGuid(), PhoneNumber = "_DELETED_123456789222" };
            Number numberFormatC = new Number { Id = Guid.NewGuid(), PhoneNumber = "12345678333_DELETED_12345678979" };
            Number numberFormatD = new Number { Id = Guid.NewGuid(), PhoneNumber = "1234567444_DELETED_12345678979" };
            Number numberFormatE = new Number { Id = Guid.NewGuid(), PhoneNumber = "123456789555_DELETED_12345678979" };
            Number numberFormatF = new Number { Id = Guid.NewGuid(), PhoneNumber = "Antani" };

            //Act
            var resultNew = _controller.AddNumber(numberNew);
            var resultDuplicate = _controller.AddNumber(numberDuplicate);
            var resultUpdate = _controller.AddNumber(numberUpdate);
            var resultFormatA = _controller.AddNumber(numberFormatA);
            var resultFormatB = _controller.AddNumber(numberFormatB);
            var resultFormatC = _controller.AddNumber(numberFormatC);
            var resultFormatD = _controller.AddNumber(numberFormatD);
            var resultFormatE = _controller.AddNumber(numberFormatE);
            var resultFormatF = _controller.AddNumber(numberFormatF);

            //Assert
            var ResponseNew = Assert.IsType<Task<NumberResponse>>(resultNew);
            var ResponseDuplicate = Assert.IsType<Task<NumberResponse>>(resultDuplicate);
            var ResponseUpdate = Assert.IsType<Task<NumberResponse>>(resultUpdate);
            var ResponseFormatA = Assert.IsType<Task<NumberResponse>>(resultFormatA);
            var ResponseFormatB = Assert.IsType<Task<NumberResponse>>(resultFormatB);
            var ResponseFormatC = Assert.IsType<Task<NumberResponse>>(resultFormatC);
            var ResponseFormatD = Assert.IsType<Task<NumberResponse>>(resultFormatD);
            var ResponseFormatE = Assert.IsType<Task<NumberResponse>>(resultFormatE);
            var ResponseFormatF = Assert.IsType<Task<NumberResponse>>(resultFormatF);

            Assert.Equal("created", ResponseNew.Result.message);
            Assert.Equal("duplicated", ResponseDuplicate.Result.message);
            Assert.Equal("updated", ResponseUpdate.Result.message);
            Assert.Equal("created - Removed _DELETED_ entry", ResponseFormatA.Result.message);
            Assert.Equal("wrong format", ResponseFormatB.Result.message);
            Assert.Equal("created - Removed concatenated _DELETED_ entry", ResponseFormatC.Result.message);
            Assert.Equal("wrong format", ResponseFormatD.Result.message);
            Assert.Equal("wrong format", ResponseFormatE.Result.message);
            Assert.Equal("wrong format", ResponseFormatF.Result.message);
        }
    }
}