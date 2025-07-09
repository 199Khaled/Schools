
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالجداول_الزمنية
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الجدول { get; set; }
        public int? معرّف_المعلم { get; set; }
        public int? معرّف_الفصل { get; set; }
        public int? معرّف_المادة { get; set; }
        public DateTime? اليوم { get; set; }
        public TimeSpan? وقت_البداية { get; set; }
        public TimeSpan? وقت_النهاية { get; set; }


        public clsالجداول_الزمنية()
        {
            this.معرّف_الجدول = null;
            this.معرّف_المعلم = 0;
            this.معرّف_الفصل = 0;
            this.معرّف_المادة = 0;
            this.اليوم = DateTime.Now;
            this.وقت_البداية = default(TimeSpan);
            this.وقت_النهاية = default(TimeSpan);
            Mode = enMode.AddNew;
        }


        private clsالجداول_الزمنية(
int? معرّف_الجدول, int? معرّف_المعلم, int? معرّف_الفصل, int? معرّف_المادة, DateTime? اليوم, TimeSpan? وقت_البداية, TimeSpan? وقت_النهاية)        {
            this.معرّف_الجدول = معرّف_الجدول;
            this.معرّف_المعلم = معرّف_المعلم;
            this.معرّف_الفصل = معرّف_الفصل;
            this.معرّف_المادة = معرّف_المادة;
            this.اليوم = اليوم;
            this.وقت_البداية = وقت_البداية;
            this.وقت_النهاية = وقت_النهاية;
            Mode = enMode.Update;
        }


       private bool _AddNewالجداول_الزمنية()
       {
        this.معرّف_الجدول = clsالجداول_الزمنيةData.AddNewالجداول_الزمنية(
this.معرّف_المعلم, this.معرّف_الفصل, this.معرّف_المادة, this.اليوم, this.وقت_البداية, this.وقت_النهاية);
        return (this.معرّف_الجدول != null);
       }


       public static bool AddNewالجداول_الزمنية(
ref int? معرّف_الجدول, int? معرّف_المعلم, int? معرّف_الفصل, int? معرّف_المادة, DateTime? اليوم, TimeSpan? وقت_البداية, TimeSpan? وقت_النهاية)        {
        معرّف_الجدول = clsالجداول_الزمنيةData.AddNewالجداول_الزمنية(
معرّف_المعلم, معرّف_الفصل, معرّف_المادة, اليوم, وقت_البداية, وقت_النهاية);

        return (معرّف_الجدول != null);

       }


       private bool _Updateالجداول_الزمنية()
       {
        return clsالجداول_الزمنيةData.Updateالجداول_الزمنيةByID(
this.معرّف_الجدول, this.معرّف_المعلم, this.معرّف_الفصل, this.معرّف_المادة, this.اليوم, this.وقت_البداية, this.وقت_النهاية);
       }


       public static bool Updateالجداول_الزمنيةByID(
int? معرّف_الجدول, int? معرّف_المعلم, int? معرّف_الفصل, int? معرّف_المادة, DateTime? اليوم, TimeSpan? وقت_البداية, TimeSpan? وقت_النهاية)        {
        return clsالجداول_الزمنيةData.Updateالجداول_الزمنيةByID(
معرّف_الجدول, معرّف_المعلم, معرّف_الفصل, معرّف_المادة, اليوم, وقت_البداية, وقت_النهاية);

        }


       public static clsالجداول_الزمنية FindByمعرّف_الجدول(int? معرّف_الجدول)

        {
            if (معرّف_الجدول == null)
            {
                return null;
            }
            int? معرّف_المعلم = 0;
            int? معرّف_الفصل = 0;
            int? معرّف_المادة = 0;
            DateTime? اليوم = DateTime.Now;
            TimeSpan? وقت_البداية = default(TimeSpan);
            TimeSpan? وقت_النهاية = default(TimeSpan);
            bool IsFound = clsالجداول_الزمنيةData.Getالجداول_الزمنيةInfoByID(معرّف_الجدول,
 ref معرّف_المعلم,  ref معرّف_الفصل,  ref معرّف_المادة,  ref اليوم,  ref وقت_البداية,  ref وقت_النهاية);

           if (IsFound)
               return new clsالجداول_الزمنية(
معرّف_الجدول, معرّف_المعلم, معرّف_الفصل, معرّف_المادة, اليوم, وقت_البداية, وقت_النهاية);
            else
                return null;
            }


       public static DataTable GetAllالجداول_الزمنية()
       {

        return clsالجداول_الزمنيةData.GetAllالجداول_الزمنية();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالجداول_الزمنية())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالجداول_الزمنية();

            }
        
            return false;
        }



       public static bool Deleteالجداول_الزمنية(int معرّف_الجدول)
       {

        return clsالجداول_الزمنيةData.Deleteالجداول_الزمنية(معرّف_الجدول);

       }


        public enum الجداول_الزمنيةColumn
         {
            معرّف_الجدول,
            معرّف_المعلم,
            معرّف_الفصل,
            معرّف_المادة,
            اليوم,
            وقت_البداية,
            وقت_النهاية
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الجداول_الزمنيةColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالجداول_الزمنيةData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
