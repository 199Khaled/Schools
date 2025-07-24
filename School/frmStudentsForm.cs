using Guna.UI2.WinForms;
using SchoolsDb_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schools
{
    public partial class frmStudentsForm : Form
    {
        public enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;


        clsالأشخاص _persons;
        clsالطلاب _students;
        BindingSource _bindingSource;
        public frmStudentsForm()
        {
            InitializeComponent();
            _bindingSource = new BindingSource();
        }

        private void _LoadAllStudentFromDatabase()
        {
            DataTable dtStudents = clsالطلاب.GetAllالطلاب();
            if (dtStudents != null && dtStudents.Rows.Count > 0)
            {
                _bindingSource.DataSource = dtStudents;
                dgvStudents.DataSource = _bindingSource;
            }
            else
                dgvStudents.DataSource = null; //falls no row found.
        }
        private void frmStudentsForm_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();

            _LoadAllStudentFromDatabase();
            _FillComboBoxCity();
            _FillClassnameWithDaten();
        }
        private void _ResetDefaultValue()
        {
            _Mode = enMode.AddNew;//at first we set the mode  to addnew

            txtFirstname.Clear();
            txtFathername.Clear();
            txtMothername.Clear();
            txtLastname.Clear();
            cbGender.SelectedIndex = -1;
            txtEmail.Clear();
            txtPhone.Clear();
            cbCity.SelectedIndex = -1;
            cbClassname.SelectedIndex = -1;
            dtpDateOfBirth.Value = DateTime.Now;

            txtFirstname.FillColor = Color.White;
            errorProvider1.SetError(txtFirstname, null);
            txtLastname.FillColor = Color.White;
            errorProvider1.SetError(txtLastname, null);

            cbCity.FillColor = Color.White;
            errorProvider1.SetError(cbCity, null);

            cbGender.FillColor = Color.White;
            errorProvider1.SetError(cbGender, null);

            cbClassname.FillColor = Color.White;
            errorProvider1.SetError(cbClassname, null);
        }

        private bool _IsInputValid(Guna2TextBox textname, string message)
        {
            if (string.IsNullOrEmpty(textname.Text))
            {
                errorProvider1.SetError(textname, message);
                textname.FillColor = Color.LightPink;
                return false;
            }
            else
            {
                errorProvider1.SetError(textname, null);
                textname.FillColor = Color.White;
                return true;
            }
        }
        bool _IsInputValidForComboBox(Guna2ComboBox comboBox, string errorMessage)
        {
            if (comboBox.SelectedIndex == -1)
            {
                errorProvider1.SetError(comboBox, errorMessage);
                comboBox.FillColor= Color.LightPink;

                return false;
            }
            else
            {
                errorProvider1.SetError(comboBox, null);
                comboBox.FillColor= Color.White;

                return true;
            }
        }
        private bool _CheckInput()
        {
            bool isValid = true;
            isValid &= _IsInputValid(txtFirstname, "الرجاء إدخال الاسم الأول!");
            isValid &= _IsInputValid(txtLastname, "الرجاء إدخال اسم العائلة!");

            isValid &= _IsInputValidForComboBox(cbCity, "الرجاء اختيار المدينة!");
            isValid &= _IsInputValidForComboBox(cbGender, "الرجاء اختيار الجنس!");
            isValid &= _IsInputValidForComboBox(cbClassname, "الرجاء اختيار اسم الصف!");

            return isValid;
        }
        private void _LoadPersonData(clsالأشخاص persons)
        {
            // _persons = clsPersons.FindByPersonID(_personID);

            if (persons == null)
            {
                MessageBox.Show("لم يتم العثور على شخص بالرقم المعطى. الرجاء المحاولة مرة أخرى.", "خطأ",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _persons = persons;

            txtFirstname.Text = _persons.الاسم_الأول;
            txtFathername.Text = _persons.اسم_الأب;
            txtMothername.Text = _persons.اسم_الأم;
            txtLastname.Text = _persons.اسم_العائلة;
            dtpDateOfBirth.Value = _persons.تاريخ_الميلاد.Value;
            cbGender.Text = _persons.الجنس;
            cbCity.Text = _persons.المدينة;
            txtPhone.Text = _persons.الهاتف;
            txtEmail.Text = _persons.البريد_الإلكتروني;

            _students = clsالطلاب.FindByمعرّف_الشخص(_persons.معرّف_الشخص);
            if (_students != null)
                cbClassname.Text = clsالصفوف.FindByمعرّف_الصف(_students.معرّف_الصف).اسم_الصف;
        }
        private void _FillPersonData()
        {
            if (_Mode == enMode.AddNew)
            {
                _persons = new clsالأشخاص();
                _students = new clsالطلاب();
            }

            _persons.الاسم_الأول = txtFirstname.Text;
            _persons.اسم_الأب = txtFathername.Text;
            _persons.اسم_الأم = txtMothername.Text;
            _persons.اسم_العائلة = txtLastname.Text;
            _persons.تاريخ_الميلاد = dtpDateOfBirth.Value.Date;
            _persons.الجنس = cbGender.Text;
            _persons.المدينة = cbCity.Text;
            _persons.الهاتف = txtPhone.Text;
            _persons.البريد_الإلكتروني = txtEmail.Text;


            if (_Mode == enMode.AddNew)
            {
                _students = new clsالطلاب();
                _students.تاريخ_الالتحاق = DateTime.Now;
            }
            _students.معرّف_الصف = clsالصفوف.FindByاسم_الصف(cbClassname.Text).معرّف_الصف;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _ResetDefaultValue();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!_CheckInput())
                return;

            _FillPersonData(); //fill the person data

            //if the person was not added
            if (!_persons.Save())
            {
                MessageBox.Show("فشل حفظ بيانات الشخص. الرجاء المحاولة مرة أخرى.", "خطأ",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          
            // If the student was not added
            if (!_students.Save(_persons.معرّف_الشخص))
            {
                MessageBox.Show("فشل في إضافة الطالب. الرجاء المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //in case everthing is good
            _Mode = enMode.Update;
            _LoadAllStudentFromDatabase();
            _ResetDefaultValue();
        }
        private void _FillComboBoxCity()
        {
            // 1. Liste der Städte und Gemeinden im Neckar-Odenwald-Kreis erstellen
            List<string> cities = new List<string>
        {
      "دمشق",       // Damascus
    "حلب",        // Aleppo
    "حمص",        // Homs
    "حماة",        // Hama
    "اللاذقية",    // Latakia
    "طرطوس",       // Tartus
    "دير الزور",   // Deir ez-Zor
    "الرقة",       // Raqqa
    "السويداء",    // As-Suwayda
    "درعا",        // Daraa
    "إدلب",        // Idlib
    "القنيطرة",    // Quneitra
    "الحسكة",      // Al-Hasakah
    "ريف دمشق",    // Rural Damascus
    "عين العرب",   // Kobane / Ayn al-Arab
    "القامشلي",    // Qamishli
    "تل أبيض",     // Tal Abyad
    "منبج",        // Manbij
    "البوكمال",    // Al-Bukamal
    "الميادين"  };

            // 2. ComboBox leeren
            cbCity.Items.Clear();

            // 3. Städte und Gemeinden zur ComboBox hinzufügen
            foreach (string city in cities)
            {
                cbCity.Items.Add(city);
            }

            // 4. Optional: Standardauswahl festlegen
            if (cbCity.Items.Count > 0)
            {
                cbCity.SelectedIndex = -1; // Wählt die erste Stadt in der Liste aus
            }
        }

        private void _FillClassnameWithDaten()
        {
            DataTable classes = clsالصفوف.GetAllالصفوف();

            if(classes != null)
            {
                foreach(DataRow row in classes.Rows)
                {
                    cbClassname.Items.Add(row["اسم_الصف"]);
                }
            }
            cbClassname.SelectedIndex = -1;
        } 

        private void dgvStudents_DoubleClick(object sender, EventArgs e)
        {
            _ResetDefaultValue(); //reset the values
            _Mode = enMode.Update; //we change the mode tho update mode

            int studentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            clsالأشخاص persons = clsالطلاب.FindByمعرّف_الطالب(studentID).الأشخاصInfo;
            _LoadPersonData(persons);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstname.Text))
            {
                MessageBox.Show("يرجى تحديد صف صالح قبل المتابعة.", "مطلوب التحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //wa make it sure, that the user will do the Transaction.
            bool warningMessage = MessageBox.Show("هل أنت متأكد من رغبتك في حذف بيانات هذا الطالب؟", "تحذير",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

            //if No, we go back from the methode
            if (!warningMessage)
                return;

            int studentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            clsالطلاب students = clsالطلاب.FindByمعرّف_الطالب(studentID);
            if(students == null)
            {
                MessageBox.Show("لم يتم العثور على طالب للبيانات المحددة.", "مطلوب التحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (warningMessage && clsالطلاب.Deleteالطلاب(studentID))
            {
                MessageBox.Show("تم حذف الطالب بنجاح!", "تم الحذف بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefaultValue();
                _LoadAllStudentFromDatabase();
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء حذف الطالب. الرجاء المحاولة مرة أخرى.", "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbFilterby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterby.SelectedIndex != -1)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (cbFilterby.SelectedIndex == -1)
                return;

            if(!string.IsNullOrEmpty(txtFilterValue.Text) && cbFilterby.Text == "معرّف_الطالب" && !_IsNumber(txtFilterValue.Text))
            {
                MessageBox.Show("إدخال غير صالح، الرجاء إدخال رقم");
                return;
            }
            else
                _ApplyFilter();
        }

        private void _ApplyFilter()
        {
            string filterValue = txtFilterValue.Text.Trim();
            string filterColumn = cbFilterby.Text.Trim();
            if (!string.IsNullOrEmpty(filterValue))
            {
                if (filterColumn == "معرّف_الطالب")
                    _bindingSource.Filter = $"{filterColumn} = {filterValue}";
                else
                    _bindingSource.Filter = $"{filterColumn} like '{filterValue}%'";
            }
            else
            {
                _bindingSource.Filter = string.Empty;
                //falls the fields are not empty, we set all fiels with default values.
                if (!string.IsNullOrEmpty(txtFirstname.Text))
                {
                    _ResetDefaultValue();
                }
            }
        }

        private bool _IsNumber(string eingabe)
        {
            if (int.TryParse(eingabe, out _))
                return true;
            else
                return false;
        }
    }
}
