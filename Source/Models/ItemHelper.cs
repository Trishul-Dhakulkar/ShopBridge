using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ShopBridge.Models
{
    public class ItemHelper
    {
        /// <summary>
        /// Get List of all Products
        /// </summary>
        /// <returns></returns>
        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            string constr = GetConnectionString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"SELECT ID,Name,Discription,Price,BatchNo,ManufactureDate,ExpiryDate,NetWt,WtUnit FROM Product";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            foreach(DataRow dr in dt.Rows)
                            {
                                Item item = new Item();
                                item.ID = Convert.ToInt32(dr["ID"]);
                                item.Name = Convert.ToString(dr["Name"]);
                                item.Discription = Convert.ToString(dr["Discription"]);
                                item.Price = Convert.ToDouble(dr["Price"]);
                                item.BatchNo = Convert.ToString(dr["BatchNo"]);
                                item.ManufactureDate = Convert.ToDateTime(dr["ManufactureDate"]).ToString("MMM/yyyy");
                                if (dr["ExpiryDate"] == DBNull.Value)
                                    item.ExpiryDate = string.Empty;
                                else
                                    item.ExpiryDate = Convert.ToDateTime(dr["ExpiryDate"]).ToString("MMM/yyyy");
                                item.NetWt = Convert.ToDouble(dr["NetWt"]);
                                item.WtUnit = Convert.ToString(dr["WtUnit"]);
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            
            return items;
        }

        /// <summary>
        /// Add new Product to Database.
        /// </summary>
        /// <param name="items">Array of Item</param>
        /// <returns></returns>
        public bool Add(List<Item> items)
        {
            bool status = false;
            int count = 0;
            string constr = GetConnectionString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    string query = @"
                                INSERT INTO Product(Name,Discription,Price,BatchNo,ManufactureDate,ExpiryDate,NetWt,WtUnit)
                                VALUES(@name,@disc,@price,@batchNo,@manuDt,@expDt,@netWt,@wtUnit)";
                    con.Open();
                    foreach (Item info in items)
                    {
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.Connection = con;
                                cmd.Parameters.Add("@name", SqlDbType.VarChar,50).Value = info.Name;
                                cmd.Parameters.Add("@disc", SqlDbType.VarChar,200).Value = info.Discription;
                                cmd.Parameters.Add("@price", SqlDbType.Float).Value = info.Price;
                                cmd.Parameters.Add("@batchNo", SqlDbType.VarChar, 20).Value = info.BatchNo;
                                cmd.Parameters.Add("@manuDt", SqlDbType.Date).Value = Convert.ToDateTime(info.ManufactureDate).Date;
                                if(info.ExpiryDate.Length > 0)
                                    cmd.Parameters.Add("@expDt", SqlDbType.Date).Value = Convert.ToDateTime(info.ExpiryDate).Date;
                                else
                                    cmd.Parameters.Add("@expDt", SqlDbType.Date).Value = DBNull.Value;
                                cmd.Parameters.Add("@netWt", SqlDbType.Float).Value = info.NetWt;
                                cmd.Parameters.Add("@wtUnit", SqlDbType.VarChar, 10).Value = info.WtUnit;
                                sda.SelectCommand = cmd;
                                int i = (int)cmd.ExecuteNonQuery();
                                if (i > 0)
                                    count++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
            }
            if (count == items.Count)
                status = true;
            return status;
        }

        /// <summary>
        /// Update Existing Product in Database based on ID
        /// </summary>
        /// <param name="items"> Array of Item</param>
        /// <returns></returns>
        public bool Update(List<Item> items)
        {
            bool status = false;
            int count = 0;
            string constr = GetConnectionString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    string query = @"
                                UPDATE Product SET Name=@name,Discription=@disc,Price=@price 
                                ,BatchNo=@batchNo,ManufactureDate=@manuDt,ExpiryDate=@expDt,NetWt=@netWt,WtUnit=@wtUnit
                                WHERE ID = @id";
                    con.Open();
                    foreach (Item info in items)
                    {
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.Connection = con;
                                cmd.Parameters.Add("@id", SqlDbType.Int).Value = info.ID;
                                cmd.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = info.Name;
                                cmd.Parameters.Add("@disc", SqlDbType.VarChar, 200).Value = info.Discription;
                                cmd.Parameters.Add("@price", SqlDbType.Float).Value = info.Price;
                                cmd.Parameters.Add("@batchNo", SqlDbType.VarChar, 20).Value = info.BatchNo;
                                cmd.Parameters.Add("@manuDt", SqlDbType.Date).Value = Convert.ToDateTime(info.ManufactureDate).Date;
                                if (info.ExpiryDate.Length > 0)
                                    cmd.Parameters.Add("@expDt", SqlDbType.Date).Value = Convert.ToDateTime(info.ExpiryDate).Date;
                                else
                                    cmd.Parameters.Add("@expDt", SqlDbType.Date).Value = DBNull.Value;
                                cmd.Parameters.Add("@netWt", SqlDbType.Float).Value = info.NetWt;
                                cmd.Parameters.Add("@wtUnit", SqlDbType.VarChar, 10).Value = info.WtUnit;
                                sda.SelectCommand = cmd;
                                int i = (int)cmd.ExecuteNonQuery();
                                if (i > 0)
                                    count++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
            }
            if (count == items.Count)
                status = true;
            return status;
        }

        /// <summary>
        ///  Delete Product From Database
        /// </summary>
        /// <param name="id"> ID of Product which will need to remove</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool status = false;
            string constr = GetConnectionString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    string query = @"
                                DELETE FROM Product WHERE ID = @id";
                    con.Open();
                      using (SqlCommand cmd = new SqlCommand(query))
                      {
                          using (SqlDataAdapter sda = new SqlDataAdapter())
                          {
                              cmd.Connection = con;
                              cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                              sda.SelectCommand = cmd;
                              int i = (int)cmd.ExecuteNonQuery();
                              if (i > 0)
                                  status = true;
                          }
                      }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
            }
            return status;
        }

        /// <summary>
        /// Read Connection string from WEB.Config file
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string conn = string.Empty;
            conn = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            return conn;
        }

        /// <summary>
        ///  Validate Data before Add or Update
        /// </summary>
        /// <param name="mode"> A for add / U for update </param>
        /// <param name="items"> Array of Items</param>
        /// <param name="errMsg"> return errMsg to Calling Method</param>
        /// <returns></returns>
        public bool ValidateProductData(char mode,List<Item> items,out string errMsg)
        {
            errMsg = string.Empty;
            bool status = false;
            for(int i = 0;i<items.Count;i++)
            {
                int _pid = i;
                _pid++;
                Item _item = items[i];
                if (mode == 'U') // In-Case of Update
                {
                    if (_item.ID <= 0) errMsg += "-Please Add ID for Product " + _pid + "in request \n";
                }
                if(string.IsNullOrEmpty(_item.Name)) errMsg += "-Please Add Name for Item " + _pid + "in request \n";
                if (_item.Price <= 0) errMsg += "-Please Add Price for Item " + i++ + "\n";
                if (string.IsNullOrEmpty(_item.BatchNo)) errMsg += "-Please Add BatchNo. for Item " + _pid + "in request \n";
                if (string.IsNullOrEmpty(_item.ManufactureDate)) errMsg += "-Please Add Manufacture Date for Item " + _pid + "in request \n";
                if (_item.NetWt <= 0) errMsg += "-Please Add NetWeight for Item " + _pid + "\n";
                if (string.IsNullOrEmpty(_item.WtUnit)) errMsg += "-Please Add Weight Unit for Item " + i++ + "\n";
                if(!CommonHelper.IsValidDate(_item.ManufactureDate)) errMsg += "-Please Add Valid Manufacturing Date in dd/MM/yyyy format for Item " + _pid + "in request \n";
                if (!string.IsNullOrEmpty(_item.ExpiryDate) && _item.ExpiryDate.Length > 0)
                {
                    if (!CommonHelper.IsValidDate(_item.ExpiryDate)) errMsg += "-Please Add Valid Expiry Date in dd/MM/yyyy format for Item " + _pid + "in request \n";
                }
            }
            if (errMsg.Length == 0) 
                status = true;
            return status;
        }
    }
}