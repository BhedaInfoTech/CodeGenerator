using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tblUserMasterManager
 {
public class tblUserMasterManager
 {

  tblUserMasterRepo _tblusermasterRepo;

  public tblUserMasterManager()
  {
  _tblusermasterRepo = new tblUserMasterRepo();
  }

  public void Insert_tblUserMaster(tblUserMasterInfo tblusermaster)
  {
  _tblusermasterRepo.Insert_tblUserMaster(tblusermaster);
  }

  public void Update_tblUserMaster(tblUserMasterInfo tblusermaster)
  {
  _tblusermasterRepo.Update_tblUserMaster(tblusermaster);
  }

  public List<tblUserMasterInfo> Get_tblUserMasters(ref PaginationInfo pager)
  {
  return _tblusermasterRepo.Get_tblUserMasters(ref pager);
  }

  public tblUserMasterInfo Get_tblUserMaster_By_Id (int tblusermasterId)
  {
  return _tblusermasterRepo.Get_tblUserMaster_By_Id(tblusermasterId);
  }

  public void Delete_tblUserMaster_By_Id(int tblusermasterId)
 {
 _enquiryRepo.Delete_tblUserMaster_By_Id(tblusermasterId);
 }
 }
} 