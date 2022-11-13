using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace gameCircle
{
    public partial class Form1 : Form
    {
        //змінні колів
        public Bitmap HandlerTexture = Resource1.Hudler,
                      TargetTexture = Resource1.Target;

        private Point _targerPosition = new Point(300,300); // позиція точки в яку ми маємо попадати
        private Point _direction = Point.Empty; //напрямок жовтого кола
        private int _score = 0; //очки
        private int speed = 10; //швидкість руху 

        public Form1()
        {
            InitializeComponent();

            //плавніший бліки
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            Cursor.Hide();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //оновлення форми (рух колів)
            Refresh();
        }
        //рух жовтого кола
        private void timer2_Tick(object sender, EventArgs e)
        {
            Random r = new Random();

            timer2.Interval = r.Next(500, 1000);
            _direction.X = r.Next(-1, 2);
            _direction.Y = r.Next(-1, 2);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var localPosition = this.PointToClient(Cursor.Position); //позиція курсова


            _targerPosition.X += _direction.X * speed;
            _targerPosition.Y += _direction.Y * speed;

            if(_targerPosition.X < 50 || _targerPosition.X > 950)
            {
                _direction.X *= -1;
            }
            if (_targerPosition.Y < 50 || _targerPosition.Y > 550)
            {
                _direction.Y *= -1;
            }

            Point between = new Point(localPosition.X - _targerPosition.X, localPosition.Y - _targerPosition.Y);
            float distance = (float)Math.Sqrt((between.X * between.X) + (between.Y * between.Y));
            if (distance < 20)
            {
                AddScore(1);
            }


            if (_score == 50)
            {
                speed = 15;
            }
            else if (_score == 100)
            {
                speed = 17;
            }
            else if (_score == 150)
            {
                speed = 22;
            }else if (_score == 200)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                Cursor.Show();
                System.Windows.MessageBox.Show("Вітаємо, ви виграли!", "Юхууу", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }

            var handleRect = new Rectangle(localPosition.X - 50, localPosition.Y - 50, 100, 100); // коло по кординамтам курсора
            var targetRect = new Rectangle(_targerPosition.X - 50, _targerPosition.Y - 50, 100, 100); //координати жовтого кола
            
            //додавання колів на форму
            g.DrawImage(TargetTexture, handleRect);
            g.DrawImage(HandlerTexture, targetRect);
        }

        //додавання очків 
        private void AddScore(int score)
        {
            _score += score;
            scoreLabel.Text = "Рахунок: "  + _score.ToString();
        }
    }
}
