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

        public async Task<BillModel> ReadBL(string url)
        {
            var client = new System.Net.Http.HttpClient();
            var tesseract = new IronTesseract();
            var input = new OcrInput();
            try
            {
                var response = await client.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();

                input.AddPdfPage(stream, 0);
                var result = await tesseract.ReadAsync(input);

                return new BillModel
                {
                    BLNumber = result.Lines.GetBLNumber()
                };
            }
            catch(System.Exception e)
            {
                throw e;
            }
            finally
            {
                client.Dispose();
                input.Dispose();
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

        public async Task<DeliveryModel> ReadDO(string url)
        {
            var client = new System.Net.Http.HttpClient();
            var tesseract = new IronTesseract();
            var input = new OcrInput();
            try
            {
                var response = await client.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();

                input.AddPdfPage(stream, 0);
                var result = await tesseract.ReadAsync(input);

                return new DeliveryModel
                {
                    BLNumber = result.Lines.GetBLNumber(),
                    DONumber = result.Lines.GetDONumber()
                };
            }
            catch(System.Exception e)
            {
                throw e;
            }
            finally
            {
                client.Dispose();
                input.Dispose();
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

        public async Task<AmountModel> ReadAmount(string url)
        {
            var client = new System.Net.Http.HttpClient();
            var tesseract = new IronTesseract();
            var model = new AmountModel();
            try
            {
                var response = await client.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                using (var input = new OcrInput())
                {
                    using (var inputPage = new OcrInput(stream))
                    {
                        var page = inputPage.PageCount();
                        input.AddPdfPage(stream, page - 1);
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
            finally
            {
                client.Dispose();
            }
        }
    }
}
