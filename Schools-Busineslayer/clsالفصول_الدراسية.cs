
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالفصول_الدراسية
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الفصل { get; set; }
        public string اسم_الفصل { get; set; }
        public int? السعة { get; set; }
        public string المبنى { get; set; } = null;


        public clsالفصول_الدراسية()
        {
            this.معرّف_الفصل = null;
            this.اسم_الفصل = "";
            this.السعة = 0;
            this.المبنى = null;
            Mode = enMode.AddNew;
        }


        private clsالفصول_الدراسية(
int? معرّف_الفصل, string اسم_الفصل, int? السعة, string المبنى = null)        {
            this.معرّف_الفصل = معرّف_الفصل;
            this.اسم_الفصل = اسم_الفصل;
            this.السعة = السعة;
            this.المبنى = المبنى;
            Mode = enMode.Update;
        }


       private bool _AddNewالفصول_الدراسية()
       {
        this.معرّف_الفصل = clsالفصول_الدراسيةData.AddNewالفصول_الدراسية(
this.اسم_الفصل, this.السعة, this.المبنى);
        return (this.معرّف_الفصل != null);
       }


       public static bool AddNewالفصول_الدراسية(
ref int? معرّف_الفصل, string اسم_الفصل, int? السعة, string المبنى = null)        {
        معرّف_الفصل = clsالفصول_الدراسيةData.AddNewالفصول_الدراسية(
اسم_الفصل, السعة, المبنى);

        return (معرّف_الفصل != null);

       }


       private bool _Updateالفصول_الدراسية()
       {
        return clsالفصول_الدراسيةData.Updateالفصول_الدراسيةByID(
this.معرّف_الفصل, this.اسم_الفصل, this.السعة, this.المبنى);
       }


       public static bool Updateالفصول_الدراسيةByID(
int? معرّف_الفصل, string اسم_الفصل, int? السعة, string المبنى = null)        {
        return clsالفصول_الدراسيةData.Updateالفصول_الدراسيةByID(
معرّف_الفصل, اسم_الفصل, السعة, المبنى);

        }


       public static clsالفصول_الدراسية FindByمعرّف_الفصل(int? معرّف_الفصل)

        {
            if (معرّف_الفصل == null)
            {
                return null;
            }
            string اسم_الفصل = "";
            int? السعة = 0;
            string المبنى = "";
            bool IsFound = clsالفصول_الدراسيةData.Getالفصول_الدراسيةInfoByID(معرّف_الفصل,
 ref اسم_الفصل,  ref السعة,  ref المبنى);

           if (IsFound)
               return new clsالفصول_الدراسية(
معرّف_الفصل, اسم_الفصل, السعة, المبنى);
            else
                return null;
            }


       public static DataTable GetAllالفصول_الدراسية()
       {

        return clsالفصول_الدراسيةData.GetAllالفصول_الدراسية();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالفصول_الدراسية())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالفصول_الدراسية();

            }
        
            return false;
        }



       public static bool Deleteالفصول_الدراسية(int معرّف_الفصل)
       {

        return clsالفصول_الدراسيةData.Deleteالفصول_الدراسية(معرّف_الفصل);

       }


        public enum الفصول_الدراسيةColumn
         {
            معرّف_الفصل,
            اسم_الفصل,
            السعة,
            المبنى
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الفصول_الدراسيةColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالفصول_الدراسيةData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
