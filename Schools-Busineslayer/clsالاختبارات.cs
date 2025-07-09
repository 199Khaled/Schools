
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالاختبارات
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الاختبار { get; set; }
        public int? معرّف_المادة { get; set; }
        public clsالمواد الموادInfo { get; set; }
        public int? معرّف_الصف { get; set; }
        public clsالصفوف الصفوفInfo { get; set; }
        public DateTime? تاريخ_الاختبار { get; set; }


        public clsالاختبارات()
        {
            this.معرّف_الاختبار = null;
            this.معرّف_المادة = 0;
            this.معرّف_الصف = 0;
            this.تاريخ_الاختبار = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsالاختبارات(
int? معرّف_الاختبار, int? معرّف_المادة, int? معرّف_الصف, DateTime? تاريخ_الاختبار)        {
            this.معرّف_الاختبار = معرّف_الاختبار;
            this.معرّف_المادة = معرّف_المادة;
            this.الموادInfo = clsالمواد.FindByمعرّف_المادة(معرّف_المادة);
            this.معرّف_الصف = معرّف_الصف;
            this.الصفوفInfo = clsالصفوف.FindByمعرّف_الصف(معرّف_الصف);
            this.تاريخ_الاختبار = تاريخ_الاختبار;
            Mode = enMode.Update;
        }


       private bool _AddNewالاختبارات()
       {
        this.معرّف_الاختبار = clsالاختباراتData.AddNewالاختبارات(
this.معرّف_المادة, this.معرّف_الصف, this.تاريخ_الاختبار);
        return (this.معرّف_الاختبار != null);
       }


       public static bool AddNewالاختبارات(
ref int? معرّف_الاختبار, int? معرّف_المادة, int? معرّف_الصف, DateTime? تاريخ_الاختبار)        {
        معرّف_الاختبار = clsالاختباراتData.AddNewالاختبارات(
معرّف_المادة, معرّف_الصف, تاريخ_الاختبار);

        return (معرّف_الاختبار != null);

       }


       private bool _Updateالاختبارات()
       {
        return clsالاختباراتData.UpdateالاختباراتByID(
this.معرّف_الاختبار, this.معرّف_المادة, this.معرّف_الصف, this.تاريخ_الاختبار);
       }


       public static bool UpdateالاختباراتByID(
int? معرّف_الاختبار, int? معرّف_المادة, int? معرّف_الصف, DateTime? تاريخ_الاختبار)        {
        return clsالاختباراتData.UpdateالاختباراتByID(
معرّف_الاختبار, معرّف_المادة, معرّف_الصف, تاريخ_الاختبار);

        }


       public static clsالاختبارات FindByمعرّف_الاختبار(int? معرّف_الاختبار)

        {
            if (معرّف_الاختبار == null)
            {
                return null;
            }
            int? معرّف_المادة = 0;
            int? معرّف_الصف = 0;
            DateTime? تاريخ_الاختبار = DateTime.Now;
            bool IsFound = clsالاختباراتData.GetالاختباراتInfoByID(معرّف_الاختبار,
 ref معرّف_المادة,  ref معرّف_الصف,  ref تاريخ_الاختبار);

           if (IsFound)
               return new clsالاختبارات(
معرّف_الاختبار, معرّف_المادة, معرّف_الصف, تاريخ_الاختبار);
            else
                return null;
            }


       public static DataTable GetAllالاختبارات()
       {

        return clsالاختباراتData.GetAllالاختبارات();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالاختبارات())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالاختبارات();

            }
        
            return false;
        }



       public static bool Deleteالاختبارات(int معرّف_الاختبار)
       {

        return clsالاختباراتData.Deleteالاختبارات(معرّف_الاختبار);

       }


        public enum الاختباراتColumn
         {
            معرّف_الاختبار,
            معرّف_المادة,
            معرّف_الصف,
            تاريخ_الاختبار
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الاختباراتColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالاختباراتData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
