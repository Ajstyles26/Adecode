using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;

namespace ACUnified.Business.Services
{
    public class StudentFeeGenerationService : IStudentFeeGenerationService
    {
        IStudentEnrolmentRepository _studentEnrolmentRepository;
        IDegreeRepository _degreeRepository;
        ILevelRepository _levelRepository;
        IProgramSetupRepository _programSetupRepository;
        IPayUploadConcessionRepository _payUConcessionRepository;
        IPayUploadCategoryRepository _payUCategoryRepository;
        IPayUploadDetailsRepository _payUploadDetailsRepository;
        IPayUploadCategoryBatchRepository _payUploadCategoryBatchRepository;

        //The Main table
        IPayConcessionRepository _payConcessionRepository;
        IPayCategoryRepository _payCategoryRepository;
        IPayDetailsRepository _payDetailsRepository;

        private readonly IMapper _mapper;

        private static readonly string[] Ones = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private static readonly string[] Teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] Tens = { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static readonly string[] Suffixes = { "", "Thousand", "Million", "Billion", "Trillion" };



        public StudentFeeGenerationService(IStudentEnrolmentRepository studentEnrolmentRepository,
            IPayUploadConcessionRepository payUConcessionRepository,
            IPayUploadCategoryRepository payUCategoryRepository,
            IPayUploadDetailsRepository payUploadDetailsRepository,
            IPayUploadCategoryBatchRepository payUploadCategoryBatchRepository,
            IDegreeRepository degreeRepository,
            ILevelRepository levelRepository,
            IProgramSetupRepository programSetupRepository,
             //Main 
             IPayConcessionRepository payConcessionRepository,
             IPayCategoryRepository payCategoryRepository,
             IPayDetailsRepository payDetailsRepository,
             IMapper mapper
            )
        {

            _studentEnrolmentRepository = studentEnrolmentRepository;
            _payUConcessionRepository = payUConcessionRepository;
            _payUCategoryRepository = payUCategoryRepository;
            _payUploadDetailsRepository = payUploadDetailsRepository;
            _payUploadCategoryBatchRepository = payUploadCategoryBatchRepository;
            _degreeRepository = degreeRepository;
            _levelRepository = levelRepository;
            _programSetupRepository = programSetupRepository;



            //Main
            _payConcessionRepository = payConcessionRepository;
            _payDetailsRepository = payDetailsRepository;
            _payCategoryRepository = payCategoryRepository;
            _mapper = mapper;

        }
        public async Task GenerateFee(long categoryId)
        {
            //Declare PayUploadDetails
            //clear existing batch records for students
            _payUploadDetailsRepository.DeletePayUploadDetailsByCategoryBatch(categoryId);

            List<PayUploadDetailsDto> PayUploadListing = new List<PayUploadDetailsDto>();
            //Get All degree
            var StudentDegreeList = await _degreeRepository.GetAllDegree();
            //process by the program id
            var ProgramListing = await _programSetupRepository.GetAllProgramSetup();
            //for each degree calculate the fees for all the student level
            //student by the level iteration thgough the pay upload category
            var StudentLevel = await _levelRepository.GetAllLevel();
            foreach (var studentDegree in StudentDegreeList)
            {

                foreach (var program in ProgramListing)
                {

                    //process payment for each student level
                    foreach (var sLevel in StudentLevel)
                    {
                        //process all mandatory fees and non mandatory fees together
                        var PayUploadCategory = await _payUCategoryRepository.GetPayUploadCategoryByBDPL(categoryId, studentDegree.Id, program.Id, sLevel.Id);
                        //fetch all student within the types of entry mode we have in the PayUploadCategory
                        var EntryList = PayUploadCategory.GroupBy(x => x.entryMode).Select(group => group.Key).ToList();
                        foreach (Data.Enum.EntryMode entryitem in EntryList)
                        {
                            
                            //degree,program,level,entrymode
                            //fetch all student with this and create pay details for them
                            var currentStudent=await  _studentEnrolmentRepository.GetAllStudentEnrolmentByDPLE(studentDegree.Id, program.Id, sLevel.Id, entryitem);
                            foreach (var student in currentStudent)
                            {
                                var NormalFee = PayUploadCategory.FirstOrDefault();
                                if (NormalFee != null)
                                {
                                    // Transform each student into payment details using the new method
                                    var currentPayDetails = TransformStudentToPayDetails        (student,0,NormalFee.TotalPayDue,NormalFee.TotalPayDue,NormalFee.Id,NormalFee.Name,NormalFee.PayInstalment1,NormalFee.PayInstalment2,NormalFee.PayInstalment3,NormalFee.PayInstalment4, categoryId, NormalFee.SessionId.Value, entryitem);

                                    // Add the payment details to the list
                                    PayUploadListing.Add(currentPayDetails);
                                }
                              

                               
                            }
                        }
                    }
                }


                //get payment for the batch 






                //var unclassifiedPayCategory = PayUploadCategory.Where(x => x.TotalPayDue >= 0 && x.IsGlobal == false && x.StudentLevelId == 9999 && x.DegreeId == 9999).FirstOrDefault();
                //foreach (var item in StudentByDegree)
                //{
                //    new Instance of PayUploadDetails
                //    PayUploadDetailsDto currentPayDetails = new PayUploadDetailsDto();
                //    Get PayCategory Information
                //    var currentPayCategory = PayUploadCategory.Where(x => x.SessionId == sessionId
                //    && x.StudentLevelId == item.LevelId
                //    && x.ProgramSetupId == item.ProgramSetupId
                //    && x.entryMode == item.Student.StudentEntryMode
                //    && x.DegreeId == item.DegreeId
                //    ).FirstOrDefault();

                //    if (currentPayDetails == null)
                //    {
                //        currentPayDetails = new PayUploadDetailsDto
                //        {
                //            ConcessionAmount = 0,
                //            IsForCourseRegistration = false,
                //            isLateFee = false,
                //            Name = "Unspecified",
                //            OriginalAmount = 0,
                //            PartAllowed = false,
                //            PayInstalment1 = 0,
                //            PayInstalment2 = 0,
                //            PayInstalment3 = 0,
                //            PayInstalment4 = 0,
                //            TotalAmount = 0,
                //            StudentId = item.StudentId,
                //            SessionId = sessionId,
                //            PayUploadCategoryBatchId = categoryId,
                //            PayUploadCategoryId = unclassifiedPayCategory.Id
                //        };
                //    }
                //    if (currentPayCategory == null)
                //    {
                //        currentPayCategory = unclassifiedPayCategory;
                //    }
                //    Also get other fees for the program
                //    var GenericFees = PayUploadCategory.Where(x => x.StudentLevelId == 9999 && x.SessionId == sessionId && x.PayUploadCategoryBatchId == categoryId && x.entryMode == item.Student.StudentEntryMode);
                //    currentPayDetails.StudentId = item.Id;

                //    currentPayDetails.ConcessionAmount = 0;
                //    currentPayDetails.OriginalAmount = currentPayCategory.TotalPayDue;
                //    currentPayDetails.TotalAmount = currentPayDetails.OriginalAmount;
                //    currentPayDetails.PayUploadCategoryId = currentPayCategory.Id;
                //    currentPayDetails.Name = currentPayCategory.Name;
                //    currentPayDetails.PayInstalment1 = currentPayCategory.PayInstalment1;
                //    currentPayDetails.PayInstalment2 = currentPayCategory.PayInstalment2;
                //    currentPayDetails.PayInstalment3 = currentPayCategory.PayInstalment3;
                //    currentPayDetails.PayInstalment4 = currentPayCategory.PayInstalment4;
                //    currentPayDetails.PayUploadCategoryBatchId = categoryId;
                //    currentPayDetails.SessionId = sessionId;







                //    add the listing to the PayUploadListing
                //    PayUploadListing.Add(currentPayDetails);

                //    foreach (var item2 in GenericFees)
                //    {
                //        //add this to the paydetails
                //        var GenericPDetails = new PayUploadDetailsDto();
                //        GenericPDetails.IsForCourseRegistration = true;
                //        GenericPDetails.PartAllowed = false;
                //        GenericPDetails.TotalAmount = item2.TotalPayDue;
                //        GenericPDetails.PayUploadCategoryId = item2.Id;
                //        GenericPDetails.Name = item2.Name;
                //        GenericPDetails.StudentId = item.Id;
                //        GenericPDetails.PayInstalment1 = 0;
                //        GenericPDetails.PayInstalment2 = 0;
                //        GenericPDetails.PayInstalment3 = 0;
                //        GenericPDetails.PayInstalment4 = 0;
                //        GenericPDetails.PayUploadCategoryBatchId = categoryId;
                //        GenericPDetails.SessionId = sessionId;
                //        //add other fees to PayUploadListing
                //        PayUploadListing.Add(GenericPDetails);
                //    }

                //}
            }
            //Get all student information
           

            
            

            
           

            _payUploadDetailsRepository.CreatePayUploadDetails(PayUploadListing);
        }
        private PayUploadDetailsDto TransformStudentToPayDetails(StudentEnrolmentDto student, decimal currentConcessionAmount,decimal originalAmount, decimal totalAmount,
            long PayUploadCategoryId, string name,decimal instalment1,decimal instalment2,decimal instalment3, decimal instalment4,long categoryBatchId,long sessionId,Data.Enum.EntryMode entryMode)
        {
            return new PayUploadDetailsDto
            {
                ConcessionAmount = currentConcessionAmount,
                OriginalAmount = originalAmount,
                TotalAmount = totalAmount,
                PayUploadCategoryId = PayUploadCategoryId,
                Name = name,
                PayInstalment1 = instalment1,
                PayInstalment2 = instalment2,
                PayInstalment3 = instalment3,
                PayInstalment4 = instalment4,
                PayUploadCategoryBatchId = categoryBatchId,
                SessionId = sessionId,
                StudentId=student.Id,
                // Initialize other properties as needed
            };
        }
        public async Task FinaliseFeeGeneration(long BatchId)
        {
            //Get batch from the PayUploadCategoryBatch
            //Get the corresponding PayUploadCategory write this to PayCategory
            //Get the corresponding PayUploadDetails write this to PayDetails
            //var currentBatch = _payUploadCategoryBatchRepository.GetPayUploadCategoryBatch(BatchId);
            var currentCategory = await _payUCategoryRepository.GetPayUploadCategoryBatch(BatchId);
            var currentDetails=await _payUploadDetailsRepository.GetPayUploadDetailsBatch(BatchId);
            var currentConcession=await _payUConcessionRepository.GetPayUploadConcessionBatch(BatchId);
            //move this data to the MainTable
            var BatchCategoryMove= _mapper.Map<IEnumerable<PayUploadCategoryDto>, IEnumerable<PayCategoryDto>>(currentCategory);

            var BatchDetailMove = _mapper.Map<IEnumerable<PayUploadDetailsDto>, IEnumerable<PayDetailsDto>>(currentDetails);


            var BatchConcessionMove = _mapper.Map<IEnumerable<PayUploadConcessionDto>, IEnumerable<PayConcessionDto>>(currentConcession);

            //save all information
            try
            {
                //check if the batch exists already in the database if yes drop that and include this
                if(BatchCategoryMove.Count()>0)
                {
                    await _payCategoryRepository.CreatePayCategory(BatchCategoryMove);
                }
                if (BatchDetailMove.Count()>0)
                {
                    await _payDetailsRepository.CreatePayDetails(BatchDetailMove);
                }
                if (BatchConcessionMove.Count()>0)
                {
                   await _payConcessionRepository.CreatePayConcession(BatchConcessionMove);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error at "+ex.InnerException);
            }
        }

        public static string NumberToWords(long number)
        {
            if (number == 0)
                return Ones[0];

            int level = 0;
            string result = string.Empty;
            long remainder = 0;

            while (number > 0)
            {
                remainder = number % 1000;
                number /= 1000;

                if (remainder > 0)
                {
                    string words = GetWordsForRange(remainder);
                    result = string.IsNullOrEmpty(result) ? words : $"{words} {Suffixes[level]} {result}";
                }

                level++;
            }

            return result.Trim();
        }

        private static string GetWordsForRange(long number)
        {
            if (number == 0)
                return string.Empty;

            if (number < 10)
                return Ones[(int)number];

            if (number < 20)
                return Teens[(int)number - 10];

            if (number < 100)
                return $"{Tens[(int)number / 10 - 2]} {GetWordsForRange(number % 10)}";

            if (number < 1000)
                return $"{Ones[(int)number / 100]} Hundred {GetWordsForRange(number % 100)}";

            return string.Empty;
        
        }
    }
}
