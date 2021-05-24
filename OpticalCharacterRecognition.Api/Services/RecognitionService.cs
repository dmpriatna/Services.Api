using IronOcr;
using Microsoft.AspNetCore.Http;
using OpticalCharacterRecognition.Api.Models;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api.Services
{
    public class RecognitionService
    {
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
