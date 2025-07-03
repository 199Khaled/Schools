
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsStudentExamResults
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? ResultID { get; set; }
        public int? StudentID { get; set; }
        public clsStudents StudentsInfo { get; set; }
        public int? ExamID { get; set; }
        public clsExams ExamsInfo { get; set; }
        public decimal? Score { get; set; }


        public clsStudentExamResults()
        {
            this.ResultID = null;
            this.StudentID = 0;
            this.ExamID = 0;
            this.Score = 0m;
            Mode = enMode.AddNew;
        }


        private clsStudentExamResults(
int? ResultID, int? StudentID, int? ExamID, decimal? Score)        {
            this.ResultID = ResultID;
            this.StudentID = StudentID;
            this.StudentsInfo = clsStudents.FindByStudentID(StudentID);
            this.ExamID = ExamID;
            this.ExamsInfo = clsExams.FindByExamID(ExamID);
            this.Score = Score;
            Mode = enMode.Update;
        }


       private bool _AddNewStudentExamResults()
       {
        this.ResultID = clsStudentExamResultsData.AddNewStudentExamResults(
this.StudentID, this.ExamID, this.Score);
        return (this.ResultID != null);
       }


       public static bool AddNewStudentExamResults(
ref int? ResultID, int? StudentID, int? ExamID, decimal? Score)        {
        ResultID = clsStudentExamResultsData.AddNewStudentExamResults(
StudentID, ExamID, Score);

        return (ResultID != null);

       }


       private bool _UpdateStudentExamResults()
       {
        return clsStudentExamResultsData.UpdateStudentExamResultsByID(
this.ResultID, this.StudentID, this.ExamID, this.Score);
       }


       public static bool UpdateStudentExamResultsByID(
int? ResultID, int? StudentID, int? ExamID, decimal? Score)        {
        return clsStudentExamResultsData.UpdateStudentExamResultsByID(
ResultID, StudentID, ExamID, Score);

        }


       public static clsStudentExamResults FindByResultID(int? ResultID)

        {
            if (ResultID == null)
            {
                return null;
            }
            int? StudentID = 0;
            int? ExamID = 0;
            decimal? Score = 0m;
            bool IsFound = clsStudentExamResultsData.GetStudentExamResultsInfoByID(ResultID,
 ref StudentID,  ref ExamID,  ref Score);

           if (IsFound)
               return new clsStudentExamResults(
ResultID, StudentID, ExamID, Score);
            else
                return null;
            }


       public static DataTable GetAllStudentExamResults()
       {

        return clsStudentExamResultsData.GetAllStudentExamResults();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewStudentExamResults())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateStudentExamResults();

            }
        
            return false;
        }



       public static bool DeleteStudentExamResults(int ResultID)
       {

        return clsStudentExamResultsData.DeleteStudentExamResults(ResultID);

       }


        public enum StudentExamResultsColumn
         {
            ResultID,
            StudentID,
            ExamID,
            Score
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(StudentExamResultsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsStudentExamResultsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
