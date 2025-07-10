
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالموظفون
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الموظف { get; set; }
        public int? معرّف_الشخص { get; set; }
        public clsالأشخاص الأشخاصInfo { get; set; }
        public string النوع { get; set; }
        public DateTime? تاريخ_التوظيف { get; set; }
        public string تاريخ_الإنهاء { get; set; }
        public bool? نشط { get; set; }


        public clsالموظفون()
        {
            this.معرّف_الموظف = null;
            this.معرّف_الشخص = 0;
            this.النوع = "";
            this.تاريخ_التوظيف = DateTime.Now;
            this.تاريخ_الإنهاء = null;
            this.نشط = false;
            Mode = enMode.AddNew;
        }


        private clsالموظفون(
int? معرّف_الموظف, int? معرّف_الشخص, string النوع, DateTime? تاريخ_التوظيف, bool? نشط, string تاريخ_الإنهاء = null)        {
            this.معرّف_الموظف = معرّف_الموظف;
            this.معرّف_الشخص = معرّف_الشخص;
            this.الأشخاصInfo = clsالأشخاص.FindByمعرّف_الشخص(معرّف_الشخص);
            this.النوع = النوع;
            this.تاريخ_التوظيف = تاريخ_التوظيف;
            this.تاريخ_الإنهاء = تاريخ_الإنهاء;
            this.نشط = نشط;
            Mode = enMode.Update;
        }


       private bool _AddNewالموظفون()
       {
        this.معرّف_الموظف = clsالموظفونData.AddNewالموظفون(
this.معرّف_الشخص, this.النوع, this.تاريخ_التوظيف, this.نشط,this.تاريخ_الإنهاء);
        return (this.معرّف_الموظف != null);
       }


       public static bool AddNewالموظفون( ref int? معرّف_الموظف, int? معرّف_الشخص, string النوع,
           DateTime? تاريخ_التوظيف, bool? نشط, string تاريخ_الإنهاء)      
        {
        معرّف_الموظف = clsالموظفونData.AddNewالموظفون(
        معرّف_الشخص  , النوع, تاريخ_التوظيف, نشط, تاريخ_الإنهاء)  ;

        return (معرّف_الموظف != null);

       }


       private bool _Updateالموظفون()
       {
        return clsالموظفونData.UpdateالموظفونByID(
this.معرّف_الموظف, this.معرّف_الشخص, this.النوع, this.تاريخ_التوظيف, this.نشط,this.تاريخ_الإنهاء);
       }


       public static bool UpdateالموظفونByID(
int? معرّف_الموظف, int? معرّف_الشخص, string النوع, DateTime? تاريخ_التوظيف, bool? نشط, string تاريخ_الإنهاء = null)        {
        return clsالموظفونData.UpdateالموظفونByID(
معرّف_الموظف, معرّف_الشخص, النوع, تاريخ_التوظيف, نشط,تاريخ_الإنهاء);

        }


       public static clsالموظفون FindByمعرّف_الموظف(int? معرّف_الموظف)

        {
            if (معرّف_الموظف == null)
            {
                return null;
            }
            int? معرّف_الشخص = 0;
            string النوع = "";
            DateTime? تاريخ_التوظيف = DateTime.Now;
            string تاريخ_الإنهاء = null;
            bool? نشط = false;
            bool IsFound = clsالموظفونData.GetالموظفونInfoByID(معرّف_الموظف,
 ref معرّف_الشخص,  ref النوع,  ref تاريخ_التوظيف,  ref تاريخ_الإنهاء,  ref نشط);

           if (IsFound)
               return new clsالموظفون(
معرّف_الموظف, معرّف_الشخص, النوع, تاريخ_التوظيف, نشط , تاريخ_الإنهاء);
            else
                return null;
            }


        public static clsالموظفون FindByمعرّف_الشخص(int? معرّف_الشخص)

        {
            if (معرّف_الشخص == null)
            {
                return null;
            }
            int? معرّف_الموظف = 0;
            string النوع = "";
            DateTime? تاريخ_التوظيف = DateTime.Now;
            string تاريخ_الإنهاء = null;
            bool? نشط = false;
            bool IsFound = clsالموظفونData.GetالموظفونInfoByمعرّف_الشخص(ref معرّف_الموظف,
  معرّف_الشخص, ref النوع, ref تاريخ_التوظيف, ref تاريخ_الإنهاء, ref نشط);

            if (IsFound)
                return new clsالموظفون(
 معرّف_الموظف, معرّف_الشخص, النوع, تاريخ_التوظيف, نشط, تاريخ_الإنهاء);
            else
                return null;
        }

        public static DataTable GetAllالموظفون()
       {

        return clsالموظفونData.GetAllالموظفون();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالموظفون())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالموظفون();

            }
        
            return false;
        }



       public static bool Deleteالموظفون(int معرّف_الموظف)
       {

        return clsالموظفونData.Deleteالموظفون(معرّف_الموظف);

       }


        public enum الموظفونColumn
         {
            معرّف_الموظف,
            معرّف_الشخص,
            النوع,
            تاريخ_التوظيف,
            تاريخ_الإنهاء,
            نشط
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الموظفونColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالموظفونData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
