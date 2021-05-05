using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class AVLTree<T> where T : IComparable
    {
        public Manual_List<string> elementos;

        public AVLTreeNode<T> Root { get; internal set; }
        public  AVLTreeNode<T> NotFound { get; internal set; }
     
        public void AddTo(T value,string nombre, string apellido, AVLTreeNode<T> current, string DPI)
        {

          
            if (Root == null)
            {
                Root = new AVLTreeNode<T>(value, null, this, nombre, apellido, DPI,elementos = new Manual_List<string>());
                Root.Treelist.AddLast(DPI);
                return;
            }
            if (current.Data.CompareTo(value) == 0)
            {
                current.Treelist.AddLast(DPI);
                return;
            }
            if (current.Data.CompareTo(value) < 0)
            {
                if (current.Left == null)
                {
                    current.Left = new AVLTreeNode<T>(value, current, this, nombre,apellido,DPI, elementos = new Manual_List<string>());
                    current.Left.Treelist.AddLast(DPI);
                }
                else
                {
                    AddTo(value,nombre,apellido, current.Left, DPI);
                }
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = new AVLTreeNode<T>(value, current, this, nombre,apellido, DPI, elementos = new Manual_List<string>());
                    current.Right.Treelist.AddLast(DPI);
                }
                else
                {
                    AddTo(value,nombre,apellido, current.Right, DPI);
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

            current = find(value,"-1",  parent= Root);

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





        public AVLTreeNode<T> find(T value, string notfound, AVLTreeNode<T> parent)
        {

         


            if (parent != null)
            {
                if (parent.Data.CompareTo(value) == 0 )
                {
          
                    return parent;
                }
                else if (parent.Data.CompareTo(value) < 0)
                {
                    return find(value,notfound, parent.Left);
                }
                else
                {
                    return find(value, notfound,  parent.Right);
                }

            }
            else
            {
                if (parent == null)
                {


                    NotFound = new AVLTreeNode<T>(value, null, this, "", "", notfound,null);
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
     
        public AVLTreeNode<T> Delete(AVLTreeNode<T> current, T target)
        {
            AVLTreeNode<T> parent;
            if (current == null)
            { return null; }
            else
            {
                //left subtree
                if (current.Data.CompareTo(target) < 0)
                {
                    current.Left = Delete(current.Left, target);
                    if (current.State != BalanceState.Balanced)//here
                    {
                        current.Balance();
                    }
                }
                //right subtree
                else if (current.Data.CompareTo(target) > 0)
                {
                    current.Right = Delete(current.Right, target);
                    if (current.State != BalanceState.Balanced)
                    {
                        current.Balance();
                    }
                }
                //if target is found
                else
                {
                    if (current.Right != null)
                    {
                        //delete its inorder successor
                        parent = current.Right;
                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }
                        current.Data = parent.Data;
                        current.Right = Delete(current.Right, parent.Data);
                        if (current.State != BalanceState.Balanced)//rebalancing
                        {
                            current.Balance();
                        }
                    }
                    else
                    {   //if current.left != null
                        return current.Left;
                    }
                }
            }
            return current;
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
        private AVLTreeNode<T> FindWithParent(T input, out AVLTreeNode<T> parent)
        {
            AVLTreeNode<T> current = Root;
            parent = null;

            while (current != null)
            {
              
                int compare = current.Data.CompareTo(input);

                if (compare < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (compare > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    return current;
                }
            }

            return null;
        }
        private bool RemoveNode(AVLTreeNode<T> current)
        {
            if (current == null || current.Tree != this)
            {
                return false;
            }

            var parent = current.Parent;


            if (current == Root)
            {
                Root = null;
            }
            if (current.Right == null)
            {
                if (parent == null)
                {
                    Root = current.Left;
                }
                else
                {

                    int compare = current.Data.CompareTo(parent.Data);
                    if (compare > 0)
                    {
                        parent.Left = current.Left;
                    }
                    else
                    {
                        parent.Right = current.Left;
                    }
                }
            }

            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    Root = current.Right;
                }
                else
                {
                    int compare = current.Data.CompareTo(parent.Data);
                    if (compare > 0)
                    {
                        parent.Left = current.Right;
                    }
                    else
                    {
                        parent.Right = current.Right;
                    }
                }
            }

            else
            {
                AVLTreeNode<T> leftMost = current.Right.Left;
                AVLTreeNode<T> leftMostParent = current.Right;

                while (leftMost.Left != null)
                {
                    leftMostParent = leftMost;
                    leftMost = leftMostParent.Left;
                }
                leftMostParent.Left = leftMost.Right;
                leftMost.Left = current.Left;
                leftMost.Left = current.Right;

                if (parent == null)
                {
                    Root = leftMost;
                }
                else
                {
                    int compare = current.Data.CompareTo(parent.Data);
                    if (compare > 0)
                    {
                        parent.Left = leftMost;
                    }
                    else
                    {
                        parent.Right = leftMost;
                    }
                }
            }
            return true;
        }

    }
}
