using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;



namespace GuidGenerator
{
    public class GetGuid
    {
        private readonly ILogger<GetGuid> _logger;

        public GetGuid(ILogger<GetGuid> logger)
        {
            _logger = logger;
        }

        // http://localhost:7094/api/GetGuid?count=:numberOfGuidsText
        [Function("GetGuid")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Started the GetGuid Function Call");

            string? numberOfGuidsText = req.Query["count"];
            int numberOfGuids = 1;
            List<string> guids = new();

            if (numberOfGuidsText is not null && int.TryParse(numberOfGuidsText, out numberOfGuids))
            {
                // numberOfGuids = parsedNumberOfGuids;
                _logger.LogInformation(message: $"Number of GUIDs Requested: {numberOfGuids}");
            }
            else
            {
                _logger.LogInformation(message: $"Number of requested GUIDs not provided. Returning 1 GUID.");
                numberOfGuids = 1;
            }

            for(int i = 0; i < numberOfGuids; i++)
            {
                guids.Add(Guid.NewGuid().ToString());
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(guids);

            // response.Headers.Add(name: "Content-Type", value: "text/plain; charset =utf-8");
            // response.WriteStringAsync(value: "Hello World!");



            // return Task.FromResult(response);
            return response;
        }
    }
}
