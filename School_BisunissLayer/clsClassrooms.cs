
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsClassrooms
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? ClassroomID { get; set; }
        public string ClassroomName { get; set; } = null;
        public int? Capacity { get; set; }
        public string Building { get; set; } = null;


        public clsClassrooms()
        {
            this.ClassroomID = null;
            this.ClassroomName = null;
            this.Capacity = 0;
            this.Building = null;
            Mode = enMode.AddNew;
        }


        private clsClassrooms(int? ClassroomID, int? Capacity, string ClassroomName, string Building = null)        {
            this.ClassroomID = ClassroomID;
            this.ClassroomName = ClassroomName;
            this.Capacity = Capacity;
            this.Building = Building;
            Mode = enMode.Update;
        }


       private bool _AddNewClassrooms()
       {
        this.ClassroomID = clsClassroomsData.AddNewClassrooms(this.Capacity, this.ClassroomName, this.Building);
        return (this.ClassroomID != null);
       }


       public static bool AddNewClassrooms(ref int? ClassroomID,  string ClassroomName, int? Capacity, string Building = null)        {
        ClassroomID = clsClassroomsData.AddNewClassrooms(Capacity, ClassroomName, Building);

        return (ClassroomID != null);

       }


       private bool _UpdateClassrooms()
       {
        return clsClassroomsData.UpdateClassroomsByID(
this.ClassroomID, this.Capacity ,this.ClassroomName, this.Building);
       }


       public static bool UpdateClassroomsByID(
int? ClassroomID, int? Capacity, string ClassroomName, string Building = null)        {
        return clsClassroomsData.UpdateClassroomsByID(
ClassroomID,Capacity, ClassroomName, Building);

        }


       public static clsClassrooms FindByClassroomID(int? ClassroomID)

        {
            if (ClassroomID == null)
            {
                return null;
            }
            string ClassroomName = "";
            int? Capacity = 0;
            string Building = "";
            bool IsFound = clsClassroomsData.GetClassroomsInfoByID(ClassroomID,
 ref ClassroomName,  ref Capacity,  ref Building);

           if (IsFound)
               return new clsClassrooms(
ClassroomID, Capacity, ClassroomName, Building);
            else
                return null;
            }


       public static DataTable GetAllClassrooms()
       {

        return clsClassroomsData.GetAllClassrooms();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewClassrooms())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateClassrooms();

            }
        
            return false;
        }



       public static bool DeleteClassrooms(int ClassroomID)
       {

        return clsClassroomsData.DeleteClassrooms(ClassroomID);

       }


        public enum ClassroomsColumn
         {
            ClassroomID,
            ClassroomName,
            Capacity,
            Building
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(ClassroomsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsClassroomsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
