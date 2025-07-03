
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsEnrollments
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? EnrollmentID { get; set; }
        public int? StudentID { get; set; }
        public clsStudents StudentsInfo { get; set; }
        public int? ClassID { get; set; }
        public clsClasses ClassesInfo { get; set; }
        public DateTime? EnrollmentDate { get; set; }


        public clsEnrollments()
        {
            this.EnrollmentID = null;
            this.StudentID = 0;
            this.ClassID = 0;
            this.EnrollmentDate = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsEnrollments(
int? EnrollmentID, int? StudentID, int? ClassID, DateTime? EnrollmentDate)        {
            this.EnrollmentID = EnrollmentID;
            this.StudentID = StudentID;
            this.StudentsInfo = clsStudents.FindByStudentID(StudentID);
            this.ClassID = ClassID;
            this.ClassesInfo = clsClasses.FindByClassID(ClassID);
            this.EnrollmentDate = EnrollmentDate;
            Mode = enMode.Update;
        }


       private bool _AddNewEnrollments()
       {
        this.EnrollmentID = clsEnrollmentsData.AddNewEnrollments(
this.StudentID, this.ClassID, this.EnrollmentDate);
        return (this.EnrollmentID != null);
       }


       public static bool AddNewEnrollments(
ref int? EnrollmentID, int? StudentID, int? ClassID, DateTime? EnrollmentDate)        {
        EnrollmentID = clsEnrollmentsData.AddNewEnrollments(
StudentID, ClassID, EnrollmentDate);

        return (EnrollmentID != null);

       }


       private bool _UpdateEnrollments()
       {
        return clsEnrollmentsData.UpdateEnrollmentsByID(
this.EnrollmentID, this.StudentID, this.ClassID, this.EnrollmentDate);
       }


       public static bool UpdateEnrollmentsByID(
int? EnrollmentID, int? StudentID, int? ClassID, DateTime? EnrollmentDate)        {
        return clsEnrollmentsData.UpdateEnrollmentsByID(
EnrollmentID, StudentID, ClassID, EnrollmentDate);

        }


       public static clsEnrollments FindByEnrollmentID(int? EnrollmentID)

        {
            if (EnrollmentID == null)
            {
                return null;
            }
            int? StudentID = 0;
            int? ClassID = 0;
            DateTime? EnrollmentDate = DateTime.Now;
            bool IsFound = clsEnrollmentsData.GetEnrollmentsInfoByID(EnrollmentID,
 ref StudentID,  ref ClassID,  ref EnrollmentDate);

           if (IsFound)
               return new clsEnrollments(
EnrollmentID, StudentID, ClassID, EnrollmentDate);
            else
                return null;
            }


       public static DataTable GetAllEnrollments()
       {

        return clsEnrollmentsData.GetAllEnrollments();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewEnrollments())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateEnrollments();

            }
        
            return false;
        }



       public static bool DeleteEnrollments(int EnrollmentID)
       {

        return clsEnrollmentsData.DeleteEnrollments(EnrollmentID);

       }


        public enum EnrollmentsColumn
         {
            EnrollmentID,
            StudentID,
            ClassID,
            EnrollmentDate
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(EnrollmentsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsEnrollmentsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
