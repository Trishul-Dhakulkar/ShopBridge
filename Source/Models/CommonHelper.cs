using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class CommonHelper
    {
        /// <summary>
        /// Function for checking whether the object contains date or not
        /// </summary>
        /// <param name="theDate"></param>
        /// <returns></returns>
        public static bool IsDate(object theDate)
        {
            try
            {
                if (theDate == null)
                    return false;
                string DateString = "";
                if (!Convert.IsDBNull(theDate))
                    DateString = Convert.ToString(theDate);
                DateTime.Parse(DateString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidDate(object pDate)
        {
            string strDate = Convert.ToString(pDate);
            if (strDate.Length == 0)
                return false;

            try
            {
                DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                return (CommonHelper.IsDate(dt));
            }
            catch
            {
                return false;
            }
        }
    }
    public class JsonHelper
    {
        public static string GetJSONData(object data)
        {
            if (data == null) return string.Empty;
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        public static T GetData<T>(string jsonData)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
        }
    }

    public class StatusInfo
    {
        public bool Status { get; set; }
        public string Remark { get; set; }
        public object Data { get; set; }
        public StatusInfo()
        {
            Status = true;
            Remark = string.Empty;
            Data = string.Empty;
        }

        public void SetExceptionStatus(string msg)
        {
            Status = false;
            Remark = msg;
            Data = string.Empty;
        }
    }

    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public double Price { get; set; }
        public string BatchNo { get; set; }
        public string ManufactureDate { get; set; }
        public string ExpiryDate { get; set; }
        public double NetWt { get; set; }
        public string WtUnit { get; set; }

        public Item()
        {
            ID = 0;
            Name = string.Empty;
            Discription = string.Empty;
            Price = 0;
            BatchNo = string.Empty;
            ManufactureDate = string.Empty;
            ExpiryDate = string.Empty;
            NetWt = 0;
            WtUnit = string.Empty;
        }
    }
}