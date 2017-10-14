using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code_Genrator.Models
{
    public class MySqlViewModel
    {
        public List<ColInfo> ColList { get; set; }

        public MySqlViewModel()
        {
            ColList = new List<ColInfo>();
            Set_Result = "";

            Get_Result = "";

            DB_Names = new List<string>();
        }

        public string Set_Result { get; set; }

        public string Get_Result { get; set; }

        public string Info { get; set; }

        public string Name { get; set; }

        public string Table_Name { get; set; }

        public string DB_Name { get; set; }

        public List<string> DB_Names { get; set; }

        public string InfoCode { get; set; }

        public string RepoCode { get; set; }

        public string ManCode { get; set; }

        public string ModelCode { get; set; }

        public string InsertProc { get; set; }

        public string UpdateProc { get; set; }

        public string SelectProc { get; set; }

        public string ViewModelCode { get; set; }

        public string Message { get; set; }

        public string SelectProcById { get; set; }

        public string DeleteProc { get; set; }

        public string AjaxViewModel { get; set; }

        public string PrintableView { get; set; }
    }

    //public class ColInfo
    //{
    //    public string Column_Name { get; set; }

    //    public string Data_Type { get; set; }

    //    public int Max_Length { get; set; }
    //}
}