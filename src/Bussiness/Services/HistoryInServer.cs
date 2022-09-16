using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HP.Core.Data;
using HP.Data.Orm.Entity;
using Bussiness.Entitys;
using HP.Data.Orm;
using HP.Utility.Data;
using Bussiness.Dtos;
using HPC.BaseService.Contracts;
using Bussiness.Contracts;

namespace Bussiness.Services
{
    class HistoryInServer : Contracts.IHistoryInContract
    {

        /// <summary>
        /// 入库单
        /// </summary>
        public Bussiness.Contracts.IInContract InContract { set; get; }

        public Bussiness.Contracts.IStockContract StockContract { set; get; }

        public IIdentityContract IdentityContract { get; set; }
        public IMaterialContract MaterialContract { set; get; }

        public IQuery<HistoryInDto> HistoryInDtos
        {
            
            get
            {
                //入库信息和明细信息和材料信息和仓库
                return InContract.InMaterialLabelRepository.Query()
                    .InnerJoin(MaterialContract.Materials, (ins, material) => ins.MaterialCode == material.Code)
                    .LeftJoin(IdentityContract.Users, (ins, material, user) => ins.Operator == user.Code)
                    .LeftJoin(StockContract.StockDtos, (ins, material, user, stock) => ins.MaterialLabel == stock.MaterialLabel)
                    .Select((ins, material, user, stock) => new HistoryInDto()
                    {
                        Id = ins.Id,
                        InCode = ins.InCode,
                        MaterialLabel = ins.MaterialLabel,
                        Quantity = ins.Quantity,
                        Unit = material.Unit,
                        Remark2 = material.Remark2,
                        MaterialCode = ins.MaterialCode,
                        MaterialName = material.Name,
                        WarehouseCode = ins.WareHouseCode,
                        WarehouseName = stock.WareHouseName,
                        ContainerCode = stock.ContainerCode,
                        TrayCode = stock.TrayCode,
                        LocationCode = stock.LocationCode,
                        BoxUrl = stock.BoxUrl,
                        BoxName = stock.BoxName,
                        CreatedUserName = ins.CreatedUserName,
                        CreatedTime = ins.CreatedTime,
                        InWarehouseTime = ins.ShelfTime,
                        Operator = ins.Operator,
                        OperatorName = user.Name
                    }); ; ;

                //return InContract.InMaterialLabelRepository.Query()
                //    .InnerJoin(StockContract.StockDtos, (ins, stock) => ins.MaterialLabel == stock.MaterialLabel)
                //    .LeftJoin(IdentityContract.Users, (ins, stock, user) => ins.Operator == user.Code)
                //    .LeftJoin(MaterialContract.Materials, (ins, stock, user, material) => ins.MaterialCode == material.Code)
                //    .Select((ins, stock, user, material) => new HistoryInDto()
                //    {
                //        Id = ins.Id,
                //        InCode = ins.InCode,
                //        MaterialLabel = ins.MaterialLabel,
                //        Quantity = ins.Quantity,
                //        Unit = material.Unit,
                //        MaterialCode = ins.MaterialCode,
                //        MaterialName = material.Name,
                //        WarehouseCode = ins.WareHouseCode,
                //        WarehouseName = stock.WareHouseName,
                //        ContainerCode = stock.ContainerCode,
                //        TrayCode = stock.TrayCode,
                //        LocationCode = stock.LocationCode,
                //        BoxUrl = stock.BoxUrl,
                //        BoxName = stock.BoxName,
                //        CreatedUserName = ins.CreatedUserName,
                //        CreatedTime = ins.CreatedTime,
                //        InWarehouseTime = ins.ShelfTime,
                //        Operator = ins.Operator,
                //        OperatorName = user.Name
                //    });
            }
        }
    }
}
