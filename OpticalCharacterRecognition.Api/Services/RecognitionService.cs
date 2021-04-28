using IronOcr;
using Microsoft.AspNetCore.Http;
using OpticalCharacterRecognition.Api.Models;

namespace OpticalCharacterRecognition.Api.Services
{
    public class RecognitionService
    {
        public BillOfLadingModel Read(IFormFile file)
        {
            var result = new BillOfLadingModel();
            var ss = new StorageService();
            var it = new IronTesseract();

            //ss.Upload(file);

            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdf(file.OpenReadStream());
                    var oRes = it.Read(input);
                    var words = oRes.Words;

                    result.Transporter.BLNumber = words.GetBLNumber();
                }
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return result;
        }
    }
}
