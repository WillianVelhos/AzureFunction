using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace TheFirst
{
    public static class OnPaymenReceived
    {
        [FunctionName("OnPaymenReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("orders", Connection = "AzureWebJobsStorage")] IAsyncCollector<Order> orderQueue,
            [Table("orders")] IAsyncCollector<Order> orderTable,
            ILogger log)
        {
            log.LogInformation("Received a payment.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            await orderQueue.AddAsync(order);

            order.PartitionKey = "orders";
            order.RowKey = order.Id;

            await orderTable.AddAsync(order);

            log.LogInformation($"Order {order.Id} received from {order.Email} for peoduct {order.ProductId}");

            return new OkObjectResult($"Deu Bom");
        }
    }

    public class Order
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string Id { get; set; }

        public int ProductId { get; set; }

        public string Email { get; set; }
    }
}
