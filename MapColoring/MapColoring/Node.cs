using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MapColoring
{
    //Clasa folosita pentru a memora detalii despre fiecare regiune( culoarea, vecinii, vectorul domeniu)
    class Node
    {
        public String color = "White";
        public List<Node> neighbors = new List<Node>();
        //Domeniul de culori posibile pentru un nod
        // Daca o valoare este -1 inseamna ca se poate atribui culoarea respectiva
        // Altfel, valoarea salvata reprezinta nodul care impiedica nodul curent din a lua culoarea respectiva
        public int[] domain = { -1, -1, -1, -1 }; 

        //Poziitiile de pe axele oX si oY pe suprafata de desenare din interfata
        public int x;
        public int y;
    }
}
