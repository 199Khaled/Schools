
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsPersons
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? PersonID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Vollname
        {
            get { return Firstname + ' ' + Lastname; }
        }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; } = null;
        public string Phone { get; set; } = null;
        public string Email { get; set; } = null;


        public clsPersons()
        {
            this.PersonID = null;
            this.Firstname = "";
            this.Lastname = "";
            this.DateOfBirth = DateTime.Now;
            this.Gender = "";
            this.City = null;
            this.Phone = null;
            this.Email = null;
            Mode = enMode.AddNew;
        }


        private clsPersons(
int? PersonID, string Firstname, string Lastname, DateTime? DateOfBirth, string Gender, string City = null, string Phone = null, string Email = null)        {
            this.PersonID = PersonID;
            this.Firstname = Firstname;
            this.Lastname = Lastname;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.City = City;
            this.Phone = Phone;
            this.Email = Email;
            Mode = enMode.Update;
        }


       private bool _AddNewPersons()
       {
        this.PersonID = clsPersonsData.AddNewPersons(
this.Firstname, this.Lastname, this.DateOfBirth, this.Gender, this.City, this.Phone, this.Email);
        return (this.PersonID != null);
       }


       public static bool AddNewPersons(
ref int? PersonID, string Firstname, string Lastname, DateTime? DateOfBirth, string Gender, string City = null, string Phone = null, string Email = null)        {
        PersonID = clsPersonsData.AddNewPersons(
Firstname, Lastname, DateOfBirth, Gender, City, Phone, Email);

        return (PersonID != null);

       }


       private bool _UpdatePersons()
       {
        return clsPersonsData.UpdatePersonsByID(
this.PersonID, this.Firstname, this.Lastname, this.DateOfBirth, this.Gender, this.City, this.Phone, this.Email);
       }


       public static bool UpdatePersonsByID(
int? PersonID, string Firstname, string Lastname, DateTime? DateOfBirth, string Gender, string City = null, string Phone = null, string Email = null)        {
        return clsPersonsData.UpdatePersonsByID(
PersonID, Firstname, Lastname, DateOfBirth, Gender, City, Phone, Email);

        }


       public static clsPersons FindByPersonID(int? PersonID)

        {
            if (PersonID == null)
            {
                return null;
            }
            string Firstname = "";
            string Lastname = "";
            DateTime? DateOfBirth = DateTime.Now;
            string Gender = "";
            string City = "";
            string Phone = "";
            string Email = "";
            bool IsFound = clsPersonsData.GetPersonsInfoByID(PersonID,
 ref Firstname,  ref Lastname,  ref DateOfBirth,  ref Gender,  ref City,  ref Phone,  ref Email);

           if (IsFound)
               return new clsPersons(
PersonID, Firstname, Lastname, DateOfBirth, Gender, City, Phone, Email);
            else
                return null;
            }


       public static DataTable GetAllPersons()
       {

        return clsPersonsData.GetAllPersons();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPersons())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePersons();

            }
        
            return false;
        }



       public static bool DeletePersons(int? PersonID)
       {

        return clsPersonsData.DeletePersons(PersonID);

       }


        public enum PersonsColumn
         {
            PersonID,
            Firstname,
            Lastname,
            DateOfBirth,
            Gender,
            City,
            Phone,
            Email
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(PersonsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsPersonsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
