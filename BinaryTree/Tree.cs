using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class Tree<DataType, KeyType> : IEnumerable<DataType>
    {
        private TreeLeaf<DataType, KeyType> Root { get; set; }
        private Func<KeyType, KeyType, int> keyComparer { get; set; }
        public Tree(DataType rootData, KeyType rootKey, Func<KeyType, KeyType, int> keyComparer)
        {
            Root = new TreeLeaf<DataType, KeyType>();
            Root.Key = rootKey;
            Root.Data = rootData;
            if (keyComparer == null)
            {
                if (!(rootKey is IComparable))
                    throw new ArgumentNullException();
                keyComparer = delegate(KeyType key1, KeyType key2)
                {
                    if (key1 == null)
                        return -1;
                    IComparable comparer = key1 as IComparable;
                    return comparer.CompareTo(key2);
                };

            }

            this.keyComparer = keyComparer;
        }

        public bool Add(DataType data, KeyType key)
        {
            TreeLeaf<DataType, KeyType> leafToAdd = new TreeLeaf<DataType,KeyType>();
            leafToAdd.Data = data;
            leafToAdd.Key = key;
            return Add(Root, leafToAdd);
        }

        public bool Remove(KeyType key)
        {
            TreeLeaf<DataType, KeyType> parentLeafToRemove = 
                SearchFromRoot(r => (keyComparer(r.Left.Key, key) == 0 || 
                    keyComparer(r.Right.Key, key) == 0));
            if (parentLeafToRemove == null)
                return false;
            TreeLeaf<DataType, KeyType> leftChild;
            TreeLeaf<DataType, KeyType> rightChild;
            if(keyComparer(parentLeafToRemove.Left.Key, key) == 0)
            {
                leftChild = parentLeafToRemove.Left.Left;
                rightChild = parentLeafToRemove.Left.Right;
                parentLeafToRemove.Left = null;
            }
            else
            {
                leftChild = parentLeafToRemove.Right.Left;
                rightChild = parentLeafToRemove.Right.Right;
                parentLeafToRemove.Right = null;
            }
            Add(Root, leftChild);
            Add(Root, rightChild);
            return true;
        }

        #region private
        private TreeLeaf<DataType, KeyType> SearchFromRoot(Func<TreeLeaf<DataType, KeyType>, bool> searchFunc)
        {
            return Search(Root, searchFunc);
        }
        private TreeLeaf<DataType, KeyType> Search(TreeLeaf<DataType, KeyType> startLeaf, Func<TreeLeaf<DataType, KeyType>, bool> searchFunc)
        {
            if (startLeaf == null || searchFunc(startLeaf))
                return startLeaf;

            TreeLeaf<DataType, KeyType> leaf = startLeaf.Left;
            leaf = Search(startLeaf.Left, searchFunc);
            if (leaf != null)
                return leaf;
            else
                return Search(startLeaf.Right, searchFunc);
        }
        private TreeLeaf<DataType, KeyType> SearchByKey(TreeLeaf<DataType, KeyType> startLeaf, KeyType keyToSearch)
        {
            if (startLeaf == null || keyComparer(startLeaf.Key, keyToSearch) == 0)
                return startLeaf;
            if (keyComparer(startLeaf.Key, keyToSearch) == 1)
                return SearchByKey(startLeaf.Left, keyToSearch);
            else
                return SearchByKey(startLeaf.Right, keyToSearch);
        }

        private bool Add(TreeLeaf<DataType, KeyType> root, TreeLeaf<DataType, KeyType> elementToAdd)
        {
            int comparationResult = keyComparer(elementToAdd.Key, root.Key);
            if (comparationResult == 0)
                return false;
            if(comparationResult == 1)
            {
                if (root.Right == null)
                {
                    root.Right = elementToAdd;
                    return true;
                }
                else 
                    return Add(root.Right, elementToAdd);
            }
            else
            {
                if (root.Left == null)
                {
                    root.Left = elementToAdd;
                    return true;
                }
                else
                    return Add(root.Left, elementToAdd);
            }

        }
        #endregion

        #region ienumerable
        public IEnumerator<DataType> GetEnumerator()
        {
            Func<TreeLeaf<DataType, KeyType>, bool> moveFunc = 
                delegate(TreeLeaf<DataType, KeyType> currentLeaf)
            {
                if (currentLeaf.Left != null)
                {
                    currentLeaf = currentLeaf.Left;
                    return true;
                }
                if (currentLeaf.Right != null)
                {
                    currentLeaf = currentLeaf.Right;
                    return true;
                }
                return false;
            };
            return new CustomEnumerator(Root, moveFunc);
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ienumerator

        private class CustomEnumerator : IEnumerator<DataType>
        {
            private readonly TreeLeaf<DataType, KeyType> root;
            private TreeLeaf<DataType, KeyType> currentLeaf;
            private readonly Func<TreeLeaf<DataType, KeyType>, bool> moving;
            public CustomEnumerator(TreeLeaf<DataType, KeyType> root, Func<TreeLeaf<DataType, KeyType>, bool> moving)
            {
                this.root = root;
                currentLeaf = null;
                this.moving = moving;
            }
            public bool MoveNext()
            {
                if (currentLeaf == null)
                    currentLeaf = root;
                return moving(currentLeaf);
            }

            public void Reset()
            {
                currentLeaf = null;
            }

            public void Dispose()
            {

            }
            public DataType Current
            {
                get
                {
                    if (currentLeaf == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return currentLeaf.Data;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

        }

        #endregion
    }
}
