
using System;
using System.Data;
using System.Runtime.InteropServices;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsEmployees
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? EmployeeID { get; set; }
        public int? PersonID { get; set; }
        public clsPersons PersonsInfo { get; set; }
        public string Typ { get; set; }
        public DateTime? HireDate { get; set; }
        public string FireDate { get; set; } = null;
        public bool? isAktive { get; set; }

        public enum enPersonType
        {
            ITAdministrator,
            SchoolAssistantOrTeachingAssistant,
            SupportStaffOrSupervisoryStaff,
            Librarian,
            SchoolSocialWorker,
            CaretakerOrJanitor,
            Secretary,
            Student,
            Principal,
            Teacher
        }
         public enPersonType personTyp;
        public clsEmployees()
        {
            this.EmployeeID = null;
            this.PersonID = 0;
            this.Typ = personTyp.ToString();//Save Enum-Wert as String 
            this.HireDate = DateTime.Now;
            Mode = enMode.AddNew;
        }


        private clsEmployees(int? EmployeeID, int? PersonID, string Typ, DateTime? HireDate, string FireDate, bool ? isAktive)       
        {
            this.EmployeeID = EmployeeID;
            this.PersonID = PersonID;
            this.PersonsInfo = clsPersons.FindByPersonID(PersonID); 
            this.Typ = Typ;
            this.HireDate = HireDate;
            this.FireDate = FireDate;
            this.isAktive = isAktive;
            Mode = enMode.Update;
        }


       private bool _AddNewEmployees()
       {
        this.EmployeeID = clsEmployeesData.AddNewEmployees(this.PersonID, this.Typ);
        return (this.EmployeeID != null);
       }


       public static bool AddNewEmployees(ref int? EmployeeID, int? PersonID, enPersonType personTyp)        
        {
        EmployeeID = clsEmployeesData.AddNewEmployees(PersonID, personTyp.ToString());

        return (EmployeeID != null);

       }


       private bool _UpdateEmployees()
       {
        return clsEmployeesData.UpdateEmployeesByID(
this.EmployeeID, this.PersonID, this.Typ, this.HireDate, this.FireDate, this.isAktive);
       }


       public static bool UpdateEmployeesByID(
int? EmployeeID, int? PersonID, string Typ, DateTime? HireDate, bool? isAktive, string FireDate = null)        {
        return clsEmployeesData.UpdateEmployeesByID(
EmployeeID, PersonID, Typ, HireDate, FireDate, isAktive);

        }


       public static clsEmployees FindByEmployeeID(int? EmployeeID)

        {
            if (EmployeeID == null)
            {
                return null;
            }
            int? PersonID = 0;
            string Typ = "";
            DateTime? HireDate = DateTime.Now;
            string FireDate = "";
            bool? isAktive = false;
            bool IsFound = clsEmployeesData.GetEmployeesInfoByID(EmployeeID,
 ref PersonID,  ref Typ,  ref HireDate,  ref FireDate,  ref isAktive);

           if (IsFound)
               return new clsEmployees(
EmployeeID, PersonID, Typ, HireDate, FireDate, isAktive);
            else
                return null;
            }


       public static DataTable GetAllEmployees()
       {

        return clsEmployeesData.GetAllEmployees();

       }

       public static DataTable GetAllTeachers()
        {
            return clsEmployeesData.GetAllTeachers();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewEmployees())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateEmployees();

            }
        
            return false;
        }



        public static bool DeleteEmployees(int EmployeeID, int? PersonID)
        {
            // First, delete the specific employee
            bool IsDeletedSuccessfully = clsEmployeesData.DeleteEmployees(EmployeeID);
            if (!IsDeletedSuccessfully)
                return false;

            // Then, delete the associated person
            return clsPersons.DeletePersons(PersonID);
        }


        public enum EmployeesColumn
         {
            EmployeeID,
            PersonID,
            Typ,
            HireDate,
            FireDate,
            isAktive
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(EmployeesColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsEmployeesData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
