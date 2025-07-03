
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsSubjects
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? SubjectID { get; set; }
        public string SubjectName { get; set; }


        public clsSubjects()
        {
            this.SubjectID = null;
            this.SubjectName = "";
            Mode = enMode.AddNew;
        }


        private clsSubjects(
int? SubjectID, string SubjectName)        {
            this.SubjectID = SubjectID;
            this.SubjectName = SubjectName;
            Mode = enMode.Update;
        }


       private bool _AddNewSubjects()
       {
        this.SubjectID = clsSubjectsData.AddNewSubjects(
this.SubjectName);
        return (this.SubjectID != null);
       }


       public static bool AddNewSubjects(
ref int? SubjectID, string SubjectName)        {
        SubjectID = clsSubjectsData.AddNewSubjects(
SubjectName);

        return (SubjectID != null);

       }


       private bool _UpdateSubjects()
       {
        return clsSubjectsData.UpdateSubjectsByID(
this.SubjectID, this.SubjectName);
       }


       public static bool UpdateSubjectsByID(
int? SubjectID, string SubjectName)        {
        return clsSubjectsData.UpdateSubjectsByID(
SubjectID, SubjectName);

        }


       public static clsSubjects FindBySubjectID(int? SubjectID)

        {
            if (SubjectID == null)
            {
                return null;
            }
            string SubjectName = "";
            bool IsFound = clsSubjectsData.GetSubjectsInfoByID(SubjectID,
 ref SubjectName);

           if (IsFound)
               return new clsSubjects(
SubjectID, SubjectName);
            else
                return null;
            }
        public static clsSubjects FindBySubjectName(string SubjectName)

        {
            if (string.IsNullOrEmpty(SubjectName))
            {
                return null;
            }
            int? SubjectID = null;
            bool IsFound = clsSubjectsData.GetSubjectsInfoByName(ref SubjectID, SubjectName);

            if (IsFound)
                return new clsSubjects(SubjectID, SubjectName);
            else
                return null;
        }


        public static DataTable GetAllSubjects()
       {

        return clsSubjectsData.GetAllSubjects();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewSubjects())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateSubjects();

            }
        
            return false;
        }



       public static bool DeleteSubjects(int SubjectID)
       {

        return clsSubjectsData.DeleteSubjects(SubjectID);

       }


        public enum SubjectsColumn
         {
            SubjectID,
            SubjectName
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(SubjectsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsSubjectsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
