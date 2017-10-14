using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tblUserMasterViewModel
 {
public class tblUserMasterViewModel
 {
  public tblUserMasterViewModel()
  {
Friendly_Message = new List<FriendlyMessageInfo>();
Pager = new PaginationInfo();
 tblusermaster = new tblUserMasterInfo();
 tblusermasters = new List<tblUserMasterInfo>();
  }
public List<FriendlyMessageInfo> Friendly_Message { get; set; }
public PaginationInfo Pager { get; set; }
public tblUserMasterInfo tblusermaster { get; set; }
public List<tblUserMasterInfo> tblusermasters { get; set; }
  }
} 