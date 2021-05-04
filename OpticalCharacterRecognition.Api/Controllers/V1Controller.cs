using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpticalCharacterRecognition.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class V1Controller : ControllerBase
    {
        public V1Controller()
        {
            ES = new Services.EmailService();
            RS = new Services.RecognitionService();
            SS = new Services.StorageService();
        }

        private Services.EmailService ES { get; }

        private Services.RecognitionService RS { get; }

        private Services.StorageService SS { get; }

        [HttpPost]
        public IActionResult ReadDocument(IFormFile file)
        {
            try
            {
                var result = RS.Read(file);
                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public IActionResult SendMail([FromBody] Models.EmailModel model)
        {
            try
            {
                ES.Send(model.To, model.Subject, model.Body);
                return Ok();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                var result = SS.Upload(file);
                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet]
        public IActionResult DownloadFile(string fileName)
        {
            try
            {
                var result = SS.Download(fileName);
                return Ok(result);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
