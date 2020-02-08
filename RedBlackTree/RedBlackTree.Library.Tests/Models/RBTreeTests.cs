using RedBlackTree.Library.Models;
using Xunit;

namespace RedBlackTree.Library.Tests.Models
{

    public class RBTreeTests
    {
        [Fact(DisplayName = "Method Add adds node properly")]
        public void AddAddsNodeProperlyTest()
        {
            var tree = new RBTree<int>
            {
                5,
                7,
                10,
                4,
            };

            Assert.Equal("4,5,7,10", tree.ToString());
        }

        [Fact(DisplayName = "Remove deletes proper node")]
        public void DeleteRemovesProperNodeTest()
        {
            var tree = new RBTree<int>
            {
                5,
                7,
                10,
                4,
            };

            tree.Remove(7);

            Assert.Equal("4,5,10", tree.ToString());
        }

        [Fact(DisplayName = "Clear removes all nodes")]
        public void ClearRemovesAllNodesTest()
        {
            var tree = new RBTree<int>
            {
                5,
                7,
                10,
                4,
            };

            tree.Clear();

            Assert.Equal("", tree.ToString());
        }

        [Fact(DisplayName = "Contains checks if tree contains certain node")]
        public void ContainsChecksIfTreeHasCertainNodeTest()
        {
            var tree = new RBTree<int>
            {
                5,
                7,
                10,
                4,
                45,
                1,
            };

            bool contains = tree.Contains(4);

            Assert.True(contains);
        }
    }
}
