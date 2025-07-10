
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالمواد
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_المادة { get; set; }
        public string اسم_المادة { get; set; }


        public clsالمواد()
        {
            this.معرّف_المادة = null;
            this.اسم_المادة = "";
            Mode = enMode.AddNew;
        }


        private clsالمواد(
int? معرّف_المادة, string اسم_المادة)        {
            this.معرّف_المادة = معرّف_المادة;
            this.اسم_المادة = اسم_المادة;
            Mode = enMode.Update;
        }


       private bool _AddNewالمواد()
       {
        this.معرّف_المادة = clsالموادData.AddNewالمواد(
this.اسم_المادة);
        return (this.معرّف_المادة != null);
       }


       public static bool AddNewالمواد(
ref int? معرّف_المادة, string اسم_المادة)        {
        معرّف_المادة = clsالموادData.AddNewالمواد(
اسم_المادة);

        return (معرّف_المادة != null);

       }


       private bool _Updateالمواد()
       {
        return clsالموادData.UpdateالموادByID(
this.معرّف_المادة, this.اسم_المادة);
       }


       public static bool UpdateالموادByID(
int? معرّف_المادة, string اسم_المادة)        {
        return clsالموادData.UpdateالموادByID(
معرّف_المادة, اسم_المادة);

        }


       public static clsالمواد FindByمعرّف_المادة(int? معرّف_المادة)

        {
            if (معرّف_المادة == null)
            {
                return null;
            }
            string اسم_المادة = "";
            bool IsFound = clsالموادData.GetالموادInfoByID(معرّف_المادة,
 ref اسم_المادة);

           if (IsFound)
               return new clsالمواد(
معرّف_المادة, اسم_المادة);
            else
                return null;
            }

        public static clsالمواد FindByاسم_المادة(string اسم_المادة)

        {
            if (اسم_المادة == null)
            {
                return null;
            }
            int? معرّف_المادة = 0;
            bool IsFound = clsالموادData.GetالموادInfoBySubjectName(ref معرّف_المادة, اسم_المادة);

            if (IsFound)
                return new clsالمواد(
 معرّف_المادة, اسم_المادة);
            else
                return null;
        }

        public static DataTable GetAllالمواد()
       {

        return clsالموادData.GetAllالمواد();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالمواد())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالمواد();

            }
        
            return false;
        }



       public static bool Deleteالمواد(int معرّف_المادة)
       {

        return clsالموادData.Deleteالمواد(معرّف_المادة);

       }


        public enum الموادColumn
         {
            معرّف_المادة,
            اسم_المادة
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الموادColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالموادData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
