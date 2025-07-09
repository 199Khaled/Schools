
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsسجل_الأخطاء
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الخطأ { get; set; }
        public string رسالة_الخطأ { get; set; }
        public string تفاصيل_التتبع { get; set; } = null;
        public DateTime? الطابع_الزمني { get; set; }
        public string شدة_الخطأ { get; set; } = null;
        public string معلومات_إضافية { get; set; } = null;


        public clsسجل_الأخطاء()
        {
            this.معرّف_الخطأ = null;
            this.رسالة_الخطأ = "";
            this.تفاصيل_التتبع = null;
            this.الطابع_الزمني = DateTime.Now;
            this.شدة_الخطأ = null;
            this.معلومات_إضافية = null;
            Mode = enMode.AddNew;
        }


        private clsسجل_الأخطاء(
int? معرّف_الخطأ, string رسالة_الخطأ, DateTime? الطابع_الزمني, string تفاصيل_التتبع = null, string شدة_الخطأ = null, string معلومات_إضافية = null)        {
            this.معرّف_الخطأ = معرّف_الخطأ;
            this.رسالة_الخطأ = رسالة_الخطأ;
            this.تفاصيل_التتبع = تفاصيل_التتبع;
            this.الطابع_الزمني = الطابع_الزمني;
            this.شدة_الخطأ = شدة_الخطأ;
            this.معلومات_إضافية = معلومات_إضافية;
            Mode = enMode.Update;
        }


       private bool _AddNewسجل_الأخطاء()
       {
        this.معرّف_الخطأ = clsسجل_الأخطاءData.AddNewسجل_الأخطاء(
this.رسالة_الخطأ, this.تفاصيل_التتبع, this.الطابع_الزمني, this.شدة_الخطأ, this.معلومات_إضافية);
        return (this.معرّف_الخطأ != null);
       }


       public static bool AddNewسجل_الأخطاء(
ref int? معرّف_الخطأ, string رسالة_الخطأ, DateTime? الطابع_الزمني, string تفاصيل_التتبع = null, string شدة_الخطأ = null, string معلومات_إضافية = null)        {
        معرّف_الخطأ = clsسجل_الأخطاءData.AddNewسجل_الأخطاء(
رسالة_الخطأ, تفاصيل_التتبع, الطابع_الزمني, شدة_الخطأ, معلومات_إضافية);

        return (معرّف_الخطأ != null);

       }


       private bool _Updateسجل_الأخطاء()
       {
        return clsسجل_الأخطاءData.Updateسجل_الأخطاءByID(
this.معرّف_الخطأ, this.رسالة_الخطأ, this.تفاصيل_التتبع, this.الطابع_الزمني, this.شدة_الخطأ, this.معلومات_إضافية);
       }


       public static bool Updateسجل_الأخطاءByID(
int? معرّف_الخطأ, string رسالة_الخطأ, DateTime? الطابع_الزمني, string تفاصيل_التتبع = null, string شدة_الخطأ = null, string معلومات_إضافية = null)        {
        return clsسجل_الأخطاءData.Updateسجل_الأخطاءByID(
معرّف_الخطأ, رسالة_الخطأ, تفاصيل_التتبع, الطابع_الزمني, شدة_الخطأ, معلومات_إضافية);

        }


       public static clsسجل_الأخطاء FindByمعرّف_الخطأ(int? معرّف_الخطأ)

        {
            if (معرّف_الخطأ == null)
            {
                return null;
            }
            string رسالة_الخطأ = "";
            string تفاصيل_التتبع = "";
            DateTime? الطابع_الزمني = DateTime.Now;
            string شدة_الخطأ = "";
            string معلومات_إضافية = "";
            bool IsFound = clsسجل_الأخطاءData.Getسجل_الأخطاءInfoByID(معرّف_الخطأ,
 ref رسالة_الخطأ,  ref تفاصيل_التتبع,  ref الطابع_الزمني,  ref شدة_الخطأ,  ref معلومات_إضافية);

           if (IsFound)
               return new clsسجل_الأخطاء(
معرّف_الخطأ, رسالة_الخطأ, تفاصيل_التتبع, الطابع_الزمني, شدة_الخطأ, معلومات_إضافية);
            else
                return null;
            }


       public static DataTable GetAllسجل_الأخطاء()
       {

        return clsسجل_الأخطاءData.GetAllسجل_الأخطاء();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewسجل_الأخطاء())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateسجل_الأخطاء();

            }
        
            return false;
        }



       public static bool Deleteسجل_الأخطاء(int معرّف_الخطأ)
       {

        return clsسجل_الأخطاءData.Deleteسجل_الأخطاء(معرّف_الخطأ);

       }


        public enum سجل_الأخطاءColumn
         {
            معرّف_الخطأ,
            رسالة_الخطأ,
            تفاصيل_التتبع,
            الطابع_الزمني,
            شدة_الخطأ,
            معلومات_إضافية
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(سجل_الأخطاءColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsسجل_الأخطاءData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
