using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapColoring
{
    class ForwardChecking
    {
        //Lista de noduri folosita de metoda care implementeaza algoritmul
        List<Node> nodes;
        String[] colors = { "Red", "Blue", "Green", "Yellow" };
        
        public ForwardChecking(List<Node> nodes)
        {
            //Se reseteaza culorile si domeniul nodurilor
            for (int i = 0; i < nodes.Count(); i++)
            {
                nodes[i].color = "White";
                nodes[i].domain[0] = -1;
                nodes[i].domain[1] = -1;
                nodes[i].domain[2] = -1;
                nodes[i].domain[3] = -1;
            }

            this.nodes = new List<Node>(nodes);
        }

        public ForwardChecking() { }


        public void setGraph(List<Node> nodes)
        {
            //Se reseteaza culorile si domeniul grafului
            for (int i = 0; i < nodes.Count(); i++)
            {
                nodes[i].color = "White";
                nodes[i].domain[0] = -1;
                nodes[i].domain[1] = -1;
                nodes[i].domain[2] = -1;
                nodes[i].domain[3] = -1;
            }

            this.nodes = new List<Node>(nodes);
        }

        public bool StartAlgoritm(int index)
        {
            // Se incarca fiecare culoare
            for (int c = 0; c < colors.Length; c++) 
            {
                if (nodes[index].domain[c] == -1) 
                {
                    // Daca o valoarea corespunzatoare culorii din vectorul domeniu este -1 se poate colora nodul cu acea culoare
                    // Se coloreaza nodul
                    nodes[index].color = colors[c];
                    // Daca s-a ajuns la ultimul nod functia returneaza true;
                    if (index == nodes.Count() - 1)
                        return true;
                    // Se verifica daca culoarea setata nu impiedica nodurile vecine din a fi colorate
                    else if (CheckForward(index))
                    {
                        // Daca algoritmul returneaza true, se continua returnarea valorii pana se iese din stiva de recursivitate
                        if (StartAlgoritm(index + 1))
                            return true;  // Daca nu se continua algoritmul
                    }
                    // Daca s-a ajuns aici insemna ca ce culoare am atribuit nodului impiedica colorarea corecta a grafului
                    //Functia Restore aduce domeniul vecinilor nodului la stare de dinainte de atribuire a culorii
                    Restore(index); 
                }
                
            }
            return false;
        }

        public void Restore(int index)
        {
            nodes[index].color = "White";
            for(int j = 0; j<nodes[index].neighbors.Count(); j++)
            {
                for (int c = 0; c < colors.Length; c++)
                    if (nodes[index].neighbors[j].domain[c] == index)
                        nodes[index].neighbors[j].domain[c] = -1;
            }
        }

        public bool CheckForward(int index)
        {
            // Se trece prin fiecare vecin al nodului cu indexul "index"
            for (int j = 0; j < nodes[index].neighbors.Count(); j++)
            {
                bool DWO = true;
                for (int c = 0; c < colors.Length; c++)
                {
                    // Daca o culoare este disponibilia
                    if (nodes[index].neighbors[j].domain[c] == -1) 
                    {
                        // Se verifica daca acea culoare este culoarea cu care s-a colorat nodul "index"
                        if (!nodes[index].color.Equals(colors[c])) 
                        {
                            // Daca nu, DWO se seteaza pe fals, ceea ce inseamna ca nodul "index" nu
                            // elimina ultima valoare posibila pentru nodul sau vecin
                            DWO = false;
                        }
                        else
                        {
                            // Daca culoare verificata este aceiasi ca cea a nodului "index" 
                            //se setaza valoarea domeniului cu valoarea "index":
                            nodes[index].neighbors[j].domain[c] = index; 
                        }
                    } 
                }
                // Daca s-a trecut prin toate nodurile si DWO este true inseamna ca valoarea atribuita culorii nodului "index" 
                //incurca macar un vecin si functia returneaza false;
                if (DWO) 
                    return false;
            }
            return true;
        }
    }
}
