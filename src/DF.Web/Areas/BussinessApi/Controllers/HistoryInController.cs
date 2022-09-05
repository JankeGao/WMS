﻿using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using HP.Core.Logging;
using HP.Data.Entity.Pagination;
using HP.Data.Orm;
using HP.Utility.Data;
using HP.Utility.Extensions;
using HP.Utility.Files;
using HP.Web.Api;
using HP.Web.Mvc.Extensions;
using HP.Web.Mvc.Interceptor;
using HP.Web.Mvc.Pagination;
using HPC.BaseService.Resources;
using Bussiness.Enums;
using NPOI.Util;
using System.Collections.Generic;
using Bussiness.Resources;

namespace DF.Web.Areas.BussinessApi.Controllers
{
    /// <summary>
    /// 历史入库信息
    /// </summary>
    [Description("入库信息的管理")]
    public class HistoryInController : BaseApiController
    {

        /// <summary>
        /// 历史入库信息
        /// </summary>
        public Bussiness.Contracts.IHistoryInContract HistoryInContract { set; get; }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <returns></returns>
        [LogFilter(Type = LogType.Operate, Name = "获取历史入库信息")]
        [HttpGet]
        public HttpResponseMessage GetPageRecords([FromUri] MvcPageCondition pageCondition)
        {
            var query = HistoryInContract.HistoryInDtos;
            // 查询条件，根据时间范围查询、根据入库单号查询、根据创建人查询、根据物料编码、物料名称查询
            var filterRule = pageCondition.FilterRuleCondition.Find(a => a.Field == "QueryCondition");
            if (filterRule != null)
            {
                string value = filterRule.Value.ToString();
                query = query.Where(p => p.InCode.Contains(value) || p.MaterialCode.Contains(value)
                || p.MaterialName.Contains(value) || p.OperatorName.Contains(value)               
                );
                pageCondition.FilterRuleCondition.Remove(filterRule);

            }
            var begin = pageCondition.FilterRuleCondition.Find(a => a.Field == "begin");
            var end = pageCondition.FilterRuleCondition.Find(a => a.Field == "end");
            if (begin != null && end != null)
            {
                var value1 = Convert.ToDateTime(begin.Value.ToString());
                var value2 = Convert.ToDateTime(end.Value.ToString());
                query = query.Where(p => (p.InWarehouseTime) >= value1 && p.InWarehouseTime <= value2);
                pageCondition.FilterRuleCondition.Remove(begin);
                pageCondition.FilterRuleCondition.Remove(end);
            }

            //以倒叙方式查询显示
            var proList = query.OrderByDesc(a => a.CreatedTime).ToPage(pageCondition);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, proList.ToMvcJson());
            return response;
        }

        /// <summary>
        /// 导出历史入库信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage DoDownLoadTemp([FromUri] MvcPageCondition pageCondition)
        {
            var query = HistoryInContract.HistoryInDtos;

            var filterRule = pageCondition.FilterRuleCondition.Find(a => a.Field == "QueryCondition");
            if (filterRule != null)
            {
                string value = filterRule.Value.ToString();
                query = query.Where(p => p.InCode.Contains(value) || p.MaterialCode.Contains(value)
                || p.MaterialName.Contains(value) || p.CreatedUserName.Contains(value)
                );
                pageCondition.FilterRuleCondition.Remove(filterRule);

            }
            var begin = pageCondition.FilterRuleCondition.Find(a => a.Field == "begin");
            var end = pageCondition.FilterRuleCondition.Find(a => a.Field == "end");
            if (begin != null && end != null)
            {
                var value1 = Convert.ToDateTime(begin.Value.ToString());
                var value2 = Convert.ToDateTime(end.Value.ToString());
                query = query.Where(p => (p.InWarehouseTime) >= value1 && p.InWarehouseTime <= value2);
                pageCondition.FilterRuleCondition.Remove(begin);
                pageCondition.FilterRuleCondition.Remove(end);
            }

            var list = query.ToList();
            var divFields = new Dictionary<string, string>//显示的字段与名称
            {
                {"Id","序号"},
                {"InCode","入库单号"},
                {"ContainerCode","货柜"},
                {"TrayCode","上架托盘"},
                {"LocationCode","上架储位"},
                {"BoxName","载具名称"},
                {"MaterialLabel","物料条码"},
                {"MaterialCode","物料编码"},
                {"Quantity","数量" },
                {"Unit","单位" },
                {"MaterialName","物料名称"},
                {"WarehouseName","仓库名称" },
                {"WarehouseCode","仓库编码" },
                {"InWarehouseTime","入库时间" },
                {"CreatedUserName","操作人" }
            };

            var fileName = "历史入库信息.xlsx";
            var excelFile = Bussiness.Common.ExcelHelper.ListToExecl(list, fileName, divFields);
            MemoryStream ms = new MemoryStream();
            excelFile.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            //获取导出文件流
            var stream = ms;
            if (stream == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = $"历史入库信息{System.DateTime.Now.ToString("yyyyMMdd")}.xls";
                return result;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}