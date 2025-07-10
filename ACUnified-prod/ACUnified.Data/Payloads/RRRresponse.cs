using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Payloads
{
    public class RRRresponse
    {
        public string? status { get; set; }
        public string? statusMessage { get; set; }
        public string? UniqueReference { get; set; }
        public string? statuscode { get; set; }
        public string?  RRR { get; set; }
    }
}
