using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition.Api.Models
{
    public class DocumentModel
    {
        public string BLNumber { get; set; }
        public string DONumber { get; set; }
        public string Amount { get; set; }
    }

    public class AmountModel : BillModel
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }

    public class BillModel
    {
        public string BLNumber { get; set; }
    }

    public class DeliveryModel : BillModel
    {
        public string DONumber { get; set; }
    }
}
