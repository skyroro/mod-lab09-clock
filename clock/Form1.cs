using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace mod_lab09_clock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();    
        }
        private void Form1_Paint(object sender, PaintEventArgs e)//Главный код программы сосредоточен в функции-обработчике события
        {
            Graphics g = e.Graphics;
            GraphicsState gs;
            gs = g.Save();

            int centerX = this.Width / 2;
            int centerY = this.Height / 2;
            int radius = 150;
            int fontSize = 20;

            // рисуем основу
            Pen thinBlackPen = new Pen(Color.Black, 1);
            Pen thickBlackPen = new Pen(Color.Black, 2);
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
            g.FillEllipse(brush, centerX - radius, centerY - radius, 2*radius, 2 * radius);
            g.DrawEllipse(thickBlackPen, centerX - radius, centerY - radius, 2*radius, 2*radius);
            g.DrawEllipse(thickBlackPen, centerX, centerY, 4, 4);

            // рисуем циферблат
            for (int i = 1; i <= 12; i++)
            {
                double angle = i * Math.PI / 6 - Math.PI / 2;
                int x = centerX + (int)((radius - 1.5 * fontSize) * Math.Cos(angle));
                int y = centerY + (int)((radius - 1.5 * fontSize) * Math.Sin(angle));
                float koeff = (float)(fontSize * 0.7);
                g.DrawString(i.ToString(), new Font("Arial", fontSize), Brushes.Black, x - koeff, y - koeff);
            }

            // рисуем секундные деления
            for (int i = 0; i <= 60; i++)
            {
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(6 * i);
                g.DrawLine(thinBlackPen, 0, -radius, 0, -radius + 7);
                g.Restore(gs);
                gs = g.Save();
            }

            // рисуем минутные деления
            for (int i = 0; i < 12; i++)
            {
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(30 * i);
                g.DrawLine(thickBlackPen, 0, -radius, 0, -radius + 12);
                g.Restore(gs);
                gs = g.Save();
            }

            // рисуем стрелки
            DateTime dt = DateTime.Now;
            float secondAngle = 6 * dt.Second - 90;
            float minuteAngle = 6 * (dt.Minute + (float)dt.Second / 60) - 90;
            float hourAngle = 30 * (dt.Hour % 12) + dt.Minute / 2 - 90;

            Pen secondPen = new Pen(Color.Red, 1);
            Pen minutePen = new Pen(Color.Black, 2);
            Pen hourPen = new Pen(Color.Black, 3);

            g.TranslateTransform(centerX, centerY);

            // часовая стрелка
            gs = g.Save();
            g.RotateTransform(30 * (dt.Hour + (float)dt.Minute / 60 + (float)dt.Second / 3600));
            Point[] points1 = new Point[4];
            points1[0] = new Point(0, 0);
            points1[1] = new Point(-4, -20);
            points1[2] = new Point(0, -radius + 60);
            points1[3] = new Point(4, -20);
            g.FillPolygon(Brushes.Black, points1);
            g.Restore(gs);

            // минутная стрелка
            gs = g.Save();
            g.RotateTransform(6 * (dt.Minute + (float)dt.Second / 60));
            Point[] points2 = new Point[4];
            points2[0] = new Point(0, 0);
            points2[1] = new Point(-3, -20);
            points2[2] = new Point(0, -radius + 30);
            points2[3] = new Point(3, -20);
            g.FillPolygon(Brushes.Black, points2);
            g.Restore(gs);

            // секундная стрелка
            gs = g.Save();
            g.RotateTransform(6 * (float)dt.Second);
            g.DrawLine(secondPen, 0, 0, 0, -radius+10);
            g.Restore(gs);          
        }
    }
}