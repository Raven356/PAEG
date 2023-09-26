using Microsoft.EntityFrameworkCore;
using PAEG1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAEG1
{
    public partial class AdminForm : Form
    {
        private Paeg1Context _context;

        public AdminForm(Paeg1Context context)
        {
            InitializeComponent();
            _context = context;
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var forms = Application.OpenForms;
            for (int i = 0; i < forms.Count; i++)
            {
                if (forms[i] != this && !forms[i].Visible)
                    forms[i].Close();
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            _context.Candidates.Load();
            dataGridView1.DataSource = _context.Candidates.Local.ToBindingList();
        }
    }
}
