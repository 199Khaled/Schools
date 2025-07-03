
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsTeacherSubjects
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? TeacherSubjectID { get; set; }
        public int? TeacherID { get; set; }
        public clsEmployees TeachersInfo { get; set; }
        public int? SubjectID { get; set; }
        public clsSubjects SubjectsInfo { get; set; }


        public clsTeacherSubjects()
        {
            this.TeacherSubjectID = null;
            this.TeacherID = 0;
            this.SubjectID = 0;
            Mode = enMode.AddNew;
        }


        private clsTeacherSubjects(
int? TeacherSubjectID, int? TeacherID, int? SubjectID)        {
            this.TeacherSubjectID = TeacherSubjectID;
            this.TeacherID = TeacherID;
            this.TeachersInfo = clsEmployees.FindByEmployeeID(TeacherID);
            this.SubjectID = SubjectID;
            this.SubjectsInfo = clsSubjects.FindBySubjectID(SubjectID);
            Mode = enMode.Update;
        }


       private bool _AddNewTeacherSubjects()
       {
        this.TeacherSubjectID = clsTeacherSubjectsData.AddNewTeacherSubjects(
this.TeacherID, this.SubjectID);
        return (this.TeacherSubjectID != null);
       }


       public static bool AddNewTeacherSubjects(
ref int? TeacherSubjectID, int? TeacherID, int? SubjectID)        {
        TeacherSubjectID = clsTeacherSubjectsData.AddNewTeacherSubjects(
TeacherID, SubjectID);

        return (TeacherSubjectID != null);

       }


       private bool _UpdateTeacherSubjects()
       {
        return clsTeacherSubjectsData.UpdateTeacherSubjectsByID(
this.TeacherSubjectID, this.TeacherID, this.SubjectID);
       }


       public static bool UpdateTeacherSubjectsByID(
int? TeacherSubjectID, int? TeacherID, int? SubjectID)        {
        return clsTeacherSubjectsData.UpdateTeacherSubjectsByID(
TeacherSubjectID, TeacherID, SubjectID);

        }


       public static clsTeacherSubjects FindByTeacherSubjectID(int? TeacherSubjectID)

        {
            if (TeacherSubjectID == null)
            {
                return null;
            }
            int? TeacherID = 0;
            int? SubjectID = 0;
            bool IsFound = clsTeacherSubjectsData.GetTeacherSubjectsInfoByID(TeacherSubjectID,
 ref TeacherID,  ref SubjectID);

           if (IsFound)
               return new clsTeacherSubjects(
TeacherSubjectID, TeacherID, SubjectID);
            else
                return null;
            }


       public static DataTable GetAllTeacherSubjects()
       {

        return clsTeacherSubjectsData.GetAllTeacherSubjects();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTeacherSubjects())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTeacherSubjects();

            }
        
            return false;
        }



       public static bool DeleteTeacherSubjects(int TeacherSubjectID)
       {

        return clsTeacherSubjectsData.DeleteTeacherSubjects(TeacherSubjectID);

       }


        public enum TeacherSubjectsColumn
         {
            TeacherSubjectID,
            TeacherID,
            SubjectID
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(TeacherSubjectsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsTeacherSubjectsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
