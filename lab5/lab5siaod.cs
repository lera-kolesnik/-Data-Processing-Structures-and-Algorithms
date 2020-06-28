using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace kochСurve
{
    public partial class Form1 : Form
    {
        public Button startDraw = new Button();
        public PictureBox pic = new PictureBox();
        public TextBox textDepth = new TextBox();
        public Label lblDepth = new Label();
        public Panel pan1 = new Panel();
        public Bitmap fracBitmap = new Bitmap(5000, 5000);
        public TextBox textFrLength = new TextBox();
        public Label lblLength = new Label();
        public Label recDepth = new Label();
        public TextBox recDepthText = new TextBox();
        public Label bTimeL = new Label();
        public TextBox bTimeT = new TextBox();
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1050, 850);
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Кривая Коха";
            startDraw.Text = "Нарисовать";
            startDraw.Size = new Size(90, 30);
            startDraw.Location = new Point(10, 9);
            startDraw.Click += new EventHandler(draw_Click);
            this.Controls.Add(startDraw);
            pic.Size = new Size(1000, 1000);
            pic.BorderStyle = BorderStyle.Fixed3D;
            pic.BackColor = Color.White;
            pic.Location = new Point(1, 1);
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            pic.MinimumSize = new Size(1000, 1000);
            this.Controls.Add(pan1);
            pan1.Controls.Add(pic);
            pan1.Location = new Point(10, 50);
            pan1.Size = new Size(1000, 750);
            pan1.AutoScroll = true;
            textDepth.Size = new Size(80, 10);
            textDepth.Location = new Point(220, 10);
            this.Controls.Add(textDepth);
            textFrLength.Size = new Size(80, 10);
            textFrLength.Location = new Point(400, 10);
            this.Controls.Add(textFrLength);
            lblDepth.Text = "Глубина"+" фрактала:";
            lblDepth.Size = new Size(120, 20);
            lblDepth.Location = new Point(100, 14);
            this.Controls.Add(lblDepth);
            lblLength.Text = "Длина прямой:";
            lblLength.Location = new Point(310, 14);
            this.Controls.Add(lblLength);
            recDepth.Text = "Глубина"+" рекурсии:";
            recDepth.Size = new Size(120, 20);
            recDepth.Location = new Point(500, 14);
            this.Controls.Add(recDepth);
            recDepthText.Size = new Size(80, 10);
            recDepthText.Location = new Point(620, 10);
            this.Controls.Add(recDepthText);

            bTimeL.Text = "Время" + " построения:";
            bTimeL.Size = new Size(120, 20);
            bTimeL.Location = new Point(720, 14);
            this.Controls.Add(bTimeL);
            bTimeT.Size = new Size(150, 10);
            bTimeT.Location = new Point(840, 10);
            this.Controls.Add(bTimeT);
        }
        public int isDigit(String line)
        {
            bool check = true;
            foreach (char c in line)
                check = Char.IsDigit(c) ? check : false;
            return check && !String.IsNullOrEmpty(line) ? Int32.Parse(line) : -1;
        }
        public Bitmap resizeBitmap(int length)
        {
            return new Bitmap(length, length);
        }
        public void draw_Click(object sender, EventArgs e)
        {
            recDepthText.Text = "";
            int N=0;
            int p = isDigit(textDepth.Text);
            int l = isDigit(textFrLength.Text);
            if (p > -1 && l>-1)
            {
                fracBitmap = resizeBitmap(l);
                Graphics g = Graphics.FromImage(fracBitmap);
                g.Clear(Color.White);
                double Y = l/2;
                double X = 0;
                long ellapledTicks = DateTime.Now.Ticks;
                b(g, X, Y, l, 0, p,ref N);
                ellapledTicks = DateTime.Now.Ticks - ellapledTicks;
                pic.Image = fracBitmap;
                recDepthText.Text = N.ToString();
                bTimeT.Text = (ellapledTicks*100).ToString()+"нс";
            }
            else
                MessageBox.Show("Некорректные данные","Error",0);
        }
        public void drawLine(Graphics g,  double x,  double y, double l, double u)
        {
            g.DrawLine(Pens.Black, (int)Math.Round(x), (int)Math.Round(y),
                      (int)Math.Round(x+l*Math.Cos(u)), (int)Math.Round(y - l * Math.Sin(u)));
        }
        public void a(Graphics g, ref double x, ref double y, double l, double u, int p,
                       ref int n)
        {
            n++;
            b(g, x, y, l, u, p, ref n);
            x += (int)Math.Round(l * Math.Cos(u));
            y -= (int)Math.Round(l * Math.Sin(u));
        }
        public void b(Graphics g,  double x,  double y, double l, double u, int p, ref int n)
        {
            if (p > 0)
            {
                l = l / 3;
                a(g, ref x, ref y, l, u, p-1, ref n);
                a(g, ref x, ref y, l, u + (Math.PI / 3), p-1, ref n);
                a(g, ref x, ref y, l, u - (Math.PI / 3), p-1, ref n);
                a(g, ref x, ref y, l, u, p-1, ref n);
            }
            else
                drawLine(g,  x, y, l,u);
        }
    }
}
