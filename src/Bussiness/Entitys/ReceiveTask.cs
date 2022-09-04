﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.Core.Data;
using HP.Core.Data.Infrastructure;
using HP.Data.Orm.Entity;

namespace Bussiness.Entitys
{
    [Description("领用任务")]
    [Table("TB_WMS_ReceiveTask")]
    public class ReceiveTask : ServiceEntityBase<int>
    {
        /// <summary>
        /// 任务单号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 领用单编码
        /// </summary>
        public string ReceiveCode { get; set; }

        /// <summary>
        /// 领用备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 领用人姓名
        /// </summary>
        public string LastTimeReceiveName { get; set; }

        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? LastTimeReceiveDatetime { get; set; }


        /// <summary>
        /// 预计归还时间
        /// </summary>
        public DateTime? PredictReturnTime { get; set; }

        /// <summary>
        /// 领用时长
        /// </summary>
        public int ReceiveTime { get; set; }

        /// <summary>
        /// 归还人姓名
        /// </summary>
        public string LastTimeReturnName { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? LastTimeReturnDatetime { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 货柜编码
        /// </summary>
        public string ContainerCode { get; set; }

        [NotMapped]
        public List<Bussiness.Entitys.Receive> AddMaterial { get; set; }
        /// <summary>
        /// 领用状态
        /// </summary>
        public int? Status { get; set; }
        [NotMapped]
        public virtual string StatusDescription
        {
            get
            {
                if (Status != null)
                {
                    return HP.Utility.EnumHelper.GetCaption(typeof(Bussiness.Enums.ReceiveTaskEnum), Status.Value);
                }
                return "";
            }
        }

        /// <summary>
        ///领用类型
        /// </summary>
        public int? ReceiveType { get; set; }
        [NotMapped]
        public virtual string ReceiveTypeEnumDescription
        {
            get
            {
                if (ReceiveType != null)
                {
                    return HP.Utility.EnumHelper.GetCaption(typeof(Bussiness.Enums.ReceiveTypeEnum), ReceiveType.Value);
                }
                return "";
            }
        }
    }
}
