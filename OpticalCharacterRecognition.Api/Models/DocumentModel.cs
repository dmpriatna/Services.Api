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
        public double Amount { get; set; }
    }
}
