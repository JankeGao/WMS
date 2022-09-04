using HP.Core.Dependency;
using HP.Data.Orm;
using HP.Utility.Data;
using HPC.BaseService.Dtos;
using HPC.BaseService.Models;

namespace HPC.BaseService.Contracts
{
    public interface ILabelContract: IScopeDependency
    {
        void Test();
        IQuery<Label> Labels { get; }

        IQuery<LabelMap> LabelMaps { get; }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DataResult CreateLabel(Label entity);

        /// <summary>
        /// ��ӱ�ǩ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DataResult AddLabel(LabelMapInputDto entity);

        /// <summary>
        /// �Ƴ���ǩ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataResult RemoveLabel(int id);
    }
}