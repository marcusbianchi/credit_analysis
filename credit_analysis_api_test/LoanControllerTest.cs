using System;
using System.Text.Json;
using System.Threading.Tasks;
using credit_analysis_api;
using credit_analysis_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace credit_analysis_api_test
{
    public class LoanControllerTest
    {
        [Fact]
        public void ShouldReturnLoanIDOnCreateWithModelIsValid()
        {

            var mockLoanDependency = new Mock<ILoanService>();
            mockLoanDependency.Setup(x => x.AddLoanRequestToQueue(It.IsAny<Loan>()))
                          .ReturnsAsync(Guid.NewGuid().ToString());
            var mockDbService = new Mock<IDBService>();

            var loan = new Loan
            {
                name = "Johnny Cash",
                cpf = "33399988855",
                birthdate = Convert.ToDateTime("2000-03-12"),
                amount = 1000,
                terms = 9,
                income = 50000
            };

            var loanController = new LoanController(mockLoanDependency.Object, mockDbService.Object);
            var createdResponse = loanController.Post(loan).Result;
            Assert.IsType<CreatedResult>(createdResponse);
            var resultActionValue = (createdResponse as CreatedResult).Value as Loan;
            Assert.NotNull(resultActionValue.id);
        }

        [Fact]
        public void ShouldReturnObjectOnGet()
        {

            var mockLoanDependency = new Mock<ILoanService>();

            var mockDbService = new Mock<IDBService>();
            var id = Guid.NewGuid().ToString();
            mockDbService.Setup(x => x.Read(It.IsAny<string>()))
                          .ReturnsAsync(new LoanRequest
                          {
                              id = id
                          }
            );
            var loanController = new LoanController(mockLoanDependency.Object, mockDbService.Object);
            var createdResponse = loanController.Get(id).Result;
            Assert.IsType<OkObjectResult>(createdResponse);
            var resultActionValue = (createdResponse as OkObjectResult).Value as LoanRequest;
            Assert.NotNull(resultActionValue.id);
        }


        [Fact]
        public void ShouldSameObjectOnPut()
        {

            var mockLoanDependency = new Mock<ILoanService>();
            var id = Guid.NewGuid().ToString();
            var mockDbService = new Mock<IDBService>();
            var LoanRequest = new LoanRequest
            {
                id = id
            };
            mockDbService.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<LoanRequest>()))
                          .ReturnsAsync(LoanRequest); ;

            var loanController = new LoanController(mockLoanDependency.Object, mockDbService.Object);
            var createdResponse = loanController.Put(id, LoanRequest).Result;
            Assert.IsType<OkObjectResult>(createdResponse);
            var resultActionValue = (createdResponse as OkObjectResult).Value as LoanRequest;
            Assert.NotNull(resultActionValue.id);
        }
    }
}
