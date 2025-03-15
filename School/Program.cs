using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schools
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new frmAddUpdatePerson(null,frmAddUpdatePerson.enMode.AddNew, frmAddUpdatePerson.enPersonType.Principal));
            Application.Run(new frmStudentsForm());
        }
    }
}
