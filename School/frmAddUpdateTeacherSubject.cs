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
        clsمواد_المعلم _teacherSubjects;
        clsالموظفون _employee;
        BindingSource _bindingSource;
        public frmAddUpdateSubjects()
        {
            InitializeComponent();

            _bindingSource = new BindingSource();
        }
        private void _LoadTeacherSubjectDataFromDatabase()
        {
            DataTable dtTeacherSucject = clsمواد_المعلم.GetAllمواد_المعلم();
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
        
        private void _LoadData(clsمواد_المعلم teacherSubjects)
        {
            if(teacherSubjects == null)
            {
                return;
            }
            _teacherSubjects = teacherSubjects;
            txtTeacherID.Text = teacherSubjects.معرّف_المعلم.ToString();  //TeacherID is equall EmployeeID for Teacher 
            txtVollname.Text = clsالموظفون.FindByمعرّف_الموظف(_teacherSubjects.معرّف_المعلم).الأشخاصInfo.الاسم_الكامل;
            cbSubjects.Text = _teacherSubjects.معرّف_المادة;
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
                errorProvider1.SetError(cbSubjects, "الرجاء اختيار المادة!");
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
            Valid &= _IsInputValid(txtTeacherID, "الرجاء إدخال رقم المعلم!");
            Valid &= _IsInputValid(txtVollname, "الرجاء إدخال اسم المعلم!");
            Valid &= _IsSubjectSelected();
         
            return Valid;
        }
        private bool _FillTeacherSubjectData()
        {
            if (!_CheckFilledDate())
                return false;

            if (_Mode == enMode.AddNew)
                _teacherSubjects = new clsمواد_المعلم();

            _teacherSubjects.معرّف_المعلم = Convert.ToInt32(txtTeacherID.Text.Trim());
            _teacherSubjects.معرّف_المادة = cbSubjects.Text.Trim();
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


            int معرّف_المعلم = Convert.ToInt32(txtTeacherID.Text.Trim());
            //if the person was not added
            if (!_teacherSubjects.Save(معرّف_المعلم))
            {
                MessageBox.Show("فشلت عملية الحفظ. الرجاء المحاولة مرة أخرى.", "خطأ",
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
            List<string> subjects = new List<string>()
      {
    "الرياضيات",
    "اللغة العربية",
    "اللغة الإنجليزية",
    "الفيزياء",
    "الكيمياء",
    "الأحياء",
    "التاريخ",
    "الجغرافيا",
    "العلوم العامة",
    "التربية الإسلامية",
    "الحاسوب",
    "الفنون",
    "التربية الرياضية",
    "الاقتصاد المنزلي",
    "اللغة الفرنسية",
    "الموسيقى",
    "التكنولوجيا",
    "العلوم الاجتماعية",
    "التربية الوطنية",
    "الفلسفة"
     };

            cbSubjects.Items.Clear();
            foreach (string subject in subjects)
            {
                cbSubjects.Items.Add(subject);
            }
            cbSubjects.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTeacherID.Text))
                return;

            int TeacherID = Convert.ToInt32(txtTeacherID.Text);
            _employee = clsالموظفون.FindByمعرّف_الموظف(TeacherID);

            if(_employee != null)
            {
                txtVollname.Text = _employee.الأشخاصInfo.الاسم_الكامل;
            }
            else
            {
                MessageBox.Show("غير موجود");
            }
        }

        private void txtTeacherID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTeacherID.Text))
            {
                txtVollname.Clear();
                cbSubjects.SelectedIndex = -1;
            }
             
        }

        private void txtVollname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVollname.Text))
            {

                txtTeacherID.Clear();
                cbSubjects.SelectedIndex = -1;
            }
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
                if (filterColumn == "معرّف_المعلم" && _IsNumber(filterValue))
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
            clsمواد_المعلم TeacherSubjects = clsمواد_المعلم.FindByمعرّف_مادة_المعلم(TeacherSujectID);

            _LoadData(TeacherSubjects);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTeacherID.Text) && string.IsNullOrEmpty(txtVollname.Text)  )
            {
                MessageBox.Show("يرجى تحديد صف صالح قبل المتابعة.", "مطلوب اختيار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //wa make it sure, that the user will do the Transaction.
            bool warningMessage = MessageBox.Show("هل أنت متأكد أنك تريد حذف بيانات هذا المعلم؟", "تحذير",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            //if No, we go back from the methode
            if (!warningMessage)
                return;

            int TeacherSubjectID = (int)dgvTeacherSubject.CurrentRow.Cells[0].Value;
            clsمواد_المعلم TeacherSubject = clsمواد_المعلم.FindByمعرّف_مادة_المعلم(TeacherSubjectID);
            if (TeacherSubject == null)
            {
                MessageBox.Show("لم يتم العثور على مادة مرتبطة بالمعلم للبيانات المحددة.", "مطلوب تحديد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (warningMessage && clsمواد_المعلم.Deleteمواد_المعلم(TeacherSubjectID))
            {
                _ResetDefaultValue();
                _LoadTeacherSubjectDataFromDatabase();
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء حذف مادة المعلم. الرجاء المحاولة مرة أخرى.", "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
