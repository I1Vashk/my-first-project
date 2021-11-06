using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace snail1
{
    public partial class Form1 : Form
    {
        Point[] p;
        Point food;
        Point super_food;
        int len;
        bool stop_game;
        int mov; // 1 - право 2 - лево 3 - вверх 4 - вниз
        int score = 0;
        int ST;
        int HighScore;
        Random rmd = new Random();

        public Form1()
        {
            InitializeComponent();
            len = 7;
            mov = 3;
            p = new Point[200];

            p[0].X = 250;
            p[0].Y = 250;
            food.X = rmd.Next(0, 50) * 10;
            food.Y = rmd.Next(0, 50) * 10;
            super_food.X = rmd.Next(0, 50) * 10;
            super_food.Y = rmd.Next(0, 50) * 10;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int i;
            for (i = 198; i >= 0; i--)
            {
                if (stop_game)
                {
                    break;
                }
                p[i + 1].X = p[i].X;
                p[i + 1].Y = p[i].Y;
            }

            switch (mov)
            {
                case 1:
                    p[0].X = p[1].X - 10;  // движение лево
                    if (p[0].X < 0)
                    {
                        p[0].X += 500;
                    }
                    p[0].Y = p[1].Y;
                    break;
                case 2:
                    p[0].X = p[1].X + 10;  // движение право
                    if (p[0].X > 500)
                    {
                        p[0].X = 0;
                    }
                    p[0].Y = p[1].Y;
                    break;
                case 3:
                    p[0].X = p[1].X;
                    p[0].Y = p[1].Y - 10;  // движение вверх
                    if (p[0].Y < 0)
                    {
                        p[0].Y = 500;
                    }
                    break;
                case 4:
                    p[0].X = p[1].X;
                    p[0].Y = p[1].Y + 10;  // движение вниз
                    if (p[0].Y > 500)
                    {
                        p[0].Y = 0;
                    }
                    break;
            }

            label1.Text = score.ToString();
            StreamReader ReadHS = new StreamReader("HighScore.txt");
            string b = ReadHS.ReadLine();
            label4.Text = b;
            HighScore = Convert.ToInt32(b);
            ReadHS.Close();

            //label5.Text = b; // 
            if (score >= HighScore)
                //HighScore = score;
            try
            {
                File.WriteAllText("HighScore.txt", null);
                StreamWriter WriteHS = new StreamWriter("HighScore.txt", true, Encoding.ASCII);
                WriteHS.WriteLine(score);
                WriteHS.Close();
            }
            catch (Exception o)
            {
                MessageBox.Show(o.Message);
            }
            finally
            {
                //label4.Text = HighScore.ToString();
            }

            for ( i = 0; i < len; i++)
            {
                if (i % 2 == 0)
                {
                    SolidBrush body1 = new SolidBrush(Color.HotPink);
                    e.Graphics.FillRectangle(body1, p[i].X, p[i].Y, 10, 10);
                }
                else
                {
                    SolidBrush body2 = new SolidBrush(Color.Pink);
                    e.Graphics.FillRectangle(body2, p[i].X, p[i].Y, 10, 10);
                }
            }

            SolidBrush head = new SolidBrush(Color.DeepPink);
            e.Graphics.FillRectangle(head, p[0].X - 2, p[0].Y - 2, 13, 13);

            SolidBrush food_draw = new SolidBrush(Color.White);
            e.Graphics.FillEllipse(food_draw, food.X, food.Y, 12, 12);

            SolidBrush super_food_draw = new SolidBrush(Color.Lime); 
            e.Graphics.FillEllipse(super_food_draw, super_food.X, super_food.Y, 13, 13);


            if (p[0].X == food.X && p[0].Y == food.Y) 
            {
                len++;
                food.X = rmd.Next(0, 50) * 10;
                food.Y = rmd.Next(0, 50) * 10;
                score++;
                if (timer1.Interval > 10)
                {
                    timer1.Interval -= 5;
                }
                else timer1.Interval = 5;
            }

            if (p[0].X == super_food.X && p[0].Y == super_food.Y)
            {
                len = len + 5;
                score = score + 5;
                super_food.X = 600; super_food.Y = 600;
                if (timer1.Interval > 30)
                {
                    timer1.Interval -= 20;
                }
                else timer1.Interval = 10;
            }
            
            for (int k = 1; k < len; k++)
            {
                if(stop_game == true)
                {
                    break;
                }
                if (p[0].X == p[k].X && p[0].Y == p[k].Y)
                {
                    stop_game = true;
                    DialogResult result = MessageBox.Show($"Ты проиграл. Попробуй еще раз! \n Твой счет: {score}", "Ты проиграл", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        Hide();
                        Form1 start = new Form1();
                        start.ShowDialog();
                    }
                    else
                    {
                        Hide();
                        FormMenu Menu = new FormMenu();
                        Menu.ShowDialog();
                    }
                }
            } 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Invalidate(); // перерисовка формы - invalidate
            if (stop_game)
            {
                timer1.Stop();
            }
        }

        private void Form1_CheckKeys(object sender, KeyEventArgs button)
        {
            switch (button.KeyCode)
            {
                case Keys.A or Keys.Left:
                    mov = 1;
                    break;
                case Keys.D or Keys.Right:
                    mov = 2;
                    break;
                case Keys.W or Keys.Up:
                    mov = 3;
                    break;
                case Keys.S or Keys.Down:
                    mov = 4;
                    break;
                case Keys.Escape:
                    timer1.Stop();
                    STimer.Stop();
                    DialogResult game_off = MessageBox.Show("Игра стоит на паузе.\nЖелаете выйти?", "Пауза", MessageBoxButtons.YesNo);
                    if (game_off == System.Windows.Forms.DialogResult.Yes)
                    {
                        Hide();
                        FormMenu start = new FormMenu();
                        start.ShowDialog();
                    }
                    else
                    {
                        timer1.Start();
                        STimer.Start();
                    }
                    break;
                default:
                    timer1.Stop();
                    STimer.Stop();
                    DialogResult Uprav = MessageBox.Show("Управление на стрелочки или WASD", "Чел, управление...", MessageBoxButtons.OK);
                    if (Uprav == System.Windows.Forms.DialogResult.OK)
                    {
                        timer1.Start();
                        STimer.Start();
                    }
                    break;

            }
        }

        private void Stimer_Tick(object sender, EventArgs e)
        {
            STimer.Interval = 1000;
            ST++;
            for ( ;ST == 10; ST = 0)
            {
                super_food.X = rmd.Next(0, 50) * 10;
                super_food.Y = rmd.Next(0, 50) * 10;
            }
        }

    }
}