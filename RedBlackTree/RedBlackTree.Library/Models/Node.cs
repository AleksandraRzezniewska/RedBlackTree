using RedBlackTree.Library.Enums;

namespace RedBlackTree.Library.Models
{
    internal class Node<T>
    {
        public T Key { get; set; }
        public Node<T> Parent { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Color Color { get; set; }

        public Node(T key)
        {
            Key = key;
            Left = null;
            Right = null;
            Color = Color.Red;
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
