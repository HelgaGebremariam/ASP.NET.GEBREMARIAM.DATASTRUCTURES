using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class TreeLeaf<DataType, KeyType>
    {
        public DataType Data { get; set; }
        public KeyType Key { get; set; }
        public TreeLeaf<DataType, KeyType> Left { get; set; }
        public TreeLeaf<DataType, KeyType> Right { get; set; }
    }
}
