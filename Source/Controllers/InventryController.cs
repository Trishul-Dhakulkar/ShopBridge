using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace ShopBridge.Controllers
{
    public class InventryController : ApiController
    {
        public object GetList()
        {
            string response = string.Empty;
            StatusInfo info = new StatusInfo();
            try
            {
                ItemHelper helper = new ItemHelper();
                List<Item> items = helper.GetItems();
                info.Data = items;
                response = JsonHelper.GetJSONData(info);
            }
            catch(Exception ex)
            {
                info.SetExceptionStatus(ex.Message);
                response = JsonHelper.GetJSONData(info);
            }
            return JObject.Parse(response);
        }

        [HttpPost]
        public object AddItem([FromBody] JArray inputParams)
        {
            string response = string.Empty, errMsg = string.Empty;
            StatusInfo info = new StatusInfo();
            if(inputParams != null)
            {
                try
                {
                    List<Item> items = inputParams.ToObject<List<Item>>();
                    ItemHelper helper = new ItemHelper();
                    bool isValid = helper.ValidateProductData('A', items, out errMsg);
                    if (isValid)
                    {
                        info.Status = helper.Add(items);
                        if (info.Status)
                            info.Remark = "Items Successfully Added";
                        else
                            info.Remark = "Items Not Added";
                        response = JsonHelper.GetJSONData(info);
                    }
                    else
                    {
                        info.SetExceptionStatus("Please resolve the following error to proceed. \n" + errMsg);
                        response = JsonHelper.GetJSONData(info);
                    }
                }
                catch (Exception ex)
                {
                    info.SetExceptionStatus(ex.Message);
                    response = JsonHelper.GetJSONData(info);
                }
            }
            else
            {
                info.SetExceptionStatus("Invalid Request please check Request JSON data.");
                response = JsonHelper.GetJSONData(info);
            }
            return JObject.Parse(response);
        }

        [HttpPut]
        public object UpdateItem([FromBody] JArray inputParams)
        {
            string response = string.Empty, errMsg = string.Empty;
            StatusInfo info = new StatusInfo();
            if (inputParams != null)
            {
                try
                {
                    List<Item> items = inputParams.ToObject<List<Item>>();
                    ItemHelper helper = new ItemHelper();
                    bool isValid = helper.ValidateProductData('U', items, out errMsg);
                    if (isValid)
                    {
                        info.Status = helper.Update(items);
                        if (info.Status)
                            info.Remark = "Items Successfully Updated";
                        else
                            info.Remark = "Items Not Update";
                        response = JsonHelper.GetJSONData(info);
                    }
                    else
                    {
                        info.SetExceptionStatus("Please resolve the following error to proceed. \n" + errMsg);
                        response = JsonHelper.GetJSONData(info);
                    }
                }
                catch (Exception ex)
                {
                    info.SetExceptionStatus(ex.Message);
                    response = JsonHelper.GetJSONData(info);
                }
            }
            else
            {
                info.SetExceptionStatus("Invalid Request please check Request JSON data.");
                response = JsonHelper.GetJSONData(info);
            }
            return JObject.Parse(response);
        }

        [HttpDelete]
        public object DeleteItem([FromUri]int id)
        {
            string response = string.Empty;
            StatusInfo info = new StatusInfo();
            if (id > 0)
            {
                try
                {
                    ItemHelper helper = new ItemHelper();
                    info.Status = helper.Delete(id);
                    if (info.Status)
                        info.Remark = "Items Successfully Deleted";
                    else
                        info.Remark = "Items Not Delete";
                    response = JsonHelper.GetJSONData(info);
                }
                catch (Exception ex)
                {
                    info.SetExceptionStatus(ex.Message);
                    response = JsonHelper.GetJSONData(info);
                }
            }
            else
            {
                info.SetExceptionStatus("Invalid ID");
                response = JsonHelper.GetJSONData(info);
            }
            return JObject.Parse(response);
        }
    }
}
