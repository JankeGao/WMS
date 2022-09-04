﻿using System;
using System.Linq;
using wms.Client.Core.Interfaces;
using wms.Client.LogicCore.Common;
using wms.Client.LogicCore.Configuration;
using wms.Client.LogicCore.Enums;
using wms.Client.Model.Entity;
using wms.Client.Model.Query;
using wms.Client.ViewModel.Base;

namespace wms.Client.ViewModel
{
    /// <summary>
    /// 用户列表
    /// </summary>
//    [Module(ModuleType.BasicData, "UserDlg", "用户管理")]
    public class UserViewModel : DataProcess<User>
    {
        private readonly IUserService service;
        public UserViewModel()
        {
            service = ServiceProvider.Instance.Get<IUserService>();
            this.Init();
        }

        public override async void GetPageData(int pageIndex)
        {
            //try
            //{
            //    var r = await service.GetUsersAsync(new UserParameters()
            //    {
            //        PageIndex = pageIndex,
            //        PageSize = PageSize,
            //        Search = SearchText,
            //    });
            //    if (r.success)
            //    {
            //        TotalCount = r.TotalRecord;
            //        GridModelList.Clear();
            //        r.users.ForEach((arg) => GridModelList.Add(arg));
            //        base.SetPageCount();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Msg.Error(ex.Message);
            //}
        }

        public override async void Del<TModel>(TModel model)
        {
            //var user = model as User;
            //if (await Msg.Question($"确认删除用户:{user.UserName}"))
            //{
            //    var delResult = await service.DeleteUserAsync(user.Id);
            //    if (delResult)
            //    {
            //        GetPageData(this.PageIndex);
            //    }
            //}
        }

        public override async void Save()
        {
            //if (!this.IsValid)
            //{
            //    return;
            //}

            //if (Mode == ActionMode.Add)
            //{
            //    Model.FlagAdmin = "0";
            //    Model.FlagOnline = "0";
            //    var addResult = await service.AddUserAsync(Model);
            //    if (addResult)
            //    {
            //        this.GetPageData(this.PageIndex);
            //    }
            //}
            //else
            //{
            //    var updateResult = await service.UpdateUserAsync(Model);
            //    if (updateResult)
            //    {
            //        var model = GridModelList.FirstOrDefault(t => t.Id == Model.Id);
            //        if (model != null)
            //        {
            //            GridModelList.Remove(model);
            //            GridModelList.Add(Model);
            //        }
            //    }
            //}
            base.Save();
        }
    }
}
