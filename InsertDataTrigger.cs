using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System;
using Dapper;

namespace AzNaPratica
{
    public static class InsertDataTrigger
    {
        [FunctionName("InsertDataTrigger")]
        public static void Run([QueueTrigger("data-to-import", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var companies = JsonConvert.DeserializeObject<List<Company>>(myQueueItem);

            var strConn = Environment.GetEnvironmentVariable("SQLConnectionString");

            using(var connection = new SqlConnection(strConn))
            {
                connection.Open();
                connection.Execute("INSERT INTO [Company] (Code, Name, CNPJ) VALUES(@Code, @Name, @CNPJ)", companies); 
            }
        }
    }
}
