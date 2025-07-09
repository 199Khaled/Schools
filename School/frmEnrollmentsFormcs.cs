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
        clsStudents _students;
        clsEnrollments _enrollments;
        BindingSource _bindingSource;
        public frmEnrollmentsFormcs()
        {
            InitializeComponent();
            _bindingSource = new BindingSource();

        }

        private void frmEnrollmentsFormcs_Load(object sender, EventArgs e)
        {
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
        }
        private void _FillClassesComboBoxWithData()
        {
            DataTable dt = clsClasses.GetAllClasses();

            cbClasses.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                cbClasses.Items.Add(row["ClassName"]);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentID.Text))
                return;

            int StudentID= Convert.ToInt32(txtStudentID.Text);
             _students = clsStudents.FindByStudentID(StudentID);

            if (_students != null)
            {
                txtVollname.Text = _students.PersonsInfo.Vollname;
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
        private bool _CheckFilledDate()
        {
            bool Valid = true;

            Valid &= _IsInputValid(txtStudentID, "حقل مطلوب!");
            Valid &= _IsInputValid(txtVollname, "حقل مطلوب!");
            Valid &= _IsClassSelected();

            return Valid;
        }

        private bool _IsClassSelected()
        {
            if (cbClasses.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbClasses, "حقل مطلوب!");
                cbClasses.FillColor = Color.LightPink;
                return false; ;
            }
            else
            {
                errorProvider1.SetError(cbClasses, null);
                cbClasses.FillColor = Color.White;
                return true;
            }
        }
        private bool _FillEnrollmentWithData()
        {
            if (!_CheckFilledDate())
                return false;

            if (_mode == enMode.Addnew)
                _enrollments = new clsEnrollments();

            _enrollments.StudentID = Convert.ToInt32(txtStudentID.Text);
            _enrollments.ClassID = clsClasses.FindByClassName(cbClasses.Text).ClassID;
            _enrollments.EnrollmentDate = dtpDateOfEnrollment.Value;

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
            DataTable dtEnrollments = clsEnrollments.GetAllEnrollments();
            if (dtEnrollments != null && dtEnrollments.Rows.Count > 0)
            {
                _bindingSource.DataSource = dtEnrollments;
                dgvEnrollments.DataSource = _bindingSource;
            }
            else
                dgvEnrollments.DataSource = null;
        }
    }
}
