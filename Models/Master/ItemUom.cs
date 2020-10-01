using System;

namespace MySambu.Api.Models.Master
{
    public class ItemUom
    {
        public short UOMID { get; set; }
        public short RevisionNo { get; set; }
        public string UOMName { get; set; }
        public bool NotActive { get; set; }
        public short? CompanyID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByPosition { get; set; }
        public string CreatedByVersion { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public string LastUpdatedByPosition { get; set; }
        public string LastUpdatedByVersion { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string Computer { get; set; }
        public DateTime ComputerDate { get; set; }

    }
}