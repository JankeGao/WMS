using System;
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
    [Description("�����Ϣ")]
    [Table("TB_WMS_WAREHOUSE")]
    public class WareHouse : ServiceEntityBase<int>
    {
        /// <summary>
        /// �ֿ����
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// �ֿ�����
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// �ֿ��ַ
        /// </summary>
        public string Address { set; get; }
        /// <summary>
        /// �ֿ�����
        /// </summary>
        public string CategoryDict { set; get; }
        [NotMapped]
        public string CategoryDictName
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// �Ƿ�Ϊ�����
        /// </summary>
        public bool? IsVirtual { set; get; }
        /// <summary>
        /// �Ƿ���������λ
        /// </summary>
        public bool? AllowManage { set; get; }

        public string Remark { get; set; }

        [NotMapped]
        /// <summary>
        ///  ������Ϣ
        /// </summary>
        public System.Collections.Generic.List<Dtos.ContainerDto> children { get; set; }
    }
}

