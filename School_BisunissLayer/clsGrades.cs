
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsGrades
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? GradeID { get; set; }
        public int? StudentID { get; set; }
        public clsStudents StudentsInfo { get; set; }
        public int? ClassSubjectID { get; set; }
        public clsClassSubjects ClassSubjectsInfo { get; set; }
        public decimal? Grade { get; set; }
        public DateTime? GradeDate { get; set; }


        public clsGrades()
        {
            this.GradeID = null;
            this.StudentID = 0;
            this.ClassSubjectID = 0;
            this.Grade = 0m;
            this.GradeDate = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsGrades(
int? GradeID, int? StudentID, int? ClassSubjectID, decimal? Grade, DateTime? GradeDate)        {
            this.GradeID = GradeID;
            this.StudentID = StudentID;
            this.StudentsInfo = clsStudents.FindByStudentID(StudentID);
            this.ClassSubjectID = ClassSubjectID;
            this.ClassSubjectsInfo = clsClassSubjects.FindByClassSubjectID(ClassSubjectID);
            this.Grade = Grade;
            this.GradeDate = GradeDate;
            Mode = enMode.Update;
        }


       private bool _AddNewGrades()
       {
        this.GradeID = clsGradesData.AddNewGrades(
this.StudentID, this.ClassSubjectID, this.Grade, this.GradeDate);
        return (this.GradeID != null);
       }


       public static bool AddNewGrades(
ref int? GradeID, int? StudentID, int? ClassSubjectID, decimal? Grade, DateTime? GradeDate)        {
        GradeID = clsGradesData.AddNewGrades(
StudentID, ClassSubjectID, Grade, GradeDate);

        return (GradeID != null);

       }


       private bool _UpdateGrades()
       {
        return clsGradesData.UpdateGradesByID(
this.GradeID, this.StudentID, this.ClassSubjectID, this.Grade, this.GradeDate);
       }


       public static bool UpdateGradesByID(
int? GradeID, int? StudentID, int? ClassSubjectID, decimal? Grade, DateTime? GradeDate)        {
        return clsGradesData.UpdateGradesByID(
GradeID, StudentID, ClassSubjectID, Grade, GradeDate);

        }


       public static clsGrades FindByGradeID(int? GradeID)

        {
            if (GradeID == null)
            {
                return null;
            }
            int? StudentID = 0;
            int? ClassSubjectID = 0;
            decimal? Grade = 0m;
            DateTime? GradeDate = DateTime.Now;
            bool IsFound = clsGradesData.GetGradesInfoByID(GradeID,
 ref StudentID,  ref ClassSubjectID,  ref Grade,  ref GradeDate);

           if (IsFound)
               return new clsGrades(
GradeID, StudentID, ClassSubjectID, Grade, GradeDate);
            else
                return null;
            }


       public static DataTable GetAllGrades()
       {

        return clsGradesData.GetAllGrades();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewGrades())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateGrades();

            }
        
            return false;
        }



       public static bool DeleteGrades(int GradeID)
       {

        return clsGradesData.DeleteGrades(GradeID);

       }


        public enum GradesColumn
         {
            GradeID,
            StudentID,
            ClassSubjectID,
            Grade,
            GradeDate
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(GradesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsGradesData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
