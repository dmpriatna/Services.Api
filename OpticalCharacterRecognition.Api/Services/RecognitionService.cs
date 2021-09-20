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
            System.Diagnostics.Debug.WriteLine(null);
            System.Diagnostics.Debug.WriteLine("".PadRight(100, '*'));
            System.Diagnostics.Debug.WriteLine($"reading start\t\t{System.DateTime.Now}");
            var tesseract = new IronTesseract();
            var model = new BillModel();
            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdfPage(file.OpenReadStream(), 0);
                    var result = await tesseract.ReadAsync(input);
                    model.BLNumber = result.GetBLNumber();
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
            System.Diagnostics.Debug.WriteLine(null);
            System.Diagnostics.Debug.WriteLine("".PadRight(100, '*'));
            System.Diagnostics.Debug.WriteLine($"reading start\t\t{System.DateTime.Now}");
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
                    BLNumber = result.GetBLNumber()
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
            System.Diagnostics.Debug.WriteLine(null);
            System.Diagnostics.Debug.WriteLine("".PadRight(100, '*'));
            System.Diagnostics.Debug.WriteLine($"reading start\t\t{System.DateTime.Now}");
            var tesseract = new IronTesseract();
            var model = new DeliveryModel();
            try
            {
                using (var input = new OcrInput())
                {
                    input.AddPdfPage(file.OpenReadStream(), 0);
                    var result = await tesseract.ReadAsync(input);
                    model.DONumber = result.GetDONumber();
                    model.BLNumber = result.GetBLNumber();
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
            System.Diagnostics.Debug.WriteLine(null);
            System.Diagnostics.Debug.WriteLine("".PadRight(100, '*'));
            System.Diagnostics.Debug.WriteLine($"reading start\t\t{System.DateTime.Now}");
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
                    BLNumber = result.GetBLNumber(),
                    DONumber = result.GetDONumber()
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
            System.Diagnostics.Debug.WriteLine(null);
            System.Diagnostics.Debug.WriteLine("".PadRight(100, '*'));
            System.Diagnostics.Debug.WriteLine($"reading start\t\t{System.DateTime.Now}");
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
                        var value = result.GetAmount();
                        if (value == null) return model;
                        string sign = value.Substring(value.Length - 3, 1);
                        if (sign == ",")
                            model.Currency = "IDR";
                        else if (sign == ".")
                            model.Currency = "USD";
                        model.Amount = value;
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
            System.Diagnostics.Debug.WriteLine(null);
            System.Diagnostics.Debug.WriteLine("".PadRight(100, '*'));
            System.Diagnostics.Debug.WriteLine($"reading start\t\t{System.DateTime.Now}");
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
                        var value = result.GetAmount();
                        if (value == null) return model;
                        string sign = value.Substring(value.Length - 3, 1);
                        if (sign == ",")
                            model.Currency = "IDR";
                        else if (sign == ".")
                            model.Currency = "USD";
                        model.Amount = result.GetAmount();
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
