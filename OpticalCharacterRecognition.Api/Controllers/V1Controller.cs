using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpticalCharacterRecognition.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class V1Controller : ControllerBase
    {
        [HttpPost]
        public Models.BillOfLadingModel ReadDocument(IFormFile file)
        {
            var rs = new Services.RecognitionService();
            return rs.Read(file);
        }

        [HttpPost]
        public IActionResult SendMail([FromBody] Models.EmailModel model)
        {
            var es = new Services.EmailService();
            es.Send(model.To, model.Subject, model.Body);
            return Ok();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            var ss = new Services.StorageService();
            ss.Upload(file);
            return Ok();
        }
    }
}
