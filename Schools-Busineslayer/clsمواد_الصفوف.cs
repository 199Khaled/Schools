
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsمواد_الصفوف
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_مادة_الصف { get; set; }
        public int? معرّف_الصف { get; set; }
        public clsالصفوف الصفوفInfo { get; set; }
        public int? معرّف_المادة { get; set; }
        public clsالمواد الموادInfo { get; set; }
        public int? معرّف_المعلم { get; set; }
        public clsالموظفون الموظفونInfo { get; set; }


        public clsمواد_الصفوف()
        {
            this.معرّف_مادة_الصف = null;
            this.معرّف_الصف = 0;
            this.معرّف_المادة = 0;
            this.معرّف_المعلم = 0;
            Mode = enMode.AddNew;
        }


        private clsمواد_الصفوف(
int? معرّف_مادة_الصف, int? معرّف_الصف, int? معرّف_المادة, int? معرّف_المعلم)        {
            this.معرّف_مادة_الصف = معرّف_مادة_الصف;
            this.معرّف_الصف = معرّف_الصف;
            this.الصفوفInfo = clsالصفوف.FindByمعرّف_الصف(معرّف_الصف);
            this.معرّف_المادة = معرّف_المادة;
            this.الموادInfo = clsالمواد.FindByمعرّف_المادة(معرّف_المادة);
            this.معرّف_المعلم = معرّف_المعلم;
            this.الموظفونInfo = clsالموظفون.FindByمعرّف_الموظف(معرّف_المعلم);
            Mode = enMode.Update;
        }


       private bool _AddNewمواد_الصفوف()
       {
        this.معرّف_مادة_الصف = clsمواد_الصفوفData.AddNewمواد_الصفوف(
this.معرّف_الصف, this.معرّف_المادة, this.معرّف_المعلم);
        return (this.معرّف_مادة_الصف != null);
       }


       public static bool AddNewمواد_الصفوف(
ref int? معرّف_مادة_الصف, int? معرّف_الصف, int? معرّف_المادة, int? معرّف_المعلم)        {
        معرّف_مادة_الصف = clsمواد_الصفوفData.AddNewمواد_الصفوف(
معرّف_الصف, معرّف_المادة, معرّف_المعلم);

        return (معرّف_مادة_الصف != null);

       }


       private bool _Updateمواد_الصفوف()
       {
        return clsمواد_الصفوفData.Updateمواد_الصفوفByID(
this.معرّف_مادة_الصف, this.معرّف_الصف, this.معرّف_المادة, this.معرّف_المعلم);
       }


       public static bool Updateمواد_الصفوفByID(
int? معرّف_مادة_الصف, int? معرّف_الصف, int? معرّف_المادة, int? معرّف_المعلم)        {
        return clsمواد_الصفوفData.Updateمواد_الصفوفByID(
معرّف_مادة_الصف, معرّف_الصف, معرّف_المادة, معرّف_المعلم);

        }


       public static clsمواد_الصفوف FindByمعرّف_مادة_الصف(int? معرّف_مادة_الصف)

        {
            if (معرّف_مادة_الصف == null)
            {
                return null;
            }
            int? معرّف_الصف = 0;
            int? معرّف_المادة = 0;
            int? معرّف_المعلم = 0;
            bool IsFound = clsمواد_الصفوفData.Getمواد_الصفوفInfoByID(معرّف_مادة_الصف,
 ref معرّف_الصف,  ref معرّف_المادة,  ref معرّف_المعلم);

           if (IsFound)
               return new clsمواد_الصفوف(
معرّف_مادة_الصف, معرّف_الصف, معرّف_المادة, معرّف_المعلم);
            else
                return null;
            }


       public static DataTable GetAllمواد_الصفوف()
       {

        return clsمواد_الصفوفData.GetAllمواد_الصفوف();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewمواد_الصفوف())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateمواد_الصفوف();

            }
        
            return false;
        }



       public static bool Deleteمواد_الصفوف(int معرّف_مادة_الصف)
       {

        return clsمواد_الصفوفData.Deleteمواد_الصفوف(معرّف_مادة_الصف);

       }


        public enum مواد_الصفوفColumn
         {
            معرّف_مادة_الصف,
            معرّف_الصف,
            معرّف_المادة,
            معرّف_المعلم
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(مواد_الصفوفColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsمواد_الصفوفData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
