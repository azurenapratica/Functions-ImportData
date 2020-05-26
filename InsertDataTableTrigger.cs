using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzNaPratica
{
    public static class InsertDataTableTrigger
    {
        [FunctionName("InsertDataTableTrigger")]
        public static void Run([QueueTrigger("data-to-import", Connection = "AzureWebJobsStorage")]string myQueueItem, 
        [Table("company")]CloudTable cloudTable, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var companies = JsonConvert.DeserializeObject<List<Company>>(myQueueItem);
            foreach (var item in companies)
            {
                var tableOperation = TableOperation.Insert(new Company(){
                    Code = item.Code,
                    Name = item.Name,
                    CNPJ = item.CNPJ,
                    PartitionKey = "Company",
                    RowKey = Guid.NewGuid().ToString()
                });
                cloudTable.ExecuteAsync(tableOperation);
            }
        }
    }
}
