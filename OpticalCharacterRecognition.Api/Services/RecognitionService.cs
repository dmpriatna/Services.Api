using IronOcr;
using Microsoft.AspNetCore.Http;
using OpticalCharacterRecognition.Api.Models;

namespace OpticalCharacterRecognition.Api.Services
{
    public class RecognitionService
    {
        public DocumentModel Read(IFormFile file)
        {
            var it = new IronTesseract();

            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdf(file.OpenReadStream());
                    var oRes = it.Read(input);
                    var lines = oRes.Lines;

                    return new DocumentModel
                    {
                        // Amount = lines.GetAmount(),
                        BLNumber = lines.GetBLNumber(),
                        DONumber = lines.GetDONumber()
                    };
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
