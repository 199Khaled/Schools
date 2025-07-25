using Guna.UI2.WinForms;
using SchoolsDb_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schools
{
    public partial class frmEnrollmentsFormcs : Form
    {
        enum enMode  {Addnew = 1, Update=2}
        enMode _mode;
        clsالطلاب _students;
        clsالتسجيلات _enrollments;
        BindingSource _bindingSource;
        public frmEnrollmentsFormcs()
        {
            InitializeComponent();
            _bindingSource = new BindingSource();

        }

        private void frmEnrollmentsFormcs_Load(object sender, EventArgs e)
        {
            _LoadEnorollmentsDataFromDatabase();
            _FillClassesComboBoxWithData();
            _ResetDefaultValues();
        }
        private void _ResetDefaultValues()
        {
            _mode = enMode.Addnew;
            txtStudentID.Clear();
            txtVollname.Clear();
            cbClasses.SelectedIndex = -1;
            dtpDateOfEnrollment.Value = DateTime.Now;

            txtStudentID.FillColor = Color.White;
            errorProvider1.SetError(txtStudentID, null);
            txtVollname.FillColor = Color.White;
            errorProvider1.SetError(txtVollname, null);

            cbClasses.FillColor = Color.White;
            errorProvider1.SetError(cbClasses, null);
        }
        private void _FillClassesComboBoxWithData()
        {
            DataTable dt = clsالصفوف.GetAllالصفوف();

            cbClasses.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                cbClasses.Items.Add(row["اسم_الصف"]);
            }
            cbClasses.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentID.Text))
                return;

            int StudentID= Convert.ToInt32(txtStudentID.Text);
             _students = clsالطلاب.FindByمعرّف_الطالب(StudentID);

            if (_students != null)
            {
                txtVollname.Text = _students.الأشخاصInfo.الاسم_الكامل;
            }
            else
            {
                MessageBox.Show("غير موجود");
            }
        }

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentID.Text))
                txtVollname.Clear();
        }

        private void txtVollname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVollname.Text))
                txtStudentID.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _ResetDefaultValues();
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
        private bool _CheckFilledDate()
        {
            bool isValid = true;

            isValid &= _IsInputValid(txtStudentID, "الرجاء إدخال رقم الطالب!");
            isValid &= _IsInputValid(txtVollname, "الرجاء إدخال الاسم الكامل!");

            isValid &= _IsInputValidForComboBox(cbClasses, "الرجاء اختيار الصف!");

            return isValid;
        }
        private bool _FillEnrollmentWithData()
        {
            if (!_CheckFilledDate())
                return false;

            if (_mode == enMode.Addnew)
                _enrollments =new clsالتسجيلات();

            _enrollments.معرّف_الطالب = Convert.ToInt32(txtStudentID.Text);
            _enrollments.معرّف_الصف = clsالصفوف.FindByاسم_الصف(cbClasses.Text).معرّف_الصف;
            _enrollments.تاريخ_التسجيل = dtpDateOfEnrollment.Value;

            return true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!_FillEnrollmentWithData())
                return;

            //if the person was not added
            if (!_enrollments.Save())
            {
                MessageBox.Show("فشلت عملية حفظ تسجيلات الطلاب. يرجى المحاولة مرة أخرى.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //in case everthing is good
            _mode = enMode.Update;
            _LoadEnorollmentsDataFromDatabase();
            _ResetDefaultValues();

        }
        private void _LoadEnorollmentsDataFromDatabase()
        {
            DataTable dtEnrollments = clsالتسجيلات.GetAllالتسجيلات();
            if (dtEnrollments != null && dtEnrollments.Rows.Count > 0)
            {
                _bindingSource.DataSource = dtEnrollments;
                dgvEnrollments.DataSource = _bindingSource;
            }
            else
                dgvEnrollments.DataSource = null;
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

            if (!string.IsNullOrEmpty(txtFilterValue.Text) && cbFilterby.Text == "معرّف_الطالب" && !_IsNumber(txtFilterValue.Text))
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
                if (filterColumn == "معرّف_الطالب" || filterColumn == "معرّف_الصف")
                    _bindingSource.Filter = $"{filterColumn} = {filterValue}";
                else
                    _bindingSource.Filter = $"{filterColumn} like '{filterValue}%'";
            }
            else
            {
                _bindingSource.Filter = string.Empty;
                //falls the fields are not empty, we set all fiels with default values.
                if (!string.IsNullOrEmpty(txtVollname.Text))
                {
                    _ResetDefaultValues();
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

        private void dgvEnrollments_DoubleClick(object sender, EventArgs e)
        {
            _ResetDefaultValues(); //reset the values
            _mode = enMode.Update; //we change the mode tho update mode

            int EnrollmentID = (int)dgvEnrollments.CurrentRow.Cells[0].Value;
            clsالتسجيلات enrollments = clsالتسجيلات.FindByمعرّف_التسجيل(EnrollmentID);
            _LoadEnrollmentData(enrollments);
        }
        private void _LoadEnrollmentData(clsالتسجيلات enrollments)
        {
            // _persons = clsPersons.FindByPersonID(_personID);

            if (enrollments == null)
            {
                MessageBox.Show("لم يتم العثور على طالب بالرقم المعطى. الرجاء المحاولة مرة أخرى.", "خطأ",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _enrollments = enrollments;
            txtStudentID.Text = _enrollments.معرّف_الطالب.ToString();
            txtVollname.Text = _enrollments.الطلابInfo.الأشخاصInfo.الاسم_الكامل;
            cbClasses.Text = _enrollments.الصفوفInfo.اسم_الصف;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
