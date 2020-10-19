using System;
using MySambu.Api.Models.Master;

namespace MySambu.Api.Models.Transaksi.BudgetItem
{
    public class BudgetItem : BaseModel
    {
        public string BudgetItemGuid { get; set; }
        public string BudgetCatGuid { get; set; }
        public long ItemSpecID { get; set; }
        public byte Status { get; set; }
        public decimal QntyDept { get; set; }
        public decimal Qnty { get; set; }
        public string CurrencyId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExchangeRateIDR { get; set; }
        public string Remark { get; set; }
        public string DeptApprovalBy { get; set; }
        public DateTime? DeptApprovalDate { get; set; }
        public string DeptApprovalRemark { get; set; }
        public string M1ApprovalBy { get; set; }
        public DateTime? M1ApprovalDate { get; set; }
        public string M1ApprovalRemark { get; set; }
        public string M2ApprovalBy { get; set; }
        public DateTime? M2ApprovalDate { get; set; }
        public string M2ApprovalRemark { get; set; }
        public string ItemIDOld { get; set; }
        public string Proposal { get; set; }

    }
}