using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class AVLTree<T> where T : IComparable
    {
        public AVLTreeNode<T> Root { get; internal set; }

        public void AddTo(T value, AVLTreeNode<T> current)
        {
            if (Root == null)
            {
                Root = new AVLTreeNode<T>(value, null, this);
                return;
            }
            if (current.Data.CompareTo(value) < 0)
            {
                if (current.Left == null)
                {
                    current.Left = new AVLTreeNode<T>(value, current, this);
                }
                else
                {
                    AddTo(value, current.Left);
                }
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = new AVLTreeNode<T>(value, current, this);
                }
                else
                {
                    AddTo(value, current.Right);
                }
            }

            var parent = current;
            while (parent != null)
            {
                if (parent.State != BalanceState.Balanced)
                {
                    parent.Balance();
                }

                parent = parent.Parent;
            }

        }


        public bool Remove(T value)
        {
            AVLTreeNode<T> current, parent;

            current = find(value, value, parent = Root);


            remove(Root, value);

            while (parent != null)
            {
                if (parent.State != BalanceState.Balanced)
                {
                    parent.Balance();
                }

                parent = parent.Parent;
            }


            return true;
        }





        public AVLTreeNode<T> find(T value, T value2, AVLTreeNode<T> parent)
        {
            AVLTreeNode<T> NotFound = Root;

            if (parent != null)
            {
                if (parent.Data.CompareTo(value) == 0)
                {
                    return parent;
                }
                else if (parent.Data.CompareTo(value) < 0)
                {
                    return find(value, value2, parent.Left);
                }
                else
                {
                    return find(value, value2, parent.Right);
                }

            }
            else
            {
                if (parent == null)
                {
                    NotFound.Data = value2;
                    return NotFound;
                }
            }





            return null;
        }



        private AVLTreeNode<T> remove(AVLTreeNode<T> parent, T key)
        {
            if (parent == null) { return parent; }

            if (parent.Data.CompareTo(key) < 0)
            {
                parent.Left = remove(parent.Left, key);

            }
            else if (parent.Data.CompareTo(key) > 0)
            {
                parent.Right = remove(parent.Right, key);
            }
            else
            {
                if (parent.Left == null)
                {
                    return parent.Right;
                }
                else if (parent.Right == null)
                {
                    return parent.Left;
                }
                parent.Data = minvalue(parent.Right);
                parent.Right = remove(parent.Right, parent.Data);
            }
            return parent;

        }
        private T minvalue(AVLTreeNode<T> node)
        {
            T minv = node.Data;
            while (node.Left != null)
            {
                minv = node.Left.Data;
                node = node.Left;
            }
            return minv;
        }


    }
}
