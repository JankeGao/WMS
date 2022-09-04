using HP.Core.Dependency;
using HP.Data.Orm;
using HP.Utility.Data;
using HPC.BaseService.Models;

namespace HPC.BaseService.Contracts
{
    public interface IRegionContract: IScopeDependency
    {
        IQuery<Region> Regions { get; }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DataResult CreateRegion(Region entity);

        /// <summary>
        /// �༭��������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DataResult EditRegion(Region entity);

        /// <summary>
        /// �Ƴ���������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataResult RemoveRegion(int id);
    }
}