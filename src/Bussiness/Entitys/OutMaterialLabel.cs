﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.Core.Data;
using HP.Data.Orm.Entity;

namespace Bussiness.Entitys
{
    [Description("出库物料清单条码")]
    [Table("TB_WMS_OUT_MATERIAL_LABEL")]
   public class OutMaterialLabel: ServiceEntityBase<int>
    {
        /// <summary>
        /// 出库单号
        /// </summary>
        public string OutCode { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public string TaskCode { get; set; }

        
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; } 
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 单据号
        /// </summary>
        public string BillCode { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        public string LocationCode { get; set; }
        [NotMapped]
        public virtual string StatusCaption
        {
            get
            {
                if (Status != null)
                {
                    return HP.Utility.EnumHelper.GetCaption(typeof(Bussiness.Enums.OutStatusCaption), Status.Value);
                }
                return "";
            }
        }

        public int OutMaterialId { get; set; }

        public string MaterialLabel { get; set; }

        public string AreaCode { get; set; }

        public string WareHouseCode { get; set; }
        /// <summary>
        /// 实际拣选数量
        /// </summary>
        public decimal? RealPickedQuantity { get; set; }

        public DateTime? PickedTime { get; set; }
        /// <summary>
        /// 拣选人
        /// </summary>
        public string Operator { get; set; }


        public DateTime? CheckedTime { get; set; }

        public string Checker { get; set; }
        /// <summary>
        /// 复核数量
        /// </summary>
        public decimal? CheckedQuantity { get; set; }

        public string SupplierCode { get; set; }
    }
}
