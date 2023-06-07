namespace App.Data.Domain
{
    using System;
    public class DeliveryRequisitionUnitRef
    {
        public long ID { get; set; }
        public long HeaderID { get; set; }
        public string RefNo { get; set; }
        public long RefItemId { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Batch { get; set; }

        public string VeselName { get; set; }
        public string PICName { get; set; }
        public string PICHp { get; set; }
        public string VeselNoPolice { get; set; }
        public string DriverName { get; set; }
        public string DriverHp { get; set; }
        public string DANo { get; set; }
        public DateTime? PickUpPlan { get; set; }
        public DateTime? EstTimeDeparture { get; set; }
        public DateTime? EstTimeArrival { get; set; }
        public DateTime? ActTimeDeparture { get; set; }
        public DateTime? ActTimeArrival { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public string Action { get; set; }
        public string ActionDescription { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string Position { get; set; }
        public string Notes { get; set; }
        public string LogDescription { get; set; }

        public int Checked { get; set; }

        public string CustID { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string Kecamatan { get; set; }
        public string Kabupaten { get; set; }
        public string Province { get; set; }

        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Selectable { get; set; }

        public string UnitType { get; set; }

        public DeliveryRequisitionUnit CastTo()
        {

            return new DeliveryRequisitionUnit
            {
                HeaderID = this.HeaderID,
                RefNo = this.RefNo,
                RefItemId = this.RefItemId,
                Model = this.Model,
                SerialNumber = this.SerialNumber,
                Batch = this.Batch,
                VeselName = this.VeselName,
                PICName = this.PICName,
                PICHp = this.PICHp,
                VeselNoPolice = this.VeselNoPolice,
                DriverName = this.DriverName,
                DriverHp = this.DriverHp,
                DANo = this.DANo,
                PickUpPlan = this.PickUpPlan,
                EstTimeDeparture = this.EstTimeDeparture,
                EstTimeArrival = this.EstTimeArrival,
                ActTimeDeparture = this.ActTimeDeparture,
                ActTimeArrival = this.ActTimeArrival,
                Attachment1 = this.Attachment1,
                Attachment2 = this.Attachment2,
                Action = this.Action,
                Status = this.Status,
                Position = this.Position,
                Notes = this.Notes,

                CustID = this.CustID,
                CustName = this.CustName,
                CustAddress = this.CustAddress,
                Kecamatan = this.Kecamatan,
                Kabupaten = this.Kabupaten,
                Province = this.Province,

                Checked = this.Checked == 1,
                CreateBy = this.CreateBy,
                CreateDate = this.CreateDate,
                UpdateBy = this.UpdateBy,
                UpdateDate = this.UpdateDate,
            };
        }
    }
}