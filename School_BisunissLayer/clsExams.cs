
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsExams
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? ExamID { get; set; }
        public int? SubjectID { get; set; }
        public clsSubjects SubjectsInfo { get; set; }
        public int? ClassID { get; set; }
        public clsClasses ClassesInfo { get; set; }
        public DateTime? ExamDate { get; set; }


        public clsExams()
        {
            this.ExamID = null;
            this.SubjectID = 0;
            this.ClassID = 0;
            this.ExamDate = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsExams(
int? ExamID, int? SubjectID, int? ClassID, DateTime? ExamDate)        {
            this.ExamID = ExamID;
            this.SubjectID = SubjectID;
            this.SubjectsInfo = clsSubjects.FindBySubjectID(SubjectID);
            this.ClassID = ClassID;
            this.ClassesInfo = clsClasses.FindByClassID(ClassID);
            this.ExamDate = ExamDate;
            Mode = enMode.Update;
        }


       private bool _AddNewExams()
       {
        this.ExamID = clsExamsData.AddNewExams(
this.SubjectID, this.ClassID, this.ExamDate);
        return (this.ExamID != null);
       }


       public static bool AddNewExams(
ref int? ExamID, int? SubjectID, int? ClassID, DateTime? ExamDate)        {
        ExamID = clsExamsData.AddNewExams(
SubjectID, ClassID, ExamDate);

        return (ExamID != null);

       }


       private bool _UpdateExams()
       {
        return clsExamsData.UpdateExamsByID(
this.ExamID, this.SubjectID, this.ClassID, this.ExamDate);
       }


       public static bool UpdateExamsByID(
int? ExamID, int? SubjectID, int? ClassID, DateTime? ExamDate)        {
        return clsExamsData.UpdateExamsByID(
ExamID, SubjectID, ClassID, ExamDate);

        }


       public static clsExams FindByExamID(int? ExamID)

        {
            if (ExamID == null)
            {
                return null;
            }
            int? SubjectID = 0;
            int? ClassID = 0;
            DateTime? ExamDate = DateTime.Now;
            bool IsFound = clsExamsData.GetExamsInfoByID(ExamID,
 ref SubjectID,  ref ClassID,  ref ExamDate);

           if (IsFound)
               return new clsExams(
ExamID, SubjectID, ClassID, ExamDate);
            else
                return null;
            }


       public static DataTable GetAllExams()
       {

        return clsExamsData.GetAllExams();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewExams())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateExams();

            }
        
            return false;
        }



       public static bool DeleteExams(int ExamID)
       {

        return clsExamsData.DeleteExams(ExamID);

       }


        public enum ExamsColumn
         {
            ExamID,
            SubjectID,
            ClassID,
            ExamDate
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ExamsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsExamsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
