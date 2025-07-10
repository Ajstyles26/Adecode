using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Payloads
{
    public class FlutterWHPayload
    {
        public string NotificationType { get; set; }
        public LineItem[] LineItems { get; set; }
    }
    public class LineItem
    {
        public string MandateId { get; set; }
        public string ActivationDate { get; set; }
        public string RequestId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Amount { get; set; }
    }
}
