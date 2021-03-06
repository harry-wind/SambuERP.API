using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MySambu.Api.DTO.Master;
using MySambu.Api.Models.Master;
using MySambu.Api.Repositorys.Interfaces;

namespace MySambu.Api.Repositorys.implements
{
    internal class ItemRepository : BaseRepository, IItemRepository
    {
        // private IDbTransaction _transaction;

        public ItemRepository(IDbTransaction transaction) : base(transaction)
        {
            // _transaction = transaction;
        }

        public Task Delete(Item obj)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string id, string by)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            // return await Connection.GetAllAsync<Item>(transaction:Transaction);
            return await Connection.QueryAsync<Item>("SELECT * FROM tMst_Item where IsActive = 1", transaction:Transaction);
        }

        public async Task<IEnumerable<Item>> GetAllByPage(ItemPageDto itemPageDto)
        {
            var pc = await Connection.ExecuteScalarAsync("Select dbo.fcMst_GetItemPageCount (@RowsOfPage)", new {RowsOfPage = itemPageDto.RowsOfPage}, transaction: Transaction);
            var data =  new
            {
                PageNumber = itemPageDto.PageNumber,
                RowsOfPage = itemPageDto.RowsOfPage,
                PageCount = pc
            };
            var dt = await Connection.QueryAsync<Item>("pMst_GetItem", data, commandType: CommandType.StoredProcedure, transaction: Transaction);

            return dt;
        }

        public async Task<Item> GetByID(string id)
        {
            return await Connection.QueryFirstOrDefaultAsync<Item>("SELECT * FROM tMst_Item where ItemID = @id", new{id = id}, transaction:Transaction);
        }

        public Task<Item> Save(Item obj)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Item> Save(Item oItem, int newItemID)
        {
            // throw new System.NotImplementedException();
            var dt = await Connection.QueryFirstOrDefaultAsync<Item>("pMst_ItemSave", new
            {
                ItemID = oItem.ItemID,
                ItemIDSambu =oItem.ItemIDSambu,
                ItemIDPSS = oItem.ItemIDPSS,
                ItemIDKSP = oItem.ItemIDKSP,
                NewItemID = newItemID,
                ItemName = oItem.ItemName,
                HSNumber = oItem.HSNumber,
                OldItemId = oItem.OldItemID,
                AlternateID = oItem.AlternateID,
                ItemDesc = oItem.ItemDesc,
                SubCategoryID = oItem.SubCategoryID,                
                UOMID = oItem.UOMID,
                DecimalInQnty = oItem.DecimalInQnty,
                AVGMonthlyUsage = oItem.AVGMonthlyUsage,
                LeadTime = oItem.LeadTime,
                MinStock = oItem.MinStock,
                MaxStock = oItem.MaxStock,
                MainProductCategoryID = oItem.MainProductCategoryID,
                DeptID = oItem.DeptID,
                IsActive = oItem.IsActive,
                StockItem = oItem.StockItem,
                Important = oItem.Important,
                BPBApprovalByManagement = oItem.BPBApprovalByManagement,
                PPBAutoApproval = oItem.PPBAutoApproval,
                PPBAutoApproval2 = oItem.PPBAutoApproval2,
                KawasanBerikatInd = oItem.KawasanBerikatInd,
                RoutineInd = oItem.RoutineInd,
                CentralisasiInd = oItem.CentralisasiInd,
                LimbahB3Ind = oItem.LimbahB3Ind,                
                IsFixedAsset = oItem.isFixedAsset,
                ImagePath = oItem.ImagePath,
                UserID = oItem.CreatedBy,
                Info = "",
                Flag = 0 
            }, commandType: CommandType.StoredProcedure, transaction: Transaction);

            return dt;
        }

        public Task Update(Item obj)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> GetPageCount(int rowOfpage)
        {
            return (int)await Connection.ExecuteScalarAsync("Select dbo.fcMst_GetItemPageCount (@RowsOfPage)", new {RowsOfPage = rowOfpage}, transaction: Transaction);
        }

        public async Task<IEnumerable<Item>> GetByName(string param)
        {
            var bhdr = new Dictionary<string, Item>();
            var bhdtl = new Dictionary<string, ItemSpec>();
            string sql = @"SELECT TOP 1000 A1.*, A.ItemSpecID, A1.ItemID, A.UOM, E.UOMName as UOMPurchase, A.QntyConvert, A.Deskripsi, B.ItemSpecDtlID, A.ItemSpecID, B.VariantValueID, C.VariantValueName, C.VariantTypeID, D.VariantTypeName  FROM tMst_Item  A1 " +
			                " INNER JOIN tMst_ItemSpec A ON A1.ItemID = A.ItemID " + 
			                " INNER JOIN tMst_ItemSpecDtl B ON A.ItemSpecID = B.ItemSpecID " +
			                " INNER JOIN tMst_ItemVariantValue C ON B.VariantValueID = C.VariantValueID " +
			                " INNER JOIN tMst_ItemVariantType D ON C.VariantTypeID = D.VariantTypeID " +
			                " INNER JOIN tMst_ItemUOM E ON A.UOM = E.UOMID Where A1.IsActive = 1 And A1.ItemName like '%' + @param + '%' ";

            await Connection.QueryAsync<Item, ItemSpec, ItemSpecDtl, Item>(sql, (hdr, dtl, subdtl) => {
                Item biHdr;
                ItemSpec biDtl;

                if (!bhdr.TryGetValue(hdr.ItemID.ToString(), out biHdr))
                {
                    biHdr = hdr;
                    biHdr.ItemSpec = new List<ItemSpec>();
                    bhdr.Add(biHdr.ItemID, biHdr);
                }
                
                if(dtl != null){
                    if (!bhdtl.TryGetValue(dtl.ItemSpecID.ToString(), out biDtl))
                    {
                        biDtl = dtl;
                        biDtl.ItemSpecDtl = new List<ItemSpecDtl>();
                        bhdtl.Add(biDtl.ItemSpecID, biDtl);
                        biHdr.ItemSpec.Add(biDtl);
                    }

                    biDtl.ItemSpecDtl.Add(subdtl);
                }
               
                return biHdr;
            }, splitOn:"ItemSpecID, ItemSpecDtlID", param : new {param = param}, transaction:Transaction);
            
            return bhdr.Values;
        }

        public async Task CancelRequest(ItemCancelRequestDto obj)
        {
            await Connection.QueryAsync("Update tMst_ItemNew SET Cancel = 1, CancelRemark = @remark, UpdatedBy = @by, UpdatedDate = @tgl  WHERE NewItemID = @id ", 
               new { remark = obj.CancelRemark, by = obj.UpdatedBy, tgl = DateTime.Now, id = obj.NewItemID}, transaction: Transaction);
        }
    }
}