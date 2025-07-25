
using System;
using System.Data;
using System.Runtime.CompilerServices;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالطلاب
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الطالب { get; set; }
        public int? معرّف_الشخص { get; set; }
        public clsالأشخاص الأشخاصInfo { get; set; }
        public clsالطلاب()
        {
            this.معرّف_الطالب = null;
            this.معرّف_الشخص = 0;
            Mode = enMode.AddNew;
        }


        private clsالطلاب(
int? معرّف_الطالب, int? معرّف_الشخص)        {
            this.معرّف_الطالب = معرّف_الطالب;
            this.معرّف_الشخص = معرّف_الشخص;
            this.الأشخاصInfo = clsالأشخاص.FindByمعرّف_الشخص(معرّف_الشخص);
            Mode = enMode.Update;
        }


       private bool _AddNewالطلاب(int? معرّف_الشخص)
       {
        this.معرّف_الطالب = clsالطلابData.AddNewالطلاب(
         معرّف_الشخص);
        return (this.معرّف_الطالب != null);
       }


       public static bool AddNewالطلاب(
ref int? معرّف_الطالب, int? معرّف_الشخص)        {
        معرّف_الطالب = clsالطلابData.AddNewالطلاب(
معرّف_الشخص);

        return (معرّف_الطالب != null);

       }


       private bool _Updateالطلاب()
       {
        return clsالطلابData.UpdateالطلابByID(
this.معرّف_الطالب, this.معرّف_الشخص);
       }


       public static bool UpdateالطلابByID(
int? معرّف_الطالب, int? معرّف_الشخص )        {
        return clsالطلابData.UpdateالطلابByID(
معرّف_الطالب, معرّف_الشخص);

        }


       public static clsالطلاب FindByمعرّف_الطالب(int? معرّف_الطالب)

        {
            if (معرّف_الطالب == null)
            {
                return null;
            }
            int? معرّف_الشخص = 0;
            DateTime? تاريخ_الالتحاق = DateTime.Now;
            bool IsFound = clsالطلابData.GetالطلابInfoByID(معرّف_الطالب,ref معرّف_الشخص);

           if (IsFound)
               return new clsالطلاب(
معرّف_الطالب, معرّف_الشخص);
            else
                return null;
            }
        public static clsالطلاب FindByمعرّف_الشخص(int? معرّف_الشخص)

        {
            if (معرّف_الشخص == null)
            {
                return null;
            }
            int? معرّف_الطالب = 0;
            bool IsFound = clsالطلابData.GetالطلابInfoByPersonID(ref معرّف_الطالب,
          معرّف_الشخص);

            if (IsFound)
                return new clsالطلاب(معرّف_الطالب, معرّف_الشخص);
            else
                return null;
        }

        public static DataTable GetAllالطلاب()
       {

        return clsالطلابData.GetAllالطلاب();

       }



        public bool Save(int? معرّف_الشخص)
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالطلاب(معرّف_الشخص))
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالطلاب();

            }
        
            return false;
        }



       public static bool Deleteالطلاب(int? معرّف_الطالب, int? معرّف_الشخص)
       {
            bool personDatenDeleted = clsالأشخاص.Deleteالأشخاص(معرّف_الشخص);

            if (!personDatenDeleted)
                return false;
           
            return clsالطلابData.Deleteالطلاب(معرّف_الطالب);

       }


        public enum الطلابColumn
         {
            معرّف_الطالب,
            معرّف_الشخص
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الطلابColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالطلابData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
