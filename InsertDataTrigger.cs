using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System;

namespace AzNaPratica
{
    public static class InsertDataTrigger
    {
        [FunctionName("InsertDataTrigger")]
        public static void Run([QueueTrigger("data-to-import", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var companies = JsonConvert.DeserializeObject<List<Company>>(myQueueItem);
            var strSQL = string.Empty; 

            foreach (var item in companies)
            {
                strSQL += $"INSERT INTO Company (Code, Name, CNPJ) VALUES ('{ item.Code }', '{ item.Name }', '{ item.CNPJ }');";
            }

            var strConn = Environment.GetEnvironmentVariable("SQLConnectionString");

            using (var conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (var cmd = new SqlCommand(strSQL, conn))
                {
                    var rows = cmd.ExecuteNonQuery();
                    log.LogInformation($"Rows inserted: {rows}");
                }
            }
        }
    }
}
