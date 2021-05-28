using Google.Cloud.Vision.V1;
using IronOcr;
using Microsoft.AspNetCore.Http;
using OpticalCharacterRecognition.Api.Models;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api.Services
{
    public class RecognitionService
    {
        public async Task<string> Vision(IFormFile file)
        {
            var path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "key.json");
            var client = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = path
            }.Build();
            var source = Image.FromStream(file.OpenReadStream());
            var text = await client.DetectDocumentTextAsync(source);

            var one = text.Pages[0].Blocks[0].Paragraphs[0].Words[0].Symbols[0].Text;
            
            return text.Text;
        }

        public async Task<BillModel> ReadBL(IFormFile file)
        {
            var tesseract = new IronTesseract();
            var model = new BillModel();
            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdfPage(file.OpenReadStream(), 0);
                    var result = await tesseract.ReadAsync(input);
                    model.BLNumber = result.Lines.GetBLNumber();
                }
                return model;
            }
            catch(System.Exception e)
            {
                throw e;
            }
        }

        public async Task<DeliveryModel> ReadDO(IFormFile file)
        {
            var tesseract = new IronTesseract();
            var model = new DeliveryModel();
            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdfPage(file.OpenReadStream(), 0);
                    var result = await tesseract.ReadAsync(input);
                    model.DONumber = result.Lines.GetDONumber();
                    model.BLNumber = result.Lines.GetBLNumber();
                }
                return model;
            }
            catch(System.Exception e)
            {
                throw e;
            }
        }


        public async Task<AmountModel> ReadAmount(IFormFile file)
        {
            var tesseract = new IronTesseract();
            var model = new AmountModel();
            try
            {
                using (var input = new OcrInput())
                {
                    using (var inputPage = new OcrInput(file.OpenReadStream()))
                    {
                        var page = inputPage.PageCount();
                        input.AddPdfPage(file.OpenReadStream(), page - 1);
                        var result = await tesseract.ReadAsync(input);
                        var value = result.Lines.GetAmount();
                        if (value == null) return model;
                        string sign = value.Substring(value.Length - 3, 1);
                        if (sign == ",")
                            model.Currency = "IDR";
                        else if (sign == ".")
                            model.Currency = "USD";
                        model.Amount = result.Lines.GetAmount();
                    }
                }
                return model;
            }
            catch(System.Exception e)
            {
                throw e;
            }
        }
    }
}
