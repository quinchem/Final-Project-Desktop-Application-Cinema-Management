using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp.Forms
{
    public partial class FormResetPassword : Form
    {
        public FormResetPassword()
        {
            InitializeComponent();
        }
        private AdminMainForm parentForm;
        public FormResetPassword(AdminMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

    }
}
