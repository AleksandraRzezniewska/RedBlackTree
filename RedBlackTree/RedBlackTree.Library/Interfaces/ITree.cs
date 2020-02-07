using System;
using System.Collections.Generic;

namespace RedBlackTree.Library.Interfaces
{
    public interface ITree<T> : ICollection<T> where T : IComparable
    {
    }
}
