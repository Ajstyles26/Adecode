using ACUnified.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Models
{
    public class PayUploadCategoryBatchDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ProcessStatus FeeUploadStatus { get; set; }
        public ProcessStatus FeeGenerationStatus { get; set; }
    }
}
