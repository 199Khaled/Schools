using Guna.UI2.WinForms;
using SchoolsDb_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schools
{
    public partial class frmTeacherForm : Form
    {
        public enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;

        clsالأشخاص _persons;
        clsالموظفون _employees;
        BindingSource _bindingSource;
        public frmTeacherForm()
        {
            InitializeComponent();
            _bindingSource = new BindingSource();
        }

        private void frmTeacherForm_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();

            _LoadAllStudentFromDatabase();
            _FillComboBoxCity();
            _FillComoBoxJobType();
        }
        private void _LoadAllStudentFromDatabase()
        {
            DataTable dtTeachers = clsالموظفون.GetAllالموظفون();
            if (dtTeachers != null && dtTeachers.Rows.Count > 0)
            {
                _bindingSource.DataSource = dtTeachers;
                dgvTeachers.DataSource = _bindingSource;
            }
            else
                dgvTeachers.DataSource = null; //falls no row found.
        }
        private void _ResetDefaultValue()
        {
            _Mode = enMode.AddNew;//at first we set the mode  to addnew

            txtFirstname.Clear();
            txtFathername.Clear();
            txtMothername.Clear();
            txtLastname.Clear();
            cbGender.SelectedIndex = -1;
            cbCity.SelectedIndex = -1;
            cbJobTyp.SelectedIndex = -1;
            txtEmail.Clear();
            txtPhone.Clear();
            dtpDateOfBirth.Value = DateTime.Now;
            txtContractFinisched.Clear();

            txtFirstname.FillColor = Color.White;
            errorProvider1.SetError(txtFirstname, null);
            txtLastname.FillColor = Color.White;
            errorProvider1.SetError(txtLastname, null);

            cbCity.FillColor = Color.White;
            errorProvider1.SetError(cbCity, null);
            cbGender.FillColor = Color.White;
            errorProvider1.SetError(cbGender, null);
            cbJobTyp.FillColor = Color.White;
            errorProvider1.SetError(cbJobTyp, null);
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
        bool _IsInputValidForComboBox(Guna2ComboBox comboBox  , string errorMessage)
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
            isValid &= _IsInputValidForComboBox(cbJobTyp, "الرجاء اختيار نوع الوظيفة!");

            return isValid;
        }
        private void _LoadPersonData(clsالأشخاص persons)
        {
            // _persons = clsPersons.FindByPersonID(_personID);

            if (persons == null)
            {
                MessageBox.Show("لم يتم العثور على أي شخص لهذا الرقم. الرجاء المحاولة مرة أخرى.", "خطأ",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _persons = persons;

            txtFirstname.Text = _persons.الاسم_الأول;
            txtFathername.Text = _persons.اسم_الأب;
            txtMothername.Text = _persons.اسم_الأم;
            txtLastname.Text = _persons.اسم_العائلة;
            dtpDateOfBirth.Value = _persons.تاريخ_الميلاد.Value;
            cbGender.Text = _persons.الجنس.Trim();
            cbCity.Text = _persons.المدينة;
            txtPhone.Text = _persons.الهاتف;
            txtEmail.Text = _persons.البريد_الإلكتروني;

            _employees = clsالموظفون.FindByمعرّف_الشخص(_persons.معرّف_الشخص);
            cbJobTyp.Text = _employees.النوع; 
            chbAktive.Checked = Convert.ToBoolean(_employees.نشط);
            txtContractFinisched.Text = _employees.تاريخ_الإنهاء;
        }
        private void _FillPersonData()
        {
            if (_Mode == enMode.AddNew)
            {
                _persons = new clsالأشخاص();
                _employees = new clsالموظفون();
            }

            _persons.الاسم_الأول = txtFirstname.Text;
            _persons.اسم_الأب = txtFathername.Text;
            _persons.اسم_الأم = txtMothername.Text;
            _persons.اسم_العائلة = txtLastname.Text;
            _persons.تاريخ_الميلاد = dtpDateOfBirth.Value.Date;
            _persons.الجنس = cbGender.Text;
            _persons.المدينة= cbCity.Text;
            _persons.الهاتف = txtPhone.Text;
            _persons.البريد_الإلكتروني = txtEmail.Text;

            //Fille EmployeesData

            if (_Mode == enMode.AddNew)
            {
                _employees = new clsالموظفون();
                _employees.تاريخ_التوظيف = DateTime.Now;
            }
            _employees.النوع = cbJobTyp.Text.Trim();
            if (!string.IsNullOrEmpty(txtContractFinisched.Text))
            {
                _employees.تاريخ_الإنهاء = txtContractFinisched.Text.Trim();
                _employees.نشط = false;
            }
            else
                _employees.نشط = chbAktive.Checked;
        }


        private void _FillComboBoxCity()
        {
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
    "الميادين"     // Al-Mayadin
        };

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

        private void _FillComoBoxJobType()
        {
            List<string> JobTyps = new List<string>()
            {
                "المعلم",                       // Lehrer
                "المدير",                      // Direktor
                "السكرتير",                    // Sekretär
                "أمين المكتبة",                // Bibliothekar
                "المشرف",                      // Aufsichtsperson
                "موظف الدعم الفني",            // IT-Administrator
                "عامل النظافة",                // Hausmeister / Reinigungskraft
                "الأخصائي الاجتماعي المدرسي",  // Schulsozialarbeiter
                "المرشد النفسي",               // Schulpsychologe
                "مساعد تدريس"
                };// Unterrichtsassistent

            cbJobTyp.Items.Clear();
            foreach (string jobtyp in JobTyps)
            {
                cbJobTyp.Items.Add(jobtyp);
            }

            cbJobTyp.SelectedIndex = -1;
        }
    
        private void dgvTeachers_DoubleClick(object sender, EventArgs e)
        {
            _ResetDefaultValue(); //reset the values
            _Mode = enMode.Update; //we change the mode tho update mode

            int employeeID = (int)dgvTeachers.CurrentRow.Cells[0].Value;
            clsالأشخاص persons = clsالموظفون.FindByمعرّف_الموظف(employeeID).الأشخاصInfo;
            _LoadPersonData(persons);
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

            if (!string.IsNullOrEmpty(txtFilterValue.Text) && cbFilterby.Text == "معرّف_الموظف" && !_IsNumber(txtFilterValue.Text))
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
                if (filterColumn == "معرّف_الموظف")
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
        private void btnAssignSubjectToTeacher_Click(object sender, EventArgs e)
        {
            frmAddUpdateSubjects frm = new frmAddUpdateSubjects();
            frm.Show();
        }

        private void dgvTeachers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_CheckInput())
                return;

            _FillPersonData(); //fill the person data

            //if the person was not added
            if (!_persons.Save())
            {
                MessageBox.Show("فشلت عملية الحفظ للشخص. الرجاء المحاولة مرة أخرى.", "خطأ",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //in case Update Mode 
            if(!_employees.Save(_persons.معرّف_الشخص))
            {
                MessageBox.Show("فشلت عملية الحفظ للموظف. الرجاء المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //in case everthing is good
            _Mode = enMode.Update;
            _LoadAllStudentFromDatabase();
            _ResetDefaultValue();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            _ResetDefaultValue();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstname.Text))
            {
                MessageBox.Show("يرجى تحديد صف صالح قبل المتابعة.", "مطلوب تحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //wa make it sure, that the user will do the Transaction.
            bool warningMessage = MessageBox.Show("هل أنت متأكد أنك تريد حذف بيانات هذا المعلم؟", "تحذير",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

            //if No, we go back from the methode
            if (!warningMessage)
                return;

            int emplyeeID = (int)dgvTeachers.CurrentRow.Cells[0].Value;
            clsالموظفون teachers =clsالموظفون.FindByمعرّف_الموظف(emplyeeID);
            if (teachers == null)
            {
                MessageBox.Show("لم يتم العثور على أي معلم للبيانات المحددة.", "مطلوب تحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (warningMessage &&  clsالأشخاص.Deleteالأشخاص(teachers.معرّف_الشخص))
            {
                _ResetDefaultValue();
                _LoadAllStudentFromDatabase();
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء حذف المعلم. الرجاء المحاولة مرة أخرى.", "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnAddTeacherSubject_Click(object sender, EventArgs e)
        {
            using (frmAddUpdateSubjects frm = new frmAddUpdateSubjects())
            {
                frm.ShowDialog();
                                         
            }
        }
    }
}
