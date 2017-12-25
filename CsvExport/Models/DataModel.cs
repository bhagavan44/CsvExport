namespace CsvExport.Models
{
    internal class DataModel
    {
        public string TransId { get; set; }

        public string AccountName { get; set; }

        public string TransCode { get; set; }

        public string Notes { get; set; }

        public string CategName { get; set; }

        public string SubCategName { get; set; }

        public string PayeeName { get; set; }

        public decimal TransAmount { get; set; }

        public string TransDate { get; set; }
    }
}