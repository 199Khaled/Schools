
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsPayments
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? PaymentID { get; set; }
        public int? StudentID { get; set; }
        public clsStudents StudentsInfo { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Status { get; set; } = null;


        public clsPayments()
        {
            this.PaymentID = null;
            this.StudentID = 0;
            this.Amount = 0m;
            this.PaymentDate = DateTime.Now;
            this.Status = null;
            Mode = enMode.AddNew;
        }


        private clsPayments(
int? PaymentID, int? StudentID, decimal? Amount, DateTime? PaymentDate, string Status = null)        {
            this.PaymentID = PaymentID;
            this.StudentID = StudentID;
            this.StudentsInfo = clsStudents.FindByStudentID(StudentID);
            this.Amount = Amount;
            this.PaymentDate = PaymentDate;
            this.Status = Status;
            Mode = enMode.Update;
        }


       private bool _AddNewPayments()
       {
        this.PaymentID = clsPaymentsData.AddNewPayments(
this.StudentID, this.Amount, this.PaymentDate, this.Status);
        return (this.PaymentID != null);
       }


       public static bool AddNewPayments(
ref int? PaymentID, int? StudentID, decimal? Amount, DateTime? PaymentDate, string Status = null)        {
        PaymentID = clsPaymentsData.AddNewPayments(
StudentID, Amount, PaymentDate, Status);

        return (PaymentID != null);

       }


       private bool _UpdatePayments()
       {
        return clsPaymentsData.UpdatePaymentsByID(
this.PaymentID, this.StudentID, this.Amount, this.PaymentDate, this.Status);
       }


       public static bool UpdatePaymentsByID(
int? PaymentID, int? StudentID, decimal? Amount, DateTime? PaymentDate, string Status = null)        {
        return clsPaymentsData.UpdatePaymentsByID(
PaymentID, StudentID, Amount, PaymentDate, Status);

        }


       public static clsPayments FindByPaymentID(int? PaymentID)

        {
            if (PaymentID == null)
            {
                return null;
            }
            int? StudentID = 0;
            decimal? Amount = 0m;
            DateTime? PaymentDate = DateTime.Now;
            string Status = "";
            bool IsFound = clsPaymentsData.GetPaymentsInfoByID(PaymentID,
 ref StudentID,  ref Amount,  ref PaymentDate,  ref Status);

           if (IsFound)
               return new clsPayments(
PaymentID, StudentID, Amount, PaymentDate, Status);
            else
                return null;
            }


       public static DataTable GetAllPayments()
       {

        return clsPaymentsData.GetAllPayments();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPayments())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePayments();

            }
        
            return false;
        }



       public static bool DeletePayments(int PaymentID)
       {

        return clsPaymentsData.DeletePayments(PaymentID);

       }


        public enum PaymentsColumn
         {
            PaymentID,
            StudentID,
            Amount,
            PaymentDate,
            Status
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(PaymentsColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsPaymentsData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
