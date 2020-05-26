using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzNaPratica
{
    public static class GetFileToImportTrigger
    {
        [FunctionName("GetFileToImportTrigger")]
        public static async Task Run([BlobTrigger("file-to-import/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, 
            [Queue("data-to-import", Connection = "AzureWebJobsStorage")]IAsyncCollector<string> queueItem, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            try{
                using (var sr = new StreamReader(myBlob))
                {
                    var companies = new List<Company>();
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        var values = line.Split(";");
                        companies.Add(new Company() { Code = values[0], Name = values[1], CNPJ = values[2] });
                    }
                    var companiesData = JsonConvert.SerializeObject(companies);
                    await queueItem.AddAsync(companiesData);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
