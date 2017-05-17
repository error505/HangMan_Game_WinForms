using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Vjesalo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string rec = "";

        List<Label> labels = new List<Label>();

        int kolicina = 0;

        enum DjeloviTijela
        {
            Glava,
            Lijevo_Oko,
            Desno_Oko,
            Usta,
            Desna_Ruka,
            Lijeva_Ruka,
            Tijelo,
            Desna_Noga,
            Lijeva_Noga
        }
        void NacrtajVjesalo()
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Brown, 10);
            g.DrawLine(p, new Point(210, 354), new Point(210, 5));
            g.DrawLine(p, new Point(215, 5), new Point(85, 5));
            g.DrawLine(p, new Point(80, 0), new Point(80, 50));
            //NacrtajDijeloveTijela(DjeloviTijela.Glava);
            //NacrtajDijeloveTijela(DjeloviTijela.Lijevo_Oko);
            //NacrtajDijeloveTijela(DjeloviTijela.Desno_Oko);
            //NacrtajDijeloveTijela(DjeloviTijela.Usta);
            //NacrtajDijeloveTijela(DjeloviTijela.Tijelo);
            //NacrtajDijeloveTijela(DjeloviTijela.Lijeva_Ruka);
            //NacrtajDijeloveTijela(DjeloviTijela.Desna_Ruka);
            //NacrtajDijeloveTijela(DjeloviTijela.Lijeva_Noga);
            //NacrtajDijeloveTijela(DjeloviTijela.Desna_Noga);         
        }

        void NacrtajDijeloveTijela(DjeloviTijela dt)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Blue, 2);
            if (dt == DjeloviTijela.Glava)
                g.DrawEllipse(p, 52, 50, 52, 52);
            else if (dt == DjeloviTijela.Lijevo_Oko)
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 63, 60, 5, 5);
            }
            else if (dt == DjeloviTijela.Desno_Oko)
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 83, 60, 5, 5);
            }
            else if (dt == DjeloviTijela.Usta)
                g.DrawArc(p, 65, 65, 20, 20, 45, 90);
            else if (dt == DjeloviTijela.Tijelo)
                g.DrawLine(p, new Point(80, 100), new Point(80, 170));
            else if (dt == DjeloviTijela.Lijeva_Ruka)
                g.DrawLine(p, new Point(80, 120), new Point(30, 85));
            else if (dt == DjeloviTijela.Desna_Ruka)
                g.DrawLine(p, new Point(80, 120), new Point(130, 85));
            else if (dt == DjeloviTijela.Lijeva_Noga)
                g.DrawLine(p, new Point(80, 170), new Point(30, 250));
            else if (dt == DjeloviTijela.Desna_Noga)
                g.DrawLine(p, new Point(80, 170), new Point(130, 250));
        }

        void NapraviLabels()
        {
           rec = UzmiNasumicnuRijec();
           char[] chars = rec.ToCharArray();
           int izmedju = 330 / chars.Length - 1;
           for (int i = 0; i < chars.Length - 1; i++)
           {
               labels.Add(new Label());
               labels[i].Location = new Point((i * izmedju) + 10, 80);
               labels[i].Text = "_";
               labels[i].Parent = groupBox2;
               labels[i].BringToFront();
               labels[i].CreateControl();
           }
            label1.Text = "Duzina Rijeci: " + (chars.Length - 1).ToString();
        }
        
        string UzmiNasumicnuRijec()
        {
            WebClient wc = new WebClient();
            string listaRijeci = wc.DownloadString("http://igoriric.com/wp-content/uploads/2013/03/lista.txt");
            string[] rijeci = listaRijeci.Split('\n');
            Random r = new Random();
            return rijeci[r.Next(0, rijeci.Length -1)];
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            NacrtajVjesalo();
            NapraviLabels();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            char slovo = textBox1.Text.ToLower().ToCharArray()[0];
            if (rec.Contains(slovo))
            {
                char[] slova = rec.ToCharArray();
                for (int i = 0; i < slova.Length; i++)
                {
                    if (slova[i] == slovo)
                        labels[i].Text = slovo.ToString();
                }
                foreach (Label l in labels)
                    if (l.Text == "_") return;
                MessageBox.Show("Pobjedili ste!", "Cestitamo");
                Reset();
            }
            else
            {
                MessageBox.Show("Slovo koje ste probali nije u rijeci!");
                label2.Text += " " + slovo.ToString() + ",";
                NacrtajDijeloveTijela((DjeloviTijela)kolicina);
                kolicina++;
                if (kolicina == 9)
                {
                    MessageBox.Show("Izgubili ste! Rijec je bila " + rec);
                    Reset();
                } 
            }
        }
        void Reset()
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(panel1.BackColor);
            UzmiNasumicnuRijec();
            NapraviLabels();
            NacrtajVjesalo();
            label2.Text = "Promasaja: ";
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == rec)
            {
                MessageBox.Show("Pobjedili ste!", "Cestitamo");
                Reset();
            }
            else
            {
                MessageBox.Show("Rijec koju ste probali je pogresna!" , "Greska!");
                NacrtajDijeloveTijela((DjeloviTijela)kolicina);
                kolicina++;
                if (kolicina == 9)
                {
                    MessageBox.Show("Izgubili ste! Rijec je bila " + rec);
                    Reset();
                } 
            }
        }

    }
}
