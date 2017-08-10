using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Xunit;
using Newtonsoft.Json;
using StatePensionAgeCalculatorApi.Models;

namespace StatePensionAgeCalculatorApi.FunctionalTests.StatePensionAgeCalculationEnpoint
{
    public class InvalidFormatGetCalculationRequestShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public InvalidFormatGetCalculationRequestShould()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Theory()]
        [InlineData("gender=mmae&dateOfBirth=1945-01-01")]
        [InlineData("gender=female&dateOfBirth=1945-01-01")]
        [InlineData("dateOfBirth=1945-01-01")]
        [InlineData("gender=m")]
        [InlineData("gender=dmdm&dateOfBirth=1945-01-01")]
        [InlineData("gender=dateOfBirth=1945-01-01")]
        [InlineData("genders=mdateOfBirth=1945-01-01")]
        [InlineData("genders=mdateOfBirth=1945-01-01p")]
        [InlineData("gender=mdateOfBirth=99999-0-91-01")]
        [InlineData("genders=1945-01-01")]
        [InlineData("")]
        public async Task Return400BadRequestResponse(string queryString)
        {
            // Act
            var response = await _client.GetAsync($"/api/calculation?{queryString}");

            // Assert
            Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}