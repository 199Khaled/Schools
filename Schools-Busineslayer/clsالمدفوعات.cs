
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالمدفوعات
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_المدفوعات { get; set; }
        public int? معرّف_الطالب { get; set; }
        public clsالطلاب الطلابInfo { get; set; }
        public decimal? المبلغ { get; set; }
        public DateTime? تاريخ_الدفع { get; set; }
        public string الحالة { get; set; } = null;


        public clsالمدفوعات()
        {
            this.معرّف_المدفوعات = null;
            this.معرّف_الطالب = 0;
            this.المبلغ = 0m;
            this.تاريخ_الدفع = DateTime.Now;
            this.الحالة = null;
            Mode = enMode.AddNew;
        }


        private clsالمدفوعات(
int? معرّف_المدفوعات, int? معرّف_الطالب, decimal? المبلغ, DateTime? تاريخ_الدفع, string الحالة = null)        {
            this.معرّف_المدفوعات = معرّف_المدفوعات;
            this.معرّف_الطالب = معرّف_الطالب;
            this.الطلابInfo = clsالطلاب.FindByمعرّف_الطالب(معرّف_الطالب);
            this.المبلغ = المبلغ;
            this.تاريخ_الدفع = تاريخ_الدفع;
            this.الحالة = الحالة;
            Mode = enMode.Update;
        }


       private bool _AddNewالمدفوعات()
       {
        this.معرّف_المدفوعات = clsالمدفوعاتData.AddNewالمدفوعات(
this.معرّف_الطالب, this.المبلغ, this.تاريخ_الدفع, this.الحالة);
        return (this.معرّف_المدفوعات != null);
       }


       public static bool AddNewالمدفوعات(
ref int? معرّف_المدفوعات, int? معرّف_الطالب, decimal? المبلغ, DateTime? تاريخ_الدفع, string الحالة = null)        {
        معرّف_المدفوعات = clsالمدفوعاتData.AddNewالمدفوعات(
معرّف_الطالب, المبلغ, تاريخ_الدفع, الحالة);

        return (معرّف_المدفوعات != null);

       }


       private bool _Updateالمدفوعات()
       {
        return clsالمدفوعاتData.UpdateالمدفوعاتByID(
this.معرّف_المدفوعات, this.معرّف_الطالب, this.المبلغ, this.تاريخ_الدفع, this.الحالة);
       }


       public static bool UpdateالمدفوعاتByID(
int? معرّف_المدفوعات, int? معرّف_الطالب, decimal? المبلغ, DateTime? تاريخ_الدفع, string الحالة = null)        {
        return clsالمدفوعاتData.UpdateالمدفوعاتByID(
معرّف_المدفوعات, معرّف_الطالب, المبلغ, تاريخ_الدفع, الحالة);

        }


       public static clsالمدفوعات FindByمعرّف_المدفوعات(int? معرّف_المدفوعات)

        {
            if (معرّف_المدفوعات == null)
            {
                return null;
            }
            int? معرّف_الطالب = 0;
            decimal? المبلغ = 0m;
            DateTime? تاريخ_الدفع = DateTime.Now;
            string الحالة = "";
            bool IsFound = clsالمدفوعاتData.GetالمدفوعاتInfoByID(معرّف_المدفوعات,
 ref معرّف_الطالب,  ref المبلغ,  ref تاريخ_الدفع,  ref الحالة);

           if (IsFound)
               return new clsالمدفوعات(
معرّف_المدفوعات, معرّف_الطالب, المبلغ, تاريخ_الدفع, الحالة);
            else
                return null;
            }


       public static DataTable GetAllالمدفوعات()
       {

        return clsالمدفوعاتData.GetAllالمدفوعات();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالمدفوعات())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالمدفوعات();

            }
        
            return false;
        }



       public static bool Deleteالمدفوعات(int معرّف_المدفوعات)
       {

        return clsالمدفوعاتData.Deleteالمدفوعات(معرّف_المدفوعات);

       }


        public enum المدفوعاتColumn
         {
            معرّف_المدفوعات,
            معرّف_الطالب,
            المبلغ,
            تاريخ_الدفع,
            الحالة
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(المدفوعاتColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالمدفوعاتData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
