using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphQLParser
{
    /// <summary>
    /// Optimized for memory allocation list for a small number of elements - from 0 to 5.
    /// </summary>
    /// <typeparam name="T"> Element type. </typeparam>
    internal struct SmallSizeOptimizedList<T> : IEnumerable<T>
    {
        private int _firstElementsCount;
        
        // stack allocated for 0-5 elements
        private T _elem1;
        private T _elem2;
        private T _elem3;
        private T _elem4;
        private T _elem5;

        // if more than 5 elements then heap allocated
        private List<T> _items;

        public int Count => _items?.Count ?? _firstElementsCount;

        public void Add(T item)
        {
            if (_firstElementsCount == 0)
            {
                _firstElementsCount = 1;
                _elem1 = item;
            }
            else if (_firstElementsCount == 1)
            {
                _firstElementsCount = 2;
                _elem2 = item;
            }
            else if (_firstElementsCount == 2)
            {
                _firstElementsCount = 3;
                _elem3 = item;
            }
            else if (_firstElementsCount == 3)
            {
                _firstElementsCount = 4;
                _elem4 = item;
            }
            else if (_firstElementsCount == 4)
            {
                _firstElementsCount = 5;
                _elem5 = item;
            }
            else
            {
                if (_items == null)
                    _items = new List<T> { _elem1, _elem2, _elem3, _elem4, _elem5, item };
                else
                    _items.Add(item);
            }

        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_items != null)
            {
                foreach (var item in _items)
                    yield return item;
            }
            else
            {
                if (_firstElementsCount > 0)
                    yield return _elem1;
                if (_firstElementsCount > 1)
                    yield return _elem2;
                if (_firstElementsCount > 2)
                    yield return _elem3;
                if (_firstElementsCount > 3)
                    yield return _elem4;
                if (_firstElementsCount > 4)
                    yield return _elem5;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// This method avoids boxing if the list is empty.
        /// </summary>
        public IEnumerable<T> AsEnumerable() => Count == 0 ? Array.Empty<T>() : (IEnumerable<T>)this;
    }
}
