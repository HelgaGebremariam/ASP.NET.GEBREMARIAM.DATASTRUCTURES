using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    public class CustomList<DataType> : IEnumerable<DataType>
    {
        ListElement<DataType> startElement;

        public DataType this[int i]
        {
            get
            {
                return GetByIndex(i).Data;
            }
            private set
            {
                if (i == 0)
                {
                    AddBack(value);
                    return;
                }
                ListElement<DataType> addPlace = GetByIndex(i);
                ListElement<DataType> elementToAdd = new ListElement<DataType>();
                elementToAdd.Data = value;
                addPlace.Previous.Next = elementToAdd;
                elementToAdd.Previous = addPlace.Previous;
                elementToAdd.Next = addPlace;
                addPlace.Previous = elementToAdd;
            }
        }

        public DataType Last
        {
            get
            {
                return GetLastElement().Data;
            }
        }

        public void AddBack(DataType data)
        {
            ListElement<DataType> element = new ListElement<DataType>();
            element.Data = data;
            element.Next = startElement;
            startElement = element;
        }

        public void AddFront(DataType data)
        {
            ListElement<DataType> element = new ListElement<DataType>();
            element.Data = data;
            ListElement<DataType> currentTail = GetLastElement();
            element.Previous = currentTail;
            currentTail.Next = element;
        }

        #region ienumerable
        public IEnumerator<DataType> GetEnumerator()
        {
            return new CustomEnumerator(startElement);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
        #region ienumerator

        private class CustomEnumerator : IEnumerator<DataType>
        {
            private readonly ListElement<DataType> startElement;
            private ListElement<DataType> currentElement;
            public CustomEnumerator(ListElement<DataType> startElement)
            {
                this.startElement = startElement;
                this.currentElement = null;
            }
            public bool MoveNext()
            {
                if (currentElement == null)
                    currentElement = startElement;
                if (currentElement.Next != null)
                {
                    currentElement = currentElement.Next;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                currentElement = null;
            }

            public void Dispose()
            {

            }

            public DataType Current
            {
                get
                {
                    if (currentElement == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return currentElement.Data;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

        }

        #endregion

        private ListElement<DataType> GetLastElement()
        {
            ListElement<DataType> current = startElement;
            while (current.Next != null)
                current = current.Next;
            return current;
        }

        private ListElement<DataType> GetByIndex(int index)
        {
            ListElement<DataType> current = startElement;
            if (index < 0)
                throw new ArgumentOutOfRangeException();
            while (index > 0)
            {
                current = current.Next;
                if (current == null)
                    throw new ArgumentOutOfRangeException();
                index--;
            }
            return current;
        }
    }
}
