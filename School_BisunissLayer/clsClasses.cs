
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsClasses
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? ClassID { get; set; }
        public string ClassName { get; set; }
        public int? GradeLevel { get; set; }


        public clsClasses()
        {
            this.ClassID = null;
            this.ClassName = "";
            this.GradeLevel = 0;
            Mode = enMode.AddNew;
        }


        private clsClasses(
int? ClassID, string ClassName, int? GradeLevel)        {
            this.ClassID = ClassID;
            this.ClassName = ClassName;
            this.GradeLevel = GradeLevel;
            Mode = enMode.Update;
        }


       private bool _AddNewClasses()
       {
        this.ClassID = clsClassesData.AddNewClasses(
this.ClassName, this.GradeLevel);
        return (this.ClassID != null);
       }


       public static bool AddNewClasses(
ref int? ClassID, string ClassName, int? GradeLevel)        {
        ClassID = clsClassesData.AddNewClasses(
ClassName, GradeLevel);

        return (ClassID != null);

       }


       private bool _UpdateClasses()
       {
        return clsClassesData.UpdateClassesByID(
this.ClassID, this.ClassName, this.GradeLevel);
       }


       public static bool UpdateClassesByID(
int? ClassID, string ClassName, int? GradeLevel)        {
        return clsClassesData.UpdateClassesByID(
ClassID, ClassName, GradeLevel);

        }


       public static clsClasses FindByClassID(int? ClassID)

        {
            if (ClassID == null)
            {
                return null;
            }
            string ClassName = "";
            int? GradeLevel = 0;
            bool IsFound = clsClassesData.GetClassesInfoByID(ClassID,
 ref ClassName,  ref GradeLevel);

           if (IsFound)
               return new clsClasses(
ClassID, ClassName, GradeLevel);
            else
                return null;
            }


       public static DataTable GetAllClasses()
       {

        return clsClassesData.GetAllClasses();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewClasses())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateClasses();

            }
        
            return false;
        }



       public static bool DeleteClasses(int ClassID)
       {

        return clsClassesData.DeleteClasses(ClassID);

       }


        public enum ClassesColumn
         {
            ClassID,
            ClassName,
            GradeLevel
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ClassesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsClassesData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
