using Microsoft.WindowsAzure.Storage.Table;

namespace AzNaPratica
{
    public class Company : TableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
    }
}