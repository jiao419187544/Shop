﻿// ==========================================================================
//  All Rights Reserved , Copyright (C) 2014 , Team.
//
//  名    称：SysOrg 控制器
//  作    者：tomCat
//  添加时间：2014-10-25 11:45:12
// ==========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//引用
using Shop.BLL;
using Shop.Model;
using Shop.Common;
using MySoft.Data;

namespace Shop.Web.Areas.SysManage.Controllers
{
	/// <summary>
	/// 组织机构(SysOrg)控制器
	/// </summary>
	public class SysOrgController:Controller
	{	     
		private readonly SysOrgBLL bll=new SysOrgBLL();
	
		/// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(DWZPageInfo page, string name = "", int enabled = -1)
        {
        	#region 搜索条件
            WhereClip where = null;
            if (!String.IsNullOrEmpty(name))
            {
                where &= SysOrg._.Name.Like("%" + name.Trim() + "%");

            }
            ViewBag.Name = name;

            //状态
            if (enabled >= 0)
            {
                where &= SysOrg._.Enabled == enabled;
            }
            ViewBag.Enabled = enabled;
            #endregion

            var usersPage = bll.GetPageList(page.NumPerPage, page.PageNum, where, SysOrg._.Relation.Asc);
            
            
            return View(usersPage);
        }
        
        #region  添加 编辑
        /// <summary>
        /// 添加 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create(int id=0,int parentId=0)
        {
            SysOrg model = null;
            if (id > 0)
                model = bll.GetModel(id);
            else
                model = new SysOrg() { Enabled=1};

            ViewBag.ParentName = "无";
            if (parentId == 0) parentId = model.ParentId;
            if (parentId > 0)
            {
                var pModel = bll.GetModel(parentId);
                if (pModel != null)
                    ViewBag.ParentName = pModel.Name;
            }
          
            return View(model);
        } 
        
        /// <summary>
        /// 添加 编辑操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Model.SysOrg model)
        {
            bool flag = false;
			DWZCallbackInfo callback=null;

            if (model.Id > 0)//修改
                flag=bll.Update(model);
            else//添加
                flag=bll.Add(model)>0;

            if (flag)
                callback = DWZMessage.Success("操作成功!", "SysManage_SysOrg", true);
			else
                callback = DWZMessage.Faild();

             return Json(callback);
        }
        #endregion
        
        #region 删除
        /// <summary>
        /// 删除单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            DWZCallbackInfo callback = null;

            if (bll.Delete(id))
                callback = DWZMessage.Success("删除成功!");
            else
                callback = DWZMessage.Faild("删除失败!");

            return Json(callback);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteList(int[] ids)
        {
           DWZCallbackInfo callback = null;
           
            int count=bll.Delete(ids) ;
            if (count > 0)
                callback = DWZMessage.Success(string.Format("删除成功！共删除{0}条！", count));
            else
                callback = DWZMessage.Faild("删除失败!");

            return Json(callback);
        }
        
        #endregion
	}
}