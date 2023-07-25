using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MS
{
    internal class Minesweeper
    {
        public int scale = 30;
        public MinesweeperForm form;

        public int width;
        public int height;
        public int amount;

        public Cell[,] field;

        public bool firstClick = true;
        public bool finished = false;

        public int flagAmount = 0;

        public DateTime startTime = DateTime.MinValue;
        public TimeSpan duration;

        public bool showCellBorders;

        public Minesweeper(MinesweeperForm form, int width, int height, int amount, int scale = 35, bool showCellBorders = false)
        {
            this.form = form;
            this.width = width;
            this.height = height;
            this.amount = amount;
            this.scale = scale;

            this.flagAmount = this.amount;

            this.showCellBorders = showCellBorders;

            this.form.Text = $"Minesweeper {this.width} x {this.height} ({this.amount})";

            this.UpdateFlagCounter();
        }

        public void CreateCells(System.Windows.Forms.Panel element)
        {
            this.field = new Cell[height, width];

            element.Controls.Clear();

            Random rand = new Random();

            string[] foliage = new string[] { "🌷", "🌳", "🍄", "🍀" };

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    Button cell = new Button();
                    cell.Size = new Size(scale, scale);
                    cell.Location = new Point(x * scale + 3, y * scale + 3);
                    cell.FlatStyle = FlatStyle.Flat;
                    cell.Name = $"{x}x{y}";
                    cell.MouseDown += ClickHandler;
                    cell.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 104, 104, 104);
                    cell.Font = new Font("Arial", this.scale / 2, FontStyle.Bold);
                    if (rand.Next(10) == 0) cell.Text = foliage[rand.Next(foliage.Length)];
                    cell.ForeColor = Color.FromArgb(255, 104, 104, 104);
                    
                    if (showCellBorders)
                    {
                        cell.FlatAppearance.BorderSize = 1;
                    }
                    else
                    {
                        cell.FlatAppearance.BorderSize = 0;
                    }

                    this.field[y, x] = new Cell(cell);

                    element.Controls.Add(cell);
                }
            }
        }

        public void Start(int click_x, int click_y)
        {
            Random rand = new Random();

            int mines_amount = amount;
            while (mines_amount > 0)
            {
                int rand_x = rand.Next(this.width);
                int rand_y = rand.Next(this.height);

                if (rand_x == click_x && rand_y == click_y) continue;

                if (!field[rand_y, rand_x].isMine)
                {
                    field[rand_y, rand_x].SetMine();
                    mines_amount--;
                }
            }

            this.CountNeightbors();

            this.startTime = DateTime.Now;
        }

        private void UpdateFlagCounter()
        {
            this.form.UpdateFlagLabel($"⚑ {this.flagAmount}");
        }

        private void ClickHandler(object sender, MouseEventArgs e)
        {
            if (finished) return;

            Button button = sender as Button;

            int x = Convert.ToInt32(button.Name.Split('x')[0]);
            int y = Convert.ToInt32(button.Name.Split('x')[1]);

            Cell cell = this.field[y, x];

            if (firstClick)
            {
                firstClick = false;
                this.Start(x, y);
                this.OpenCell(cell);

                this.CheckWin();
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (cell.flagged)
                {
                    cell.RemoveFlag();
                    this.flagAmount++;
                    this.UpdateFlagCounter();
                }
                else
                {
                    if (cell.hidden && this.flagAmount > 0)
                    {
                        cell.SetFlag();
                        this.flagAmount--;
                        this.UpdateFlagCounter();
                    };
                }

                this.CheckWin();
                return;
            }

            if (cell.flagged) return;

            if (cell.hidden)
            { 
                this.OpenCell(cell);
            }
            else
            {
                int flags_amount = 0;
                foreach (Cell neighbor_cell in this.GetNeightbors(cell.x, cell.y))
                {
                    if (neighbor_cell.flagged) {
                        flags_amount++;
                    }
                }

                if (flags_amount == cell.number)
                {
                    foreach (Cell neighbor_cell in this.GetNeightbors(cell.x, cell.y))
                    {
                        if (!neighbor_cell.flagged)
                        {
                            this.OpenCell(neighbor_cell);

                            if (neighbor_cell.isMine) return;
                        }
                    }
                }
            };

            this.CheckWin();
        }

        public void Lose()
        {
            for (int i = 0; i < this.field.GetLength(0); i++)
            {
                for (int j = 0; j < this.field.GetLength(1); j++)
                {
                    Cell mine_cell = this.field[i, j];

                    if (mine_cell.isMine) mine_cell.RemoveFlag();
                    if (mine_cell.isMine) mine_cell.Show();
                }
            }

            duration = DateTime.Now - startTime;
            this.finished = true;

            MessageBox.Show("Вы проиграли!");
        }

        public void CheckWin()
        {
            bool open_win = true;
            bool flag_win = true;

            for (int y = 0; y < this.field.GetLength(0); y++)
            {
                for (int x = 0; x < this.field.GetLength(1); x++)
                {
                    Cell cell = this.field[y, x];

                    if (!cell.isMine && cell.hidden) open_win = false;
                    if (cell.isMine && !cell.flagged) flag_win = false;
                }
            }

            if (open_win || flag_win)
            {
                for (int i = 0; i < this.field.GetLength(0); i++)
                {
                    for (int j = 0; j < this.field.GetLength(1); j++)
                    {
                        Cell mine_cell = this.field[i, j];

                        if (mine_cell.isMine) mine_cell.SetFlag();
                        if (!mine_cell.isMine) mine_cell.Show();
                    }
                }

                this.finished = true;
                duration = DateTime.Now - startTime;

                this.flagAmount = 0;
                this.UpdateFlagCounter();

                MessageBox.Show("Вы выиграли!");
            };
        }

        public void OpenCell(Cell cell)
        {
            cell.Show();

            if (cell.number != 0) return;
            if (cell.isMine) 
            { 
                this.Lose();
                return;
            }

            foreach (Cell neightbor_cell in this.GetNeightbors(cell.x, cell.y))
            {
                if (neightbor_cell.hidden)
                {
                    if (neightbor_cell.flagged)
                    {
                        neightbor_cell.RemoveFlag();
                        this.flagAmount++;
                        this.UpdateFlagCounter();
                    }
                    this.OpenCell(neightbor_cell);
                };
            }
        }

        public void CountNeightbors()
        {

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    Cell current_cell = this.field[y, x];
                    if (current_cell.isMine) continue;

                    foreach (Cell cell in this.GetNeightbors(x, y))
                    {
                        if (cell.isMine) current_cell.SetNumber(current_cell.number + 1);
                    }
                }
            }
        }

        public List<Cell> GetNeightbors(int x, int y)
        {
            List<Cell> cells = new List<Cell>();

            if (y > 0)
            {
                cells.Add(this.field[y - 1, x]);
                if (x > 0) cells.Add(this.field[y - 1, x - 1]);
                if (x < this.width - 1) cells.Add(this.field[y - 1, x + 1]);
            }

            if (y < this.height - 1)
            {
                cells.Add(this.field[y + 1, x]);
                if (x > 0) cells.Add(this.field[y + 1, x - 1]);
                if (x < this.width - 1) cells.Add(this.field[y + 1, x + 1]);
            }

            if (x > 0) cells.Add(this.field[y, x - 1]);
            if (x < this.width - 1) cells.Add(this.field[y, x + 1]);

            return cells;
        }
    }
}
