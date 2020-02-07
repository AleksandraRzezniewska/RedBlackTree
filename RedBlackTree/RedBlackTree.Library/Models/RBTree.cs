using RedBlackTree.Library.Enums;
using RedBlackTree.Library.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RedBlackTree.Library.Models
{
    public class RBTree<T> : ITree<T> where T : IComparable
    {
        private Node<T> _root;

        public int Count { get; private set; }

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T key)
        {
            var newNode = new Node<T>(key)
            {
                Parent = _root
            };

            if (Contains(key))
            {
                return;
            }

            if (_root == null)
            {
                _root = newNode;
                _root.Color = Color.Black;

                Count++;

                return;
            }

            Node<T> node = null;

            while (newNode.Parent != null)
            {
                node = newNode.Parent;
                newNode.Parent = (key.CompareTo(newNode.Parent.Key) < 0) ? newNode.Parent.Left : newNode.Parent.Right;
            }

            newNode.Parent = node;

            if (newNode.Parent == null)
            {
                _root = newNode;
            }
            else if (key.CompareTo(newNode.Parent.Key) < 0)
            {
                newNode.Parent.Left = newNode;
            }
            else
            {
                newNode.Parent.Right = newNode;
            }

            Count++;

            FixAfterAdd(newNode);
        }

        public void AddRecursive(T key)
        {
            _root = AddRecursiveReturnNode(_root, key);

            FixAfterAdd(_root);
        }

        public bool Remove(T key)
        {
            var nodeToRemove = Search(key, _root);
            Node<T> nodeToRemoveSon;

            if (nodeToRemove == null)
            {
                return false;
            }

            var node = (nodeToRemove.Left == null || nodeToRemove.Right == null) ? nodeToRemove : FindSuccessorNode(key, _root);

            nodeToRemoveSon = node.Left ?? node.Right;

            if (nodeToRemoveSon != null)
            {
                nodeToRemoveSon.Parent = node.Parent;
            }

            if (node.Parent == null)
            {
                _root = nodeToRemoveSon;
            }
            else
            {
                if (node == node.Parent.Left)
                {
                    node.Parent.Left = nodeToRemoveSon;
                }
                else
                {
                    node.Parent.Right = nodeToRemoveSon;
                }
            }

            if (node != nodeToRemove)
            {
                nodeToRemove.Key = node.Key;
            }

            if (node.Color == Color.Red)
            {
                Count--;

                return true;
            }
            else
            {
                Count--;

                FixAfterRemove(nodeToRemoveSon);

                return true;
            }
        }

        public override string ToString()
        {
            return string.Join(",", TraverseInOrder(_root));
        }

        public void Clear()
        {
            if (_root == null)
            {
                return;
            }

            while (_root != null)
            {
                Remove(_root.Key);
            }
        }

        public bool Contains(T key)
        {
            return (Search(key, _root) != null);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var node in TraverseInOrder(_root))
            {
                yield return node.Key;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private Node<T> AddRecursiveReturnNode(Node<T> node, T key)
        {
            if (node == null)
            {
                Count++;

                return new Node<T>(key);
            }
            else if (key.CompareTo(node.Key) == 0)
            {
                return node;
            }
            else if (key.CompareTo(node.Key) < 0)
            {
                node.Left = AddRecursiveReturnNode(node.Left, key);
            }
            else
            {
                node.Right = AddRecursiveReturnNode(node.Right, key);
            }

            return node;
        }

        private void FixAfterAdd(Node<T> newNode)
        {
            while (newNode.Parent != null && newNode != _root && newNode.Parent.Color == Color.Red)
            {
                if (newNode.Parent == newNode.Parent.Parent.Left)
                {
                    var node = newNode.Parent.Parent.Right;

                    if (node != null && node.Color != Color.Black)
                    {
                        RecolorWhenUncleIsRed(newNode, node);

                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Right)
                        {
                            newNode = newNode.Parent;

                            RotateLeft(newNode);
                        }
                        else
                        {
                            newNode.Parent.Color = Color.Black;
                            newNode.Parent.Parent.Color = Color.Red;

                            RotateRight(newNode.Parent.Parent);

                            _root.Color = Color.Black;
                        }
                    }
                }
                else
                {
                    var node = newNode.Parent.Parent.Left;

                    if (node != null && node.Color != Color.Black)
                    {
                        RecolorWhenUncleIsRed(newNode, node);

                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Left)
                        {
                            newNode = newNode.Parent;

                            RotateRight(newNode);
                        }
                        else
                        {
                            newNode.Parent.Color = Color.Black;
                            newNode.Parent.Parent.Color = Color.Red;

                            RotateLeft(newNode.Parent.Parent);

                            _root.Color = Color.Black;
                        }
                    }
                }
            }

            _root.Color = Color.Black;
        }

        private void FixAfterRemove(Node<T> nodeToRemoveSon)
        {
            while (nodeToRemoveSon != null && nodeToRemoveSon != _root && nodeToRemoveSon.Color == Color.Black)
            {
                if (nodeToRemoveSon != nodeToRemoveSon.Parent.Right)
                {
                    var node = nodeToRemoveSon.Parent.Right;

                    if (node.Color != Color.Black)
                    {
                        RecolorWhenBrotherIsRed(nodeToRemoveSon, node);

                        RotateLeft(nodeToRemoveSon.Parent);
                    }
                    else
                    {
                        if (node.Left.Color == Color.Red | node.Right.Color == Color.Red)
                        {
                            if (node.Right.Color != Color.Red)
                            {
                                RecolorWhenBrotherIsBlackWithRedLeftSon(node);

                                RotateRight(node);
                            }
                            else
                            {
                                RecolorWhenBrotherIsBlackWithRedRightSon(nodeToRemoveSon, node);

                                RotateLeft(nodeToRemoveSon.Parent);

                                nodeToRemoveSon = _root;
                            }
                        }
                        else
                        {
                            RecolorWhenBrotherIsBlackWithBlackSons(node);

                            nodeToRemoveSon = nodeToRemoveSon.Parent;
                        }
                    }
                }
                else
                {
                    var node = nodeToRemoveSon.Parent.Left;

                    if (node.Color != Color.Black)
                    {
                        RecolorWhenBrotherIsRed(nodeToRemoveSon, node);

                        RotateRight(nodeToRemoveSon.Parent);
                    }
                    else
                    {
                        if (node.Left.Color == Color.Red | node.Right.Color == Color.Red)
                        {
                            if (node.Right.Color != Color.Red)
                            {
                                RecolorWhenBrotherIsBlackWithRedLeftSon(node);

                                RotateLeft(node);
                            }
                            else
                            {
                                RecolorWhenBrotherIsBlackWithRedRightSon(nodeToRemoveSon, node);

                                RotateRight(nodeToRemoveSon.Parent);

                                nodeToRemoveSon = _root;
                            }
                        }
                        else
                        {
                            RecolorWhenBrotherIsBlackWithBlackSons(node);

                            nodeToRemoveSon = nodeToRemoveSon.Parent;
                        }
                    }
                }
            }

            if (nodeToRemoveSon != null)
            {
                nodeToRemoveSon.Color = Color.Black;
            }
        }

        private void RecolorWhenUncleIsRed(Node<T> newNode, Node<T> node)
        {
            newNode.Parent.Color = Color.Black;
            node.Color = Color.Black;
            newNode.Parent.Parent.Color = Color.Red;
        }

        private void RecolorWhenBrotherIsRed(Node<T> nodeToRemoveSon, Node<T> node)
        {
            node.Color = Color.Black;
            nodeToRemoveSon.Parent.Color = Color.Red;
        }

        private void RecolorWhenBrotherIsBlackWithRedLeftSon(Node<T> node)
        {
            node.Left.Color = Color.Black;
            node.Color = Color.Red;
        }

        private void RecolorWhenBrotherIsBlackWithRedRightSon(Node<T> nodeToRemoveSon, Node<T> node)
        {
            node.Color = nodeToRemoveSon.Parent.Color;
            nodeToRemoveSon.Parent.Color = Color.Black;
            node.Right.Color = Color.Black;
        }

        private void RecolorWhenBrotherIsBlackWithBlackSons(Node<T> node)
        {
            node.Color = Color.Red;
        }

        private void RotateRight(Node<T> node)
        {
            var leftSon = node.Left;

            if (leftSon == null)
            {
                return;
            }

            var parent = node.Parent;
            node.Left = leftSon.Right;

            if (node.Left != null)
            {
                node.Left.Parent = node;
            }

            leftSon.Right = node;
            leftSon.Parent = parent;
            node.Parent = leftSon;

            if (parent != null)
            {
                if (parent.Left == node)
                {
                    parent.Left = leftSon;
                }
                else
                {
                    parent.Right = leftSon;
                }
            }
            else
            {
                _root = leftSon;
            }
        }

        private void RotateLeft(Node<T> node)
        {
            var rightSon = node.Right;

            if (rightSon == null)
            {
                return;
            }

            var parent = node.Parent;
            node.Right = rightSon.Left;

            if (node.Right != null)
            {
                node.Right.Parent = node;
            }

            rightSon.Left = node;
            rightSon.Parent = parent;
            node.Parent = rightSon;

            if (parent != null)
            {
                if (parent.Left == node)
                {
                    parent.Left = rightSon;
                }
                else
                {
                    parent.Right = rightSon;
                }
            }
            else
            {
                _root = rightSon;
            }
        }

        private Node<T> Search(T key, Node<T> root)
        {
            var node = root;

            while (node != null)
            {
                if (key.CompareTo(node.Key) == 0)
                {
                    return node;
                }
                else if (key.CompareTo(node.Key) < 0)
                {
                    node = node.Left;
                }
                else if (key.CompareTo(node.Key) > 0)
                {
                    node = node.Right;
                }
            }

            return null;
        }

        private Node<T> FindMinimalNode(Node<T> root)
        {
            var node = root;

            if (node == null)
            {
                return node;
            }

            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        private Node<T> FindSuccessorNode(T key, Node<T> root)
        {
            var node = Search(key, root);

            if (node == null)
            {
                return node;
            }

            if (node.Right != null)
            {
                return FindMinimalNode(node.Right);
            }

            var succNode = node.Parent;

            while (succNode != null & node == succNode.Right)
            {
                node = succNode;
                succNode = succNode.Parent;
            }

            return succNode;
        }

        private IEnumerable<Node<T>> TraverseInOrder(Node<T> node)
        {
            if (node == null)
            {
                yield break;
            }

            if (node.Left != null)
            {
                foreach (var leftNode in TraverseInOrder(node.Left))
                {
                    yield return leftNode;
                }
            }

            yield return node;

            if (node.Right != null)
            {
                foreach (var rightNode in TraverseInOrder(node.Right))
                {
                    yield return rightNode;
                }
            }
        }
    }
}

