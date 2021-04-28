using System;

namespace OpticalCharacterRecognition.Api.Models
{
    public class BillOfLadingModel
    {
        public BillOfLadingModel()
        {
            Transporter = new TransporterInfo();
            Harbor = new HarborInfo
            {
                Shipper = new MajorInfo(),
                Consignee = new MajorInfo(),
                NotifyParty = new MajorInfo(),
                Cargo = new CargoInfo()
            };
        }

        public LoaderInfo Loader { get; set; }
        public TransporterInfo Transporter { get; set; }
        public HarborInfo Harbor { get; set; }
    }

    public class LoaderInfo
    {

    }

    public class TransporterInfo
    {
        public string ShipName { get; set; }
        public string BLNumber { get; set; }
        public string VoyageNumber { get; set; }
    }

    public class HarborInfo
    {
        public string PortOfLoading { get; set; }
        public string PortOfTranshipment { get; set; }
        public string PortOfDischarge { get; set; }
        public string PortOfDestination { get; set; }
        public string DeliveryAgentName { get; set; }
        public DateTime DateOfLoading { get; set; }
        public DateTime DateOfBL { get; set; }
        public MajorInfo Shipper { get; set; }
        public MajorInfo Consignee { get; set; }
        public MajorInfo NotifyParty { get; set; }
        public CargoInfo Cargo { get; set; }
    }

    public class MajorInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class CargoInfo
    {
        public long PackingAmount { get; set; }
        public string Descriptions { get; set; }
        public string ShippingMark { get; set; }
    }
}
