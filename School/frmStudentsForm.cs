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
    public partial class frmStudentsForm: Form
    {
        public enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;


        clsPersons _persons;
        BindingSource _bindingSource;
        public frmStudentsForm()
        {
            InitializeComponent();
            _bindingSource = new BindingSource();
        }

        private void _LoadAllStudentFromDatabase()
        {
            DataTable dtStudents = clsStudents.GetAllStudents();
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
        }
        private void _ResetDefaultValue()
        {
            _Mode = enMode.AddNew;//at first we set the mode  to addnew

            txtFirstname.Clear();
            txtLastname.Clear();
            cbGender.SelectedIndex = -1;
            txtEmail.Clear();
            txtPhone.Clear();
            cbCity.SelectedIndex = -1;
            dtpDateOfBirth.Value = DateTime.Now;
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

        private bool _CheckInput()
        {
            bool isValid = true;
            isValid &= _IsInputValid(txtFirstname, "لا يمكن أن يكون الاسم الأول فارغًا!");
            isValid &= _IsInputValid(txtLastname, "لا يمكن أن يكون اسم العائلة فارغًا!");

            return isValid;
        }
        private void _LoadPersonData(clsPersons persons)
        {
           // _persons = clsPersons.FindByPersonID(_personID);

            if (persons == null)
            {
                MessageBox.Show("لم يتم العثور على شخص بالرقم المعطى. الرجاء المحاولة مرة أخرى.", "خطأ",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _persons = persons; 

            txtFirstname.Text = _persons.Firstname;
            txtLastname.Text = _persons.Lastname;
            dtpDateOfBirth.Value = _persons.DateOfBirth.Value;
            cbGender.Text = _persons.Gender;
            cbCity.Text = _persons.City;
            txtPhone.Text = _persons.Phone;
            txtEmail.Text = _persons.Email;
        }
        private void _FillPersonData()
        {
            if (_Mode == enMode.AddNew)
                _persons = new clsPersons();

            _persons.Firstname = txtFirstname.Text;
            _persons.Lastname = txtLastname.Text;
            _persons.DateOfBirth = dtpDateOfBirth.Value.Date;
            _persons.Gender = cbGender.Text;
            _persons.City = cbCity.Text;
            _persons.Phone = txtPhone.Text;
            _persons.Email = txtEmail.Text;
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
            //in case Update Mode 
            if (_Mode == enMode.Update)
            {
                _LoadAllStudentFromDatabase();
                _ResetDefaultValue();
                return;
            }
            int? studentID = null;
            // If the student was not added
            if (!clsStudents.AddNewStudents(ref studentID, _persons.PersonID))
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
        "Adelsheim","Aglasterhausen", "Billigheim","Binau","Buchen","Elztal", "Fahrenbach", "Hardheim",
        "Haßmersheim","Höpfingen","Hüffenhardt", "Limbach","Mosbach","Mudau", "Neckargerach", "Neckarzimmern",
        "Neunkirchen", "Obrigheim", "Osterburken",  "Ravenstein", "Rosenberg", "Schefflenz", "Schwarzach", "Seckach",
        "Waldbrunn", "Walldürn", "Zwingenberg"};

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

        private void dgvStudents_DoubleClick(object sender, EventArgs e)
        {
            _ResetDefaultValue(); //reset the values
            _Mode = enMode.Update; //we change the mode tho update mode

            int studentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            clsPersons persons = clsStudents.FindByStudentID(studentID).PersonsInfo;
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
            clsStudents students = clsStudents.FindByStudentID(studentID);
            if(students == null)
            {
                MessageBox.Show("لم يتم العثور على طالب للبيانات المحددة.", "مطلوب التحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (warningMessage && clsStudents.DeleteStudents(studentID,students.PersonID))
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
            if(!string.IsNullOrEmpty(txtFilterValue.Text) && cbFilterby.Text == "StudentID" && !_IsNumber(txtFilterValue.Text))
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
                if (filterColumn == "StudentID")
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
