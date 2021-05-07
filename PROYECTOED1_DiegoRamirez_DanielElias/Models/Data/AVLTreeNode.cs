using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PROYECTOED1_DiegoRamirez_DanielElias.Models.Data;
namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class AVLTreeNode<T> where T : IComparable
    {



        private AVLTreeNode<T> left;
        private AVLTreeNode<T> right;
        internal AVLTree<T> Tree;

        public AVLTreeNode(T value, AVLTreeNode<T> parent, AVLTree<T> tree, string Dpi, Manual_List<string> treelist)
        {
            Data = value;
            Parent = parent;
            Tree = tree;
       
            DPI = Dpi;
            Treelist = treelist;
     
        }



        public T Data { get; set; }
        public string DPI { get; set; }


        public Manual_List<string> Treelist { get; set; }

     
        public AVLTreeNode<T> Left
        {
            get { return left; }
            set
            {
                left = value;
                if (left != null)
                {
                    left.Parent = this;
                }
            }
        }
        public AVLTreeNode<T> Right
        {
            get { return right; }
            set
            {
                right = value;
                if (right != null)
                {
                    right.Parent = this;
                }
            }
        }
        public AVLTreeNode<T> Parent { get; set; }



        internal void Balance()
        {
            if (State == BalanceState.RightHeavy)
            {
                if (Right != null && Right.BalanceFactor < 0)
                {

                    LeftRightRotation();
                }
                else
                {
                    LeftRotation();
                }
            }
            else if (State == BalanceState.LeftHeavy)
            {
                if (Left != null && Left.BalanceFactor > 0)
                {

                    RightLeftRotation();
                }
                else
                {
                    RotatetoRight();


                }
            }
        }
        private void LeftRotation()
        {
            AVLTreeNode<T> rootParent = Parent;
            AVLTreeNode<T> root = this;
            AVLTreeNode<T> temp = Right;

            bool isLeftChild = (rootParent != null) && rootParent.Left == root;
            root.Right = temp.Left;
            temp.Left = root;

            root.Parent = temp;
            temp.Parent = rootParent;

            if (root.Right != null)
                root.Right.Parent = root;

            if (Tree.Root == root)
            {
                Tree.Root = temp;
            }
            else if (isLeftChild)
            {
                rootParent.Left = temp;
            }
            else if (rootParent != null)
            {
                rootParent.Right = temp;
            }
        }
        private void RotatetoRight()
        {
            AVLTreeNode<T> rootParent = Parent;
            AVLTreeNode<T> root = this;
            AVLTreeNode<T> temp = Left;
            bool isLeftChild = (rootParent != null) && rootParent.Left == root;

            root.Left = temp.Right;
            temp.Right = root;

            root.Parent = temp;
            temp.Parent = rootParent;

            if (root.Left != null)
                root.Left.Parent = root;

            if (Tree.Root == root)
            {
                Tree.Root = temp;
            }
            else if (isLeftChild)
            {
                rootParent.Left = temp;
            }
            else if (rootParent != null)
            {
                rootParent.Right = temp;
            }
        }
        private void LeftRightRotation()
        {
            Right.RotatetoRight();
            LeftRotation();
        }
        private void RightLeftRotation()
        {
            Left.LeftRotation();
            RotatetoRight();
        }
        private int MaxHeight(AVLTreeNode<T> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(MaxHeight(node.Left), MaxHeight(node.Right));
            }
            return 0;
        }
        private int LeftHeight { get { return MaxHeight(Left); } }
        private int RightHeight { get { return MaxHeight(Right); } }
        public BalanceState State
        {
            get
            {
                if (LeftHeight - RightHeight > 1)
                {

                    return BalanceState.LeftHeavy;
                }

                if (RightHeight - LeftHeight > 1)
                {
                    return BalanceState.RightHeavy;
                }

                return BalanceState.Balanced;
            }
        }
        private int BalanceFactor
        {
            get { return RightHeight - LeftHeight; }
        }


    }
}
