using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Payloads
{
    public class PaymentResponse
    {
        public decimal? amount { get; set; }
        public string RRR { get; set; }
        public string orderId { get; set; }
        public string message { get; set; }
        public string paymentDate { get; set; }
        public string transactiontime { get; set; }
        public string status { get; set; }
    }
}
