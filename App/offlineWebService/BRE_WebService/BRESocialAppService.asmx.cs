using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

namespace BRE_WebService
{
    /// <summary>
    /// Summary description for BRESocialAppService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class BRESocialAppService : System.Web.Services.WebService
    {
        SqlConnection sqlcon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BRE_WebService.Properties.Settings.Setting"].ToString());
        //SqlConnection sqlcon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BRE_WebService.Properties.Settings.SettingOnline"].ToString());

        //TODO: update countNormal to have virtual too

        # region GET METHODS

        # region User Related

        [WebMethod]
        public List<User> GetAllUser()
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM BRE_User", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<User> listOfUsers = AddUsers(sr);

            sqlcon.Close();
            return listOfUsers;
        }

        [WebMethod]
        public int GetUserID(string Username, string Password)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT UserID FROM BRE_User WHERE Username='" + Username + "' and Password='" + Password + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            int id = -1;
            while (sr.Read())
            {
                id = sr.GetInt32(0);
            }

            sqlcon.Close();

            return id;
            
        }

        [WebMethod]
        public User GetUser(string Username, string Password)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM BRE_User WHERE Username='" + Username + "' AND Password='" + Password + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            User userDetails = new User();
            while (sr.Read())
            {
                
                userDetails.UserID = sr.GetInt32(0);
                userDetails.Firstname = sr.GetString(1);
                userDetails.Surname = sr.GetString(2);
                userDetails.Gender = sr.GetString(3);
                userDetails.DateOfBirth = sr.GetDateTime(4);
                userDetails.Email = sr.GetString(5);
                userDetails.Username = sr.GetString(6);
                userDetails.Password = sr.GetString(7);

                userDetails.HouseNumberName = sr.GetString(8);
                userDetails.Address = sr.GetString(9);
                userDetails.Town = sr.GetString(10);
                userDetails.Postcode = sr.GetString(11);
                userDetails.Categories = new List<CategoryInfo>();

            }

            sqlcon.Close();
            sqlcon.Open();
            SqlCommand com2 = new SqlCommand("SELECT Category.CategoryID, Category.CategoryName, UserCategoryPair.CategoryValue FROM Category JOIN UserCategoryPair on Category.CategoryID=UserCategoryPair.CategoryID WHERE UserCategoryPair.UserID='" + userDetails.UserID + "'", sqlcon);
            SqlDataReader sr2 = com2.ExecuteReader();

            while (sr2.Read())
            {
                CategoryInfo info = new CategoryInfo();
                info.CategoryID = sr2.GetInt32(0);
                info.CategoryName = sr2.GetString(1);
                info.CategoryValue = sr2.GetInt32(2);

                userDetails.Categories.Add(info);
            }

            sqlcon.Close();
            return userDetails;
        }

        [WebMethod]
        public List<double> GetUserRating(int UserID)
        {
            List<Category> categories = GetListOfCategories();
            List<double> numOfCategories = new List<double>(categories.Count + 1);

            List<double> tempValue = new List<double>();

            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM BRE_User WHERE UserID='" + UserID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                tempValue.Add(sr.GetInt32(12));
                tempValue.Add(sr.GetInt32(13));
                tempValue.Add(sr.GetInt32(14));
                tempValue.Add(sr.GetInt32(15));
                tempValue.Add(sr.GetInt32(16));
                tempValue.Add(sr.GetInt32(17));
                tempValue.Add(sr.GetInt32(18));
                tempValue.Add(sr.GetInt32(19));
                tempValue.Add(sr.GetInt32(20));
                tempValue.Add(sr.GetInt32(21));
                tempValue.Add(sr.GetInt32(22));
                tempValue.Add(sr.GetInt32(23));
                tempValue.Add(sr.GetInt32(24));
                tempValue.Add(sr.GetInt32(25));
                tempValue.Add(sr.GetInt32(26));
                tempValue.Add(sr.GetInt32(27));
                tempValue.Add(sr.GetInt32(28));
            }
            sqlcon.Close();
            numOfCategories.Add(0);

            for (int i = 1; i <= numOfCategories.Capacity - 1; i++)
            {
                numOfCategories.Add(tempValue[i - 1] * 10);
            }

            return numOfCategories;
        }

        [WebMethod]
        public bool GetUserCategoryPairExists(UserCategoryPair userCategoryPair)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT TOP 1 UserCategoryPair.Id FROM UserCategoryPair WHERE UserCategoryPair.CategoryID = '" + userCategoryPair.CategoryID + "' AND UserCategoryPair.UserID='" + userCategoryPair.UserID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            bool success = sr.HasRows;
            sqlcon.Close();

            return success;
        }

        # endregion

        # region Category Related

        [WebMethod]
        public List<Category> GetListOfCategories()
        {
            List<Category> listOfCategories = new List<Category>();
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Category", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                Category categories = new Category();

                categories.CategoryID = sr.GetInt32(0);
                categories.CategoryName = sr.GetString(1);

                listOfCategories.Add(categories);
            }

            sqlcon.Close();
            return listOfCategories;
        }

        [WebMethod]
        public Category GetCategoryByID(int CategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Category WHERE CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                Category categories = new Category();

                categories.CategoryID = sr.GetInt32(0);
                categories.CategoryName = sr.GetString(1);

                sqlcon.Close();
                return categories;
            }

            sqlcon.Close();
            return new Category();
        }

        [WebMethod]
        public List<Category> GetListOfCategoriesByServiceID(int ServiceID)
        {
            List<Category> listOfCategories = new List<Category>();
            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT DISTINCT Category.CategoryID, Category.CategoryName FROM Category 
                                                INNER JOIN CategorySubCategoryPair ON Category.CategoryID=CategorySubCategoryPair.CategoryID 
                                                INNER JOIN ServiceSubCategoryPair ON CategorySubCategoryPair.SubCategoryID=ServiceSubCategoryPair.SubCategoryID
                                                WHERE ServiceSubCategoryPair.ServiceID=" + ServiceID, sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                Category categories = new Category();

                categories.CategoryID = sr.GetInt32(0);
                categories.CategoryName = sr.GetString(1);
                

                listOfCategories.Add(categories);
            }

            sqlcon.Close();
            return listOfCategories;
        }

        [WebMethod]
        public List<TownCategoryCount> GetListOfCountNormalForTownByCategoryID(int CategoryID)
        {
            List<TownCategoryCount> result = new List<TownCategoryCount>();

            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM TownCategoryCount WHERE CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader Data = com.ExecuteReader();

            while (Data.Read())
            {
                TownCategoryCount TCC = new TownCategoryCount();

                TCC.TownID = Data.GetInt32(0);
                TCC.CategoryID = Data.GetInt32(1);
                TCC.Count = Data.GetInt32(2);
                TCC.Normal = Data.GetInt32(3);
                TCC.CountVirt = Data.GetInt32(4);
                TCC.NormalVirt = Data.GetInt32(5);

                result.Add(TCC);
            }

            sqlcon.Close();

            return result;
        }

        # endregion

        # region Subcategory Related

        [WebMethod]
        public List<SubCategory> GetListOfSubCategories()
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Sub_Category", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<SubCategory> listOfSubCategories = AddSubcategories(sr);

            sqlcon.Close();
            return listOfSubCategories;
        }

        [WebMethod]
        public List<SubCategory> GetListOfSubCategoriesByCategoryID(int CategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT DISTINCT Sub_Category.SubCategoryID, Sub_Category.SubCategoryName FROM Sub_Category 
                                                INNER JOIN CategorySubCategoryPair ON Sub_Category.SubCategoryID=CategorySubCategoryPair.SubCategoryID 
                                                WHERE CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<SubCategory> listOfSubCategories = AddSubcategories(sr);

            sqlcon.Close();
            return listOfSubCategories;
        }

        [WebMethod]
        public SubCategory GetSubCategoryByID(int SubCategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Sub_Category WHERE SubCategoryID='" + SubCategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                SubCategory subcategory = new SubCategory();

                subcategory.SubCategoryID = sr.GetInt32(0);
                subcategory.SubCategoryName = sr.GetString(1);

                sqlcon.Close();
                return subcategory;
            }

            sqlcon.Close();
            return new SubCategory();
        }

        [WebMethod]
        public List<SubCategory> GetListOfSubCategoriesByServiceID(int ServiceID)
        {
            List<SubCategory> listOfSubCategories = new List<SubCategory>();
            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT Sub_Category.SubCategoryID, Sub_Category.SubCategoryName FROM Sub_Category 
                                                INNER JOIN ServiceSubCategoryPair ON Sub_Category.SubCategoryID=ServiceSubCategoryPair.SubCategoryID 
                                                WHERE ServiceSubCategoryPair.ServiceID=" + ServiceID, sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                SubCategory subcategories = new SubCategory();

                subcategories.SubCategoryID = sr.GetInt32(0);
                subcategories.SubCategoryName = sr.GetString(1);


                listOfSubCategories.Add(subcategories);
            }

            sqlcon.Close();
            return listOfSubCategories;
        }

        # endregion

        # region Service Related

        [WebMethod]
        public List<TownServices> GetListOfServices()
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Towns ON Town_Services.TownID=Towns.TownID", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<TownServices> listOfServices = AddTownServices(sr);

            sqlcon.Close();
            return listOfServices;
        }

        [WebMethod]
        public int GetNumberOfServicesByCategoryIDAndTownID(int TownID, int CategoryID)
        {
            int count = -1;

            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT count(*) FROM Town_Services 
                                                INNER JOIN ServiceSubCategoryPair ON Town_Services.ServiceID=ServiceSubCategoryPair.ServiceID 
                                                INNER JOIN CategorySubCategoryPair ON ServiceSubCategoryPair.SubCategoryID=CategorySubCategoryPair.SubCategoryID 
                                                WHERE Town_Services.TownID = '" + TownID + "' AND CategorySubCategoryPair.CategoryID = '" + CategoryID + "'", sqlcon);

            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
                count = sr.GetInt32(0);

            sqlcon.Close();

            return count; 
  
        }
        public int GetNumberOfVirtualServicesByCategoryIDAndTownID(int TownID, int CategoryID)
        {
            int count = -1;

            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT count(*) FROM Town_Services 
                                                INNER JOIN ServiceSubCategoryPair ON Town_Services.ServiceID=ServiceSubCategoryPair.ServiceID 
                                                INNER JOIN CategorySubCategoryPair ON ServiceSubCategoryPair.SubCategoryID=CategorySubCategoryPair.SubCategoryID 
                                                WHERE Town_Services.TownID = '" + TownID + "' AND Town_Services.HasVirtualServices=1 AND CategorySubCategoryPair.CategoryID = '" + CategoryID + "'", sqlcon);

            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
                count = sr.GetInt32(0);

            sqlcon.Close();

            return count;

        }

        [WebMethod]
        public List<AllServiceInfo> GetListOfServices_Detailed()
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Towns ON Town_Services.TownID=Towns.TownID", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<AllServiceInfo> AllInfo = AddAllServiceInfo(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetAllServicesForTownByID(int TownID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Towns ON Town_Services.TownID=Towns.TownID WHERE Town_Services.TownID='" + TownID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            

            List<AllServiceInfo> AllInfo = AddAllServiceInfo(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetAllServicesByCategoryID(int CategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT DISTINCT Town_Services.*, Towns.* FROM Town_Services
                                                INNER JOIN Towns ON Town_Services.TownID=Towns.TownID 
                                                INNER JOIN ServiceSubCategoryPair ON Town_Services.ServiceID=ServiceSubCategoryPair.ServiceID
                                                INNER JOIN CategorySubCategoryPair ON ServiceSubCategoryPair.SubCategoryID=CategorySubCategoryPair.SubCategoryID
                                                WHERE CategorySubCategoryPair.CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();



            List<AllServiceInfo> AllInfo = AddAllServiceInfo(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetAllServicesByTownAndCategoryID(int CategoryID, int TownID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand(@"  SELECT DISTINCT Town_Services.*, Towns.* FROM Town_Services
                                                INNER JOIN Towns ON Town_Services.TownID=Towns.TownID 
                                                INNER JOIN ServiceSubCategoryPair ON Town_Services.ServiceID=ServiceSubCategoryPair.ServiceID
                                                INNER JOIN CategorySubCategoryPair ON ServiceSubCategoryPair.SubCategoryID=CategorySubCategoryPair.SubCategoryID
                                                WHERE Town_Services.TownID = '" + TownID + "' AND CategorySubCategoryPair.CategoryID = '" + CategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();



            List<AllServiceInfo> AllInfo = AddAllServiceInfo(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetParticularServiceForTownByIDandCategory(int TownID, int CategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Category ON Town_Services.CategoryID=Category.CategoryID INNER JOIN Sub_Category ON Town_Services.SubCategoryID=Sub_Category.SubCategoryID INNER JOIN Towns ON Town_Services.TownID=Towns.TownID WHERE Town_Services.TownID='" + TownID + "' and Town_Services.CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<AllServiceInfo> AllInfo = AddAllServiceInfo(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetParticularServiceForTownByIDandSubCategory(int TownID, int SubcategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Category ON Town_Services.CategoryID=Category.CategoryID INNER JOIN Sub_Category ON Town_Services.SubCategoryID=Sub_Category.SubCategoryID INNER JOIN Towns ON Town_Services.TownID=Towns.TownID WHERE Town_Services.TownID='" + TownID + "' and Town_Services.SubCategoryID='" + SubcategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<AllServiceInfo> AllInfo = AddAllServiceInfo(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetAllVirtualServicesForTownByID(int TownID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Category ON Town_Services.CategoryID=Category.CategoryID INNER JOIN Sub_Category ON Town_Services.SubCategoryID=Sub_Category.SubCategoryID INNER JOIN Towns ON Town_Services.TownID=Towns.TownID WHERE Town_Services.TownID='" + TownID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<AllServiceInfo> AllInfo = AddAllServiceInfoVirtual(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetParticularVirtualServiceForTownByIDandCategory(int TownID, int CategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Category ON Town_Services.CategoryID=Category.CategoryID INNER JOIN Sub_Category ON Town_Services.SubCategoryID=Sub_Category.SubCategoryID INNER JOIN Towns ON Town_Services.TownID=Towns.TownID WHERE Town_Services.TownID='" + TownID + "' and Town_Services.CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<AllServiceInfo> AllInfo = AddAllServiceInfoVirtual(sr);

            sqlcon.Close();
            return AllInfo;
        }

        [WebMethod]
        public List<AllServiceInfo> GetParticularVirtualServiceForTownByIDandSubCategory(int TownID, int SubcategoryID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Town_Services INNER JOIN Category ON Town_Services.CategoryID=Category.CategoryID INNER JOIN Sub_Category ON Town_Services.SubCategoryID=Sub_Category.SubCategoryID INNER JOIN Towns ON Town_Services.TownID=Towns.TownID WHERE Town_Services.TownID='" + TownID + "' and Town_Services.SubCategoryID='" + SubcategoryID + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<AllServiceInfo> AllInfo = AddAllServiceInfoVirtual(sr);

            sqlcon.Close();
            return AllInfo;
        }

        # endregion

        # region Town Related

        [WebMethod]
        public List<Towns> GetListOfTowns()
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Towns", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            List<Towns> Towns = AddTowns(sr);

            sqlcon.Close();
            return Towns;
        }

        [WebMethod]
        public Towns GetTownFromTownID(int TownID)
        {
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Towns WHERE TownID='" + TownID + "'", sqlcon);
            SqlDataReader Data = com.ExecuteReader();

            while (Data.Read())
            {
                Towns Town = new Towns();

                Town.TownID = Data.GetInt32(0);
                Town.Town = Data.GetString(1);
                Town.County = Data.GetString(2);
                Town.Latitude = Data.GetDouble(3);
                Town.Longitude = Data.GetDouble(4);

                sqlcon.Close();

                Town.CountNormal = GetListOfCountNormalForTownByTownID(Town.TownID).ToArray();

                return Town;
            }

            sqlcon.Close();
            return new Towns();
        }

        [WebMethod]
        public void GetRatingForTownByID(int TownID)
        {
            
        }
        
        [WebMethod]
        public void GetRatingForVirtualByTownByID(int TownID)
        {
           
        }

        [WebMethod]
        public List<TownCategoryCount> GetListOfCountNormalForTownByTownID(int TownID)
        {
            List<TownCategoryCount> result = new List<TownCategoryCount>();

            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM TownCategoryCount WHERE TownID='" + TownID + "'", sqlcon);
            SqlDataReader Data = com.ExecuteReader();

            while (Data.Read())
            {
                TownCategoryCount TCC = new TownCategoryCount();

                TCC.TownID = Data.GetInt32(0);
                TCC.CategoryID = Data.GetInt32(1);
                TCC.Count = Data.GetInt32(2);
                TCC.Normal = Data.GetInt32(3);
                TCC.CountVirt = Data.GetInt32(4);
                TCC.NormalVirt = Data.GetInt32(5);

                result.Add(TCC);
            }

            sqlcon.Close();

            return result;
        }

        [WebMethod]
        public TownCategoryCount GetCountNormalForTownByTownIDAndCategoryID(int TownID, int CategoryID)
        {
            TownCategoryCount result = new TownCategoryCount();

            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM TownCategoryCount WHERE TownID='" + TownID + "' AND CategoryID='" + CategoryID + "'", sqlcon);
            SqlDataReader Data = com.ExecuteReader();

            while (Data.Read())
            {
                result.TownID = Data.GetInt32(0);
                result.CategoryID = Data.GetInt32(1);
                result.Count = Data.GetInt32(2);
                result.Normal = Data.GetInt32(3);
                result.CountVirt = Data.GetInt32(4);
                result.NormalVirt = Data.GetInt32(5);
            }

            sqlcon.Close();

            return result;
        }

        # endregion

        # endregion

        # region SET METHODS

        [WebMethod]
        public bool SetNewUser(User UserDetails)
        {
            bool UploadSuccess = false;

            if (!CheckDuplicateUsername(UserDetails.Username))
            {
                sqlcon.Open();

                SqlCommand com = new SqlCommand("INSERT into BRE_User (FirstName, Surname, Gender, DateOfBirth, Email, Username, Password, [HouseNumber/Name], Address, Town, Postcode) values('" +
                    UserDetails.Firstname + "','" +
                    UserDetails.Surname + "','" +
                    UserDetails.Gender + "','" +
                    UserDetails.DateOfBirth.ToString("yyyy-MM-dd") + "','" +
                    UserDetails.Email + "','" +
                    UserDetails.Username + "','" +
                    UserDetails.Password + "','" +

                    UserDetails.HouseNumberName + "','" +
                    UserDetails.Address + "','" +
                    UserDetails.Town + "','" +
                    UserDetails.Postcode + "')", sqlcon);

                int i = com.ExecuteNonQuery();
                sqlcon.Close();
                if (i != 0)
                {
                    int id = GetUserID(UserDetails.Username, UserDetails.Password);
                    if (id > -1)
                    {
                        bool anyfail = false;
                        foreach (CategoryInfo catInfo in UserDetails.Categories)
                        {
                            UserCategoryPair catPair = new UserCategoryPair();
                            catPair.CategoryID = catInfo.CategoryID;
                            catPair.UserID = UserDetails.UserID;
                            catPair.CategoryValue = catInfo.CategoryValue;

                            if (!SetNewUserCategoryPair(catPair))
                            {
                                anyfail = true;
                                break;
                            }
                        }

                        // if any failed, delete everything 
                        if (anyfail)
                        {
                            UserDetails.UserID = id;
                            DeleteUser(UserDetails);
                        }
                        else
                            UploadSuccess = true;
                    }
                }
            }
            return UploadSuccess;
        }

        [WebMethod]
        public bool SetNewUserCategoryPair(UserCategoryPair userCategoryPair)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("INSERT into UserCategoryPair (UserID, CategoryID, CategoryValue) values('" +
                userCategoryPair.UserID + "', '" + userCategoryPair.CategoryID + "', '" + userCategoryPair.CategoryValue + "')", sqlcon);


            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool SetNewCategory(Category Category)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("INSERT into Category (CategoryName) values('" +
                Category.CategoryName + "')", sqlcon);
                    

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool SetNewSubCategory(SubCategory Subcategory)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("INSERT into Sub_Category (SubCategoryName) values('" +
                    Subcategory.SubCategoryName + "')", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public int SetNewService(TownServices Service)
        {
     
            int returnID = -1;

            //if (CheckIfTownExists(Service.TownID))
            //{
            sqlcon.Open();
            SqlCommand com = new SqlCommand("INSERT into Town_Services (Name, TownID, Rating, Latitude, Longitude, HasPerimeter, Perimeter, HasVirtualServices, VirtualServices) values('" +
                Service.Name + "','" +
                Service.TownID + "','" +
                Service.Rating + "','" +
                Service.Latitude + "','" +
                Service.Longitude + "','" +
                Service.HasPerimeter + "','" +
                Service.Perimeter + "','" +
                Service.HasVirtualServices + "','" +
                string.Join(",", Service.VirtualServices) + "');" +
                "SELECT ServiceID FROM Town_Services WHERE ServiceID = SCOPE_IDENTITY();", sqlcon);

            SqlDataReader sr = com.ExecuteReader();
            returnID = sr.GetInt32(0);
            sqlcon.Close();

            return returnID;
            
        }

        [WebMethod]
        public bool SetNewSubCategoryServicePair(SubCategoryServicePair pair)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("Insert into ServiceSubCategoryPair (ServiceID, SubCategoryID) values ('" +
                pair.ServiceID + "','" + pair.SubCategoryID + "')", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool SetNewCategorySubCategoryPair(CategorySubCategoryPair pair)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("Insert into CategorySubCategoryPair (CategoryID, SubCategoryID) values ('" +
                pair.CategoryID + "','" + pair.SubCategoryID + "')", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool SetNewTown(Towns_Trunc Town)
        {
            bool UploadSuccess = false;

            //if (CheckIfTownExists(Service.TownID))
            //{
            sqlcon.Open();
            SqlCommand com = new SqlCommand("INSERT into Towns (Town, County, Latitude, Longitude"+
                ") values('" +
                Town.Town + "','" +
                Town.County + "','" +
                Town.Latitude + "','" +
                Town.Longitude + "'" +
                ")", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;
            //}
            return UploadSuccess;
        }

        [WebMethod]
        public bool SetNewTownCategoryCount(TownCategoryCount TCC)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("INSERT into TownCategoryCount values('" + TCC.TownID + "','" + TCC.CategoryID + "','" + TCC.Count + "','-1','" + TCC.CountVirt + "','-1')", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;

        }

        # endregion

        # region UPDATE METHODS

        [WebMethod]
        public bool UpdateUser(User UserDetails)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("UPDATE BRE_User SET FirstName='" + UserDetails.Firstname + "', Surname='" + UserDetails.Surname + "', Gender='" + UserDetails.Gender + "', DateOfBirth='" + UserDetails.DateOfBirth.ToString("yyyy-MM-dd") + "', Email='" + UserDetails.Email + "', Username='" + UserDetails.Username + "',  Password='" + UserDetails.Password + "', [HouseNumber/Name]='" + UserDetails.HouseNumberName + "', Address='" + UserDetails.Address + "', Town='" + UserDetails.Town + "', Postcode='" + UserDetails.Postcode + "' WHERE UserID='" + UserDetails.UserID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
            {
                bool anyfail = false;
                foreach (CategoryInfo catInfo in UserDetails.Categories)
                {
                    UserCategoryPair catPair = new UserCategoryPair();
                    catPair.CategoryID = catInfo.CategoryID;
                    catPair.UserID = UserDetails.UserID;
                    catPair.CategoryValue = catInfo.CategoryValue;

                    if (GetUserCategoryPairExists(catPair))
                    {
                        if (!UpdateUserCategoryPair(catPair))
                            anyfail = true;
                    }
                    else
                    {
                        if (!SetNewUserCategoryPair(catPair))
                            anyfail = true;
                    }
                }

                // success if none of the above failed
                if (!anyfail)
                    UploadSuccess = true;
            }
            

            return UploadSuccess;
        }

        [WebMethod]
        public bool UpdateUserCategoryPair(UserCategoryPair userCategoryPair)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("UPDATE UserCategoryPair SET CategoryValue='" + userCategoryPair.CategoryValue + "' WHERE UserID='" + userCategoryPair.UserID + "' AND CategoryID='" + userCategoryPair.CategoryID + "'", sqlcon);


            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool UpdateCategory(Category Category)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("UPDATE Category SET CategoryName='" + Category.CategoryName + "' WHERE CategoryID='" + Category.CategoryID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool UpdateSubCategory(SubCategory SubCategory)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("UPDATE Sub_Category SET SubCategoryName='" + SubCategory.SubCategoryName + "' WHERE SubCategoryID='" + SubCategory.SubCategoryID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool UpdateTownServices(TownServices Service)
        {

            bool UploadSuccess = false;

            string query = "UPDATE Town_Services SET Name='" + Service.Name +
                "', TownID='" + Service.TownID +
                "', Rating='" + Service.Rating +
                "', Latitude='" + Service.Latitude +
                "', Longitude='" + Service.Longitude +
                "', HasPerimeter='" + Service.HasPerimeter +
                "', Perimeter='" + Service.Perimeter +
                "', HasVirtualServices='" + Service.HasVirtualServices +
                "', VirtualServices='" + string.Join(",", Service.VirtualServices) +
                "' WHERE ServiceID='" + Service.ServiceID + "'";

            sqlcon.Open();
            SqlCommand com = new SqlCommand(query, 
                sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool UpdateTownCategoryCount(TownCategoryCount TCC)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("UPDATE TownCategoryCount SET Count=" + TCC.Count + ", Normal=" + TCC.Normal + ", CountVirt=" + TCC.CountVirt + ", NormalVirt=" + TCC.NormalVirt + " WHERE TownID=" + TCC.TownID + " AND CategoryID=" + TCC.CategoryID, sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool UpdateTown(Towns Town)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("UPDATE Towns SET Town='" + Town.Town + "', County='" + Town.County + "', Latitude='" + Town.Latitude + "', Longitude='" + Town.Longitude + 
                "' WHERE TownID='" + Town.TownID + "'"
                , sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public string UpdateTownServiceCount(int TownID)
        {
            int success = 0;

            List<Category> allCategories = GetListOfCategories();

            foreach (Category Category in allCategories)
            {
                int CategoryID = Category.CategoryID;
                int Count = GetNumberOfServicesByCategoryIDAndTownID(TownID, CategoryID);
                int virtCount = GetNumberOfVirtualServicesByCategoryIDAndTownID(TownID, CategoryID);

                bool uploadSucessful = false;
                TownCategoryCount TCC = GetCountNormalForTownByTownIDAndCategoryID(TownID, CategoryID);
                TCC.Count = Count;
                TCC.CountVirt = virtCount;

                if (TCC.TownID > 0)
                    uploadSucessful = UpdateTownCategoryCount(TCC); // update existing 
                else
                {
                    TCC.TownID = TownID;
                    TCC.CategoryID = CategoryID;
                    uploadSucessful = SetNewTownCategoryCount(TCC); // create new   
                }

                if (uploadSucessful)
                {
                    if (success == 0) success = 1;
                }
                else if (success == 1) success = 2;
            }


            string str = "True";
            if (success == 2) str = "True Partial";
            if (success == 0) str = "False";

            return str;
        }

        [WebMethod]
        public string UpdateTownNormalisations()
        {
            int success = 0;
            List<Category> allCategories = GetListOfCategories();

            foreach (Category Category in allCategories)
            {
                int currentMax = -1;
                int currentMaxVirt = -1;
                List<TownCategoryCount> allTCC = GetListOfCountNormalForTownByCategoryID(Category.CategoryID);

                foreach (TownCategoryCount TCC in allTCC)
                {
                    if (TCC.Count > currentMax) currentMax = TCC.Count;
                    if (TCC.CountVirt > currentMaxVirt) currentMaxVirt = TCC.CountVirt;
                }


                foreach (TownCategoryCount TCC in allTCC)
                {
                    TCC.Normal = (TCC.Count < 1 || currentMax == 0) ? 0 : (int)Math.Round(((double)TCC.Count / (double)currentMax) * 10, MidpointRounding.AwayFromZero);
                    TCC.NormalVirt = (TCC.CountVirt < 1 || currentMaxVirt == 0) ? 0 : (int)Math.Round(((double)TCC.CountVirt / (double)currentMaxVirt) * 10, MidpointRounding.AwayFromZero);

                    if (UpdateTownCategoryCount(TCC))
                    {
                        if (success == 0) success = 1;
                    }
                    else if (success == 1) success = 2;
                }
   
            }

            string str = "success";
            if (success == 2) str = "partial success";
            if (success == 0) str = "failure";

            return str;
        }


        # endregion

        # region DELETE METHODS

        [WebMethod]
        public bool DeleteUser(User UserID)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("DELETE FROM BRE_User WHERE UserID='" + UserID.UserID + "'", sqlcon);
            SqlCommand com2 = new SqlCommand("DELETE FROM UserCategoryPair WHERE UserID='" + UserID.UserID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            int i2 = com2.ExecuteNonQuery();

            sqlcon.Close();
            if (i != 0 && i2 != 0)
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool DeleteCategory(Category CategoryID)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("DELETE FROM Category WHERE CategoryID='" + CategoryID.CategoryID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool DeleteSubCategory(SubCategory SubcategoryID)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("DELETE FROM Sub_Category WHERE SubCategoryID='" + SubcategoryID.SubCategoryID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool DeleteServiceSubCategoryPair(SubCategoryServicePair pair)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("DELETE FROM ServiceSubCategoryPair WHERE SubCategoryID='" + pair.SubCategoryID + "'" + 
                                            "AND ServiceID='" + pair.ServiceID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool DeleteServices(TownServices ServiceID)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("DELETE FROM Town_Services WHERE ServiceID='" + ServiceID.ServiceID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        [WebMethod]
        public bool DeleteTown(Towns TownID)
        {
            bool UploadSuccess = false;

            sqlcon.Open();
            SqlCommand com = new SqlCommand("DELETE FROM Towns WHERE TownID='" + TownID.TownID + "'", sqlcon);

            int i = com.ExecuteNonQuery();
            sqlcon.Close();
            if (i == 0)
                UploadSuccess = false;
            else
                UploadSuccess = true;

            return UploadSuccess;
        }

        # endregion
        
        # region HELPERS

        private List<TownServices> AddTownServices(SqlDataReader Data)
        {
            List<TownServices> listOfServices = new List<TownServices>();

            while (Data.Read())
            {
                TownServices Town_Services = new TownServices();

                Town_Services.ServiceID = Data.GetInt32(0);
                Town_Services.Name = Data.GetString(1);
                Town_Services.TownID = Data.GetInt32(2);
                Town_Services.Rating = Data.GetDouble(3);
                Town_Services.Latitude = Data.GetDouble(4);
                Town_Services.Longitude = Data.GetDouble(5);
                Town_Services.HasPerimeter = Data.GetBoolean(6);
                if (Town_Services.HasPerimeter)
                {
                    string tempPerimeter = Data.GetString(7);
                    Town_Services.Perimeter = tempPerimeter.Split(',');
                }

                Town_Services.HasVirtualServices = Data.GetBoolean(8);

                if (Town_Services.HasVirtualServices)
                {
                    string tempServices = Data.GetString(9);
                    string[] tempServicesArr = tempServices.Split(',');
                    for (int i = 0; i < tempServicesArr.Length; i++)
                    {
                        Town_Services.VirtualServices = tempServicesArr;
                    }
                }

                listOfServices.Add(Town_Services);

            }
            return listOfServices;
        }

        private List<User> AddUsers(SqlDataReader Data)
        {
            List<User> listOfUsers = new List<User>();

            while (Data.Read())
            {
                User userDetails = new User();

                userDetails.UserID = Data.GetInt32(0);
                userDetails.Firstname = Data.GetString(1);
                userDetails.Surname = Data.GetString(2);
                userDetails.Gender = Data.GetString(3);
                userDetails.DateOfBirth = Data.GetDateTime(4);
                userDetails.Email = Data.GetString(5);
                userDetails.Username = Data.GetString(6);
                userDetails.Password = Data.GetString(7);

                userDetails.HouseNumberName = Data.GetString(8);
                userDetails.Address = Data.GetString(9);
                userDetails.Town = Data.GetString(10);
                userDetails.Postcode = Data.GetString(11);

                sqlcon.Open();
                SqlCommand com2 = new SqlCommand("SELECT Category.CategoryID, Category.CategoryName, UserCategoryPair.CategoryValue FROM Category JOIN UserCategoryPair on Category.CategoryID=UserCategoryPair.CategoryID WHERE UserCategoryPair.UserID='" + userDetails.UserID + "'", sqlcon);
                SqlDataReader sr2 = com2.ExecuteReader();

                while (sr2.Read())
                {
                    CategoryInfo info = new CategoryInfo();
                    info.CategoryID = sr2.GetInt32(0);
                    info.CategoryName = sr2.GetString(1);
                    info.CategoryValue = sr2.GetInt32(2);

                    userDetails.Categories.Add(info);
                }
                sqlcon.Close();

                listOfUsers.Add(userDetails);
            }
            return listOfUsers;
        }

        private List<Towns> AddTowns(SqlDataReader Data)
        {
            List<Towns> listOfTowns = new List<Towns>();

            while (Data.Read())
            {
                Towns Town = new Towns();

                int t = Data.FieldCount;

                Town.TownID = Data.GetInt32(0);
                Town.Town = Data.GetString(1);
                Town.County = Data.GetString(2);
                Town.Latitude = Data.GetDouble(3);
                Town.Longitude = Data.GetDouble(4);

                listOfTowns.Add(Town);
            }

            sqlcon.Close();

            foreach (Towns Town in listOfTowns)
                Town.CountNormal = GetListOfCountNormalForTownByTownID(Town.TownID).ToArray();
            

            return listOfTowns;
        }

        private List<SubCategory> AddSubcategories(SqlDataReader Data)
        {
            List<SubCategory> listOfSubCategories = new List<SubCategory>();

            while (Data.Read())
            {
                SubCategory subCategories = new SubCategory();

                subCategories.SubCategoryID = Data.GetInt32(0);
                subCategories.SubCategoryName = Data.GetString(1);

                listOfSubCategories.Add(subCategories);
            }

            sqlcon.Close();
            return listOfSubCategories;
        }

        private List<AllServiceInfo> AddAllServiceInfo(SqlDataReader Data)
        {
            List<AllServiceInfo> AllServices = new List<AllServiceInfo>();
            List<int> serviceIDs = new List<int>();            

            while (Data.Read())
            {
                if (!serviceIDs.Contains(Data.GetInt32(0)))
                {

                    serviceIDs.Add(Data.GetInt32(0));

                    AllServiceInfo AllInfo = new AllServiceInfo();
                    AllInfo.Service = new TownServices();
                    AllInfo.Town = new Towns();

                    AllInfo.Service.ServiceID = Data.GetInt32(0);
                    AllInfo.Service.Name = Data.GetString(1);
                    AllInfo.Service.TownID = Data.GetInt32(2);
                    AllInfo.Service.Rating = Data.GetDouble(3);
                    AllInfo.Service.Latitude = Data.GetDouble(4);
                    AllInfo.Service.Longitude = Data.GetDouble(5);
                    AllInfo.Service.HasPerimeter = Data.GetBoolean(6);
                    if (AllInfo.Service.HasPerimeter)
                    {
                        string tempPerimeter = Data.GetString(7);
                        AllInfo.Service.Perimeter = tempPerimeter.Split(',');
                    }

                    AllInfo.Service.HasVirtualServices = Data.GetBoolean(8);

                    if (AllInfo.Service.HasVirtualServices)
                    {
                        string tempServices = Data.GetString(9);
                        string[] tempServicesArr = tempServices.Split(',');
                        for (int i = 0; i < tempServicesArr.Length; i++)
                        {
                            AllInfo.Service.VirtualServices = tempServicesArr;
                        }
                    }

                    AllInfo.Town.TownID = Data.GetInt32(10);
                    AllInfo.Town.Town = Data.GetString(11);
                    AllInfo.Town.County = Data.GetString(12);
                    AllInfo.Town.Latitude = Data.GetDouble(13);
                    AllInfo.Town.Longitude = Data.GetDouble(14);

                    AllServices.Add(AllInfo);
                }
            }

            sqlcon.Close();

            foreach (AllServiceInfo AllInfo in AllServices)
            {
                AllInfo.Categories = GetListOfCategoriesByServiceID(AllInfo.Service.ServiceID).ToArray();
                AllInfo.Subcategories = GetListOfSubCategoriesByServiceID(AllInfo.Service.ServiceID).ToArray();
            }

            
            return AllServices;
        }

        private List<AllServiceInfo> AddAllServiceInfoVirtual(SqlDataReader Data)
        {
            List<AllServiceInfo> AllServices = new List<AllServiceInfo>();

            while (Data.Read())
            {

                AllServiceInfo AllInfo = new AllServiceInfo();
                AllInfo.Service = new TownServices();
                AllInfo.Town = new Towns();

                AllInfo.Service.ServiceID = Data.GetInt32(0);
                AllInfo.Service.Name = Data.GetString(1);
                AllInfo.Service.TownID = Data.GetInt32(2);
                AllInfo.Service.Rating = Data.GetDouble(3);
                AllInfo.Service.Latitude = Data.GetDouble(4);
                AllInfo.Service.Longitude = Data.GetDouble(5);
                AllInfo.Service.HasPerimeter = Data.GetBoolean(6);
                if (AllInfo.Service.HasPerimeter)
                {
                    string tempPerimeter = Data.GetString(7);
                    AllInfo.Service.Perimeter = tempPerimeter.Split(',');
                }

                AllInfo.Service.HasVirtualServices = Data.GetBoolean(8);

                if (AllInfo.Service.HasVirtualServices)
                {
                    string tempServices = Data.GetString(9);
                    string[] tempServicesArr = tempServices.Split(',');
                    for (int i = 0; i < tempServicesArr.Length; i++)
                    {
                        AllInfo.Service.VirtualServices = tempServicesArr;
                    }
                }

                AllInfo.Town.TownID = Data.GetInt32(10);
                AllInfo.Town.Town = Data.GetString(11);
                AllInfo.Town.County = Data.GetString(12);
                AllInfo.Town.Latitude = Data.GetDouble(13);
                AllInfo.Town.Longitude = Data.GetDouble(14);

                if (AllInfo.Service.HasVirtualServices)
                {
                    AllServices.Add(AllInfo);
                }
            }

            sqlcon.Close();

            foreach (AllServiceInfo AllInfo in AllServices)
            {
                AllInfo.Categories = GetListOfCategoriesByServiceID(AllInfo.Service.ServiceID).ToArray();
                AllInfo.Subcategories = GetListOfSubCategoriesByServiceID(AllInfo.Service.ServiceID).ToArray();
            }

            return AllServices;
        }

        [WebMethod]
        public bool CheckDuplicateUsername(string Username)
        {
            bool AlreadyExists = false;
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM BRE_User WHERE Username='" + Username + "'", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                if (sr.GetString(6).Equals(Username, StringComparison.OrdinalIgnoreCase))
                {
                    AlreadyExists = true;
                }
            }

            sqlcon.Close();
            return AlreadyExists;
        }

        [WebMethod]
        public bool CheckIfTownExists(string Town)
        {
            bool Exists = false;
            List<Towns> ListOfTowns = new List<Towns>();
            sqlcon.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Towns", sqlcon);
            SqlDataReader sr = com.ExecuteReader();

            while (sr.Read())
            {
                Towns towns = new Towns();

                towns.Town = sr.GetString(1);

                ListOfTowns.Add(towns);
            }
            sqlcon.Close();

            for (int i = 0; i < ListOfTowns.Count; i++)
            {
                if (ListOfTowns[i].Town.Equals(Town, StringComparison.OrdinalIgnoreCase))
                {
                    Exists = true;
                }
            }

            return Exists;
        }

        [WebMethod]
        public bool TestServer()
        {
            bool isConnected = false;

            try
            {
                isConnected = true;
            }
            catch { }

            return isConnected;
        }

        # endregion

    }
}

