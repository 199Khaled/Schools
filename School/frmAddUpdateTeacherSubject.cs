using Guna.UI2.WinForms;
using SchoolsDb_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schools
{
    public partial class frmAddUpdateSubjects: Form
    {
        public enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;
        clsTeacherSubjects _teacherSubjects;
        clsEmployees _employee;
        BindingSource _bindingSource;
        public frmAddUpdateSubjects()
        {
            InitializeComponent();

            _bindingSource = new BindingSource();
        }
        private void _LoadTeacherSubjectDataFromDatabase()
        {
            DataTable dtTeacherSucject = clsTeacherSubjects.GetAllTeacherSubjects();
            if (dtTeacherSucject != null && dtTeacherSucject.Rows.Count > 0)
            {
                _bindingSource.DataSource = dtTeacherSucject;
                dgvTeacherSubject.DataSource = _bindingSource;
            }
            else
                dgvTeacherSubject.DataSource = null;
        }
        private void _ResetDefaultValue()
        {
            _Mode = enMode.AddNew; //at first 
            txtTeacherID.Clear();
            txtTeacherID.Select();
            txtVollname.Clear();
            cbFilterby.SelectedIndex = -1;
            cbSubjects.SelectedIndex = -1;
        }
        
        private void _LoadData(clsTeacherSubjects teacherSubjects)
        {
            if(teacherSubjects == null)
            {
                return;
            }
            _teacherSubjects = teacherSubjects;
            txtTeacherID.Text = teacherSubjects.TeacherID.ToString();  //TeacherID is equall EmployeeID for Teacher 
            txtVollname.Text = clsEmployees.FindByEmployeeID(_teacherSubjects.TeacherID).PersonsInfo.Vollname;
            cbSubjects.Text = _teacherSubjects.SubjectsInfo.SubjectName;
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

        private bool _IsSubjectSelected()
        {
            if (cbSubjects.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbSubjects, "Subject cannot be empty!");
                cbSubjects.FillColor = Color.LightPink;
                return false; ;
            }
            else
            {
                errorProvider1.SetError(cbSubjects, null);
                cbSubjects.FillColor = Color.White;
                return true;
            }
        }
        private bool _CheckFilledDate()
        {
            bool Valid = true;

            Valid &= _IsInputValid(txtTeacherID, "TeacherID cannot be empty!");
            Valid &= _IsInputValid(txtVollname, "Teachername cannot be empty!");
            Valid &= _IsSubjectSelected();
         
            return Valid;
        }
        private bool _FillTeacherSubjectData()
        {
            if (!_CheckFilledDate())
                return false;

            if (_Mode == enMode.AddNew)
                _teacherSubjects = new clsTeacherSubjects();

            _teacherSubjects.TeacherID = Convert.ToInt32(txtTeacherID.Text.Trim());
            _teacherSubjects.SubjectID = clsSubjects.FindBySubjectName(cbSubjects.Text).SubjectID;

            return true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _ResetDefaultValue();

         
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!_FillTeacherSubjectData())
                return;//fill the teacher Subject Data

            //if the person was not added
            if (!_teacherSubjects.Save())
            {
                MessageBox.Show("Save operation fore Teacher-Subject failed. Please try again.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //in case everthing is good
            _Mode = enMode.Update;
            _LoadTeacherSubjectDataFromDatabase();
            _ResetDefaultValue();
        }
        private void frmAddUpdateSubjects_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            _LoadTeacherSubjectDataFromDatabase();
            _FillSubjectComboBoxWithData();
        }
        
        private void _FillSubjectComboBoxWithData()
        {
            DataTable dt = clsSubjects.GetAllSubjects();

            cbSubjects.Items.Clear();
            foreach(DataRow row in dt.Rows)
            {
                cbSubjects.Items.Add(row["SubjectName"]);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTeacherID.Text))
                return;

            int TeacherID = Convert.ToInt32(txtTeacherID.Text);
            _employee = clsEmployees.FindByEmployeeID(TeacherID);

            if(_employee != null)
            {
                txtVollname.Text = _employee.PersonsInfo.Vollname;
            }
            else
            {
                MessageBox.Show("Not Found");
            }
        }

        private void txtTeacherID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTeacherID.Text))
                txtVollname.Clear();
        }

        private void txtVollname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVollname.Text))
                txtTeacherID.Clear();
        }

        private void cbFilterby_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterby.SelectedIndex != -1)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterValue = txtFilterValue.Text.Trim();
            string filterColumn = cbFilterby.Text.Trim();

            if (!string.IsNullOrEmpty(filterValue))
            {
                if ((filterColumn == "TeacherID" || filterColumn == "SubjectID") && _IsNumber(filterValue))
                    _bindingSource.Filter = $"{filterColumn} = {filterValue}";
                
            }
            else
                _bindingSource.Filter = string.Empty;
        }
        private bool _IsNumber(string eingabe)
        {
            if (int.TryParse(eingabe, out _))
                return true;
            else
                return false;
        }

        private void dgvTeacherSubject_DoubleClick(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            _Mode = enMode.Update;

            int TeacherSujectID = (int)dgvTeacherSubject.CurrentRow.Cells[0].Value;
            clsTeacherSubjects TeacherSubjects = clsTeacherSubjects.FindByTeacherSubjectID(TeacherSujectID);

            _LoadData(TeacherSubjects);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTeacherID.Text) && string.IsNullOrEmpty(txtVollname.Text)  )
            {
                MessageBox.Show("Please select a valid row before proceeding.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //wa make it sure, that the user will do the Transaction.
            bool warningMessage = MessageBox.Show("Are you sure, you want to delete Data of this Teacher?", "Warning",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

            //if No, we go back from the methode
            if (!warningMessage)
                return;

            int TeacherSubjectID = (int)dgvTeacherSubject.CurrentRow.Cells[0].Value;
            clsTeacherSubjects TeacherSubject = clsTeacherSubjects.FindByTeacherSubjectID(TeacherSubjectID);
            if (TeacherSubject == null)
            {
                MessageBox.Show("No TeacherSubject found for the selected data.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (warningMessage && clsTeacherSubjects.DeleteTeacherSubjects(TeacherSubjectID))
            {
                _ResetDefaultValue();
                _LoadTeacherSubjectDataFromDatabase();
            }
            else
            {
                MessageBox.Show("An error occurred while deleting the TeacherSubject. Please try again.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
