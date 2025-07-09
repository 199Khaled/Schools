
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالتسجيلات
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_التسجيل { get; set; }
        public int? معرّف_الطالب { get; set; }
        public clsالطلاب الطلابInfo { get; set; }
        public int? معرّف_الصف { get; set; }
        public clsالصفوف الصفوفInfo { get; set; }
        public DateTime? تاريخ_التسجيل { get; set; }


        public clsالتسجيلات()
        {
            this.معرّف_التسجيل = null;
            this.معرّف_الطالب = 0;
            this.معرّف_الصف = 0;
            this.تاريخ_التسجيل = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsالتسجيلات(
int? معرّف_التسجيل, int? معرّف_الطالب, int? معرّف_الصف, DateTime? تاريخ_التسجيل)        {
            this.معرّف_التسجيل = معرّف_التسجيل;
            this.معرّف_الطالب = معرّف_الطالب;
            this.الطلابInfo = clsالطلاب.FindByمعرّف_الطالب(معرّف_الطالب);
            this.معرّف_الصف = معرّف_الصف;
            this.الصفوفInfo = clsالصفوف.FindByمعرّف_الصف(معرّف_الصف);
            this.تاريخ_التسجيل = تاريخ_التسجيل;
            Mode = enMode.Update;
        }


       private bool _AddNewالتسجيلات()
       {
        this.معرّف_التسجيل = clsالتسجيلاتData.AddNewالتسجيلات(
this.معرّف_الطالب, this.معرّف_الصف, this.تاريخ_التسجيل);
        return (this.معرّف_التسجيل != null);
       }


       public static bool AddNewالتسجيلات(
ref int? معرّف_التسجيل, int? معرّف_الطالب, int? معرّف_الصف, DateTime? تاريخ_التسجيل)        {
        معرّف_التسجيل = clsالتسجيلاتData.AddNewالتسجيلات(
معرّف_الطالب, معرّف_الصف, تاريخ_التسجيل);

        return (معرّف_التسجيل != null);

       }


       private bool _Updateالتسجيلات()
       {
        return clsالتسجيلاتData.UpdateالتسجيلاتByID(
this.معرّف_التسجيل, this.معرّف_الطالب, this.معرّف_الصف, this.تاريخ_التسجيل);
       }


       public static bool UpdateالتسجيلاتByID(
int? معرّف_التسجيل, int? معرّف_الطالب, int? معرّف_الصف, DateTime? تاريخ_التسجيل)        {
        return clsالتسجيلاتData.UpdateالتسجيلاتByID(
معرّف_التسجيل, معرّف_الطالب, معرّف_الصف, تاريخ_التسجيل);

        }


       public static clsالتسجيلات FindByمعرّف_التسجيل(int? معرّف_التسجيل)

        {
            if (معرّف_التسجيل == null)
            {
                return null;
            }
            int? معرّف_الطالب = 0;
            int? معرّف_الصف = 0;
            DateTime? تاريخ_التسجيل = DateTime.Now;
            bool IsFound = clsالتسجيلاتData.GetالتسجيلاتInfoByID(معرّف_التسجيل,
 ref معرّف_الطالب,  ref معرّف_الصف,  ref تاريخ_التسجيل);

           if (IsFound)
               return new clsالتسجيلات(
معرّف_التسجيل, معرّف_الطالب, معرّف_الصف, تاريخ_التسجيل);
            else
                return null;
            }

        public static DataTable GetAllالتسجيلات()
       {

        return clsالتسجيلاتData.GetAllالتسجيلات();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالتسجيلات())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالتسجيلات();

            }
        
            return false;
        }



       public static bool Deleteالتسجيلات(int معرّف_التسجيل)
       {

        return clsالتسجيلاتData.Deleteالتسجيلات(معرّف_التسجيل);

       }


        public enum التسجيلاتColumn
         {
            معرّف_التسجيل,
            معرّف_الطالب,
            معرّف_الصف,
            تاريخ_التسجيل
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(التسجيلاتColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالتسجيلاتData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
