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
    public class ProperlyFormattedGetCalculationRequestShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ProperlyFormattedGetCalculationRequestShould()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Return200StatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/calculation?gender=f&dateOfBirth=1985-01-01");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ReturnTheExpectedResponse()
        {
            // Act
            var response = await _client.GetAsync("/api/calculation?gender=m&dateOfBirth=1945-01-01");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<CalculationGetResponse>(content);

            // Assert
            Assert.Equal(responseObject.StatePensionAge, 65);
        }
    }
}