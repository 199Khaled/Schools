
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالحضور
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الحضور { get; set; }
        public int? معرّف_الطالب { get; set; }
        public clsStudents StudentsInfo { get; set; }
        public int? معرّف_الفصل { get; set; }
        public clsClasses ClassesInfo { get; set; }
        public DateTime? تاريخ_الحضور { get; set; }
        public string الحالة { get; set; } = null;


        public clsالحضور()
        {
            this.معرّف_الحضور = null;
            this.معرّف_الطالب = 0;
            this.معرّف_الفصل = 0;
            this.تاريخ_الحضور = DateTime.Now;
            this.الحالة = null;
            Mode = enMode.AddNew;
        }


        private clsالحضور(
int? معرّف_الحضور, int? معرّف_الطالب, int? معرّف_الفصل, DateTime? تاريخ_الحضور, string الحالة = null)        {
            this.معرّف_الحضور = معرّف_الحضور;
            this.معرّف_الطالب = معرّف_الطالب;
            this.StudentsInfo = clsStudents.FindByStudentID(معرّف_الطالب);
            this.معرّف_الفصل = معرّف_الفصل;
            this.ClassesInfo = clsClasses.FindByClassID(معرّف_الفصل);
            this.تاريخ_الحضور = تاريخ_الحضور;
            this.الحالة = الحالة;
            Mode = enMode.Update;
        }


       private bool _AddNewالحضور()
       {
        this.معرّف_الحضور = clsالحضورData.AddNewالحضور(
this.معرّف_الطالب, this.معرّف_الفصل, this.تاريخ_الحضور, this.الحالة);
        return (this.معرّف_الحضور != null);
       }


       public static bool AddNewالحضور(
ref int? معرّف_الحضور, int? معرّف_الطالب, int? معرّف_الفصل, DateTime? تاريخ_الحضور, string الحالة = null)        {
        معرّف_الحضور = clsالحضورData.AddNewالحضور(
معرّف_الطالب, معرّف_الفصل, تاريخ_الحضور, الحالة);

        return (معرّف_الحضور != null);

       }


       private bool _Updateالحضور()
       {
        return clsالحضورData.UpdateالحضورByID(
this.معرّف_الحضور, this.معرّف_الطالب, this.معرّف_الفصل, this.تاريخ_الحضور, this.الحالة);
       }


       public static bool UpdateالحضورByID(
int? معرّف_الحضور, int? معرّف_الطالب, int? معرّف_الفصل, DateTime? تاريخ_الحضور, string الحالة = null)        {
        return clsالحضورData.UpdateالحضورByID(
معرّف_الحضور, معرّف_الطالب, معرّف_الفصل, تاريخ_الحضور, الحالة);

        }


       public static clsالحضور FindByمعرّف_الحضور(int? معرّف_الحضور)

        {
            if (معرّف_الحضور == null)
            {
                return null;
            }
            int? معرّف_الطالب = 0;
            int? معرّف_الفصل = 0;
            DateTime? تاريخ_الحضور = DateTime.Now;
            string الحالة = "";
            bool IsFound = clsالحضورData.GetالحضورInfoByID(معرّف_الحضور,
 ref معرّف_الطالب,  ref معرّف_الفصل,  ref تاريخ_الحضور,  ref الحالة);

           if (IsFound)
               return new clsالحضور(
معرّف_الحضور, معرّف_الطالب, معرّف_الفصل, تاريخ_الحضور, الحالة);
            else
                return null;
            }


       public static DataTable GetAllالحضور()
       {

        return clsالحضورData.GetAllالحضور();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالحضور())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالحضور();

            }
        
            return false;
        }



       public static bool Deleteالحضور(int معرّف_الحضور)
       {

        return clsالحضورData.Deleteالحضور(معرّف_الحضور);

       }


        public enum الحضورColumn
         {
            معرّف_الحضور,
            معرّف_الطالب,
            معرّف_الفصل,
            تاريخ_الحضور,
            الحالة
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الحضورColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالحضورData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
