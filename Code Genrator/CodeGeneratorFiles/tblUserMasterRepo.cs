using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tblUserMasterRepo
 {
public class tblUserMasterRepo
 {

  SQLHelperRepo _sqlRepo;

  public tblUserMasterRepo()
  {
   _sqlRepo = new SQLHelperRepo();
  }

  public void Insert_tblUserMaster(tblUserMasterInfo tblusermaster)
  {
   _sqlRepo.ExecuteNonQuery(SetValues_In_tblUserMaster(tblusermaster), StoredProcedures.Insert_tblUserMaster.ToString(), CommandType.StoredProcedure);
  }

  public void Update_tblUserMaster(tblUserMasterInfo tblusermaster)
  {
   _sqlRepo.ExecuteNonQuery(Set_Values_In_tblUserMaster(tblusermaster), StoredProcedures.Update_tblUserMaster.ToString(), CommandType.StoredProcedure);
  }

  private List<SqlParameter> Set_Values_In_tblUserMaster(tblUserMasterInfo tblusermaster)
  {
   List<SqlParameter> sqlParams = new List<SqlParameter>();
   sqlParams.Add(new SqlParameter("@Id",tblusermaster.Id));
   sqlParams.Add(new SqlParameter("@UserName",tblusermaster.UserName));
   sqlParams.Add(new SqlParameter("@Password",tblusermaster.Password));
   sqlParams.Add(new SqlParameter("@Linkedwith",tblusermaster.Linkedwith));
   sqlParams.Add(new SqlParameter("@Active",tblusermaster.Active));
   sqlParams.Add(new SqlParameter("@CreatedBy",tblusermaster.CreatedBy));
   sqlParams.Add(new SqlParameter("@CreatedOn",tblusermaster.CreatedOn));
   sqlParams.Add(new SqlParameter("@UpdatedBy",tblusermaster.UpdatedBy));
   sqlParams.Add(new SqlParameter("@UpdatedOn",tblusermaster.UpdatedOn));
   return sqlParams;
  }

  public List<tblUserMasterInfo> Get_tblUserMasters(ref PaginationInfo pager)
  {
   List<tblUserMasterInfo> tblusermasters = new List<tblUserMasterInfo>();
   DataTable dt = _sqlRepo.ExecuteDataTable(null, StoredProcedures.Get_tblUserMaster.ToString(), CommandType.StoredProcedure);
   foreach (DataRow dr in CommonMethods.GetRows(dt, ref pager))
   {
    tblusermasters.Add(Get_tblUserMaster_Values(dr));
   }
    return tblusermasters;
 }

  public tblUserMasterInfo Get_tblUserMaster_By_Id (int tblusermasterId)
  {
   List<SqlParameter> sqlParams = new List<SqlParameter>();
   tblUserMasterInfo tblusermaster = new tblUserMasterinfo();
   sqlParams.Add(new SqlParameter("@Id",tblusermasterId));
   DataTable dt = _sqlRepo.ExecuteDataTable(sqlParams, StoredProcedures.Get_tblUserMaster_By_Id.ToString(), CommandType.StoredProcedure);
   List<DataRow> drList = new List<DataRow>();
   drList = dt.AsEnumerable().ToList();
   foreach (DataRow dr in drList)
   {
    tblusermaster= Get_tblUserMaster_Values(dr);
   }
    return tblusermaster;
 }
 
  private tblUserMasterInfo Get_tblUserMaster_Values(DataRow dr)
 {
   tblUserMasterInfo tblusermaster = new tblUserMasterInfo();
 
   tblusermaster.Id = Convert.ToInt32(dr["Id"]);
   tblusermaster.UserName = Convert.ToString(dr["UserName"]);
   tblusermaster.Password = Convert.ToString(dr["Password"]);
   tblusermaster.Linkedwith = Convert.ToString(dr["Linkedwith"]);
   tblusermaster.Active = Convert.ToBoolean(dr["Active"]);
   tblusermaster.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
   tblusermaster.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
   tblusermaster.UpdatedBy = Convert.ToInt32(dr["UpdatedBy"]);
   tblusermaster.UpdatedOn = Convert.ToDateTime(dr["UpdatedOn"]);
   return tblusermaster;
 }

  public void Delete_tblUserMaster_By_Id(int tblusermasterId)
{
   List<SqlParameter> sqlParams = new List<SqlParameter>();
   sqlParams.Add(new SqlParameter("@Id", tblusermasterId));
   _sqlRepo.ExecuteNonQuery(sqlParams, StoredProcedures.Delete_tblUserMaster_By_Id.ToString(), CommandType.StoredProcedure);
 }

 }
} 