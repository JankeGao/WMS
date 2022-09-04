using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Contracts;
using HP.Core.Data;
using HP.Core.Data.Infrastructure;
using HP.Data.Orm.Entity;


namespace Bussiness.Entitys
{
    [Description("��λ��Ϣ")]
    [Table("TB_WMS_LOCATION")]

    public class Location : ServiceEntityBase<int>
    {
        /// <summary>
        /// ��λ����
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// X�ƺ�
        /// </summary>
        public int XLight { set; get; }
        /// <summary>
        /// Y�ƺ�
        /// </summary>
        public int YLight { set; get; }

        /// <summary>
        /// �ؾ߱���
        /// </summary>
        public string BoxCode { set; get; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool? Enabled { set; get; }
        /// <summary>
        /// �Ƿ�Ϊ���λ
        /// </summary>
        public bool IsLocked { set; get; }
        /// <summary>
        /// �������ϱ���
        /// </summary>
        public string SuggestMaterialCode { set; get; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// �ֿ����
        /// </summary>
        public string WareHouseCode { set; get; }

        /// <summary>
        /// ���̱���
        /// </summary>
        public int? TrayId { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string ContainerCode { set; get; }

        /// <summary>
        /// ͼ�ο��ӻ�Id
        /// </summary>
        public string LayoutId { set; get; }


        /// <summary>
        /// ͼ�ο��ӻ�Id
        /// </summary>
        public decimal? LockQuantity { set; get; }
        /// <summary>
        /// ��������
        /// </summary>
        public string LockMaterialCode { get; set; }

        [NotMapped]
        public string TrayCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? XLenght { get; set; }

    }

    /// <summary>
    /// ��λ�����߼���ҪӦ����ͼ���Դ�λ��Ϊ���˲�ô�λ����ŵ��ؾߣ��������ؾ�-����ӳ���
    /// </summary>
    [Table("VIEW_WMS_LOCATION")]
    //[SugarTable("VIEW_WMS_LOCATION")] // �ͻ���ʹ��
    public class LocationVIEW : Location
    {
        /// <summary>
        /// �ô�λ�ؾ߿ɴ�ŵ�����
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// �ô�λ��ŵ��������
        /// </summary>
        public int MaterialType{ get; set; }

        /// <summary>
        /// �洢����
        /// </summary>
        public bool IsBatch { get; set; }

        /// <summary>
        /// �ÿ�λ�ɴ洢�������Ƿ�洢����
        /// </summary>
        public bool IsNeedBlock { get; set; }

        /// <summary>
        /// �ÿ�λ�ɴ洢�������Ƿ����
        /// </summary>
        public bool IsMaxBatch { get; set; }

        /// <summary>
        /// �ÿ�λ�ɴ洢�����ϵ�λ����
        /// </summary>
        public decimal? UnitWeight { get; set; }


        /// <summary>
        /// �ÿ�λ��ǰ�洢������
        /// </summary>
        public string MaterialLabel { get; set; }

        /// <summary>
        /// �ÿ�λ��ǰ�洢����������
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// �ÿ�λ��ǰ�洢������
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// �ÿ�λ��������������
        /// </summary>
        public decimal? MaxWeight { get; set; }

        /// <summary>
        /// �ô�λ������ŵ�����
        /// </summary>
        public decimal? BoxCount { get; set; }


        /// <summary>
        /// �ô�λ������ŵ�����
        /// </summary>
        public string MaterialName { get; set; }


        /// <summary>
        /// �ô�λ������ŵ�����
        /// </summary>
        public string TrayCode { get; set; }

        
    }

    public class LocationVM : Location
    {
        /// <summary>
        /// ���̱���
        /// </summary>
        public string TrayCode { set; get; }
        /// <summary>
        /// ������������
        /// </summary>
        public string SuggestMaterialName { set; get; }

        /// <summary>
        /// �������ϵ�λ
        /// </summary>
        public string SuggestMaterialUnit { set; get; }
        /// <summary>
        /// �ֿ�����
        /// </summary>
        public string BoxName { set; get; }

        /// <summary>
        /// �ؾ߿��
        /// </summary>
        public int BoxWidth { get; set; }

        /// <summary>
        /// �ؾ߳���
        /// </summary>
        public int BoxLength { get; set; }

        /// <summary>
        /// ��λ��ǰ�洢������
        /// </summary>
        public string StockMaterialLabel { get; set; }

        /// <summary>
        /// ��λ��ǰ�洢������
        /// </summary>
        public decimal? StockLabelQuantity{ get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string StockMaterialBatch { get; set; }


        /// <summary>
        /// ͼƬ��ַ
        /// </summary>
        public string BoxUrl { get; set; }


        /// <summary>
        /// �ɴ�ŵ�����
        /// </summary>
        public decimal? AviQuantity { get; set; }


        /// <summary>
        /// ��λ��ǰ�洢������
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// �мܺ� Ĭ��Ϊһ
        /// </summary>
        public int BracketNumber { get; set; }

        /// <summary>
        /// �м������̺�
        /// </summary>
        public int BracketTrayNumber { get; set; }

        public int ContainerType { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public decimal? StockQuantity { get; set; }

        public decimal? MinStockQuantity { get; set; }

        public decimal? MaxStockQuantity { get; set; }

    }
}

