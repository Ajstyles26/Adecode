using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Payloads
{
    public class GenerateRRRRequest
    {
        public GenerateRRRRequest() { }

        public string serviceTypeId { get; set; }

        public string amount { get; set; }

        public string orderId { get; set; }

        public string payerName { get; set; }

        public string payerEmail { get; set; }

        public string payerPhone { get; set; }

        public string description { get; set; }

       // public List<CustomField> customFields { get; set; }
    }
}
