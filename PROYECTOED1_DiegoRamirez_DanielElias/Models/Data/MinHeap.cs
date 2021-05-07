using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PROYECTOED1_DiegoRamirez_DanielElias.Models.Data;
namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
 
    public class MinHeap<T> where T :IComparable
    {
       
        public Manual_List<Paciente> elementos;

        public MinHeap()
        {
            elementos = new Manual_List<Paciente>();
        }

        public void Add(Paciente node)
        {
          

                elementos.AddLast(node);
                Heapify();
            
        }

        public void Delete(Node<Paciente> node)
        {
            int i = elementos.getpositionNode(node.Data);
            int last = elementos.Length-1;

            elementos.swap(i, last);
            elementos.Remove(last);
            Heapify();
        }

        public  Node<Paciente> PopMin()
        {
            if (elementos.Length-1 > 0)
            {
                Node<Paciente> item = elementos.Nodeatposition(0);
                Delete(item);
                return item;
            }

            return null;
        }

        public void Heapify()
        {
            for (int i = elementos.Length-1  ; i > 0; i--)
            {
                int parentPosition = (i + 1) / 2 - 1;
                parentPosition = parentPosition >= 0 ? parentPosition : 0;
                if (elementos != null)
                {
                    if (elementos.Nodeatposition(parentPosition).Data.Prioridad == (elementos.Nodeatposition(i).Data.Prioridad))
                    {

                    }
                    else if (elementos.Nodeatposition(parentPosition).Data.Prioridad > (elementos.Nodeatposition(i).Data.Prioridad))
                    {
                        elementos.swap(parentPosition, i);
                    }
                }
            }
            listPriority();
        }

        public Node<Paciente> GetNode(int position)
        {
            
           Node<Paciente> min = elementos.Nodeatposition(position);
            
            return min;
        }

        public void listPriority()
        {
          
            for (int i = 0; i < elementos.Length; i++)
            {
                for (int j = 0; j < elementos.Length - 1; j++)
                {
                    if ( elementos.Nodeatposition(j).Data.Prioridad== elementos.Nodeatposition(j+1).Data.Prioridad)
                    {

                    }
                    else if (elementos.Nodeatposition(j).Data.Prioridad > elementos.Nodeatposition(j + 1).Data.Prioridad)
                    {
                     
                        elementos.swap(j, j + 1);
                    }
                }
            }
        }


    }
}
