using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tblUserMasterInfo
 {
public class tblUserMasterInfo

 {

 public int Id { get; set; }

 public string UserName { get; set; }

 public string Password { get; set; }

 public string Linkedwith { get; set; }

 public bool Active { get; set; }

 public int CreatedBy { get; set; }

 public DateTime CreatedOn { get; set; }

 public int UpdatedBy { get; set; }

 public DateTime UpdatedOn { get; set; }

 }
} 