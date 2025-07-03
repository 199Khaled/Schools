
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsSchedules
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? ScheduleID { get; set; }
        public int? TeacherID { get; set; }
        public clsEmployees TeachersInfo { get; set; }
        public int? ClassroomID { get; set; }
        public clsClassrooms ClassroomsInfo { get; set; }
        public int? SubjectID { get; set; }
        public clsSubjects SubjectsInfo { get; set; }
        public DateTime? Day { get; set; } = null;
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }


        public clsSchedules()
        {
            this.ScheduleID = null;
            this.TeacherID = 0;
            this.ClassroomID = 0;
            this.SubjectID = 0;
            this.StartTime = default(TimeSpan);
            this.EndTime = default(TimeSpan);
            Mode = enMode.AddNew;
        }


        private clsSchedules(
int? ScheduleID, int? TeacherID, int? ClassroomID, int? SubjectID, DateTime? Day,TimeSpan? StartTime, TimeSpan? EndTime)        {
            this.ScheduleID = ScheduleID;
            this.TeacherID = TeacherID;
            this.TeachersInfo = clsEmployees.FindByEmployeeID(TeacherID);
            this.ClassroomID = ClassroomID;
            this.ClassroomsInfo = clsClassrooms.FindByClassroomID(ClassroomID);
            this.SubjectID = SubjectID;
            this.SubjectsInfo = clsSubjects.FindBySubjectID(SubjectID);
            this.Day = Day;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            Mode = enMode.Update;
        }


       private bool _AddNewSchedules()
       {
        this.ScheduleID = clsSchedulesData.AddNewSchedules(
this.TeacherID, this.ClassroomID, this.SubjectID, this.Day, this.StartTime, this.EndTime);
        return (this.ScheduleID != null);
       }


       public static bool AddNewSchedules(
ref int? ScheduleID, int? TeacherID, int? ClassroomID, int? SubjectID, TimeSpan? StartTime, TimeSpan? EndTime, DateTime Day)        {
        ScheduleID = clsSchedulesData.AddNewSchedules(
TeacherID, ClassroomID, SubjectID, Day, StartTime, EndTime);

        return (ScheduleID != null);

       }


       private bool _UpdateSchedules()
       {
        return clsSchedulesData.UpdateSchedulesByID(
this.ScheduleID, this.TeacherID, this.ClassroomID, this.SubjectID, this.Day, this.StartTime, this.EndTime);
       }


       public static bool UpdateSchedulesByID(
int? ScheduleID, int? TeacherID, int? ClassroomID, int? SubjectID, DateTime? Day, TimeSpan? StartTime, TimeSpan? EndTime)        {
        return clsSchedulesData.UpdateSchedulesByID(
ScheduleID, TeacherID, ClassroomID, SubjectID, Day, StartTime, EndTime);

        }


       public static clsSchedules FindByScheduleID(int? ScheduleID)

        {
            if (ScheduleID == null)
            {
                return null;
            }
            int? TeacherID = 0;
            int? ClassroomID = 0;
            int? SubjectID = 0;
            DateTime Day = DateTime.Now;
            TimeSpan? StartTime = default(TimeSpan);
            TimeSpan? EndTime = default(TimeSpan);
            bool IsFound = clsSchedulesData.GetSchedulesInfoByID(ScheduleID,
 ref TeacherID,  ref ClassroomID,  ref SubjectID,  ref Day,  ref StartTime,  ref EndTime);

           if (IsFound)
               return new clsSchedules(
ScheduleID, TeacherID, ClassroomID, SubjectID, Day, StartTime, EndTime);
            else
                return null;
            }


       public static DataTable GetAllSchedules()
       {

        return clsSchedulesData.GetAllSchedules();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewSchedules())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateSchedules();

            }
        
            return false;
        }



       public static bool DeleteSchedules(int ScheduleID)
       {

        return clsSchedulesData.DeleteSchedules(ScheduleID);

       }


        public enum SchedulesColumn
         {
            ScheduleID,
            TeacherID,
            ClassroomID,
            SubjectID,
            Day,
            StartTime,
            EndTime
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(SchedulesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsSchedulesData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
