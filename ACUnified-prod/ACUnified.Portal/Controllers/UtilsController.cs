using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Payloads;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ACUnified.Portal.Controllers
{
    /// <summary>
    /// This endpoint provides implementation to webhooks and endpoint for exchange of data between different processing platforms
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        IRemitaNotificationRepository remitaNotificationRepository;
        IPaymentRepository paymentRepository;
        IApplicantPaymentRepository applicantPaymentRepository;
        /// <summary>
        /// Instantiate the Utility Controller 
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="_remitaNotificationRepository"></param>
        /// <param name="_paymentRepository"></param>
        /// <param name="_applicantPaymentRepository"></param>
        public UtilsController(IConfiguration _configuration,IRemitaNotificationRepository _remitaNotificationRepository, IPaymentRepository _paymentRepository, IApplicantPaymentRepository _applicantPaymentRepository)
        {
            configuration = _configuration;
            remitaNotificationRepository = _remitaNotificationRepository;
            paymentRepository = _paymentRepository;
            applicantPaymentRepository = _applicantPaymentRepository;

        }
        // GET: api/<UtilsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UtilsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// process the request coming from remita provided they have the whkey
        /// </summary>
        /// <param name="whkey"></param>
        /// <returns>ok or bad request</returns>
        [HttpGet("PayProcessWHRemita/{whkey}")]
        public async Task<IActionResult> PayProcessWHRemita(string whkey)
        {
            if (whkey == configuration.GetSection("WHKeys")["RemitaKey"])
            {
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var payload = await reader.ReadToEndAsync();

                try
                {


                    var data = JsonConvert.DeserializeObject<RemitaNotificationDto>(payload);
                    if (data != null)
                    {
                        // Process the webhook payload
                        // You still need to validate some important instances and also check if you 
                        //already have this in the database
                        //check if it exists in payment also check if it exist in remita notification before knowing what todo
                        var existingRemita=await remitaNotificationRepository.GetRemitaNotificationByOrderId(data.orderId);
                        var existPaymentList = await paymentRepository.GetAllPaymentByReferenceNo(data.orderId);
                        var existPayment = existPaymentList.FirstOrDefault();
                        var existApplicantPaymentList = await applicantPaymentRepository.GetAllPaymentByReferenceNo(data.orderId);
                        var existApplicantPayment = existApplicantPaymentList.FirstOrDefault();
                        if(existingRemita == null)
                        {
                           //create the record in remita repository
                           await  remitaNotificationRepository.CreateRemitaNotification(data);

                            if (existPayment != null)
                            {
                                if (existPayment.isSuccessful == true)
                                {
                                    //leave the transaction alone it is successful
                                }
                                else
                                {
                                    existPayment.isSuccessful = true;
                                    await paymentRepository.UpdatePayment(existPayment);
                                }
                                //update with status from platform if the status has changed
                            }
                            if (existApplicantPayment != null)
                            {
                                if (existApplicantPayment.isSuccessful == true)
                                {
                                    //leave the transaction alone it is successful
                                }
                                else
                                {
                                    existApplicantPayment.isSuccessful = true;
                                    await applicantPaymentRepository.
                                        UpdatePayment(existApplicantPayment);
                                }
                                //update with status from platform if the status has changed
                            }
                           //check the two repository if the record exist and update accordingly
                           //do not create cos the before the record was sent the record was
                           //already created with false status in the payment table
                        }
                        else
                        {
                            //check the two repository if the record exist and update accordingly based on the success or error
                            if (existPayment != null)
                            {
                                if (existPayment.isSuccessful == true)
                                {
                                    //leave the transaction alone it is successful
                                }
                                else
                                {
                                    existPayment.isSuccessful = true;
                                    await paymentRepository.UpdatePayment(existPayment);
                                }
                                //update with status from platform if the status has changed
                            }
                            if (existApplicantPayment != null)
                            {
                                if (existApplicantPayment.isSuccessful == true)
                                {
                                    //leave the transaction alone it is successful
                                }
                                else
                                {
                                    existApplicantPayment.isSuccessful = true;
                                    await applicantPaymentRepository.
                                        UpdatePayment(existApplicantPayment);
                                }
                                //update with status from platform if the status has changed
                            }
                        }
                        await remitaNotificationRepository.CreateRemitaNotification(data);
                    }
                  

                    return Ok();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    return BadRequest($"Invalid JSON payload: {ex.Message}");
                }
            }
            else
            {
                return BadRequest($"Unable to Process Invalid Key");
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whkey"></param>
        /// <param name="payparams"></param>
        /// <returns></returns>
        [HttpGet("PayProcessWHPayStack/{payparams}")]
        public string PayProcessWHPayStack(string whkey, string payparams)
        {
            //Confirm if payment is from wh
            //if from wh then check if you have the payment already if 
            //yes and failed then update to new status if no just create the payment 
            //in the database as paid
            //process if from wh if not just discard
            return "value";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="whkey"></param>
        /// <param name="payparams"></param>
        /// <returns></returns>
        [HttpGet("PayProcessWHFlutter/{payparams}")]
        public  async Task<IActionResult> PayProcessWHFlutterAsync(string whkey, string payparams)
        {

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var payload = await reader.ReadToEndAsync();

            try
            {
                var data = JsonConvert.DeserializeObject<FlutterWHPayload>(payload);
                // Process the webhook payload
                // ...

                return Ok();
            }
            catch (System.Text.Json.JsonException ex)
            {
                return BadRequest($"Invalid JSON payload: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        // POST api/<UtilsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="whkey"></param>
        /// <returns></returns>
        [HttpPost("PayNotifyWH")]
        public IActionResult ReceiveWebhook([FromBody] JsonElement payload,string whkey)
        {
            //provide different keys for the databases
            // Access the payload properties
            foreach (var item in payload.EnumerateArray())
            {
                string rrr = item.GetProperty("rrr").GetString();
                string channel = item.GetProperty("channel").GetString();
                string billerName = item.GetProperty("billerName").GetString();
                double amount = item.GetProperty("amount").GetDouble();
                // Access other properties as needed
                // ...
            }

            // Process the received payload
            // ...

            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/<UtilsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<UtilsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
