using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS
{
    public partial class CustomFieldForm : Form
    {
        public CustomFieldForm()
        {
            InitializeComponent();
        }

        public int width = 9;
        public int height = 9;
        public int amount = 10;

        private void clearButton_Click(object sender, EventArgs e)
        {
            this.widthTextBox.Text = this.heightTextBox.Text = this.amountTextBox.Text = "";
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            int width = 9;
            int height = 9;
            int amount = 9;
            try
            {
                width = int.Parse(this.widthTextBox.Text);
            }
            catch { }

            try
            {
                height = int.Parse(this.heightTextBox.Text);
            }
            catch { }

            try
            {
                amount = int.Parse(this.amountTextBox.Text);
            }
            catch { }

            this.width = Math.Min(Math.Max(width, 9), 50);
            this.height = Math.Min(Math.Max(height, 9), 25);
            this.amount = Math.Min(Math.Max(amount, 10), (int)Math.Floor(this.width * this.height * 0.8));

            this.widthTextBox.Text = this.width.ToString();
            this.widthTextBox.Text = this.height.ToString();
            this.widthTextBox.Text = this.amount.ToString();

            this.Close();
        }
    }
}
