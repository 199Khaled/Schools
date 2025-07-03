
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsStudents
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? StudentID { get; set; }
        public int? PersonID { get; set; }
        public clsPersons PersonsInfo { get; set; }
        public DateTime? EnrollmentDate { get; set; }


        public clsStudents()
        {
            this.StudentID = null;
            this.PersonID = 0;
            this.EnrollmentDate = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsStudents(
int? StudentID, int? PersonID, DateTime? EnrollmentDate)        {
            this.StudentID = StudentID;
            this.PersonID = PersonID;
            this.PersonsInfo = clsPersons.FindByPersonID(PersonID);
            this.EnrollmentDate = EnrollmentDate;
            Mode = enMode.Update;
        }


       private bool _AddNewStudents()
       {
        this.StudentID = clsStudentsData.AddNewStudents(
this.PersonID);
        return (this.StudentID != null);
       }


       public static bool AddNewStudents(
ref int? StudentID, int? PersonID)        {
        StudentID = clsStudentsData.AddNewStudents(PersonID);

        return (StudentID != null);

       }


       private bool _UpdateStudents()
       {
        return clsStudentsData.UpdateStudentsByID(
this.StudentID, this.PersonID, this.EnrollmentDate);
       }


       public static bool UpdateStudentsByID(
int? StudentID, int? PersonID, DateTime? EnrollmentDate)        {
        return clsStudentsData.UpdateStudentsByID(
StudentID, PersonID, EnrollmentDate);

        }


       public static clsStudents FindByStudentID(int? StudentID)

        {
            if (StudentID == null)
            {
                return null;
            }
            int? PersonID = 0;
            DateTime? EnrollmentDate = DateTime.Now;
            bool IsFound = clsStudentsData.GetStudentsInfoByID(StudentID,
 ref PersonID,  ref EnrollmentDate);

           if (IsFound)
               return new clsStudents(
StudentID, PersonID, EnrollmentDate);
            else
                return null;
            }


       public static DataTable GetAllStudents()
       {

        return clsStudentsData.GetAllStudents();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewStudents())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateStudents();

            }
        
            return false;
        }



       public static bool DeleteStudents(int? StudentID, int? personID)
       {
            //we delete first the specific Person
            bool studentDeletedSuccessfully =  clsStudentsData.DeleteStudents(StudentID);
            //in case the student was not deleted successfully
            if (!studentDeletedSuccessfully)
                return false;

           return clsPersons.DeletePersons(personID);

        }


        public enum StudentsColumn
         {
            StudentID,
            PersonID,
            EnrollmentDate
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(StudentsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsStudentsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
