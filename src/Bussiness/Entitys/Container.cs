using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Enums;
using HP.Core.Data;
using HP.Core.Data.Infrastructure;
using HP.Data.Orm.Entity;
using SqlSugar;
//using SqlSugar;

namespace Bussiness.Entitys
{
    [Description("����ʵ��")]
    [Table("TB_WMS_Container")]
    [SugarTable("TB_WMS_Container")]
    public class Container : ServiceEntityBase<int>
    {
        /// <summary>
        /// �������
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// �����ͺ�
        /// </summary>
        public string EquipmentCode { set; get; }

        /// <summary>
        /// �ֿ����
        /// </summary>
        public string WareHouseCode { set; get; }

        /// <summary>
        /// Ip ��ַ
        /// </summary>
        public string Ip { set; get; }

        /// <summary>
        /// �˿ں�
        /// </summary>
        public string Port { set; get; }

        /// <summary>
        /// ����Ψһ����ͻ���ȷ��
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// �豸״̬
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// ״̬����
        /// </summary>
        [NotMapped]
        [SugarColumn(IsIgnore = true)]
        public string StatusCaption
        {
            get
            {
                return HP.Utility.EnumHelper.GetCaption(typeof(DeviceStatusEnum), Status);
            }
        }
        

        /// <summary>
        /// ����״̬
        /// </summary>
        public int AlarmStatus { get; set; }


        /// <summary>
        /// ״̬����
        /// </summary>
        [NotMapped]
        [SugarColumn(IsIgnore = true)]
        public string ALarmStatusCaption
        {
            get
            {
                return HP.Utility.EnumHelper.GetCaption(typeof(DeviceAlarmStateEnum), AlarmStatus);
            }
        }

        /// <summary>
        /// �����ͺ�
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        public bool IsVirtual { get; set; }
        /// <summary>
        /// ��������  0 �ʽܻ�ת��  1 ����˹���� 2 ���˶����� 3 �ʽ�������
        /// </summary>
        public int ContainerType { get; set; }
    }
}

