﻿using System;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs
{
    public class PayCategoryDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public decimal TotalPayDue { get; set; }
        public decimal PayInstalment1 { get; set; }
        public decimal PayInstalment2 { get; set; }
        public decimal PayInstalment3 { get; set; }
        //second and third payment fee
        public decimal PayInstalment4 { get; set; }
        public Boolean IsGlobal { get; set; }
        public long? StudentLevelId { get; set; }
        public virtual Level StudentLevel { get; set; }
        //public long? DepartmentId { get; set; }
        //public virtual Department Department { get; set; }
        public long? SessionId { get; set; }
        public virtual Session Session { get; set; }

        public virtual Semester Semester { get; set; }
        public long? SemesterId { get; set; }

        public long? DegreeId { get; set; } = 2;
        public Degree Degree { get; set; }

        public long? ProgramSetupId { get; set; }
        public ProgramSetup ProgramSetup { get; set; }

        public long? PayUploadCategoryBatchId { get; set; }
        public virtual PayUploadCategoryBatch? PayUploadCategoryBatch { get; set; }

        public PayCategoryType payCategoryType { get; set; } = PayCategoryType.Tuition;
        public EntryMode entryMode { get; set; } = EntryMode.UTME;

        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
    }
}

