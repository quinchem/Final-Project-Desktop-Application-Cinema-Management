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
    public partial class FormFilmDetail : Form
    {
        public FormFilmDetail()
        {
            InitializeComponent();
        }

        private void btnDatVe_Click(object sender, EventArgs e)
        {
            // Tìm MainForm để gọi OpenChildForm()
            UserMainForm parent = this.ParentForm as UserMainForm;

            if (parent != null)
            {
                parent.OpenChildForm(new FormShowtimeDetail());
            }
        }
    }
}
