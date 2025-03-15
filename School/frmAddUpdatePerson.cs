using Guna.UI2.WinForms;
using SchoolsDb_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schools
{
    public partial class frmAddUpdatePerson: Form
    {
        public enum enPersonType
        {
            ITAdministrator,
            SchoolAssistantOrTeachingAssistant,
            SupportStaffOrSupervisoryStaff,
            Librarian,
            SchoolSocialWorker,
            CaretakerOrJanitor,
            Secretary,
            Student,
            Principal,
            Teacher
        }
        enPersonType _personTyp;
        public enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode;

        clsPersons _persons;
        int? _personID = null;
        public frmAddUpdatePerson(int? personID,enMode mode, enPersonType personTyp)
        {
            InitializeComponent();
            this._personID = personID;
            this._Mode = mode;
            this._personTyp = personTyp;
        }

        private void _ResetDefaultValue()
        {
            _FillComboBoxCity();

            txtFirstname.Clear();
            txtLastname.Clear();
            cbGender.SelectedIndex = 0;
            txtEmail.Clear();
            txtPhone.Clear();
            dtpDateOfBirth.Value = DateTime.Now;
        }

        private bool _IsInputValid(Guna2TextBox textname, string message)
        {
            if(string.IsNullOrEmpty(textname.Text))
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
            isValid &= _IsInputValid(txtFirstname, "Firstname cannot be empty!");
            isValid &= _IsInputValid(txtLastname, "Lastname cannot be empty!");

            return isValid;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            if (_Mode == enMode.Update)
                _LoadPersonData();
        }

        private void _LoadPersonData()
        {
            _persons = clsPersons.FindByPersonID(_personID);
            
            if(_persons == null)
            {
                MessageBox.Show("No Person found with this given ID. Please try again.", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!_CheckInput())
                return;

            _FillPersonData(); //fill the person data

            if(!_persons.Save())
            {
                MessageBox.Show("Save operation fore Person failed. Please try again.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(_Mode == enMode.Update)
            {
                MessageBox.Show("Person updated successfully.", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    
            if (!_AddTyp(_persons))
            {
                MessageBox.Show($"Failed to add Person Typ. Please try again.", "Error",
                                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _Mode = enMode.Update;
            _ResetDefaultValue();
        }
        
        private bool _AddTyp(clsPersons persons)
        {
            switch(_personTyp)
            {
                case enPersonType.Student:
                    {
                        int? studentID = null;
                        if(clsStudents.AddNewStudents(ref studentID, persons.PersonID))
                        {                         
                            MessageBox.Show($"Student added successfully with ID [ {studentID} ].", "Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        break;
                    }
                case enPersonType.Teacher:
                    {
                        int? employeeID = null;
                        if(clsEmployees.AddNewEmployees(ref employeeID, persons.PersonID, "Teacher"))
                        {
                            MessageBox.Show($"Teacher added successfully with ID [ {employeeID} ].", "Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        break;
                    }

                case enPersonType.Principal:
                    {
                        int? employeeID = null;
                        if (clsEmployees.AddNewEmployees(ref employeeID, persons.PersonID, "Principal"))
                        {
                            MessageBox.Show($"Principal added successfully with ID [ {employeeID} ].", "Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        break;
                    }
                case enPersonType.Librarian:
                    {
                        int? employeeID = null;
                        if (clsEmployees.AddNewEmployees(ref employeeID, persons.PersonID, "Librarian"))
                        {
                            MessageBox.Show($"Librarian added successfully with ID [ {employeeID} ].", "Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        break;
                    }
            }
            return false;

        } 
  
        private void btnAddAndClose_Click(object sender, EventArgs e)
        {
            if (!_CheckInput())
                return;

            _FillPersonData(); //fill the person data

            if (!_persons.Save())
            {
                MessageBox.Show("Save operation fore Person failed. Please try again.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_Mode == enMode.Update)
            {
                MessageBox.Show("Person updated successfully.", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            if (!_AddTyp(_persons))
            {
                MessageBox.Show($"Failed to add Person Typ. Please try again.", "Error",
                                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _Mode = enMode.Update;
            this.Close();
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
                cbCity.SelectedIndex = 0; // Wählt die erste Stadt in der Liste aus
            }
        }
    }
}
