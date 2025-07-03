
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsClassSubjects
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? ClassSubjectID { get; set; }
        public int? ClassID { get; set; }
        public clsClasses ClassesInfo { get; set; }
        public int? SubjectID { get; set; }
        public clsSubjects SubjectsInfo { get; set; }
        public int? TeacherID { get; set; }
        public clsEmployees TeachersInfo { get; set; }


        public clsClassSubjects()
        {
            this.ClassSubjectID = null;
            this.ClassID = 0;
            this.SubjectID = 0;
            this.TeacherID = 0;
            Mode = enMode.AddNew;
        }


        private clsClassSubjects(
int? ClassSubjectID, int? ClassID, int? SubjectID, int? TeacherID)        {
            this.ClassSubjectID = ClassSubjectID;
            this.ClassID = ClassID;
            this.ClassesInfo = clsClasses.FindByClassID(ClassID);
            this.SubjectID = SubjectID;
            this.SubjectsInfo = clsSubjects.FindBySubjectID(SubjectID);
            this.TeacherID = TeacherID;
            this.TeachersInfo = clsEmployees.FindByEmployeeID(TeacherID);
            Mode = enMode.Update;
        }


       private bool _AddNewClassSubjects()
       {
        this.ClassSubjectID = clsClassSubjectsData.AddNewClassSubjects(
this.ClassID, this.SubjectID, this.TeacherID);
        return (this.ClassSubjectID != null);
       }


       public static bool AddNewClassSubjects(
ref int? ClassSubjectID, int? ClassID, int? SubjectID, int? TeacherID)        {
        ClassSubjectID = clsClassSubjectsData.AddNewClassSubjects(
ClassID, SubjectID, TeacherID);

        return (ClassSubjectID != null);

       }


       private bool _UpdateClassSubjects()
       {
        return clsClassSubjectsData.UpdateClassSubjectsByID(
this.ClassSubjectID, this.ClassID, this.SubjectID, this.TeacherID);
       }


       public static bool UpdateClassSubjectsByID(
int? ClassSubjectID, int? ClassID, int? SubjectID, int? TeacherID)        {
        return clsClassSubjectsData.UpdateClassSubjectsByID(
ClassSubjectID, ClassID, SubjectID, TeacherID);

        }


       public static clsClassSubjects FindByClassSubjectID(int? ClassSubjectID)

        {
            if (ClassSubjectID == null)
            {
                return null;
            }
            int? ClassID = 0;
            int? SubjectID = 0;
            int? TeacherID = 0;
            bool IsFound = clsClassSubjectsData.GetClassSubjectsInfoByID(ClassSubjectID,
 ref ClassID,  ref SubjectID,  ref TeacherID);

           if (IsFound)
               return new clsClassSubjects(
ClassSubjectID, ClassID, SubjectID, TeacherID);
            else
                return null;
            }


       public static DataTable GetAllClassSubjects()
       {

        return clsClassSubjectsData.GetAllClassSubjects();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewClassSubjects())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateClassSubjects();

            }
        
            return false;
        }



       public static bool DeleteClassSubjects(int ClassSubjectID)
       {

        return clsClassSubjectsData.DeleteClassSubjects(ClassSubjectID);

       }


        public enum ClassSubjectsColumn
         {
            ClassSubjectID,
            ClassID,
            SubjectID,
            TeacherID
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ClassSubjectsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsClassSubjectsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
