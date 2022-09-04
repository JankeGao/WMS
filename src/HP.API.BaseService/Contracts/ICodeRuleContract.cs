using HP.Core.Dependency;
using HP.Core.Sequence.Entitys;
using HP.Data.Orm;
using HP.Utility.Data;
using HPC.BaseService.Dtos;

namespace HPC.BaseService.Contracts
{
    public interface ICodeRuleContract: IScopeDependency
    {
        IQuery<CodeRule> CodeRules { get; }
        IQuery<CodeRuleItem> CodeRuleItems { get; }

        /// <summary>
        /// �����ֵ�
        /// </summary>
        /// <returns></returns>
        DataResult CreateCodeRule(CodeRuleInputDto dto);

        /// <summary>
        /// �༭�ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult EditCodeRule(CodeRuleInputDto dto);

        /// <summary>
        /// �Ƴ��ֵ�����
        /// </summary>
        /// <returns></returns>
        DataResult RemoveCodeRule(string id);
    }
}