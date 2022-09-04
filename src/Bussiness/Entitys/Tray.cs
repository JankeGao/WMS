using System.ComponentModel;
using HP.Core.Data;
using HP.Data.Orm.Entity;

namespace Bussiness.Entitys
{
    [Description("����ʵ��")]
    [Table("TB_WMS_Tray")]
    public class Tray : ServiceEntityBase<int>
    {
        /// <summary>
        /// ���̱��
        /// </summary>
        public string Code { set; get; }

        /// <summary>
        /// ������
        /// </summary>
        public string ContainerCode { set; get; }

        /// <summary>
        /// �ֿ���
        /// </summary>
        public string WareHouseCode { set; get; }


        /// <summary>
        /// ���̳���
        /// </summary>
        public decimal? MaxWeight { set; get; }

        /// <summary>
        /// ���̿��
        /// </summary>
        public int TrayWidth { set; get; }

        /// <summary>
        /// ���̿��
        /// </summary>
        public int TrayLength { set; get; }


        /// <summary>
        /// X ���������
        /// </summary>
        public int XNumber { set; get; }

        /// <summary>
        /// Y ���������
        /// </summary>
        public int YNumber { set; get; }


        /// <summary>
        /// ������ͼJson
        /// </summary>
        public string LayoutJson { set; get; }

        /// <summary>
        /// ������ͼJson
        /// </summary>
        [NotMapped]
        public string LocationList { set; get; }

        /// <summary>
        /// ������ͼJson
        /// </summary>
        public decimal? LockWeight { set; get; }
        /// <summary>
        /// �мܺ� Ĭ��Ϊһ
        /// </summary>
        public int BracketNumber { get; set; }

        /// <summary>
        /// �м������̺�
        /// </summary>
        public int BracketTrayNumber { get; set; }



    }
}

