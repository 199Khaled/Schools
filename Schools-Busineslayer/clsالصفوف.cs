
using System;
using System.Data;
using SchoolsDb_DataLayer;

namespace SchoolsDb_BusinessLayer
{
    public class clsالصفوف
    {
        //#nullable enable

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? معرّف_الصف { get; set; }
        public string اسم_الصف { get; set; }
        public string المستوى_الصفّي { get; set; }


        public clsالصفوف()
        {
            this.معرّف_الصف = null;
            this.اسم_الصف = "";
            this.المستوى_الصفّي = "";
            Mode = enMode.AddNew;
        }


        private clsالصفوف(
int? معرّف_الصف, string اسم_الصف, string المستوى_الصفّي)        {
            this.معرّف_الصف = معرّف_الصف;
            this.اسم_الصف = اسم_الصف;
            this.المستوى_الصفّي = المستوى_الصفّي;
            Mode = enMode.Update;
        }


       private bool _AddNewالصفوف()
       {
        this.معرّف_الصف = clsالصفوفData.AddNewالصفوف(
this.اسم_الصف, this.المستوى_الصفّي);
        return (this.معرّف_الصف != null);
       }


       public static bool AddNewالصفوف(
ref int? معرّف_الصف, string اسم_الصف, string المستوى_الصفّي)        {
        معرّف_الصف = clsالصفوفData.AddNewالصفوف(
اسم_الصف, المستوى_الصفّي);

        return (معرّف_الصف != null);

       }


       private bool _Updateالصفوف()
       {
        return clsالصفوفData.UpdateالصفوفByID(
this.معرّف_الصف, this.اسم_الصف, this.المستوى_الصفّي);
       }


       public static bool UpdateالصفوفByID(
int? معرّف_الصف, string اسم_الصف, string المستوى_الصفّي)        {
        return clsالصفوفData.UpdateالصفوفByID(
معرّف_الصف, اسم_الصف, المستوى_الصفّي);

        }


       public static clsالصفوف FindByمعرّف_الصف(int? معرّف_الصف)

        {
            if (معرّف_الصف == null)
            {
                return null;
            }
            string اسم_الصف = "";
            string المستوى_الصفّي = "";
            bool IsFound = clsالصفوفData.GetالصفوفInfoByID(معرّف_الصف,
 ref اسم_الصف,  ref المستوى_الصفّي);

           if (IsFound)
               return new clsالصفوف(
معرّف_الصف, اسم_الصف, المستوى_الصفّي);
            else
                return null;
            }


       public static DataTable GetAllالصفوف()
       {

        return clsالصفوفData.GetAllالصفوف();

       }



        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewالصفوف())
                    {
                        Mode = enMode.Update;
                         return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _Updateالصفوف();

            }
        
            return false;
        }



       public static bool Deleteالصفوف(int معرّف_الصف)
       {

        return clsالصفوفData.Deleteالصفوف(معرّف_الصف);

       }


        public enum الصفوفColumn
         {
            معرّف_الصف,
            اسم_الصف,
            المستوى_الصفّي
         }


        public enum SearchMode
        {
            Anywhere,
            StartsWith,
            EndsWith,
            ExactMatch
        }
    

        public static DataTable SearchData(الصفوفColumn ChosenColumn, string SearchValue, SearchMode Mode = SearchMode.Anywhere)
        {
            if (string.IsNullOrWhiteSpace(SearchValue) || !SqlHelper.IsSafeInput(SearchValue))
                return new DataTable();

            string modeValue = Mode.ToString(); // Get the mode as string for passing to the stored procedure

            return clsالصفوفData.SearchData(ChosenColumn.ToString(), SearchValue, modeValue);
        }        



    }
}
