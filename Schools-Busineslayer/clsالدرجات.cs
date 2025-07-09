
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالدرجات
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الدرجة { get; set; }
        public int? معرّف_الطالب { get; set; }
        public int? معرّف_مادة_الصف { get; set; }
        public decimal? الدرجة { get; set; }
        public DateTime? تاريخ_الدرجة { get; set; }


        public clsالدرجات()
        {
            this.معرّف_الدرجة = null;
            this.معرّف_الطالب = 0;
            this.معرّف_مادة_الصف = 0;
            this.الدرجة = 0m;
            this.تاريخ_الدرجة = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsالدرجات(
int? معرّف_الدرجة, int? معرّف_الطالب, int? معرّف_مادة_الصف, decimal? الدرجة, DateTime? تاريخ_الدرجة)        {
            this.معرّف_الدرجة = معرّف_الدرجة;
            this.معرّف_الطالب = معرّف_الطالب;
            this.معرّف_مادة_الصف = معرّف_مادة_الصف;
            this.الدرجة = الدرجة;
            this.تاريخ_الدرجة = تاريخ_الدرجة;
            Mode = enMode.Update;
        }


       private bool _AddNewالدرجات()
       {
        this.معرّف_الدرجة = clsالدرجاتData.AddNewالدرجات(
this.معرّف_الطالب, this.معرّف_مادة_الصف, this.الدرجة, this.تاريخ_الدرجة);
        return (this.معرّف_الدرجة != null);
       }


       public static bool AddNewالدرجات(
ref int? معرّف_الدرجة, int? معرّف_الطالب, int? معرّف_مادة_الصف, decimal? الدرجة, DateTime? تاريخ_الدرجة)        {
        معرّف_الدرجة = clsالدرجاتData.AddNewالدرجات(
معرّف_الطالب, معرّف_مادة_الصف, الدرجة, تاريخ_الدرجة);

        return (معرّف_الدرجة != null);

       }


       private bool _Updateالدرجات()
       {
        return clsالدرجاتData.UpdateالدرجاتByID(
this.معرّف_الدرجة, this.معرّف_الطالب, this.معرّف_مادة_الصف, this.الدرجة, this.تاريخ_الدرجة);
       }


       public static bool UpdateالدرجاتByID(
int? معرّف_الدرجة, int? معرّف_الطالب, int? معرّف_مادة_الصف, decimal? الدرجة, DateTime? تاريخ_الدرجة)        {
        return clsالدرجاتData.UpdateالدرجاتByID(
معرّف_الدرجة, معرّف_الطالب, معرّف_مادة_الصف, الدرجة, تاريخ_الدرجة);

        }


       public static clsالدرجات FindByمعرّف_الدرجة(int? معرّف_الدرجة)

        {
            if (معرّف_الدرجة == null)
            {
                return null;
            }
            int? معرّف_الطالب = 0;
            int? معرّف_مادة_الصف = 0;
            decimal? الدرجة = 0m;
            DateTime? تاريخ_الدرجة = DateTime.Now;
            bool IsFound = clsالدرجاتData.GetالدرجاتInfoByID(معرّف_الدرجة,
 ref معرّف_الطالب,  ref معرّف_مادة_الصف,  ref الدرجة,  ref تاريخ_الدرجة);

           if (IsFound)
               return new clsالدرجات(
معرّف_الدرجة, معرّف_الطالب, معرّف_مادة_الصف, الدرجة, تاريخ_الدرجة);
            else
                return null;
            }


       public static DataTable GetAllالدرجات()
       {

        return clsالدرجاتData.GetAllالدرجات();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالدرجات())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالدرجات();

            }
        
            return false;
        }



       public static bool Deleteالدرجات(int معرّف_الدرجة)
       {

        return clsالدرجاتData.Deleteالدرجات(معرّف_الدرجة);

       }


        public enum الدرجاتColumn
         {
            معرّف_الدرجة,
            معرّف_الطالب,
            معرّف_مادة_الصف,
            الدرجة,
            تاريخ_الدرجة
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الدرجاتColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالدرجاتData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
