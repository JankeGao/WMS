using HP.Core.Dependency;
using HP.Data.Orm;
using HP.Utility.Data;
using HPC.BaseService.Models;

namespace HPC.BaseService.Contracts
{
    public interface IDictionaryContract: IScopeDependency
    {
        #region �ֵ�

        IQuery<Dictionary> Dictionaries { get; }

        /// <summary>
        /// �����ֵ�
        /// </summary>
        /// <returns></returns>
        DataResult CreateDictionary(Dictionary entity);

        /// <summary>
        /// �༭�ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult EditDictionary(Dictionary entity);

        /// <summary>
        /// �Ƴ��ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult RemoveDictionary(int id);

        #endregion

        #region �ֵ����

        IQuery<DictionaryType> DictionaryTypes { get; }

        /// <summary>
        /// �����ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult CreateDictionaryType(DictionaryType entity);

        /// <summary>
        /// �༭�ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult EditDictionaryType(DictionaryType entity);

        /// <summary>
        /// �Ƴ��ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult RemoveDictionaryType(int id);

        #endregion
    }
}