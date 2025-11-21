using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormResetPassword : Form
    {
        public FormResetPassword()
        {
            InitializeComponent();
        }
        private UserMainForm parentForm;
        public FormResetPassword(UserMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }
    }
}
