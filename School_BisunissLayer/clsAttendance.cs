
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsAttendance
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? AttendanceID { get; set; }
        public int? StudentID { get; set; }
        public clsStudents StudentsInfo { get; set; }
        public int? ClassID { get; set; }
        public clsClasses ClassesInfo { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string Status { get; set; } = null;


        public clsAttendance()
        {
            this.AttendanceID = null;
            this.StudentID = 0;
            this.ClassID = 0;
            this.AttendanceDate = DateTime.Now;
            this.Status = null;
            Mode = enMode.AddNew;
        }


        private clsAttendance(
int? AttendanceID, int? StudentID, int? ClassID, DateTime? AttendanceDate, string Status = null)        {
            this.AttendanceID = AttendanceID;
            this.StudentID = StudentID;
            this.StudentsInfo = clsStudents.FindByStudentID(StudentID);
            this.ClassID = ClassID;
            this.ClassesInfo = clsClasses.FindByClassID(ClassID);
            this.AttendanceDate = AttendanceDate;
            this.Status = Status;
            Mode = enMode.Update;
        }


       private bool _AddNewAttendance()
       {
        this.AttendanceID = clsAttendanceData.AddNewAttendance(
this.StudentID, this.ClassID, this.AttendanceDate, this.Status);
        return (this.AttendanceID != null);
       }


       public static bool AddNewAttendance(
ref int? AttendanceID, int? StudentID, int? ClassID, DateTime? AttendanceDate, string Status = null)        {
        AttendanceID = clsAttendanceData.AddNewAttendance(
StudentID, ClassID, AttendanceDate, Status);

        return (AttendanceID != null);

       }


       private bool _UpdateAttendance()
       {
        return clsAttendanceData.UpdateAttendanceByID(
this.AttendanceID, this.StudentID, this.ClassID, this.AttendanceDate, this.Status);
       }


       public static bool UpdateAttendanceByID(
int? AttendanceID, int? StudentID, int? ClassID, DateTime? AttendanceDate, string Status = null)        {
        return clsAttendanceData.UpdateAttendanceByID(
AttendanceID, StudentID, ClassID, AttendanceDate, Status);

        }


       public static clsAttendance FindByAttendanceID(int? AttendanceID)

        {
            if (AttendanceID == null)
            {
                return null;
            }
            int? StudentID = 0;
            int? ClassID = 0;
            DateTime? AttendanceDate = DateTime.Now;
            string Status = "";
            bool IsFound = clsAttendanceData.GetAttendanceInfoByID(AttendanceID,
 ref StudentID,  ref ClassID,  ref AttendanceDate,  ref Status);

           if (IsFound)
               return new clsAttendance(
AttendanceID, StudentID, ClassID, AttendanceDate, Status);
            else
                return null;
            }


       public static DataTable GetAllAttendance()
       {

        return clsAttendanceData.GetAllAttendance();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewAttendance())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateAttendance();

            }
        
            return false;
        }



       public static bool DeleteAttendance(int AttendanceID)
       {

        return clsAttendanceData.DeleteAttendance(AttendanceID);

       }


        public enum AttendanceColumn
         {
            AttendanceID,
            StudentID,
            ClassID,
            AttendanceDate,
            Status
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(AttendanceColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsAttendanceData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
