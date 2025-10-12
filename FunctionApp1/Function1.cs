using CosmosMongoCrud.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Function triggered.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Deserialize item (make sure Item class is accessible here)
            var item = JsonSerializer.Deserialize<Item>(requestBody);

            if (item == null)
            {
                _logger.LogWarning("Deserialized item is null.");
                var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid input.");
                return badResponse;
            }

            item.Name += " - Processed";

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(item);
            return response;
        }
    }
}
