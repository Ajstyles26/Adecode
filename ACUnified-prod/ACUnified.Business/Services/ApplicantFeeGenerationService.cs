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
    public class ApplicantFeeGenerationService : IApplicantFeeGenerationService
    {
       
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



        public ApplicantFeeGenerationService(
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
