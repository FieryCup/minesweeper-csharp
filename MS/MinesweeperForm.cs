using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS
{
    public partial class MinesweeperForm : Form
    {
        private Minesweeper game;

        private int[] widths = new int[] { 9, 16, 30 };
        private int[] heights = new int[] { 9, 16, 16 };
        private int[] amounts = new int[] { 10, 40, 99 };

        private int width;
        private int height;
        private int amount;

        private bool customMode = false;

        private bool showCellBorders = false;

        public MinesweeperForm()
        {
            InitializeComponent();
            this.difficultToolStripMenuItem.SelectedIndex = 0;

            this.gameMenuStrip.Renderer = new MenuRender();

            foreach (ToolStripMenuItem menuItem in this.gameMenuStrip.Items)
                ((ToolStripDropDownMenu)menuItem.DropDown).ShowImageMargin = false;
        }

        public void UpdateFlagLabel(string text)
        {
            this.flagsLabel.Text = text;
        }

        private void StartGame(int selected_index)
        {
            if (selected_index < 0) selected_index = 0;


            if (!this.customMode)
            {
                this.width = this.widths[selected_index];
                this.height = this.heights[selected_index];
                this.amount = this.amounts[selected_index];
            }

            this.game = new Minesweeper(this, this.width, this.height, this.amount, showCellBorders: this.showCellBorders);
            this.game.CreateCells(this.fieldPanel);

            //this.controlsGroupBox.Width = this.fieldGroupBox.Width;

            this.gameTimer.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            StartGame(0);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGame(this.difficultToolStripMenuItem.SelectedIndex);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.game.finished)
            {
                this.gameTimer.Enabled = false;
                return;
            }
            if (this.game.startTime == DateTime.MinValue)
            {
                this.timeLabel.Text = "00:00:00.00";
                return;
            }
            this.timeLabel.Text = (DateTime.Now - this.game.startTime).ToString(@"hh\:mm\:ss\.ff");
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame(this.difficultToolStripMenuItem.SelectedIndex);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Приложение Windows Forms (.NET Framework) C#\n\n" +
                "Разработал: FieryCup"
            );
        }

        private void cellBordersToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.showCellBorders = this.cellBordersToolStripMenuItem.Checked;
        }

        private void difficultToolStripMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox difficultMenu = sender as ToolStripComboBox;

            if (difficultMenu.SelectedIndex != difficultMenu.Items.Count - 1)
            {
                this.customMode = false;
                return;
            }

            CustomFieldForm custom_form = new CustomFieldForm();
            this.Hide();
            custom_form.ShowDialog();
            this.Show();

            // TODO: Добавить ввод значений

            this.width = custom_form.width;
            this.height = custom_form.height;
            this.amount = custom_form.amount;
            this.customMode = true;
        }
    }
}
