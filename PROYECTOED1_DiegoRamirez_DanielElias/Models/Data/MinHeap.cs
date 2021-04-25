using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class PriorityNode<T>
    {
        public PriorityNode<T> Next { get; set; }
        public int prioridad;
        public T Data { get; set; }
    }
    public class MinHeap<T>
    {
        public List<PriorityNode<T>> elements;
        public MinHeap()
        {
            elements = new List<PriorityNode<T>>();
        }

        public void Add(PriorityNode<T> node)
        {


            elements.Add(node);
            Heapify();
        }

        public void Delete(PriorityNode<T> node)
        {
            int i = elements.IndexOf(node);
            int last = elements.Count - 1;

            elements[i] = elements[last];
            elements.RemoveAt(last);
            Heapify();
        }

        public PriorityNode<T> PopMin()
        {
            if (elements.Count > 0)
            {
                PriorityNode<T> item = elements[0];
                Delete(item);
                return item;
            }
            ////relook at this - should we just throw exception?
            return null;
        }

        public void Heapify()
        {
            for (int i = elements.Count - 1; i > 0; i--)
            {
                int parentPosition = (i + 1) / 2 - 1;
                parentPosition = parentPosition >= 0 ? parentPosition : 0;

                if (elements[parentPosition].prioridad > (elements[i].prioridad))
                {
                    PriorityNode<T> tmp = elements[parentPosition];
                    elements[parentPosition] = elements[i];
                    elements[i] = tmp;
                }
            }
        }

        public PriorityNode<T> GetMin()
        {
            PriorityNode<T> min = elements[0];
            return min;
        }

    }
}
