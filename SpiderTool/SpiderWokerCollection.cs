using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool
{
    public class SpiderWokerCollection : ICollection<SpiderWorker>
    {
        readonly List<SpiderWorker> _list = new List<SpiderWorker>();

        public int Count => _list.Count;

        public bool IsReadOnly => true;

        public void Add(SpiderWorker item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(SpiderWorker item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(SpiderWorker[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SpiderWorker> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Remove(SpiderWorker item)
        {
            return _list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
