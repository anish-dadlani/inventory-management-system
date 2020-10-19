using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;

namespace Medical_Store
{
    class reterival
    {
        updation u = new updation();
        private static string user_name = null, pass_word = null, user_email = null;
        private object product_stock_count = 0, product_Pack_stock_count = 0;
        private static bool checkLogin, checkEmail;
        private object[] StockReport = new object[4];
        private object[] EarningTotal = new object[2];
        private object[] EarningToday = new object[2];
        private string[] productsData = new string[4];
        private string[] productsData1 = new string[9];
        private string[] productsData2 = new string[9];
        private object[] productPriceDetails = new object[5];
        private object[] StockReportExpireProduct = new object[4];
        private object[] StockReportAbouttoExpireProduct = new object[4];

        public void show_users(DataGridView gv, DataGridViewColumn userIDGV, DataGridViewColumn NameGV, DataGridViewColumn UserNameGV, DataGridViewColumn PassGV, DataGridViewColumn PhoneGV, DataGridViewColumn EmailGV, DataGridViewColumn StatusGV, string data=null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getUserData", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getUserDataLike", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                userIDGV.DataPropertyName = dt.Columns["ID"].ToString();
                NameGV.DataPropertyName = dt.Columns["NAME"].ToString();
                UserNameGV.DataPropertyName = dt.Columns["USERNAME"].ToString();
                PassGV.DataPropertyName = dt.Columns["PASSWORD"].ToString();
                PhoneGV.DataPropertyName = dt.Columns["PHONE"].ToString();
                EmailGV.DataPropertyName = dt.Columns["Email"].ToString();
                StatusGV.DataPropertyName = dt.Columns["Status"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void show_categories(DataGridView gv, DataGridViewColumn catIDGV, DataGridViewColumn catNameGV, DataGridViewColumn StatusGV, string data=null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getCategoriesData", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getCategoryDataLike", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                catIDGV.DataPropertyName = dt.Columns["ID"].ToString();
                catNameGV.DataPropertyName = dt.Columns["Category"].ToString();
                StatusGV.DataPropertyName = dt.Columns["Status"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void getList(string proc, ComboBox cb, string displayMember, string valueMember)
        {
            try
            {
                cb.DataSource = null;
                SqlCommand cmd = new SqlCommand(proc, MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "Select" };
                dt.Rows.InsertAt(dr,0);
                cb.DisplayMember = displayMember;
                cb.ValueMember = valueMember;
                cb.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void show_products(DataGridView gv, DataGridViewColumn proIDGV, DataGridViewColumn proNameGV, DataGridViewColumn packGV, DataGridViewColumn ExpiryGV, DataGridViewColumn CatGV, DataGridViewColumn BarcodeGV, DataGridViewColumn Cat_IDGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getProductsData", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getProductsDataLike", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                proNameGV.DataPropertyName = dt.Columns["Product"].ToString();
                packGV.DataPropertyName = dt.Columns["Packing"].ToString();
                ExpiryGV.DataPropertyName = dt.Columns["Expiry"].ToString();
                CatGV.DataPropertyName = dt.Columns["Category"].ToString();
                BarcodeGV.DataPropertyName = dt.Columns["Barcode"].ToString();
                Cat_IDGV.DataPropertyName = dt.Columns["Category ID"].ToString();
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public static int USER_ID
        {
            get;
            private set;
        }

        public static string EMP_NAME
        {
            get;
            private set;
        }

        public static string USER_NAME
        {
            get;
            private set;
        }

        public static bool getUserDetails(string username, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getUserDetails", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    checkLogin = true;
                    while (dr.Read())
                    {
                        USER_ID = Convert.ToInt32(dr["ID"].ToString());
                        EMP_NAME = dr["Name"].ToString();
                        USER_NAME = dr["Username"].ToString();
                        user_name = dr["Username"].ToString();
                        pass_word = dr["Password"].ToString();
                    }
                }
                else
                {
                    checkLogin = false;
                    if (username != null && password != null)
                    {
                        if (user_name != username && pass_word == password)
                        {
                            MainClass.show_msg("Invalid Username", "Error", "Error");
                        }
                        else if (user_name == username && pass_word != password)
                        {
                            MainClass.show_msg("Invalid Password", "Error", "Error");
                        }
                        else if (user_name != username && pass_word != password)
                        {
                            MainClass.show_msg("Invalid Username and Password", "Error", "Error");
                        }
                    }
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return checkLogin;
        }

        public static bool getUserEmail(string useremail)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getEmail", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", useremail);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    checkEmail = true;
                    while (dr.Read())
                    {
                        user_email = dr["EMAIL"].ToString();
                    }
                }
                else
                {
                    checkEmail = false;
                    if (useremail != null)
                    {
                        if (user_email != useremail)
                            MainClass.show_msg("Email Address not Found.", "Error", "Error");
                    }
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return checkEmail;
        }

        public void show_suppliers(DataGridView gv, DataGridViewColumn suppIDGV, DataGridViewColumn comNameGV, DataGridViewColumn personGV, DataGridViewColumn mobileGV, DataGridViewColumn phoneGV, DataGridViewColumn addressGV, DataGridViewColumn ntnGV, DataGridViewColumn StatusGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getSupplierData", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getSupplierDataLike", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                suppIDGV.DataPropertyName   = dt.Columns["ID"].ToString();
                comNameGV.DataPropertyName  = dt.Columns["Company"].ToString();
                personGV.DataPropertyName   = dt.Columns["Contact Person"].ToString();
                mobileGV.DataPropertyName   = dt.Columns["Mobile"].ToString();
                phoneGV.DataPropertyName    = dt.Columns["Phone"].ToString();
                addressGV.DataPropertyName  = dt.Columns["Address"].ToString();
                ntnGV.DataPropertyName      = dt.Columns["NTN"].ToString();
                StatusGV.DataPropertyName   = dt.Columns["Status"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public string[] getPWRBarcode(string barcode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductByBarcode", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@barcode", barcode);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        productsData[0] = dr[0].ToString(); // Product ID
                        productsData[1] = dr[1].ToString(); // Product
                        productsData[2] = dr[2].ToString(); // Barcode
                        productsData[3] = dr[3].ToString(); // Packing
                        //productsData[3] = dr[3].ToString(); // Selling Price
                        //productsData[4] = dr[4].ToString(); // Discount
                        //productsData[5] = dr[5].ToString(); // Final Selling Price
                    }
                }
                else
                {
                    Array.Clear(productsData, 0, productsData.Length);
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return productsData;
        }

        public void getListWithTwoParam(string proc, ComboBox cb, string displayMember, string valueMember, string param1, object val1, string param2, object val2)
        {
            try
            {
                cb.DataSource = null;
                SqlCommand cmd = new SqlCommand(proc, MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(param1,val1);
                cmd.Parameters.AddWithValue(param2, val2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "Select" };
                dt.Rows.InsertAt(dr, 0);
                cb.DisplayMember = displayMember;
                cb.ValueMember = valueMember;
                cb.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }

        }

        public object getProductQuantity(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductQuantity", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "st_PurchaseInvoiceDetails";
                cmd.Parameters.AddWithValue("@proID", proID);
                MainClass.con.Open();
                product_stock_count = cmd.ExecuteScalar();
                MainClass.con.Close();

            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return product_stock_count;
        }

        public object getProductPackingQuantity(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductPackQuantity", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "st_PurchaseInvoiceDetails";
                cmd.Parameters.AddWithValue("@proID", proID);
                MainClass.con.Open();
                product_Pack_stock_count = cmd.ExecuteScalar();
                MainClass.con.Close();

            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return product_Pack_stock_count;
        }

        public object getProductQuantityWithoutConnection(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductQuantity", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                product_stock_count = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return product_stock_count;
        }

        public object getProductPackingQuantityWithoutConnection(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductPackQuantity", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                product_Pack_stock_count = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return product_Pack_stock_count;
        }

        public void show_purchase_invoice_details(Int64 pid, DataGridView gv, DataGridViewColumn mPIDGV, DataGridViewColumn proIDGV, DataGridViewColumn ProductGV, DataGridViewColumn quantityGV, DataGridViewColumn pupGV, DataGridViewColumn totalGV, string data=null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getPurchaseInvoiceDetails", MainClass.con);
                    cmd.Parameters.AddWithValue("@pid", pid);
                }
                else
                {
                    cmd = new SqlCommand("st_getPurchaseInvoiceDetails", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                mPIDGV.DataPropertyName = dt.Columns["mPID"].ToString();
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                ProductGV.DataPropertyName = dt.Columns["Product"].ToString();
                quantityGV.DataPropertyName = dt.Columns["Quantity"].ToString();
                pupGV.DataPropertyName = dt.Columns["Per Unit Price"].ToString();
                totalGV.DataPropertyName = dt.Columns["Total Price"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void show_stock(DataGridView gv, DataGridViewColumn proIDGV, DataGridViewColumn proGV, DataGridViewColumn barcodeGV, DataGridViewColumn expiryGV, DataGridViewColumn bpGV, DataGridViewColumn spGV, DataGridViewColumn catGV, DataGridViewColumn availstGV, DataGridViewColumn StatusGV, DataGridViewColumn totGV, DataGridViewColumn totSellGV, DataGridViewColumn packingGV, DataGridViewColumn itemPackQGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getAllStock", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getAllStockSearch", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                proGV.DataPropertyName = dt.Columns["Product"].ToString();
                packingGV.DataPropertyName = dt.Columns["Packing"].ToString();
                barcodeGV.DataPropertyName = dt.Columns["Barcode"].ToString();
                expiryGV.DataPropertyName = dt.Columns["Expiry Date"].ToString();
                bpGV.DataPropertyName = dt.Columns["Buying Price"].ToString();
                spGV.DataPropertyName = dt.Columns["Selling Price"].ToString();
                catGV.DataPropertyName = dt.Columns["Category"].ToString();
                availstGV.DataPropertyName = dt.Columns["Available Stock"].ToString();
                totGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                totSellGV.DataPropertyName = dt.Columns["Total Amount Sell"].ToString();
                StatusGV.DataPropertyName = dt.Columns["Status"].ToString();
                itemPackQGV.DataPropertyName = dt.Columns["Per Packing Quantity"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void showProductsWRTCategory(int catID, DataGridView gv, DataGridViewColumn proIDGV, DataGridViewColumn ProductGV, DataGridViewColumn packingGV, DataGridViewColumn bpGV, DataGridViewColumn spGV, DataGridViewColumn discGV, DataGridViewColumn profitGV, DataGridViewColumn pispGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getProductsWRTCategory", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getProductsWRTCategoryData", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.Parameters.AddWithValue("@catID", catID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                ProductGV.DataPropertyName = dt.Columns["Product"].ToString();
                packingGV.DataPropertyName = dt.Columns["Packing"].ToString();
                bpGV.DataPropertyName = dt.Columns["Buying Price"].ToString();
                spGV.DataPropertyName = dt.Columns["Selling Price"].ToString();
                discGV.DataPropertyName = dt.Columns["Discount"].ToString();
                profitGV.DataPropertyName = dt.Columns["Profit Percentage"].ToString();
                pispGV.DataPropertyName = dt.Columns["Per Item Selling Price"].ToString();
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public object[] checkProductPriceExistance(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_checkProductPriceExist", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while (dr.Read())
                    {
                        productPriceDetails[0] = dr[0].ToString(); //proID
                        productPriceDetails[1] = dr[1].ToString(); //bp
                        productPriceDetails[2] = dr[2].ToString(); //sp
                        productPriceDetails[3] = dr[3].ToString(); //disPercentage
                        productPriceDetails[4] = dr[4].ToString(); //profitPercentage
                    }
                }
                else
                {
                    Array.Clear(productPriceDetails, 0, productPriceDetails.Length);
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return productPriceDetails;
        }

        public void showSalesInvoices(DateTime date, DataGridView gv, DataGridViewColumn saleIDGV, DataGridViewColumn userGV, DataGridViewColumn totAmtGV, DataGridViewColumn totDisGV, DataGridViewColumn givenGV, DataGridViewColumn returnGV, DataGridViewColumn payGV, DataGridViewColumn dateGV, DataGridViewColumn userIDGV ,string data=null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getSalesInvoices", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getSalesInvoicesData", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", date);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                saleIDGV.DataPropertyName   = dt.Columns["Sales ID"].ToString();
                userGV.DataPropertyName     = dt.Columns["User"].ToString();
                //productGV.DataPropertyName = dt.Columns["Product Name"].ToString();
                totAmtGV.DataPropertyName   = dt.Columns["Total Amount"].ToString();
                totDisGV.DataPropertyName   = dt.Columns["Total Discount"].ToString();
                givenGV.DataPropertyName    = dt.Columns["Amount Given"].ToString();
                returnGV.DataPropertyName   = dt.Columns["Amount Returned"].ToString();
                payGV.DataPropertyName      = dt.Columns["Payment Type"].ToString();
                dateGV.DataPropertyName     = dt.Columns["Date"].ToString();
                userIDGV.DataPropertyName   = dt.Columns["User ID"].ToString();
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void showSaleDataReturn(Int64 saleID, DataGridView gv, DataGridViewColumn saleIDGV, DataGridViewColumn barcodeGV, DataGridViewColumn productGV, DataGridViewColumn quantityGV, DataGridViewColumn priceGV, DataGridViewColumn totdisGV, DataGridViewColumn totAmtGV, DataGridViewColumn givenGV, DataGridViewColumn changeGV, DataGridViewColumn ppDGV, DataGridViewColumn ppTGV, DataGridViewColumn dateGV, DataGridViewColumn usrGV, DataGridViewColumn payTypeGV, DataGridViewColumn proIDGV, DataGridViewColumn packingGV, DataGridViewColumn itemNogv, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getReciptWRTID", MainClass.con);
                    cmd.Parameters.AddWithValue("@salesID", saleID);
                }
                else
                {
                    cmd = new SqlCommand("st_getReciptWRTID", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                saleIDGV.DataPropertyName = dt.Columns["Sales ID"].ToString();
                barcodeGV.DataPropertyName = dt.Columns["Barcode"].ToString();
                productGV.DataPropertyName = dt.Columns["Product"].ToString();
                quantityGV.DataPropertyName = dt.Columns["Quantity"].ToString();
                priceGV.DataPropertyName = dt.Columns["Product Price"].ToString();
                ppDGV.DataPropertyName = dt.Columns["Per Product Discount"].ToString();
                ppTGV.DataPropertyName = dt.Columns["Per Product Total"].ToString();
                totdisGV.DataPropertyName = dt.Columns["Total Discount"].ToString();
                totAmtGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                givenGV.DataPropertyName = dt.Columns["Amount Given"].ToString();
                changeGV.DataPropertyName = dt.Columns["Amount Returned"].ToString();
                dateGV.DataPropertyName = dt.Columns["Date"].ToString();
                usrGV.DataPropertyName = dt.Columns["User"].ToString();
                payTypeGV.DataPropertyName = dt.Columns["Payment Type"].ToString();
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                packingGV.DataPropertyName = dt.Columns["Packing"].ToString();
                itemNogv.DataPropertyName = dt.Columns["Item Quantity"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public string[] getProductsWRBarcode(string barcode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductPrByBarcode", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@barcode", barcode);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        productsData1[0] = dr[0].ToString(); // Product ID
                        productsData1[1] = dr[1].ToString(); // Product
                        productsData1[2] = dr[2].ToString(); // Barcode
                        productsData1[3] = dr[3].ToString(); // Selling Price
                        productsData1[4] = dr[4].ToString(); // Discount
                        productsData1[5] = dr[5].ToString(); // Final Selling Price
                        productsData1[6] = dr[6].ToString(); // Packing
                        productsData1[7] = dr[7].ToString(); // Expiry
                        productsData1[8] = dr[8].ToString(); // ItemSP
                    }
                }
                else
                {
                    Array.Clear(productsData1, 0, productsData1.Length);
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return productsData1;
        }

        public object[] showStockReport() 
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getReport", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        StockReport[0] = dr[0].ToString(); //product
                        StockReport[1] = dr[1].ToString(); //stock
                        StockReport[2] = dr[2].ToString(); //bp
                        StockReport[3] = dr[3].ToString(); //sp
                    }
                }
                else
                    Array.Clear(StockReport, 0, StockReport.Length);
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return StockReport;
        }

        public object[] showEarningTotal()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getSaleEarning", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        EarningTotal[0] = dr[0].ToString(); //total earning
                        EarningTotal[1] = dr[1].ToString(); //total remaining money to customers
                    }
                }
                else
                    Array.Clear(EarningTotal, 0, EarningTotal.Length);
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return EarningTotal;
        }

        public object[] showEarningToday(DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getSaleEarningToday", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", date);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        EarningToday[0] = dr[0].ToString(); //total earning today
                        EarningToday[1] = dr[1].ToString(); //total remaining today
                    }
                }
                else
                    Array.Clear(EarningToday, 0, EarningToday.Length);
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return EarningToday;
        }

        public void show_stock_Expired(DataGridView gv, DataGridViewColumn proIDGV, DataGridViewColumn proGV, DataGridViewColumn barcodeGV, DataGridViewColumn expiryGV, DataGridViewColumn bpGV, DataGridViewColumn spGV, DataGridViewColumn catGV, DataGridViewColumn availstGV, DataGridViewColumn StatusGV, DataGridViewColumn totGV, DataGridViewColumn totSellGV, DataGridViewColumn packingGV, DataGridViewColumn itemPackQGV)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getAllStockWRTExpiredProduct", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                proGV.DataPropertyName = dt.Columns["Product"].ToString();
                packingGV.DataPropertyName = dt.Columns["Packing"].ToString();
                barcodeGV.DataPropertyName = dt.Columns["Barcode"].ToString();
                expiryGV.DataPropertyName = dt.Columns["Expiry Date"].ToString();
                bpGV.DataPropertyName = dt.Columns["Buying Price"].ToString();
                spGV.DataPropertyName = dt.Columns["Selling Price"].ToString();
                catGV.DataPropertyName = dt.Columns["Category"].ToString();
                availstGV.DataPropertyName = dt.Columns["Available Stock"].ToString();
                totGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                totSellGV.DataPropertyName = dt.Columns["Total Amount Sell"].ToString();
                StatusGV.DataPropertyName = dt.Columns["Status"].ToString();
                itemPackQGV.DataPropertyName = dt.Columns["Per Packing Quantity"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void show_stock_About_to_Expire(DataGridView gv, DataGridViewColumn proIDGV, DataGridViewColumn proGV, DataGridViewColumn barcodeGV, DataGridViewColumn expiryGV, DataGridViewColumn bpGV, DataGridViewColumn spGV, DataGridViewColumn catGV, DataGridViewColumn availstGV, DataGridViewColumn StatusGV, DataGridViewColumn totGV, DataGridViewColumn totSellGV, DataGridViewColumn packingGV, DataGridViewColumn itemPackQGV)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getAllStockWRTAboutToProduct", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                proIDGV.DataPropertyName = dt.Columns["Product ID"].ToString();
                proGV.DataPropertyName = dt.Columns["Product"].ToString();
                packingGV.DataPropertyName = dt.Columns["Packing"].ToString();
                barcodeGV.DataPropertyName = dt.Columns["Barcode"].ToString();
                expiryGV.DataPropertyName = dt.Columns["Expiry Date"].ToString();
                bpGV.DataPropertyName = dt.Columns["Buying Price"].ToString();
                spGV.DataPropertyName = dt.Columns["Selling Price"].ToString();
                catGV.DataPropertyName = dt.Columns["Category"].ToString();
                availstGV.DataPropertyName = dt.Columns["Available Stock"].ToString();
                totGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                totSellGV.DataPropertyName = dt.Columns["Total Amount Sell"].ToString();
                StatusGV.DataPropertyName = dt.Columns["Status"].ToString();
                itemPackQGV.DataPropertyName = dt.Columns["Per Packing Quantity"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void showCustomersMoneyRemaining(DataGridView gv, DataGridViewColumn saleIDGV, DataGridViewColumn custIDGV, DataGridViewColumn userGV, DataGridViewColumn totAmtGV, DataGridViewColumn totDisGV, DataGridViewColumn givenGV, DataGridViewColumn payGV, DataGridViewColumn remainGV, DataGridViewColumn customerGV, DataGridViewColumn customerAddGV, DataGridViewColumn dateGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                    cmd = new SqlCommand("st_getCustomersMoneyRemaining", MainClass.con);
                else
                {
                    cmd = new SqlCommand("st_getCustomersMoneyRemainingData", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                saleIDGV.DataPropertyName = dt.Columns["Sales ID"].ToString();
                custIDGV.DataPropertyName = dt.Columns["Customer ID"].ToString();
                userGV.DataPropertyName = dt.Columns["User"].ToString();
                totAmtGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                totDisGV.DataPropertyName = dt.Columns["Total Discount"].ToString();
                givenGV.DataPropertyName = dt.Columns["Amount Given"].ToString();
                payGV.DataPropertyName = dt.Columns["Payment Type"].ToString();
                remainGV.DataPropertyName = dt.Columns["Amount Remaining"].ToString();
                dateGV.DataPropertyName = dt.Columns["Date"].ToString();
                customerGV.DataPropertyName = dt.Columns["Customer Name"].ToString();
                customerAddGV.DataPropertyName = dt.Columns["Customer Address"].ToString();
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public object[] showStockReportExpireProduct()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getReportExpireProduct", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        StockReportExpireProduct[0] = dr[0].ToString(); //product
                        StockReportExpireProduct[1] = dr[1].ToString(); //stock
                        StockReportExpireProduct[2] = dr[2].ToString(); //bp
                        StockReportExpireProduct[3] = dr[3].ToString(); //sp
                    }
                }
                else
                    Array.Clear(StockReportExpireProduct, 0, StockReportExpireProduct.Length);
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return StockReportExpireProduct;
        }

        public object[] showStockReportAbouttoExpireProduct()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getReportAbouttoExpireProduct", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        StockReportAbouttoExpireProduct[0] = dr[0].ToString(); //product
                        StockReportAbouttoExpireProduct[1] = dr[1].ToString(); //stock
                        StockReportAbouttoExpireProduct[2] = dr[2].ToString(); //bp
                        StockReportAbouttoExpireProduct[3] = dr[3].ToString(); //sp
                    }
                }
                else
                    Array.Clear(StockReportAbouttoExpireProduct, 0, StockReportAbouttoExpireProduct.Length);
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return StockReportAbouttoExpireProduct;
        }

        public void showStatistics(DataGridView gv, DataGridViewColumn yearGV, DataGridViewColumn monthGV, DataGridViewColumn salesGV, DataGridViewColumn totalAmountGV, DataGridViewColumn monthNOGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getSalesMonthWise", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getSalesMonthWise", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                yearGV.DataPropertyName = dt.Columns["Year"].ToString();
                monthGV.DataPropertyName = dt.Columns["Month Name"].ToString();
                salesGV.DataPropertyName = dt.Columns["sales Count"].ToString();
                totalAmountGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                monthNOGV.DataPropertyName = dt.Columns["Month"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void showReport(ReportDocument rd, CrystalReportViewer crv,  string proc, string param1, object val1)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(proc, MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(param1,val1);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rd.Load(Application.StartupPath + "\\Reports\\salesRecipt.rpt");
                rd.SetDataSource(dt);
                crv.ReportSource = rd;
                crv.RefreshReport();
            }
            catch(Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void showCustomers(DataGridView gv, DataGridViewColumn custIDGV, DataGridViewColumn customerNameGV, DataGridViewColumn mobileGV, DataGridViewColumn addressGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getCustomerData", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getCustomerDataSearch", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                custIDGV.DataPropertyName = dt.Columns["ID"].ToString();
                customerNameGV.DataPropertyName = dt.Columns["Customer Name"].ToString();
                mobileGV.DataPropertyName = dt.Columns["Mobile No"].ToString();
                addressGV.DataPropertyName = dt.Columns["Address"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        public void showSupplierCDB(DataGridView gv, DataGridViewColumn mPIDGV, DataGridViewColumn suppIDGV, DataGridViewColumn totalGV, DataGridViewColumn supCompGV, DataGridViewColumn supAddGV, DataGridViewColumn amtGivenGV, DataGridViewColumn amtRemainGV, DataGridViewColumn dateGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getSupplierPurchaseCDB", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getSupplierPurchaseCDBSearch", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                mPIDGV.DataPropertyName = dt.Columns["Purchase Invoice ID"].ToString();
                suppIDGV.DataPropertyName = dt.Columns["Supplier ID"].ToString();
                supCompGV.DataPropertyName = dt.Columns["Supplier Company"].ToString();
                supAddGV.DataPropertyName = dt.Columns["Supplier Address"].ToString();
                totalGV.DataPropertyName = dt.Columns["Total Amount"].ToString();
                amtGivenGV.DataPropertyName = dt.Columns["Amount Given"].ToString();
                amtRemainGV.DataPropertyName = dt.Columns["Amount Remaining"].ToString();
                dateGV.DataPropertyName = dt.Columns["Date"].ToString();
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        object producPacking;
        public object getProductQuantityWRTID(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductBYID", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                MainClass.con.Open();
                producPacking = cmd.ExecuteScalar();
                MainClass.con.Close();

            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return producPacking;
        }

        public string[] getProductsBYWRTID(Int64 proID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getProductPRodbyID", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@proID", proID);
                MainClass.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        productsData2[0] = dr[0].ToString(); // Product ID
                        productsData2[1] = dr[1].ToString(); // Product
                        productsData2[2] = dr[2].ToString(); // Barcode
                        productsData2[3] = dr[3].ToString(); // Selling Price
                        productsData2[4] = dr[4].ToString(); // Discount
                        productsData2[5] = dr[5].ToString(); // Final Selling Price
                        productsData2[6] = dr[6].ToString(); // Packing
                        productsData2[7] = dr[7].ToString(); // Expiry
                        productsData2[8] = dr[8].ToString(); // ItemSP
                    }
                }
                else
                {
                    Array.Clear(productsData2, 0, productsData2.Length);
                }
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.con.Close();
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return productsData2;
        }

        public void showDailyExp(DataGridView gv, DataGridViewColumn expIDGV, DataGridViewColumn descGV, DataGridViewColumn amountGV, DataGridViewColumn dateGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getDailyExp", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getDailyExp", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                expIDGV.DataPropertyName = dt.Columns["Expense ID"].ToString();
                descGV.DataPropertyName = dt.Columns["Description"].ToString();
                amountGV.DataPropertyName = dt.Columns["Amount"].ToString();
                dateGV.DataPropertyName = dt.Columns["Date"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        object incomeAmountCheck;
        public object getIncomeAmountDate(DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getIncomeAmountDate", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", date);
                MainClass.con.Open();
                incomeAmountCheck = cmd.ExecuteScalar();
                MainClass.con.Close();

            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return incomeAmountCheck;
        }

        public void showIncomeExpeneseBalance(DataGridView gv, DataGridViewColumn dateGV, DataGridViewColumn incomeGV, DataGridViewColumn expenseGV, DataGridViewColumn balanceGV, string data = null)
        {
            try
            {
                SqlCommand cmd;
                if (data == null)
                {
                    cmd = new SqlCommand("st_getIncomeExpenseBalance", MainClass.con);
                }
                else
                {
                    cmd = new SqlCommand("st_getIncomeExpenseBalance", MainClass.con);
                    cmd.Parameters.AddWithValue("@data", data);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                incomeGV.DataPropertyName = dt.Columns["TotalInc"].ToString();
                expenseGV.DataPropertyName = dt.Columns["TotalExp"].ToString();
                balanceGV.DataPropertyName = dt.Columns["ProfitNLoss"].ToString();
                dateGV.DataPropertyName = dt.Columns["Date"].ToString();

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }
   
        object expID;
        public object getExpID()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getExpID", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                MainClass.con.Open();
                expID = cmd.ExecuteScalar();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return expID;
        }

        object expAmount;
        public object getExpAmount(DateTime date)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_gtExpAmount", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", date);
                MainClass.con.Open();
                expAmount = cmd.ExecuteScalar();
                MainClass.con.Close();
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
            return expAmount;
        }
    }
}
