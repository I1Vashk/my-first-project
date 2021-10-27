using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snail1
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 start = new Form1(); // создание формы
            start.ShowDialog();

        }

        private void Exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
    }
}
