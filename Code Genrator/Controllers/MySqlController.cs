using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data.SqlTypes;
using Code_Genrator.Models;
using Code_Genrator.Common;

namespace Code_Genrator.Controllers
{
    public class MySqlController : Controller
    {
        //
        // GET: /MySql/

        SQLHelperRepo sqlHelper = null;

        public MySqlController()
        {
            sqlHelper = new SQLHelperRepo();
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GenrateCode(string DBName, string TableName)
        {
            MySqlViewModel hViewModel = new MySqlViewModel();
            hViewModel.DB_Name = DBName;
            hViewModel.Table_Name = TableName;

            string _sqlCon = string.Empty;

            _sqlCon = hViewModel.DB_Name;

            string Info = "customer.Customer_Entity.";

            DataTable dt = new DataTable();

            string str = "SELECT COLUMN_NAME, DATA_TYPE, IFNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH FROM information_schema.columns WHERE table_name='" + DBName + "' and TABLE_SCHEMA = '" + DBName + "'";

            dt = sqlHelper.ExecuteDataTable(null, str, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                ColInfo col = new ColInfo();

                col = GetValuesFromDataReader(dr);

                hViewModel.ColList.Add(col);
            }



            hViewModel.Info = Info;




            hViewModel.InfoCode = GetInfo(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.RepoCode = GetRepo(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.ManCode = GetMan(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.InsertProc = GetInsertProc(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.UpdateProc = GetUpdateProc(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.SelectProc = GetSelectProc(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.ViewModelCode = GetViewModel(hViewModel.Table_Name);

            hViewModel.SelectProc = GetSelectProc(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.SelectProcById = GetSelectByIdProc(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.DeleteProc = GetDeleteProc(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.AjaxViewModel = GetAjaxViewModel(hViewModel.ColList, hViewModel.Table_Name);

            hViewModel.PrintableView = GetPrintableView(hViewModel.ColList, hViewModel.Table_Name);

            return PartialView("_Genrate", hViewModel);
        }

        public JsonResult Get_Table_Names(string DBName)
        {
            List<string> Tables = new List<string>();

            string _sqlCon = DBName;

            try
            {

                using (SqlConnection con = new SqlConnection(_sqlCon))
                {
                    con.Open();

                    //select table_name from information_schema.tables;


                    string str = "SELECT distinct table_name FROM information_schema.columns WHERE  TABLE_SCHEMA = '" + DBName + "'"; 
                    using (SqlCommand command = new SqlCommand(str, con))
                    {
                        command.CommandType = CommandType.Text;

                        SqlDataReader dataReader = command.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Tables.Add(Convert.ToString(dataReader["TABLE_NAME"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }



            return Json(new { Tables }, JsonRequestBehavior.AllowGet);
        }

        public ColInfo GetValuesFromDataReader(DataRow dr)
        {
            ColInfo col = new ColInfo();

            col.Column_Name = Convert.ToString(dr["COLUMN_NAME"]);

            col.Data_Type = Convert.ToString(dr["DATA_TYPE"]);

            col.Max_Length = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]);

            return col;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string GetInfo(List<ColInfo> ColList, string TableName)
        {
            string Info = "";
            string InfoName = "";
            foreach (var item in TableName.Split('_'))
            {
                InfoName += item;
            }

            Info += "public class " + InfoName + "Info";

            Info += Environment.NewLine + " {";

            foreach (var item in ColList)
            {
                if (item.Data_Type == "int")
                {
                    Info += Environment.NewLine + " public int " + item.Column_Name + "{ get; set; }";
                }
                else if (item.Data_Type == "nvarchar" || item.Data_Type == "varchar")
                {
                    Info += Environment.NewLine + " public string " + item.Column_Name + "{ get; set; }";
                }
                else if (item.Data_Type == "bit")
                {
                    Info += Environment.NewLine + " public bool " + item.Column_Name + "{ get; set; }";
                }
                else if (item.Data_Type == "datetime")
                {
                    Info += Environment.NewLine + " public DateTime " + item.Column_Name + "{ get; set; }";
                }
                else if (item.Data_Type == "decimal")
                {
                    Info += Environment.NewLine + " public decimal " + item.Column_Name + "{ get; set; }";
                }
                Info += Environment.NewLine + "";
            }

            Info += Environment.NewLine + " }";

            return Info;
        }

        public string GetRepo(List<ColInfo> ColList, string TableName)
        {
            string Repo = "";
            string RepoName = "";
            foreach (var item in TableName.Split('_'))
            {
                RepoName += item;
            }

            Repo += "public class " + RepoName + "Repo";

            Repo += Environment.NewLine + " {";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  SQLHelperRepo _sqlRepo;";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public " + RepoName + "Repo()";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   _sqlRepo = new SQLHelperRepo();";

            Repo += Environment.NewLine + "  }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public void Insert_" + TableName + "(" + RepoName + "Info " + RepoName.ToLower() + ")";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   _sqlRepo.ExecuteNonQuery(SetValues_In_" + TableName + "(" + RepoName.ToLower() + "), StoredProcedures.Insert_" + TableName + "_Sp.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + "  }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public void Update_" + TableName + "(" + RepoName + "Info " + RepoName.ToLower() + ")";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   _sqlRepo.ExecuteNonQuery(Set_Values_In_" + TableName + "(" + RepoName.ToLower() + "), StoredProcedures.Update_" + TableName + "_Sp.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + "  }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  private List<SqlParameter> Set_Values_In_" + TableName + "(" + RepoName + "Info " + RepoName.ToLower() + ")";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   List<SqlParameter> sqlParams = new List<SqlParameter>();";

            foreach (var item in ColList)
            {
                Repo += Environment.NewLine + "   sqlParams.Add(new SqlParameter(\"@" + item.Column_Name + "\"," + RepoName.ToLower() + "." + item.Column_Name + "));";
            }

            Repo += Environment.NewLine + "   return sqlParams;";

            Repo += Environment.NewLine + "  }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public List<" + RepoName + "Info> Get_" + TableName + "s(ref PaginationInfo Pager)";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   List<" + RepoName + "Info> " + RepoName.ToLower() + "s = new List<" + RepoName + "Info>();";

            Repo += Environment.NewLine + "   DataTable dt = _sqlRepo.ExecuteDataTable(null, StoredProcedures.Get_" + TableName + "_Sp.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + "   foreach (DataRow dr in CommonMethods.GetRows(dt, ref pager))";

            Repo += Environment.NewLine + "   {";

            Repo += Environment.NewLine + "    " + RepoName.ToLower() + "s.Add(Get_" + TableName + "_Values(dr));";

            Repo += Environment.NewLine + "   }";

            Repo += Environment.NewLine + "    return " + RepoName.ToLower() + "s;";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + "  public " + RepoName + "Info Get_" + TableName + "_By_Id (int " + ColList[0].Column_Name + ")";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   " + RepoName + "Info " + RepoName.ToLower() + " = new " + RepoName + "();";

            Repo += Environment.NewLine + "   DataTable dt = _sqlRepo.ExecuteDataTable(null, StoredProcedures.Get_" + TableName + "_Sp.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + " List<DataRow> drList = new List<DataRow>();";

            Repo += Environment.NewLine + "drList = dt.AsEnumerable().ToList();";

            Repo += Environment.NewLine + "   foreach (DataRow dr in drList)";

            Repo += Environment.NewLine + "   {";

            Repo += Environment.NewLine + "    " + RepoName.ToLower() + "= Get_" + TableName + "_Values(dr);";

            Repo += Environment.NewLine + "   }";

            Repo += Environment.NewLine + "    return " + RepoName.ToLower() + ";";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + " ";

            Repo += Environment.NewLine + "   private " + RepoName + "Info Get_" + TableName + "_Values(DataRow dr)";

            Repo += Environment.NewLine + " {";

            Repo += Environment.NewLine + "   " + RepoName + "Info " + RepoName.ToLower() + " = new " + RepoName + "Info();";

            Repo += Environment.NewLine + " ";

            foreach (var item in ColList)
            {
                if (item.Data_Type == "int")
                {
                    Repo += Environment.NewLine + "   " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToInt32(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "nvarchar" || item.Data_Type == "varchar")
                {
                    Repo += Environment.NewLine + "  " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToString(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "bit")
                {
                    Repo += Environment.NewLine + "  " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToBoolean(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "datetime")
                {
                    Repo += Environment.NewLine + "  " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToDateTime(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "decimal")
                {
                    Repo += Environment.NewLine + "  " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToDecimal(dr[\"" + item.Column_Name + "\"]);";
                }
            }

            Repo += Environment.NewLine + " return " + RepoName.ToLower() + ";";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + " public void Delete_" + TableName + "_By_Id(int " + ColList[0].Column_Name.ToLower() + ")";

            Repo += Environment.NewLine + "{";

            Repo += Environment.NewLine + "List<SqlParameter> sqlParams = new List<SqlParameter>();";

            Repo += Environment.NewLine + "sqlParams.Add(new SqlParameter(\"@" + ColList[0].Column_Name + "\", " + ColList[0].Column_Name.ToLower() + "));";

            Repo += Environment.NewLine + "_sqlRepo.ExecuteNonQuery(sqlParams, StoredProcedures.Delete_" + TableName + "_By_Id.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + " }";

            Repo += "";

            Repo += Environment.NewLine + " }";

            return Repo;
        }

        public string GetMan(List<ColInfo> ColList, string TableName)
        {
            string Man = "";
            string ManName = "";
            foreach (var item in TableName.Split('_'))
            {
                ManName += item;
            }

            Man += "public class " + ManName + "Manager";

            Man += Environment.NewLine + " {";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  " + ManName + "Repo _" + ManName.ToLower() + "Repo;";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public " + ManName + "Manager()";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + "  _" + ManName.ToLower() + "Repo = new " + ManName + "Repo();";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public void Insert_" + TableName + "(" + ManName + "Info " + ManName.ToLower() + ")";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + "  _" + ManName.ToLower() + "Repo.Insert_" + TableName + "(" + ManName.ToLower() + ");";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public void Update_" + TableName + "(" + ManName + "Info " + ManName.ToLower() + ")";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + "  _" + ManName.ToLower() + "Repo.Update_" + TableName + "(" + ManName.ToLower() + ");";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public List<" + ManName + "Info> Get_" + TableName + "s(ref PaginationInfo Pager)";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + " return _" + ManName.ToLower() + "Repo.Get_" + TableName + "s(ref Pager);";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public " + ManName + "Info Get_" + TableName + "_By_Id (int " + ColList[0].Column_Name + ")";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + " return _" + ManName.ToLower() + "Repo.Get_" + TableName + "_By_Id(" + ColList[0].Column_Name + ");";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "  public void Delete_" + TableName + "_By_Id(int " + ColList[0].Column_Name + ")";

            Man += Environment.NewLine + " {";

            Man += Environment.NewLine + " _enquiryRepo.Delete_" + TableName + "_By_Id(" + ColList[0].Column_Name + ");";

            Man += Environment.NewLine + "}";

            Man += Environment.NewLine + " }";

            return Man;
        }

        public string GetInsertProc(List<ColInfo> ColList, string TableName)
        {
            string Proc = "";
            string ProcName = "";
            foreach (var item in TableName.Split('_'))
            {
                ProcName += item;
            }

            Proc += "Create Procedure Insert_" + TableName + "_Sp";

            Proc += Environment.NewLine + "(";

            int count = ColList.Count;

            int i = 1;

            foreach (var item in ColList)
            {
                if (i != 1)
                {
                    if (item.Data_Type == "int")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                    else if (item.Data_Type == "nvarchar" || item.Data_Type == "varchar")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "(" + item.Max_Length + ")";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "(" + item.Max_Length + "),";
                        }
                    }
                    else if (item.Data_Type == "bit")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                    else if (item.Data_Type == "datetime")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                    else if (item.Data_Type == "decimal")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }

                }
                i = i + 1;

            }

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "as";

            Proc += Environment.NewLine + "begin";

            Proc += Environment.NewLine + "Insert into " + TableName;

            Proc += Environment.NewLine + "(";

            i = 1;

            foreach (var item in ColList)
            {
                if (i != 1)
                {
                    if (count == i)
                    {
                        Proc += Environment.NewLine + "" + item.Column_Name + "";
                    }
                    else
                    {
                        Proc += Environment.NewLine + "" + item.Column_Name + ",";
                    }
                }
                i = i + 1;
            }

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "values";

            Proc += Environment.NewLine + "(";

            i = 1;

            foreach (var item in ColList)
            {
                if (i != 1)
                {
                    if (count == i)
                    {
                        Proc += Environment.NewLine + "@" + item.Column_Name + "";
                    }
                    else
                    {
                        Proc += Environment.NewLine + "@" + item.Column_Name + ",";
                    }
                }
                i = i + 1;
            }

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "end";

            return Proc;
        }

        public string GetUpdateProc(List<ColInfo> ColList, string TableName)
        {
            string Proc = "";
            string ProcName = "";
            foreach (var item in TableName.Split('_'))
            {
                ProcName += item;
            }

            Proc += "Create Procedure Update_" + TableName + "_Sp";

            Proc += Environment.NewLine + "(";

            int count = ColList.Count;

            int i = 1;

            foreach (var item in ColList)
            {
                if (item.Column_Name.ToLower().Contains("created"))
                {

                }
                else
                {
                    if (item.Data_Type == "int")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                    else if (item.Data_Type == "nvarchar" || item.Data_Type == "varchar")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "(" + item.Max_Length + ")";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "(" + item.Max_Length + "),";
                        }
                    }
                    else if (item.Data_Type == "bit")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                    else if (item.Data_Type == "datetime")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                    else if (item.Data_Type == "decimal")
                    {
                        if (count == i)
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + "";
                        }
                        else
                        {
                            Proc += Environment.NewLine + "@" + item.Column_Name + " " + item.Data_Type + ",";
                        }
                    }
                }

                i = i + 1;

            }

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "as";

            Proc += Environment.NewLine + "begin";

            Proc += Environment.NewLine + "Update " + TableName + " Set ";

            i = 1;

            foreach (var item in ColList)
            {
                if (i == 1)
                {

                }
                else if (item.Column_Name.ToLower().Contains("created"))
                {


                }
                else
                {
                    if (count == i)
                    {
                        Proc += Environment.NewLine + " " + item.Column_Name + " = @" + item.Column_Name;
                    }
                    else
                    {
                        Proc += Environment.NewLine + " " + item.Column_Name + " = @" + item.Column_Name + " ,";
                    }
                }
                i = i + 1;

            }

            Proc += Environment.NewLine + "Where " + ColList[0].Column_Name + " = @" + ColList[0].Column_Name;

            Proc += Environment.NewLine + "end";


            return Proc;
        }

        public string GetSelectProc(List<ColInfo> ColList, string TableName)
        {
            string Proc = "";
            string ProcName = "";
            foreach (var item in TableName.Split('_'))
            {
                ProcName += item;
            }

            Proc += Environment.NewLine + "Create Procedure Get_" + TableName + "_Sp";

            Proc += Environment.NewLine + "(";

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "as";

            Proc += Environment.NewLine + "begin";

            Proc += Environment.NewLine + "Select ";

            int count = ColList.Count;

            int i = 1;

            foreach (var item in ColList)
            {
                if (count == i)
                {
                    Proc += item.Column_Name + " ";
                }
                else
                {
                    Proc += item.Column_Name + ", ";
                }
                i = i + 1;
            }

            Proc += Environment.NewLine + " from " + TableName;

            Proc += Environment.NewLine + "end";

            return Proc;
        }

        public string GetDeleteProc(List<ColInfo> ColList, string TableName)
        {
            string Proc = "";
            string ProcName = "";
            foreach (var item in TableName.Split('_'))
            {
                ProcName += item;
            }

            Proc += Environment.NewLine + "Create Procedure Delete_" + TableName + "_By_Id_Sp";

            Proc += Environment.NewLine + "(";

            Proc += Environment.NewLine + "@" + ColList[0].Column_Name + " int";

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "as";

            Proc += Environment.NewLine + "begin";

            Proc += Environment.NewLine + " Delete from " + TableName;

            Proc += Environment.NewLine + " Where " + ColList[0].Column_Name + " = " + "@" + ColList[0].Column_Name;

            Proc += Environment.NewLine + "end";

            return Proc;
        }

        public string GetViewModel(string TableName)
        {
            string Model = "";
            string ModelName = "";
            foreach (var item in TableName.Split('_'))
            {
                ModelName += item;
            }

            Model += "public class " + ModelName + "ViewModel";

            Model += Environment.NewLine + " {";

            Model += Environment.NewLine + "  public " + ModelName + "ViewModel()";

            Model += Environment.NewLine + "  {";

            Model += Environment.NewLine + "Friendly_Message = new List<FriendlyMessageInfo>();";

            Model += Environment.NewLine + "Pager = new PaginationInfo();";

            Model += Environment.NewLine + " " + ModelName.ToLower() + " = new " + ModelName + "Info();";

            Model += Environment.NewLine + " " + ModelName.ToLower() + "s = new List<" + ModelName + "Info>();";

            Model += Environment.NewLine + "  }";

            Model += Environment.NewLine + "public List<FriendlyMessageInfo> Friendly_Message { get; set; }";

            Model += Environment.NewLine + "public PaginationInfo Pager { get; set; }";

            Model += Environment.NewLine + "public " + ModelName + "Info " + ModelName.ToLower() + " { get; set; }";

            Model += Environment.NewLine + "public List<" + ModelName + "Info> " + ModelName.ToLower() + "s { get; set; }";

            Model += Environment.NewLine + "  }";

            return Model;
        }

        public string GetSelectByIdProc(List<ColInfo> ColList, string TableName)
        {
            string Proc = "";
            string ProcName = "";
            foreach (var item in TableName.Split('_'))
            {
                ProcName += item;
            }

            Proc += Environment.NewLine + "Create Procedure Get_" + TableName + "_By_Id_Sp";

            Proc += Environment.NewLine + "(";

            Proc += Environment.NewLine + "@" + ColList[0].Column_Name + " int";

            Proc += Environment.NewLine + ")";

            Proc += Environment.NewLine + "as";

            Proc += Environment.NewLine + "begin";

            Proc += Environment.NewLine + "Select ";

            int count = ColList.Count;

            int i = 1;

            foreach (var item in ColList)
            {
                if (count == i)
                {
                    Proc += item.Column_Name + " ";
                }
                else
                {
                    Proc += item.Column_Name + ", ";
                }
                i = i + 1;
            }

            Proc += Environment.NewLine + " from " + TableName;

            Proc += Environment.NewLine + " where  " + ColList[0].Column_Name + " = @" + ColList[0].Column_Name;

            Proc += Environment.NewLine + "end";

            return Proc;
        }

        public string GetAjaxViewModel(List<ColInfo> ColList, string TableName)
        {
            string ViewModel = "";
            string ViewModelName = "";
            foreach (var item in TableName.Split('_'))
            {
                ViewModelName += item;
            }

            ViewModel += Environment.NewLine + TableName + ": { ";

            foreach (var item in ColList)
            {
                if (!item.Column_Name.ToLower().Contains("created") && !item.Column_Name.ToLower().Contains("updated"))
                {

                    ViewModel += Environment.NewLine;

                    ViewModel += Environment.NewLine + item.Column_Name + " : $('#txt" + item.Column_Name + "').val(),";
                }
            }

            ViewModel += Environment.NewLine + " }";

            return ViewModel;
        }

        public string GetPrintableView(List<ColInfo> ColList, string TableName)
        {
            string Print = "";
            string PrintName = "";
            foreach (var item in TableName.Split('_'))
            {
                PrintName += item;
            }

            foreach (var item in ColList)
            {
                if (!item.Column_Name.ToLower().Contains("created") && !item.Column_Name.ToLower().Contains("updated"))
                {

                    Print += Environment.NewLine;

                    Print += Environment.NewLine + "<label>" + item.Column_Name.Replace("_", " ") + "</label> @Model." + TableName + "." + item.Column_Name;
                }
            }

            return Print;
        }
    }
}
