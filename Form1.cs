using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        int days = 0;
        int speed = 1;
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        public Form1()
        {
            InitializeComponent();

            foreach (CheckBox cb in tableLayoutPanel1.Controls)
            {
                field.Add(cb, new Cell());
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked) Plant(cb);
            else Harvest(cb);
        }

        private void Plant(CheckBox cb)
        {
            field[cb].Plant();
            UpdateBox(cb);
            moneyLabel.Text = "Money: " + (Cell.money).ToString();
        }

        private void Harvest(CheckBox cb)
        {
            field[cb].Harvest();
            UpdateBox(cb);
            moneyLabel.Text = "Money: " + (Cell.money).ToString();
        }

      private void timer1_Tick(object sender, EventArgs e)
        {
            daysLabel.Text = "Days: " + (days++);

            foreach (CheckBox cb in tableLayoutPanel1.Controls)
            {
                NextState(cb);
            }
        }

        private void NextState(CheckBox cb)
        {
            field[cb].NextState();
            UpdateBox(cb);
        }

        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case State.Planted: 
                    c = Color.Black;
                    break;
                case State.PlantShoots:
                    c = Color.Green;
                    break;
                case State.NearbyMature:
                    c = Color.Yellow;
                    break;
                case State.Mature:
                    c = Color.Red;
                    break;
                case State.Rotten:
                    c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }

        private void btnFaster_Click(object sender, EventArgs e)
        {
            try
            {
                if (timer1.Interval <= 100)
                {
                    timer1.Interval -= 10;
                    ++speed;
                }
                else if (timer1.Interval == 1000)
                {
                    timer1.Interval -= 500;
                    ++speed;
                }
                else
                {
                    timer1.Interval -= 100;
                    ++speed;
                }
            }
            catch (Exception)
            {
                timer1.Interval = 10;
            }

            labelSpeed.Text = "Speed: " + speed;
        }

        private void btnSlower_Click(object sender, EventArgs e)
        {
            if (timer1.Interval < 100)
            {
                timer1.Interval += 10;
                --speed;
            }
            else if(timer1.Interval == 500)
            {
                timer1.Interval += 500;
                --speed;
            }
            else
            {
                timer1.Interval += 100;
                --speed;
            }
            if (timer1.Interval >= 1000) timer1.Interval = 1000;
            if (speed <= 0) speed = 1;

            labelSpeed.Text = "Speed: " + speed;
        }
    }

    enum State
    {
        Empty,
        Planted,
        PlantShoots,
        NearbyMature,
        Mature,
        Rotten
    }

    class Cell
    {
        public State state = State.Empty;
        public int progress = 0;
        public static int money = 100;

        private const int prPlanted = 20;
        private const int prPlantShoots = 80;
        private const int prNearbyMature = 120;
        private const int prMature = 140;



        public void Plant()
        {
            money -= 1; // почему-то умножает на 2
            if (money <= 0)
            {
                MessageBox.Show("Закончились деньги, бизнес прогорел...");
                Application.Exit();
            }
            else
            {
                state = State.Planted;
                progress = 1;
            }
        }

        public void Harvest()
        {
            switch (state)
            {
                case State.NearbyMature:
                    money += 3;
                    break;
                case State.Mature:
                    money += 5;
                    break;
                case State.Rotten:
                    money -= 1;
                    break;
            }
            if (money <= 0)
            {
                MessageBox.Show("Закончились деньги, бизнес прогорел...");
                Application.Exit();
            }
            else
            {
                state = State.Empty;
                progress = 0;
            }
        }

        public void NextState()
        {
            if (state != State.Rotten && state != State.Empty)
            {
                progress++;
                if (progress < prPlanted) state = State.Planted;
                else if (progress < prPlantShoots) state = State.PlantShoots;
                else if (progress < prNearbyMature) state = State.NearbyMature;
                else if (progress < prMature) state = State.Mature;
                else state = State.Rotten;
            }
        }
    }
}
