
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالطلاب
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الطالب { get; set; }
        public int? معرّف_الشخص { get; set; }
        public clsالأشخاص الأشخاصInfo { get; set; }
        public DateTime? تاريخ_الالتحاق { get; set; }


        public clsالطلاب()
        {
            this.معرّف_الطالب = null;
            this.معرّف_الشخص = 0;
            this.تاريخ_الالتحاق = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsالطلاب(
int? معرّف_الطالب, int? معرّف_الشخص, DateTime? تاريخ_الالتحاق)        {
            this.معرّف_الطالب = معرّف_الطالب;
            this.معرّف_الشخص = معرّف_الشخص;
            this.الأشخاصInfo = clsالأشخاص.FindByمعرّف_الشخص(معرّف_الشخص);
            this.تاريخ_الالتحاق = تاريخ_الالتحاق;
            Mode = enMode.Update;
        }


       private bool _AddNewالطلاب()
       {
        this.معرّف_الطالب = clsالطلابData.AddNewالطلاب(
this.معرّف_الشخص, this.تاريخ_الالتحاق);
        return (this.معرّف_الطالب != null);
       }


       public static bool AddNewالطلاب(
ref int? معرّف_الطالب, int? معرّف_الشخص, DateTime? تاريخ_الالتحاق)        {
        معرّف_الطالب = clsالطلابData.AddNewالطلاب(
معرّف_الشخص, تاريخ_الالتحاق);

        return (معرّف_الطالب != null);

       }


       private bool _Updateالطلاب()
       {
        return clsالطلابData.UpdateالطلابByID(
this.معرّف_الطالب, this.معرّف_الشخص, this.تاريخ_الالتحاق);
       }


       public static bool UpdateالطلابByID(
int? معرّف_الطالب, int? معرّف_الشخص, DateTime? تاريخ_الالتحاق)        {
        return clsالطلابData.UpdateالطلابByID(
معرّف_الطالب, معرّف_الشخص, تاريخ_الالتحاق);

        }


       public static clsالطلاب FindByمعرّف_الطالب(int? معرّف_الطالب)

        {
            if (معرّف_الطالب == null)
            {
                return null;
            }
            int? معرّف_الشخص = 0;
            DateTime? تاريخ_الالتحاق = DateTime.Now;
            bool IsFound = clsالطلابData.GetالطلابInfoByID(معرّف_الطالب,
 ref معرّف_الشخص,  ref تاريخ_الالتحاق);

           if (IsFound)
               return new clsالطلاب(
معرّف_الطالب, معرّف_الشخص, تاريخ_الالتحاق);
            else
                return null;
            }


       public static DataTable GetAllالطلاب()
       {

        return clsالطلابData.GetAllالطلاب();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالطلاب())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالطلاب();

            }
        
            return false;
        }



       public static bool Deleteالطلاب(int معرّف_الطالب)
       {

        return clsالطلابData.Deleteالطلاب(معرّف_الطالب);

       }


        public enum الطلابColumn
         {
            معرّف_الطالب,
            معرّف_الشخص,
            تاريخ_الالتحاق
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الطلابColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالطلابData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
