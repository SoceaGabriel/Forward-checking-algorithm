using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapColoring
{
    public partial class Form1 : Form
    {
        //Algoritmul forward Checking
        ForwardChecking forwardCheching = new ForwardChecking();

        Graphics g;
        Pen black;
        SolidBrush red;

        //Variabile folosite pentru desenare grafului pe suprafata de desenare
        int radius = 30; // raza cercuilor ce reprezinta nodurile
        bool drawingLine = false; // flag ce spune daca se deseneaza o linie(se creeaza o legaura intre noduri) pe suprafata de desenare
        int x1, y1;//Coordnatele primului punct cu ajutorul caruia desenam o linie
        int startNode;//Primul nod de la care se face o legatura


        List<Node> nodes = new List<Node>();// Lista de noduri ce reprezinta graficul desenat

        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            black = new Pen(Color.Black, 4);
            red = new SolidBrush(Color.Red);
            g.TranslateTransform(0, 0);

        }


        //Handler-ul butonului care da startul algoritmului
        private void tryButton_Click(object sender, EventArgs e)
        {
            List<Node> coloredNodes;

            if (nodes.Count() != 0)
                forwardCheching.setGraph(nodes);
            else
            {
                display.Text = "Desenati un graf corespunzator unei harti!";
                return;
            }

            display.Text = "";
            if (!forwardCheching.StartAlgoritm(0))
                display.Text = "Graful nu corespunde unei harti!";
            else
                for(int i = 0; i<nodes.Count(); i++)
                {
                    display.Text += "NODE " + i + " -> " + nodes[i].color + "\r\n";
                }

            panel1.Refresh();
        }

        //Handlerul apelat atunci cand se transmite un eveniment de tip PAINT catre panel 1
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            //Pentru fiecare grapf se desenaza un cerc, se umple cercul 
            for (int i = 0; i< nodes.Count; i++)
            {
                g.DrawEllipse(black, nodes[i].x - radius, nodes[i].y - radius, 2 * radius, 2 * radius);
                g.FillEllipse(new SolidBrush(Color.FromName(nodes[i].color)), nodes[i].x - radius, nodes[i].y - radius, 2 * radius, 2 * radius);
                g.DrawString(Convert.ToString(i), new Font("Arial", 16), new SolidBrush(Color.Black), nodes[i].x - 40, nodes[i].y - 40);
   
            }

            // Pentru fiecare vecin al unui nod se desenaza o linie ce reprezinta legatura dintre doua noduri
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].neighbors.Count(); j++)
                    g.DrawLine(black, nodes[i].x, nodes[i].y, nodes[i].neighbors[j].x, nodes[i].neighbors[j].y);
            }
        }

        //Reseteaza suprafata de desenare si lista de noduri;
        private void clearButton_Click(object sender, EventArgs e)
        {
            nodes.Clear();
            panel1.Invalidate();
        }

        //Functie apelata cand se intampla un eveniment de tip "MouseClick", utilizata pentru desenarea si reprezentarea graficului;
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {

            for (int i = 0; i < nodes.Count(); i++)
            {
                double distance = Math.Sqrt(Math.Pow((double)(e.X - nodes[i].x), 2) + Math.Pow((double)(e.Y - nodes[i].y), 2));
                if (distance < radius)// Cazul in care s-a facut un click in interiorul unui
                {
                    if (!drawingLine)// Cazul in care se incepe desenarea unei linii/legaturi (FLAGUL drawingLine nu este setat
                    {
                        drawingLine = true; // se seteaza FLAGUL drawinLine
                        startNode = i; // se memoreaza nodul de la care se incepe legatura
                        x1 = nodes[i].x;
                        y1 = nodes[i].y;
                    }
                    else // Cazul in care se termina desenare unei legaturi.( FLAGUL drawingLine este setat)
                    {
                        if(i != startNode && !nodes[i].neighbors.Contains(nodes[startNode])) // daca al doilea nod selectat nu este este chiar primul si nu este deja in vecinii primului nod
                        {
                            g.DrawLine(black, x1, y1, nodes[i].x, nodes[i].y); 
                            nodes[i].neighbors.Add(nodes[startNode]);
                            nodes[startNode].neighbors.Add(nodes[i]);
                        }

                        drawingLine = false;
                    }
                    
                    return;
                }
            }

            if(drawingLine) // Daca flagul drawingLine e setat dar s-a dat click pe un spatiu gol, se reseteaza flagul
            {
                drawingLine = false;
                return;
            }

            //Daca am auns aici inseamana ca s-a dat click pe o zona goala sa suprafetei de desenare
            for (int i = 0; i < nodes.Count(); i++)
            {
                double distance = Math.Sqrt(Math.Pow((double)(e.X - nodes[i].x), 2) + Math.Pow((double)(e.Y - nodes[i].y), 2));
                if (distance < 2 * radius + 10) // Se verfica daca s-a dat click la o distanta destul de mare fata de orice nod deja desenat, daca nu iesim din functie
                    return;
            }

            //Daca am ajuns aici inseamna ca urmeaza a se desena un nou nod

            Node node = new Node(); // Se creeaza in nou nod in memorie
            node.x = e.X;
            node.y = e.Y;

            nodes.Add(node);

            //Se deseaza un nou nod pe supafata de desenare
            g.DrawEllipse(black, node.x - radius, node.y - radius, 2 * radius, 2 * radius);
            g.FillEllipse(new SolidBrush(Color.White), node.x - radius, node.y - radius, 2 * radius, 2 * radius);
            g.DrawString(Convert.ToString(nodes.Count() - 1), new Font("Arial", 16), new SolidBrush(Color.Black), node.x - 40, node.y - 40);

        }
    }
}
