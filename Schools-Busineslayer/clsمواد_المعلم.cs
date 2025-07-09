
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsمواد_المعلم
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_مادة_المعلم { get; set; }
        public int? معرّف_المعلم { get; set; }
        public clsالموظفون الموظفونInfo { get; set; }
        public int? معرّف_المادة { get; set; }
        public clsالمواد الموادInfo { get; set; }


        public clsمواد_المعلم()
        {
            this.معرّف_مادة_المعلم = null;
            this.معرّف_المعلم = 0;
            this.معرّف_المادة = 0;
            Mode = enMode.AddNew;
        }


        private clsمواد_المعلم(
int? معرّف_مادة_المعلم, int? معرّف_المعلم, int? معرّف_المادة)        {
            this.معرّف_مادة_المعلم = معرّف_مادة_المعلم;
            this.معرّف_المعلم = معرّف_المعلم;
            this.الموظفونInfo = clsالموظفون.FindByمعرّف_الموظف(معرّف_المعلم);
            this.معرّف_المادة = معرّف_المادة;
            this.الموادInfo = clsالمواد.FindByمعرّف_المادة(معرّف_المادة);
            Mode = enMode.Update;
        }


       private bool _AddNewمواد_المعلم()
       {
        this.معرّف_مادة_المعلم = clsمواد_المعلمData.AddNewمواد_المعلم(
this.معرّف_المعلم, this.معرّف_المادة);
        return (this.معرّف_مادة_المعلم != null);
       }


       public static bool AddNewمواد_المعلم(
ref int? معرّف_مادة_المعلم, int? معرّف_المعلم, int? معرّف_المادة)        {
        معرّف_مادة_المعلم = clsمواد_المعلمData.AddNewمواد_المعلم(
معرّف_المعلم, معرّف_المادة);

        return (معرّف_مادة_المعلم != null);

       }


       private bool _Updateمواد_المعلم()
       {
        return clsمواد_المعلمData.Updateمواد_المعلمByID(
this.معرّف_مادة_المعلم, this.معرّف_المعلم, this.معرّف_المادة);
       }


       public static bool Updateمواد_المعلمByID(
int? معرّف_مادة_المعلم, int? معرّف_المعلم, int? معرّف_المادة)        {
        return clsمواد_المعلمData.Updateمواد_المعلمByID(
معرّف_مادة_المعلم, معرّف_المعلم, معرّف_المادة);

        }


       public static clsمواد_المعلم FindByمعرّف_مادة_المعلم(int? معرّف_مادة_المعلم)

        {
            if (معرّف_مادة_المعلم == null)
            {
                return null;
            }
            int? معرّف_المعلم = 0;
            int? معرّف_المادة = 0;
            bool IsFound = clsمواد_المعلمData.Getمواد_المعلمInfoByID(معرّف_مادة_المعلم,
 ref معرّف_المعلم,  ref معرّف_المادة);

           if (IsFound)
               return new clsمواد_المعلم(
معرّف_مادة_المعلم, معرّف_المعلم, معرّف_المادة);
            else
                return null;
            }


       public static DataTable GetAllمواد_المعلم()
       {

        return clsمواد_المعلمData.GetAllمواد_المعلم();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewمواد_المعلم())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateمواد_المعلم();

            }
        
            return false;
        }



       public static bool Deleteمواد_المعلم(int معرّف_مادة_المعلم)
       {

        return clsمواد_المعلمData.Deleteمواد_المعلم(معرّف_مادة_المعلم);

       }


        public enum مواد_المعلمColumn
         {
            معرّف_مادة_المعلم,
            معرّف_المعلم,
            معرّف_المادة
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(مواد_المعلمColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsمواد_المعلمData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
