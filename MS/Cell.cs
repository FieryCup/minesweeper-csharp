using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS
{
    internal class Cell
    {
        public bool isMine;
        public Button button;
        public int number = 0;
        public int x;
        public int y;
        public bool hidden = true;
        public bool flagged = false;

        public Cell(Button button)
        {
            this.button = button;
            this.x = Convert.ToInt32(button.Name.Split('x')[0]);
            this.y = Convert.ToInt32(button.Name.Split('x')[1]);
        }

        public void SetMine()
        {
            this.isMine = true;
        }

        public void SetNumber(int number)
        {
            this.number = number;
        }

        public void Show()
        {
            this.hidden = false;

            this.Update();

            //if (this.number == 0) this.button.Enabled = false;
        }

        public void SetFlag()
        {
            this.flagged = true;

            Update();
        }

        public void RemoveFlag()
        {
            this.flagged = false;

            Update();
        }

        public void Update()
        {
            Color[] colors = new Color[] {
                Color.DeepSkyBlue,
                Color.YellowGreen,
                Color.OrangeRed,
                Color.Violet,
                Color.Red,
                Color.Aquamarine,
                Color.Silver,
                Color.DarkGray,
            };

            this.button.BackColor = Color.FromArgb(255, 72, 72, 72);

            if (this.flagged)
            {
                this.button.Text = "⚑";
                this.button.ForeColor = Color.Red;
                return;
            }

            if (this.hidden)
            {
                this.button.Text = " ";
                this.button.ForeColor = Color.FromArgb(255, 104, 104, 104);
                this.button.BackColor = Color.FromArgb(255, 23, 23, 23);
                return;
            }

            if (this.isMine)
            {
                this.button.Text = "⭙";
                this.button.ForeColor = Color.Transparent;
                this.button.BackColor = Color.FromArgb(255, 239, 68, 68);
                return;
            }

            if (this.number == 0)
            {
                this.button.Text = "░";
                return;
            };

            this.button.Text = Convert.ToString(number);
            this.button.ForeColor = colors[number - 1];
        }
    }
}
