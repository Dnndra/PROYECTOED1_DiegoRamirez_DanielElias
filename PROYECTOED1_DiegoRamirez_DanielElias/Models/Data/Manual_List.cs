using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class Node<T>
    {
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
        public T Data { get; set; }
        public Node(T data, Node<T> next, Node<T> previous)
        {
            this.Data = data;
            this.Next = next;
            this.Previous = previous;
        }
    }
    public class Manual_List<T> :IEnumerable<T> where T :IComparable

    {
        private Node<T> head;
        private Node<T> tail;
        private Node<T> current;

        public int Length { get; private set; }

        public Manual_List()
        {
            this.head = this.tail = null;
            this.Length = 0;
        }

        public void AddLast(T item)
        {
            if (head == null)
            {

                this.head = new Node<T>(item, null, null);
                this.tail = head;
                this.Length++;
            }
            else
            {
                Node<T> newLink = new Node<T>(item, null, this.tail);
                this.tail.Next = newLink;

                this.tail = newLink;
                this.Length++;
            }
        }
        public void swap(int pos1, int pos2)
        {
            Node<T> current = head;
            if  (pos1 == pos2)
            {
                return; 
            }
            int cont = 0; 
            var  node1 = Nodeatposition(pos1).Data;
            var node2 = Nodeatposition(pos2).Data;
            while (current != null)
            {
                if  (pos1 == cont)
                {
                    current.Data = node2;

                }
                if (pos2 == cont)
                {
                    current.Data = node1;
                }
                current = current.Next;
                cont++;
            }
            return;
        
        }
        

        public bool Remove(int pos)
        {
            int XD = 0;
            current = head;
            while (current != null)
            {
                if (XD == pos)
                {
                    if (this.current.Next == null)
                    {
                        tail = this.current.Previous;
                    }
                    else
                    {
                        this.current.Next.Previous = this.current.Previous;
                    }
                    if (this.current.Previous == null)
                    {
                        head = this.current.Next;
                    }
                    else
                    {
                        this.current.Previous.Next = current.Next;
                    }
                    current = null;
                    this.Length--;
                    return true;
                }
                current = this.current.Next;
                XD++;
            }
            return false;
        }
        public Node<T>  Nodeatposition(int pos)
        {
            int XD = 0;
           Node<T> Current = head;
            if  (head == null)
            {
                return head; 
            }
            while (Current != null)
            {
                if (XD == pos)
                {
                    return Current;
                }
                Current = Current.Next;
                XD++;
            }
            return Current; 
        }

        public  int getpositionNode(T value)
        {
            Node<T> current = head;
            int cont = 0; 
            while (current != null )
            {
                if (current.Data.CompareTo(value)== 0)
                {
                    return cont;
                }
                current = current.Next;
                cont++;
            }
            return -1;
        }
     

        private Manual_List<T>.LinkedListEnumerator GetEnumerator()
        {
            return new LinkedListEnumerator(this.head, this.tail, this.Length);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public struct LinkedListEnumerator : IEnumerator<T>, IEnumerator
        {
            private Node<T> head;
            private Node<T> tail;
            private Node<T> currentLink;
            private int length;
            private bool startedFlag;

            public LinkedListEnumerator(Node<T> head, Node<T> tail, int length)
            {
                this.head = head;
                this.tail = tail;
                this.currentLink = null;
                this.length = length;
                this.startedFlag = false;
            }

            public T Current
            {
                get { return this.currentLink.Data; }
            }

            public void Dispose()
            {
                this.head = null;
                this.tail = null;
                this.currentLink = null;
            }

            object IEnumerator.Current
            {
                get { return this.currentLink.Data; }
            }

            public bool MoveNext()
            {
                if (this.startedFlag == false)
                {
                    this.currentLink = this.head;
                    this.startedFlag = true;
                }
                else
                {
                    this.currentLink = this.currentLink.Next;
                }

                return this.currentLink != null;
            }

            public void Reset()
            {
                this.currentLink = this.head;
            }




        }
    }
}
