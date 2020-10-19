using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Medical_Store
{
    class updation
    {
        public void updateUsers(int id, string name, string username, string password, string email, string phone, Int16 status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateUsers", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwd", password);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", id);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(name + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }
        
        public void updateCategory(int id, string name, Int16 status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateCategory", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@isActive", status);
                cmd.Parameters.AddWithValue("@id", id);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(name + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateProduct(Int64 id, string product, string barcode, int catID, string packing, DateTime? expiry = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_productUpdate", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", product);
                cmd.Parameters.AddWithValue("@barcode", barcode);;
                if (expiry == null)
                    cmd.Parameters.AddWithValue("@expiry", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@expiry", expiry);
                cmd.Parameters.AddWithValue("@catID", catID);
                cmd.Parameters.AddWithValue("@pack", packing);
                cmd.Parameters.AddWithValue("@id", id);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(product + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateSupplier(int supID, string company, string person, string mobile, string address, Int16 status, string phone = null, string ntn = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateSupplier", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@company", company);
                cmd.Parameters.AddWithValue("@conPerson", person);
                cmd.Parameters.AddWithValue("@mobile", mobile);
                if (phone == null)
                    cmd.Parameters.AddWithValue("@phone", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@address", address);
                if (ntn == null)
                    cmd.Parameters.AddWithValue("@ntn", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ntn", ntn);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@suppID", supID);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(company + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateStock(Int64 proID, int quan, int? pack_quan=null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateStock", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                cmd.Parameters.AddWithValue("@quan", quan);
                if (pack_quan == null)
                    cmd.Parameters.AddWithValue("@pack_quan", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@pack_quan", pack_quan);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateStockWithoutConnection(Int64 proID, int quan, int? pack_quan = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateStock", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                cmd.Parameters.AddWithValue("@quan", quan);
                if (pack_quan == null)
                    cmd.Parameters.AddWithValue("@pack_quan", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@pack_quan", pack_quan);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateProductPrice(Int64 proID, float bp, float sp = 0, float dis = 0, float proPer = 0, float itemSP = 0)
        {
            try
            {
                SqlCommand cmd;
                if (sp == 0 && dis == 0 && proPer == 0)
                {
                    cmd = new SqlCommand("st_updateProductPrice1", MainClass.con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@proID", proID);
                    cmd.Parameters.AddWithValue("@bp", bp);
                }
                else
                {
                    cmd = new SqlCommand("st_updateProductPrice", MainClass.con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@proID", proID);
                    cmd.Parameters.AddWithValue("@bp", bp);
                    cmd.Parameters.AddWithValue("@sp", sp);
                    cmd.Parameters.AddWithValue("@dis", dis);
                    cmd.Parameters.AddWithValue("@profPer", proPer);
                    cmd.Parameters.AddWithValue("@itemSp", itemSP);
                }
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateSalesQuantity(Int64 salesID, Int64 proID, int quantity) 
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateQuantitySalesDetails", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@quan", quantity);
                cmd.Parameters.AddWithValue("@saleID", salesID);
                cmd.Parameters.AddWithValue("@proID", proID);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateSalesAmount(Int64 salesID, float totalAmount, float amountReturn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateSaleAmount", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@totalamount", totalAmount);
                cmd.Parameters.AddWithValue("@amountReturn", amountReturn);
                cmd.Parameters.AddWithValue("@saleID", salesID);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateSalesEarning(Int64 salesID, Int64 custID, float amountGiven, float? amountRemain=null, float? changeGive=null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateSaleEarning", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.AddWithValue("@amountGiven", amountGiven);
                
                if (amountRemain == null)
                    cmd.Parameters.AddWithValue("@amountRemain", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@amountRemain", amountRemain);

                if (changeGive == null)
                    cmd.Parameters.AddWithValue("@amountReturn", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@amountReturn", changeGive);
                cmd.Parameters.AddWithValue("@id", custID);
                cmd.Parameters.AddWithValue("@saleID", salesID);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateSaleReturn(Int64 saleID, Int64 proID, int quantity, int? itemQty=null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateSalesReturn", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@saleID", saleID);
                cmd.Parameters.AddWithValue("@proID", proID);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                if (itemQty==null)
                    cmd.Parameters.AddWithValue("@itemQty", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@itemQty", itemQty);

                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                //MainClass.show_msg(name + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateSaleData(Int64 saleID, float totalAmount, float amountReturn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateSalesData", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@saleID", saleID);
                cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@amountReturn", amountReturn);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                //MainClass.show_msg(name + " updated successfully.", "Success", "Success");
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateCustomers(Int64 id, string name, string mobile, string address)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateCustomers", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@mobile", mobile);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@id", id);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(name + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updatePurchaseSuppCDB(Int64 mPID, float amountGiven, float? amountRemain = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updatePurchaseSuppCDB", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@amountGiven", amountGiven);
                if (amountRemain == null)
                    cmd.Parameters.AddWithValue("@amountRemain", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@amountRemain", amountRemain);
                cmd.Parameters.AddWithValue("@id", mPID);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateDailyExpenditure(Int64 id, string desc, string amount, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateDailyExp", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@id", id);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(amount + " updated successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateDailyIncome(float amount, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateIncomeFSales", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", date);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void updateExpAmount(float amount, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_updateEXP", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", date);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }
    }
}
