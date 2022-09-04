﻿using HP.Core.Data;
using HP.Data.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bussiness.Entitys.SMT
{
    [Table("TB_WMS_SPLIT_AREA")]
    public class WmsSplitArea : ServiceEntityBase<int>
    {
        /// <summary>
        /// 拆分单号
        /// </summary>
        public string SplitNo { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 标志位 0初始 1表示Wms已获取
        /// </summary>
        public int? Status { get; set; }

        [NotMapped]
        public string StatusCaption
        {
            get
            {
                if (Status != null)
                {
                    return HP.Utility.EnumHelper.GetCaption(typeof(Bussiness.Enums.SMT.SplitStatusEnum), Status.GetValueOrDefault(0));
                }
                return "";
            }
        }
        /// <summary>
        /// 当前区域任务ID
        /// </summary>
        public string ProofId { get; set; }
    }
}