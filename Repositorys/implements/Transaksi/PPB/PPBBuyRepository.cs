using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySambu.Api.DTO.Transaksi.PPB;
using MySambu.Api.helper;
using MySambu.Api.Repositorys.Interfaces.Transaksi;

namespace MySambu.Api.Repositorys.implements
{
    internal class PPBBuyRepository : BaseRepository, IPbbBuyRepository
    {
        public PPBBuyRepository(IDbTransaction transaction) : base(transaction)
        {
            
        }

        public async Task<List<PPBBuyDto>> GetListPPBUy(PPBBuyGetDataDto dt)
        {
            string sql = "";
            
            sql = SqlQuery.SetCondition(sql, dt.PPBNoQuery);
            sql = SqlQuery.SetCondition(sql, dt.PeriodeQuery);
            sql = SqlQuery.SetCondition(sql, dt.BudgetQuery);
            sql = SqlQuery.SetCondition(sql, dt.DeptQuery);
            sql = SqlQuery.SetCondition(sql, dt.PcsQuery);
            sql = SqlQuery.SetCondition(sql, dt.ItemQuery);
            sql = SqlQuery.SetCondition(sql, dt.CategoryQuery);
            sql = SqlQuery.SetCondition(sql, dt.SubCategoryQuery);
            sql = SqlQuery.SetCondition(sql, dt.SupplierQuery);
            sql = SqlQuery.SetCondition(sql, dt.StatusQuery);

            var bhdr = new Dictionary<string, PPBBuyDto>();
            await Connection.QueryAsync<PPBBuyDto, PPBBuySupplierDto, PPBBuyDto>(
                "pTrn_GetPPBBuyPLG", (hdr, dtl) => {
                    PPBBuyDto biHdr;

                    if (!bhdr.TryGetValue(hdr.PPBGUID.ToString(), out biHdr))
                    {
                        biHdr = hdr;
                        biHdr.SupplierPrice = new List<PPBBuySupplierDto>();
                        bhdr.Add(biHdr.PPBGUID, biHdr);
                    }

                    biHdr.SupplierPrice.Add(dtl);

                    return biHdr;
                }, splitOn: "PriceID", param: new{sqlstatement=sql}, commandType: CommandType.StoredProcedure, transaction: Transaction);

            return bhdr.Values.ToList();
            // sql = "Select * FROM vTrn_PPBBuyPLG WHERE RequestStatus in (3,4) " + sql;

            // var data = await Connection.QueryAsync<PPBBuyDto>(sql, transaction:Transaction);

            // return data.ToList();

        }

        public async Task Save(List<PPBBuySaveDto> dt)
        {
            foreach(var d in dt){
                await Connection.QueryAsync("pTrn_PPBBuySave", new
                {
                    PPBDtlBuyGUID = d.PPBDtlBuyGUID,
                    PPBDtlRequestGUID = d.PPBDtlRequestGUID,
                    BuyDNo = d.BuyDNo,
                    ItemID = d.ItemID,
                    ItemSpecID = d.ItemSpecID,
                    QntyBuy = d.QntyBuy,
                    SupplierID = d.SupplierID,
                    CurrencyID = d.CurrencyID,
                    UnitPrice = d.UnitPrice,
                    ExchangeRateIDR = d.ExchangeRateIDR,
                    PPHHdrGUID = d.PPHHdrGUID,
                    DeliveryDate = d.DeliveryDate,
                    Remark = d.Remark,
                    Status = d.Status,
                    UserID = d.UserID,
                    Computer = d.Computer,
                    Flag = 0
                }, commandType: CommandType.StoredProcedure, transaction: Transaction);
            }
        }
    }
}