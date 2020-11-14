using System;

namespace MySambu.Api.DTO.Transaksi.PPB
{
    public class PPBBuyGetDataDto
    {
        public string PPBNo { get; set; }

        public Nullable<DateTime> PeriodAwal { get; set; }
        public Nullable<DateTime> PeriodAkhir { get; set; }
        public long BudgetCategoryID { get; set; } = 0;
        public long DeptID { get; set; } = 0;
        public string Purchaser { get; set; } = "";
        public long CategoryID { get; set; } = 0;
        public long SubCategoryID { get; set; } = 0;
        public string Item { get; set; }
        public string SupplierID { get; set; } = "";
        public long Status { get; set; } = 0;
        public string PPBNoQuery
        {
            get
            {
                if (PPBNo != "" && PPBNo != null) return " PPBNo = '" + PPBNo + "'"; return "";
            }
        }
        public string PeriodeQuery
        {
            get
            {
                if (PeriodAwal != null && PeriodAkhir != null)
                    return " TransDate Between '" + PeriodAwal.Value.ToString("yyyy-MM-01") + "' AND '" + PeriodAkhir.Value + "'";
                return "";
            }
        }
        public string BudgetQuery
        {
            get
            {
                if (BudgetCategoryID != 0) return " BudgetCategoryID = '" + BudgetCategoryID + "'"; return "";
            }
        }
        public string DeptQuery
        {
            get
            {
                if (DeptID != 0)
                    return " DeptID = '" + DeptID + "'";
                return "";
            }
        }
        public string PcsQuery
        {
            get
            {
                if(Purchaser != "")
                    return " PLGUpdatedBy = '" + Purchaser + "'";
                return "";
            }
        }
        public string ItemQuery
        {
            get
            {
                if (Item != "" && Item != null)
                    return "  ItemID LIKE '%" + Item + "%' OR ItemName Like '%" + Item + "%' OR ItemSpecID LIKE '%" + Item + "%'";
                return "";
            }
        }
        public string CategoryQuery
        {
            get
            {
                if (CategoryID != 0) return " CategoryID = " + CategoryID; return "";
            }
        }
        public string SubCategoryQuery
        {
            get
            {
                if (SubCategoryID != 0)
                    return " SubCategoryID = " + SubCategoryID;
                return "";
            }
        }
        public string SupplierQuery
        {
            get
            {
                if (SupplierID != "")
                    return " SupplierID = '" + SupplierID + "'";
                return "";
            }
        }
        public string StatusQuery
        {
            get
            {
                if (Status != 0)
                    return " Status = " + Status;
                return "";
            }
        }
    }
}