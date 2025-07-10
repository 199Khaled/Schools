
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsنتائج_الامتحانات_للطلاب
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_النتيجة { get; set; }
        public int? معرّف_الطالب { get; set; }
        public clsالطلاب الطلابInfo { get; set; }
        public int? معرّف_الاختبار { get; set; }
        public decimal? الدرجة { get; set; }
        public clsنتائج_الامتحانات_للطلاب نتائج_الامتحانات_للطلابInfo { get; set; }

        public clsنتائج_الامتحانات_للطلاب()
        {
            this.معرّف_النتيجة = null;
            this.معرّف_الطالب = 0;
            this.معرّف_الاختبار = 0;
            this.الدرجة = 0m;
            Mode = enMode.AddNew;
        }


        private clsنتائج_الامتحانات_للطلاب(
int? معرّف_النتيجة, int? معرّف_الطالب, int? معرّف_الاختبار, decimal? الدرجة)        {
            this.معرّف_النتيجة = معرّف_النتيجة;
            this.نتائج_الامتحانات_للطلابInfo = clsنتائج_الامتحانات_للطلاب.FindByمعرّف_النتيجة(معرّف_النتيجة);
            this.معرّف_الطالب = معرّف_الطالب;
            this.الطلابInfo = clsالطلاب.FindByمعرّف_الطالب(معرّف_الطالب);
            this.معرّف_الاختبار = معرّف_الاختبار;
            this.الدرجة = الدرجة;
            Mode = enMode.Update;
        }


       private bool _AddNewنتائج_الامتحانات_للطلاب()
       {
        this.معرّف_النتيجة = clsنتائج_الامتحانات_للطلابData.AddNewنتائج_الامتحانات_للطلاب(
this.معرّف_الطالب, this.معرّف_الاختبار, this.الدرجة);
        return (this.معرّف_النتيجة != null);
       }


       public static bool AddNewنتائج_الامتحانات_للطلاب(
ref int? معرّف_النتيجة, int? معرّف_الطالب, int? معرّف_الاختبار, decimal? الدرجة)        {
        معرّف_النتيجة = clsنتائج_الامتحانات_للطلابData.AddNewنتائج_الامتحانات_للطلاب(
معرّف_الطالب, معرّف_الاختبار, الدرجة);

        return (معرّف_النتيجة != null);

       }


       private bool _Updateنتائج_الامتحانات_للطلاب()
       {
        return clsنتائج_الامتحانات_للطلابData.Updateنتائج_الامتحانات_للطلابByID(
this.معرّف_النتيجة, this.معرّف_الطالب, this.معرّف_الاختبار, this.الدرجة);
       }


       public static bool Updateنتائج_الامتحانات_للطلابByID(
int? معرّف_النتيجة, int? معرّف_الطالب, int? معرّف_الاختبار, decimal? الدرجة)        {
        return clsنتائج_الامتحانات_للطلابData.Updateنتائج_الامتحانات_للطلابByID(
معرّف_النتيجة, معرّف_الطالب, معرّف_الاختبار, الدرجة);

        }


       public static clsنتائج_الامتحانات_للطلاب FindByمعرّف_النتيجة(int? معرّف_النتيجة)

        {
            if (معرّف_النتيجة == null)
            {
                return null;
            }
            int? معرّف_الطالب = 0;
            int? معرّف_الاختبار = 0;
            decimal? الدرجة = 0m;
            bool IsFound = clsنتائج_الامتحانات_للطلابData.Getنتائج_الامتحانات_للطلابInfoByID(معرّف_النتيجة,
 ref معرّف_الطالب,  ref معرّف_الاختبار,  ref الدرجة);

           if (IsFound)
               return new clsنتائج_الامتحانات_للطلاب(
معرّف_النتيجة, معرّف_الطالب, معرّف_الاختبار, الدرجة);
            else
                return null;
            }


       public static DataTable GetAllنتائج_الامتحانات_للطلاب()
       {

        return clsنتائج_الامتحانات_للطلابData.GetAllنتائج_الامتحانات_للطلاب();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewنتائج_الامتحانات_للطلاب())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateنتائج_الامتحانات_للطلاب();

            }
        
            return false;
        }



       public static bool Deleteنتائج_الامتحانات_للطلاب(int معرّف_النتيجة)
       {

        return clsنتائج_الامتحانات_للطلابData.Deleteنتائج_الامتحانات_للطلاب(معرّف_النتيجة);

       }


        public enum نتائج_الامتحانات_للطلابColumn
         {
            معرّف_النتيجة,
            معرّف_الطالب,
            معرّف_الاختبار,
            الدرجة
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(نتائج_الامتحانات_للطلابColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsنتائج_الامتحانات_للطلابData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
