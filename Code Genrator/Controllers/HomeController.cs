using Code_Genrator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Code_Genrator.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public ActionResult Index(HomeViewModel hViewModel)
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //string _sqlCon = string.Empty;

            //_sqlCon = "Data Source=AIS-1;Initial Catalog=master;User=pkuser; Password=Password1#";

            //string str = "SELECT * FROM sys.databases";
            //using (SqlConnection con = new SqlConnection(_sqlCon))
            //{
            //    using (SqlCommand command = new SqlCommand(str, con))
            //    {
            //        command.CommandType = CommandType.Text;

            //        SqlDataReader dataReader = command.ExecuteReader();

            //        if (dataReader.HasRows)
            //        {
            //            while (dataReader.Read())
            //            {
            //                hViewModel.DB_Names.Add(Convert.ToString(dataReader["name"]));
            //            }
            //        }
            //    }
            //}

            return View(hViewModel);
        }

        public PartialViewResult GenrateCode(string DBName, string TableName)
        {
            HomeViewModel hViewModel = new HomeViewModel();

            hViewModel.DB_Name = DBName;

            hViewModel.Table_Name = TableName;

            string _sqlCon = string.Empty;

            _sqlCon = hViewModel.DB_Name;

            string Info = "customer.Customer_Entity.";

            using (SqlConnection con = new SqlConnection(_sqlCon))
            {
                con.Open();

                //SELECT column_name FROM information_schema.columnsWHERE table_name='insert table name here'; 

                string str = "SELECT c.name 'Column_Name',t.Name 'Data_type',c.max_length 'Max_Length',c.precision ,c.scale ,c.is_nullable,ISNULL(i.is_primary_key, 0) 'Primary Key' FROM sys.columns c INNER JOIN  sys.types t ON c.user_type_id = t.user_type_id LEFT OUTER JOIN  sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id LEFT OUTER JOIN  sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id WHERE c.object_id = OBJECT_ID('" + hViewModel.Table_Name + "')";

                using (SqlCommand command = new SqlCommand(str, con))
                {
                    command.CommandType = CommandType.Text;

                    SqlDataReader dataReader = command.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ColInfo col = new ColInfo();

                            col = GetValuesFromDataReader(dataReader);

                            hViewModel.ColList.Add(col);
                        }
                    }
                }
                con.Close();

                hViewModel.Info = Info;

            }

            string Path = Server.MapPath(ConfigurationManager.AppSettings["CodeGeneratedFiles"].ToString());

            string Path2 = Server.MapPath(ConfigurationManager.AppSettings["CodeGeneratedZipFolder"].ToString());


            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            if (!Directory.Exists(Path2))
            {
                Directory.CreateDirectory(Path2);
            }

            hViewModel.InfoCode = GetInfo(hViewModel.ColList, hViewModel.Table_Name);

            if (!string.IsNullOrEmpty(hViewModel.InfoCode))
            {
                CreateFiles(hViewModel.InfoCode, hViewModel.Table_Name + "Info.cs");
            }

            hViewModel.RepoCode = GetRepo(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.RepoCode))
            {
                CreateFiles(hViewModel.RepoCode, hViewModel.Table_Name + "Repo.cs");
            }

            hViewModel.ManCode = GetMan(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.ManCode))
            {
                CreateFiles(hViewModel.ManCode, hViewModel.Table_Name + "Manager.cs");
            }

            hViewModel.InsertProc = GetInsertProc(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.InsertProc))
            {
                CreateFiles(hViewModel.InsertProc, hViewModel.Table_Name + "InsertProc.sql");
            }

            hViewModel.UpdateProc = GetUpdateProc(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.UpdateProc))
            {
                CreateFiles(hViewModel.UpdateProc, hViewModel.Table_Name + "UpdateProc.sql");
            }

            hViewModel.SelectProc = GetSelectProc(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.SelectProc))
            {
                CreateFiles(hViewModel.SelectProc, hViewModel.Table_Name + "SelectProc.sql");
            }

            hViewModel.ViewModelCode = GetViewModel(hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.ViewModelCode))
            {
                CreateFiles(hViewModel.ViewModelCode, hViewModel.Table_Name + "ViewModel.cs");
            }

            hViewModel.SelectProcById = GetSelectByIdProc(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.SelectProcById))
            {
                CreateFiles(hViewModel.SelectProcById, hViewModel.Table_Name + "SelectProcById.sql");
            }

            hViewModel.DeleteProc = GetDeleteProc(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.DeleteProc))
            {
                CreateFiles(hViewModel.DeleteProc, hViewModel.Table_Name + "DeleteProc.sql");
            }

            hViewModel.AjaxViewModel = GetAjaxViewModel(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.AjaxViewModel))
            {
                CreateFiles(hViewModel.AjaxViewModel, hViewModel.Table_Name + "AjaxViewModel.js");
            }

            hViewModel.PrintableView = GetPrintableView(hViewModel.ColList, hViewModel.Table_Name);
            
            if (!string.IsNullOrEmpty(hViewModel.PrintableView))
            {
                CreateFiles(hViewModel.PrintableView, hViewModel.Table_Name + ".cshtml");
            }

            //Start Code edited by sushant on 05102017 

            // I tried using Environement variables to store data at special folder. It works fine on Local but after deploying on IIS, it doesnt work because IIS doesnt take environment variables.

            string fileName = "DownloadedSelectedEntity.zip";

            

             string ZipFileName = Path2 + hViewModel.Table_Name + ".zip";


             using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(ZipFileName))
            {
                try
                {

                    string[] a = System.IO.Directory.GetFiles(Path , hViewModel.Table_Name + "*.*");
                    
                    string attachmentPath = string.Empty;
                    
                    if (System.IO.File.Exists(Path + fileName))
                    {
                        System.IO.File.Delete(Path + fileName);
                    }

                    foreach (string ditem in a)
                    {
                        attachmentPath =  ditem;
                        
                        zip.AddFile(attachmentPath, "/");

                    }

                    zip.Save();

                    MemoryStream ms = new MemoryStream();

                    using (FileStream fileStream = System.IO.File.OpenRead(ZipFileName))
                    {
                        ms.SetLength(fileStream.Length);
                    
                        fileStream.Read(ms.GetBuffer(), 0, (int)fileStream.Length);
                    }

                    Response.ContentType = "application/zip";
                   
                    Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}\"", fileName));
                    
                    Response.AddHeader("Set-Cookie", "fileDownload=true; path=/");
                    
                    ms.Position = 0;
                    
                    Response.Charset = "";
                    
                    ms.WriteTo(Response.OutputStream);
                    
                    Response.Flush();
                    
                    Response.End();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //End Code edited by sushant on 05102017

            return PartialView("_Genrate", hViewModel);
        }

        void CreateFiles(string data, string fileName)
        {
            bool isCSfile = false;

            isCSfile = (!fileName.Contains(".cshtml") && !fileName.Contains(".sql") && !fileName.Contains(".js"));

            if (isCSfile)
            {
                data = "using System;" + Environment.NewLine +
                       "using System.Collections.Generic;" + Environment.NewLine +
                       "using System.Linq;" + Environment.NewLine +
                       "using System.Text;" + Environment.NewLine +
                       "using System.Threading.Tasks;" + Environment.NewLine +
                       "namespace " + fileName.Split('.')[0] + Environment.NewLine + " {" + Environment.NewLine +
                       data + Environment.NewLine +
                       "} ";

            }

            if (!string.IsNullOrEmpty(data))
            {

                // Start Code edited by sushant on 05102017
 
                string Path = Server.MapPath(ConfigurationManager.AppSettings["CodeGeneratedFiles"].ToString());

                if (!System.IO.Directory.Exists(Path))
                {
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(Path);

                    string folderPath = Path;

                    TextWriter txt = new StreamWriter(folderPath + "\\" + fileName);

                    txt.Write(data);

                    txt.Close();

                }
                else
                {
                    string folderPath = Path;

                    TextWriter txt = new StreamWriter(folderPath + "\\" + fileName);

                    txt.Write(data);

                    txt.Close();
                }

                // End Code edited by sushant on 05102017


                // Start - Working Code where Hardcode path was set.
                
                //TextWriter txt = new StreamWriter("C:\\Code Genrator Files\\" + fileName);

                //txt.Write(data);

                //txt.Close();

                // End Working Code where Hardcode path was set.

            }
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

                    string str = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'  order by TABLE_NAME";

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

        public ColInfo GetValuesFromDataReader(SqlDataReader dataReader)
        {
            ColInfo col = new ColInfo();

            col.Column_Name = Convert.ToString(dataReader["Column_Name"]);

            col.Data_Type = Convert.ToString(dataReader["Data_type"]);

            col.Max_Length = Convert.ToInt32(dataReader["Max_Length"]);

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

            Info += Environment.NewLine + "";

            Info += Environment.NewLine + " {";

            Info += Environment.NewLine + "";

            foreach (var item in ColList)
            {
                if (item.Data_Type == "int")
                {
                    Info += Environment.NewLine + " public int " + item.Column_Name + " { get; set; }";
                }
                else if (item.Data_Type == "nvarchar" || item.Data_Type == "varchar")
                {
                    Info += Environment.NewLine + " public string " + item.Column_Name + " { get; set; }";
                }
                else if (item.Data_Type == "bit")
                {
                    Info += Environment.NewLine + " public bool " + item.Column_Name + " { get; set; }";
                }
                else if (item.Data_Type == "datetime")
                {
                    Info += Environment.NewLine + " public DateTime " + item.Column_Name + " { get; set; }";
                }
                else if (item.Data_Type == "decimal")
                {
                    Info += Environment.NewLine + " public decimal " + item.Column_Name + " { get; set; }";
                }
                else if (item.Data_Type == "nchar" || item.Data_Type == "char")
                {
                    Info += Environment.NewLine + " public char [" + item.Max_Length + "] " + item.Column_Name + " { get; set; }";
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

            Repo += Environment.NewLine + "   _sqlRepo.ExecuteNonQuery(SetValues_In_" + TableName + "(" + RepoName.ToLower() + "), StoredProcedures.Insert_" + TableName + ".ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + "  }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public void Update_" + TableName + "(" + RepoName + "Info " + RepoName.ToLower() + ")";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   _sqlRepo.ExecuteNonQuery(Set_Values_In_" + TableName + "(" + RepoName.ToLower() + "), StoredProcedures.Update_" + TableName + ".ToString(), CommandType.StoredProcedure);";

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

            Repo += Environment.NewLine + "  public List<" + RepoName + "Info> Get_" + TableName + "s(ref PaginationInfo pager)";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   List<" + RepoName + "Info> " + RepoName.ToLower() + "s = new List<" + RepoName + "Info>();";

            Repo += Environment.NewLine + "   DataTable dt = _sqlRepo.ExecuteDataTable(null, StoredProcedures.Get_" + TableName + ".ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + "   foreach (DataRow dr in CommonMethods.GetRows(dt, ref pager))";

            Repo += Environment.NewLine + "   {";

            Repo += Environment.NewLine + "    " + RepoName.ToLower() + "s.Add(Get_" + TableName + "_Values(dr));";

            Repo += Environment.NewLine + "   }";

            Repo += Environment.NewLine + "    return " + RepoName.ToLower() + "s;";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public " + RepoName + "Info Get_" + TableName + "_By_Id (int " + TableName.ToLower() + "Id)";

            Repo += Environment.NewLine + "  {";

            Repo += Environment.NewLine + "   List<SqlParameter> sqlParams = new List<SqlParameter>();";

            Repo += Environment.NewLine + "   " + RepoName + "Info " + RepoName.ToLower() + " = new " + RepoName + "info();";

            Repo += Environment.NewLine + "   sqlParams.Add(new SqlParameter(\"@" + ColList[0].Column_Name + "\"," + TableName.ToLower() + "Id));";

            Repo += Environment.NewLine + "   DataTable dt = _sqlRepo.ExecuteDataTable(sqlParams, StoredProcedures.Get_" + TableName + "_By_Id.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + "   List<DataRow> drList = new List<DataRow>();";

            Repo += Environment.NewLine + "   drList = dt.AsEnumerable().ToList();";

            Repo += Environment.NewLine + "   foreach (DataRow dr in drList)";

            Repo += Environment.NewLine + "   {";

            Repo += Environment.NewLine + "    " + RepoName.ToLower() + "= Get_" + TableName + "_Values(dr);";

            Repo += Environment.NewLine + "   }";

            Repo += Environment.NewLine + "    return " + RepoName.ToLower() + ";";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + " ";

            Repo += Environment.NewLine + "  private " + RepoName + "Info Get_" + TableName + "_Values(DataRow dr)";

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
                    Repo += Environment.NewLine + "   " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToString(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "bit")
                {
                    Repo += Environment.NewLine + "   " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToBoolean(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "datetime")
                {
                    Repo += Environment.NewLine + "   " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToDateTime(dr[\"" + item.Column_Name + "\"]);";
                }
                else if (item.Data_Type == "decimal")
                {
                    Repo += Environment.NewLine + "   " + RepoName.ToLower() + "." + item.Column_Name + " = Convert.ToDecimal(dr[\"" + item.Column_Name + "\"]);";
                }
            }

            Repo += Environment.NewLine + "   return " + RepoName.ToLower() + ";";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + "";

            Repo += Environment.NewLine + "  public void Delete_" + TableName + "_By_Id(int " + TableName.ToLower() + "Id)";

            Repo += Environment.NewLine + "{";

            Repo += Environment.NewLine + "   List<SqlParameter> sqlParams = new List<SqlParameter>();";

            Repo += Environment.NewLine + "   sqlParams.Add(new SqlParameter(\"@" + ColList[0].Column_Name + "\", " + TableName.ToLower() + "Id));";

            Repo += Environment.NewLine + "   _sqlRepo.ExecuteNonQuery(sqlParams, StoredProcedures.Delete_" + TableName + "_By_Id.ToString(), CommandType.StoredProcedure);";

            Repo += Environment.NewLine + " }";

            Repo += Environment.NewLine + "";

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

            Man += Environment.NewLine + "  public List<" + ManName + "Info> Get_" + TableName + "s(ref PaginationInfo pager)";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + "  return _" + ManName.ToLower() + "Repo.Get_" + TableName + "s(ref pager);";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public " + ManName + "Info Get_" + TableName + "_By_Id (int " + TableName.ToLower() + "Id)";

            Man += Environment.NewLine + "  {";

            Man += Environment.NewLine + "  return _" + ManName.ToLower() + "Repo.Get_" + TableName + "_By_Id(" + TableName.ToLower() + "Id);";

            Man += Environment.NewLine + "  }";

            Man += Environment.NewLine + "";

            Man += Environment.NewLine + "  public void Delete_" + TableName + "_By_Id(int " + TableName.ToLower() + "Id)";

            Man += Environment.NewLine + " {";

            Man += Environment.NewLine + " _enquiryRepo.Delete_" + TableName + "_By_Id(" + TableName.ToLower() + "Id);";

            Man += Environment.NewLine + " }";

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

            Proc += "Create Procedure Insert_" + TableName + "";

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

            Proc += "Create Procedure Update_" + TableName + "";

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

            Proc += Environment.NewLine + "Create Procedure Get_" + TableName + "";

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

            Proc += Environment.NewLine + "Create Procedure Delete_" + TableName + "_By_Id";

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

            Proc += Environment.NewLine + "Create Procedure Get_" + TableName + "_By_Id";

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

            ViewModel += Environment.NewLine;

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
