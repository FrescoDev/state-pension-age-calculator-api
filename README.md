# State Pension Age calculator API

Simple RESTful API, which takes in a date of birth and a gender to calculate the corresponding state pension age.

Written in dot net core.

### Prerequisities

* dotnet cli v1+ OR Docker

### Install and Run Using DotNet CLI

1. Clone the repo and navigate to the repository's directory
2. Run: ```dotnet restore``` to install solution dependencies
3. Run: ```dotnet build``` to build the application
4. Change to the StatePensionAgeCalculatorApi project directory (should contain a .csproj file)
5. Run: ```dotnet run``` to kickstart and run the server. It will run on localhost, port 5000 as default

### Run as Docker Container

1. Clone the repo and navigate to the repository's directory
2. Run ```docker build -t state-pension-calculator-api .``` to build the docker image
3. Run  ```docker run -t -p 8080:80 state-pension-calculator-api``` to run the docker container. It will run on localhost, port 8080 as default.

## Running the Tests

1. Navigate to the root of the solution i.e. the repository's root. (Should contain .sln file)
2. Run  ```dotnet test ./test/StatePensionAgeCalculatorApi.Tests/StatePensionAgeCalculatorApi.Tests.csproj``` to run the unit tests.
3. Run  ```dotnet test ./test/StatePensionAgeCalculatorApi.Tests/StatePensionAgeCalculatorApi.FunctionalTests.csproj``` to run the functional tests.

## Request & Response Examples

### API Resources

  - [GET /api/calculation](#get-calculation)

### GET /api/calculation

Example: http://localhost:5000/api/calculation?gender=m&dateOfBirth=1985-01-01

Response body:

    {
        "statePensionAge": 68
    }

Status code: 200

Content-Type: application/json; charset=utf-8