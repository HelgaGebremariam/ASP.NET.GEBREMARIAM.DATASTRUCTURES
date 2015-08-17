using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    public class ListElement<DataType>
    {
        public DataType Data { get; set; }
        public ListElement<DataType> Next { get; set; }
        public ListElement<DataType> Previous { get; set; }
    }
}
