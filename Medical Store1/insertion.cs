using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Transactions;
using System.Text.RegularExpressions;

namespace Medical_Store
{
    class insertion
    {
        reterival r = new reterival();
        updation u = new updation();
        int saleCount = 0, i = 0, pidCount;
        Int64 salesID;
        private Int64 purchaseInvoiceID;

        public void insertUsers(string name, string username, string password, string email, string phone, Int16 status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertUsers", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name",name);
                cmd.Parameters.AddWithValue("@username",username);
                cmd.Parameters.AddWithValue("@pwd",password);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email",email);
                cmd.Parameters.AddWithValue("@status", status);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(name+" added to the system successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message,"Error","Error");
            }
        }

        public void insertCategory(string name, Int16 status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertCategory", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@isActive", status);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(name + " added to the system successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void insertProduct(string product, string barcode, int catID, string packing, DateTime? expiry=null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_productInsert", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", product);
                cmd.Parameters.AddWithValue("@barcode", barcode);
                if (expiry == null)
                    cmd.Parameters.AddWithValue("@expiry", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@expiry", expiry);
                cmd.Parameters.AddWithValue("@catID", catID);
                cmd.Parameters.AddWithValue("@pack", packing);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(product + " added to the system successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void insertSupplier(string company, string person, string mobile, string address, Int16 status, string phone=null, string ntn=null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertSupplier", MainClass.con);
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
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(company + " added to the system successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public Int64 insertPurchaseInvoice(DateTime date, int doneby, int supid, float tprice, float? amountGiven = null, float? amountRemain = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_purchaseInvoice_Insert", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@doneBy", doneby);
                cmd.Parameters.AddWithValue("@suppID", supid);
                cmd.Parameters.AddWithValue("@total_amount", tprice);
                if (amountGiven == null)
                    cmd.Parameters.AddWithValue("@amount_given", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@amount_given", amountGiven);
                if (amountRemain == null)
                    cmd.Parameters.AddWithValue("@amount_remain", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@amount_remain", amountRemain);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "st_getLastPID";
                cmd.Parameters.Clear();
                purchaseInvoiceID = Convert.ToInt64(cmd.ExecuteScalar());
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return purchaseInvoiceID;
        }

        public int insertPurchaseInvoiceDetails(Int64 purchaseInvoiceID, Int64 proID, int quan, float amount)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_purchaseInvoice_Insert", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "st_PurchaseInvoiceDetails";
                cmd.Parameters.AddWithValue("@purchaseID",purchaseInvoiceID);
                cmd.Parameters.AddWithValue("@proID",proID);
                cmd.Parameters.AddWithValue("@quan",quan);
                cmd.Parameters.AddWithValue("@amount", amount);
                MainClass.con.Open();
                pidCount = cmd.ExecuteNonQuery();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return pidCount;
        }

        public void insertStock(Int64 proID, int quan, int? pack_quan=null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertStock", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                cmd.Parameters.AddWithValue("@quan", quan);
                if (pack_quan==null)
                {
                    cmd.Parameters.AddWithValue("@pack_quan", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@pack_quan", pack_quan);
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

        public void insertDeletedItem(Int64 pid, Int64 proid, int quan, int userid, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertDeletedItemPI", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pi", pid);
                cmd.Parameters.AddWithValue("@usrID", userid);
                cmd.Parameters.AddWithValue("@proID", proid);
                cmd.Parameters.AddWithValue("@quan", quan);
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

        public void insertProductPrice(Int64 proID, float buyingAmount)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertProductPrice", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                cmd.Parameters.AddWithValue("@bp", buyingAmount);
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
        
        int packingCount = 0, finalPackProductQuanityt = 0, finalProductQuantity = 0, itemCountNo = 0;
        public void insertSales(DataGridView gv, string proIDgv, string proQuangv, string spgv, string discountgv, int doneBy, DateTime dt, float tAmount, float tDiscount, float aGiven, string payType, string packingCountGV, string itemCountNoGV=null, float? aReturn = null, float? amountRemaining = null, Int64? custID = null)
        {
            try
            {
                using(TransactionScope sc = new TransactionScope())
                {
                    int stockQuantityItem = 0;
                    SqlCommand cmd = new SqlCommand("insertSales", MainClass.con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@done", doneBy);
                    cmd.Parameters.AddWithValue("@date", dt);
                    cmd.Parameters.AddWithValue("@totamt", tAmount);
                    cmd.Parameters.AddWithValue("@totdis", tDiscount);
                    cmd.Parameters.AddWithValue("@given", aGiven);
                    if (payType == "Cash")
                        cmd.Parameters.AddWithValue("@payType", 0);
                    else if (payType == "Debit Card")
                        cmd.Parameters.AddWithValue("@payType", 1);
                    else if (payType == "Credit Card")
                        cmd.Parameters.AddWithValue("@payType", 2);
                    if (amountRemaining == null)
                        cmd.Parameters.AddWithValue("@remainAmount", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@remainAmount", amountRemaining);
                    if (aReturn == null)
                        cmd.Parameters.AddWithValue("@return", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@return", aReturn);
                    if (custID == null)
                        cmd.Parameters.AddWithValue("@custID", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@custID", custID);
                    MainClass.con.Open();
                    saleCount = cmd.ExecuteNonQuery();
                    if (saleCount > 0)
                    {
                        SqlCommand cmd2 = new SqlCommand("st_getSalesID", MainClass.con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        salesID = Convert.ToInt64(cmd2.ExecuteScalar());
                         
                        foreach (DataGridViewRow row in gv.Rows)
                        {
                            SqlCommand cmd3 = new SqlCommand("st_insertSalesDetails", MainClass.con);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.AddWithValue("@saleID", salesID);
                            cmd3.Parameters.AddWithValue("@proID", Convert.ToInt64(row.Cells[proIDgv].Value.ToString()));
                            cmd3.Parameters.AddWithValue("@quan", Convert.ToInt32(row.Cells[proQuangv].Value.ToString()));
                            cmd3.Parameters.AddWithValue("@sp", Convert.ToSingle(row.Cells[spgv].Value.ToString()));
                            cmd3.Parameters.AddWithValue("@disc", Convert.ToSingle(row.Cells[discountgv].Value.ToString()));

                            object output = Regex.Replace(row.Cells[packingCountGV].Value.ToString(), "[^0-9]+", string.Empty);
                            object output_String = Regex.Replace(row.Cells[packingCountGV].Value.ToString(), "[^a-zA-Z]", "");
                            string output_str = Convert.ToString(output_String);

                            if (Convert.ToInt32(row.Cells[proQuangv].Value) >= 1 && Convert.ToInt32(row.Cells[itemCountNoGV].Value) == 0)
                            {
                                int stockofProduct = Convert.ToInt32(r.getProductQuantityWithoutConnection(Convert.ToInt64(row.Cells[proIDgv].Value.ToString())));
                                if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                {
                                    cmd3.Parameters.AddWithValue("@itemQuan", 0);
                                    finalPackProductQuanityt = 0;
                                }
                                else
                                {
                                    packingCount = Convert.ToInt32(output) * Convert.ToInt32(row.Cells[proQuangv].Value);
                                    cmd3.Parameters.AddWithValue("@itemQuan", packingCount);
                                    stockQuantityItem = Convert.ToInt32(r.getProductPackingQuantityWithoutConnection(Convert.ToInt64(row.Cells[proIDgv].Value.ToString())));
                                    finalPackProductQuanityt = stockQuantityItem - packingCount;
                                }
                                int currentQuantityofProduct = Convert.ToInt32(row.Cells[proQuangv].Value.ToString());
                                //finalPackProductQuanityt = stockQuantityItem - packingCount;
                                finalProductQuantity = stockofProduct - currentQuantityofProduct;
                                u.updateStockWithoutConnection(Convert.ToInt64(row.Cells[proIDgv].Value.ToString()), finalProductQuantity, finalPackProductQuanityt);
                                finalPackProductQuanityt = 0;
                                finalProductQuantity = 0;
                                stockQuantityItem = 0;
                            }
                            else if (Convert.ToInt32(row.Cells[proQuangv].Value) == 1 && Convert.ToInt32(row.Cells[itemCountNoGV].Value) >= 1 && Convert.ToInt32(row.Cells[itemCountNoGV].Value) < Convert.ToInt32(output))
                            {
                                itemCountNo = Convert.ToInt32(row.Cells[itemCountNoGV].Value);
                                cmd3.Parameters.AddWithValue("@itemQuan", itemCountNo);
                                int stockofProduct = Convert.ToInt32(r.getProductQuantityWithoutConnection(Convert.ToInt64(row.Cells[proIDgv].Value.ToString())));
                                int updatedPackQuantity = Convert.ToInt32(output) * stockofProduct;
                                if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                {
                                    finalPackProductQuanityt = 0;
                                }
                                else
                                {
                                    stockQuantityItem = Convert.ToInt32(r.getProductPackingQuantityWithoutConnection(Convert.ToInt64(row.Cells[proIDgv].Value.ToString())));
                                    int checkProductPackQty = stockQuantityItem - itemCountNo;
                                    updatedPackQuantity = updatedPackQuantity - Convert.ToInt32(output);
                                    if (updatedPackQuantity != checkProductPackQty)
                                    {
                                        finalPackProductQuanityt = stockQuantityItem - itemCountNo;
                                        finalProductQuantity = stockofProduct;
                                    }
                                    else
                                    {
                                        finalPackProductQuanityt = stockQuantityItem - itemCountNo;
                                        finalProductQuantity = stockofProduct - 1;
                                    }
                                }
                                int currentQuantityofProduct = Convert.ToInt32(row.Cells[proQuangv].Value.ToString());
                                //finalPackProductQuanityt = stockQuantityItem - itemCountNo;
                                //finalProductQuantity = stockofProduct;
                                u.updateStockWithoutConnection(Convert.ToInt64(row.Cells[proIDgv].Value.ToString()), finalProductQuantity, finalPackProductQuanityt);
                                stockQuantityItem = 0;
                            }
                            cmd3.ExecuteNonQuery();
                        }
                    }
                    MainClass.con.Close();
                    MainClass.show_msg("Sales Successfull","Success","Success");
                    sc.Complete();
                }
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public int insertRefundReturn(Int64 saleID, DateTime date, int userID, Int64 proID, int quantity, float amtRefund)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertRefundReturn", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@saleID", saleID);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@doneby", userID);
                cmd.Parameters.AddWithValue("@proID", proID);
                cmd.Parameters.AddWithValue("@quan", quantity);
                cmd.Parameters.AddWithValue("@amount", amtRefund);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                i++;
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return i;
        }

        public void insertCustomers(string name, string mobile, string address)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertCustomers", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@mobile", mobile);
                cmd.Parameters.AddWithValue("@address", address);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(name + " added successfully.", "Success", "Success");

            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void insertDailyExpenditure(string desc, float amount, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertDailyExp", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", date);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MainClass.show_msg(amount + " added successfully.", "Success", "Success");
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void insertDailyIncome(float amount, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertIncomeFSales", MainClass.con);
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

        public void insertExpAmount(Int64 expID, float amount, DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_insertEXP", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@expID", expID);
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
