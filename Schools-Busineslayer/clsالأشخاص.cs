
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالأشخاص
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الشخص { get; set; }
        public string الاسم_الأول { get; set; }
        public string اسم_الأب { get; set; } = null;
        public string اسم_الأم { get; set; } = null;
        public string اسم_العائلة { get; set; }
        public DateTime? تاريخ_الميلاد { get; set; }
        public string الجنس { get; set; } = null;
        public string المدينة { get; set; } = null;
        public string الهاتف { get; set; } = null;
        public string البريد_الإلكتروني { get; set; } = null;


        public clsالأشخاص()
        {
            this.معرّف_الشخص = null;
            this.الاسم_الأول = "";
            this.اسم_الأب = null;
            this.اسم_الأم = null;
            this.اسم_العائلة = "";
            this.تاريخ_الميلاد = DateTime.Now;
            this.الجنس = null;
            this.المدينة = null;
            this.الهاتف = null;
            this.البريد_الإلكتروني = null;
            Mode = enMode.AddNew;
        }


        private clsالأشخاص(
int? معرّف_الشخص, string الاسم_الأول, string اسم_العائلة, DateTime? تاريخ_الميلاد, string اسم_الأب = null, string اسم_الأم = null, string الجنس = null, string المدينة = null, string الهاتف = null, string البريد_الإلكتروني = null)        {
            this.معرّف_الشخص = معرّف_الشخص;
            this.الاسم_الأول = الاسم_الأول;
            this.اسم_الأب = اسم_الأب;
            this.اسم_الأم = اسم_الأم;
            this.اسم_العائلة = اسم_العائلة;
            this.تاريخ_الميلاد = تاريخ_الميلاد;
            this.الجنس = الجنس;
            this.المدينة = المدينة;
            this.الهاتف = الهاتف;
            this.البريد_الإلكتروني = البريد_الإلكتروني;
            Mode = enMode.Update;
        }


       private bool _AddNewالأشخاص()
       {
        this.معرّف_الشخص = clsالأشخاصData.AddNewالأشخاص(
this.الاسم_الأول, this.اسم_العائلة, this.اسم_الأب, this.اسم_الأم, this.تاريخ_الميلاد, this.الجنس, this.المدينة, this.الهاتف, this.البريد_الإلكتروني);
        return (this.معرّف_الشخص != null);
       }


       public static bool AddNewالأشخاص(
ref int? معرّف_الشخص, string الاسم_الأول, string اسم_العائلة, DateTime? تاريخ_الميلاد, string اسم_الأب = null, string اسم_الأم = null, string الجنس = null, string المدينة = null, string الهاتف = null, string البريد_الإلكتروني = null)        {
        معرّف_الشخص = clsالأشخاصData.AddNewالأشخاص(
الاسم_الأول, اسم_العائلة, اسم_الأب, اسم_الأم, تاريخ_الميلاد, الجنس, المدينة, الهاتف, البريد_الإلكتروني);

        return (معرّف_الشخص != null);

       }


       private bool _Updateالأشخاص()
       {
        return clsالأشخاصData.UpdateالأشخاصByID(
this.معرّف_الشخص, this.الاسم_الأول, this.اسم_العائلة, this.اسم_الأب, this.اسم_الأم, this.تاريخ_الميلاد, this.الجنس, this.المدينة, this.الهاتف, this.البريد_الإلكتروني);
       }


       public static bool UpdateالأشخاصByID(
int? معرّف_الشخص, string الاسم_الأول, string اسم_العائلة, DateTime? تاريخ_الميلاد, string اسم_الأب = null, string اسم_الأم = null, string الجنس = null, string المدينة = null, string الهاتف = null, string البريد_الإلكتروني = null)        {
        return clsالأشخاصData.UpdateالأشخاصByID(
معرّف_الشخص, الاسم_الأول, اسم_العائلة, اسم_الأب, اسم_الأم, تاريخ_الميلاد, الجنس, المدينة, الهاتف, البريد_الإلكتروني);

        }


       public static clsالأشخاص FindByمعرّف_الشخص(int? معرّف_الشخص)

        {
            if (معرّف_الشخص == null)
            {
                return null;
            }
            string الاسم_الأول = "";
            string اسم_الأب = "";
            string اسم_الأم = "";
            string اسم_العائلة = "";
            DateTime? تاريخ_الميلاد = DateTime.Now;
            string الجنس = "";
            string المدينة = "";
            string الهاتف = "";
            string البريد_الإلكتروني = "";
            bool IsFound = clsالأشخاصData.GetالأشخاصInfoByID(معرّف_الشخص,
 ref الاسم_الأول,  ref اسم_الأب,  ref اسم_الأم,  ref اسم_العائلة,  ref تاريخ_الميلاد,  ref الجنس,  ref المدينة,  ref الهاتف,  ref البريد_الإلكتروني);

           if (IsFound)
               return new clsالأشخاص(
معرّف_الشخص, الاسم_الأول, اسم_العائلة, اسم_الأب, اسم_الأم, تاريخ_الميلاد, الجنس, المدينة, الهاتف, البريد_الإلكتروني);
            else
                return null;
            }


       public static DataTable GetAllالأشخاص()
       {

        return clsالأشخاصData.GetAllالأشخاص();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالأشخاص())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالأشخاص();

            }
        
            return false;
        }



       public static bool Deleteالأشخاص(int معرّف_الشخص)
       {

        return clsالأشخاصData.Deleteالأشخاص(معرّف_الشخص);

       }


        public enum الأشخاصColumn
         {
            معرّف_الشخص,
            الاسم_الأول,
            اسم_الأب,
            اسم_الأم,
            اسم_العائلة,
            تاريخ_الميلاد,
            الجنس,
            المدينة,
            الهاتف,
            البريد_الإلكتروني
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الأشخاصColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالأشخاصData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
