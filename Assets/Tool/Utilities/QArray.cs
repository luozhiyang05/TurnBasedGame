using System;
using System.Collections;
using System.Linq;

namespace Tool.Utilities
{

    public class QArray<T>
    {
        private T[] _array;
        private int _maxSize;
        private int _headIdx;
        private int _tailIdx;
        private Action<T> _addEvent;
        private Action<T> _removeEvent;
        private Action<T> _modifyEvent;
        public int Count;

        #region 初始化数组
        /// <summary>
        /// 初始化数组
        /// </summary>
        /// <param name="size">容量</param>
        public QArray(int size)
        {
            _array = new T[size];
            _maxSize = size;
            Count = 0;
            _headIdx = 0;
            _tailIdx = -1;
        }

        public QArray()
        {
        }
        #endregion

        #region 索引器
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Count - 1)
                {
                    throw new Exception("索引越界");
                }

                return _array[_headIdx + index];
            }
            set
            {
                var oldValue = _array[_headIdx + index];
                if (!oldValue.Equals(value))
                {
                    _modifyEvent?.Invoke(value);
                }
                _array[_headIdx + index] = value;
            }
        }
        #endregion

        #region 事件监听
        public void AddListenEvent(IListEventType type, Action<T> action)
        {
            switch (type)
            {
                case IListEventType.Add:
                    _addEvent += action;
                    break;
                case IListEventType.Remove:
                    _removeEvent += action;
                    break;
                case IListEventType.Modify:
                    _modifyEvent += action;
                    break;
                default:
                    throw new Exception("没有该类型监听事件");
            }
        }

        public void RemoveListenEvent(IListEventType type, Action<T> action)
        {
            switch (type)
            {
                case IListEventType.Add:
                    _addEvent -= action;
                    break;
                case IListEventType.Remove:
                    _removeEvent -= action;
                    break;
                case IListEventType.Modify:
                    _modifyEvent -= action;
                    break;
                default:
                    throw new Exception("没有该类型监听事件");
            }
        }
        #endregion

        #region 在尾部添加元素
        /// <summary>
        /// 在尾部添加元素
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            //如果尾指针达到数组长度，则判断当前数组中元素个数是否达到数组长度
            if (_tailIdx == _maxSize - 1)
            {
                if (Count == _maxSize)
                {
                    // 数组长度达到最大，则两倍扩容
                    var newSize = Count * 2;
                    var newArray = new T[newSize];
                    _array.CopyTo(newArray, 0);
                    _array = newArray;
                    _maxSize = newSize;
                }
                else
                {
                    // 数组长度未达到最大，则从头部开始填充
                    for (var i = _headIdx; i <= _tailIdx; i++)
                    {
                        _array[i - _headIdx] = _array[i];
                    }

                    _headIdx = 0;
                    _tailIdx = Count - 1;
                }
            }

            _tailIdx++;
            _array[_tailIdx] = value;
            Count++;
            _addEvent?.Invoke(value);
        }
        #endregion

        #region 出栈
        /// <summary>
        /// 出栈
        /// </summary>
        /// <returns></returns>
        public T GetFromTail()
        {
            if (IsEmpty()) throw new Exception("数组为空");

            var value = _array[_tailIdx];
            _array[_tailIdx] = default;
            _tailIdx--;
            Count--;
            _removeEvent?.Invoke(value);
            return value;
        }
        #endregion

        #region 出队
        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T GetFromHead()
        {
            if (IsEmpty()) throw new Exception("数组为空");

            var value = _array[_headIdx];
            _array[_headIdx] = default;
            _headIdx++;
            Count--;
            _removeEvent?.Invoke(value);
            return value;
        }
        #endregion

        #region 根据下标移除元素
        public T RemoveAt(int index)
        {
            if (IsEmpty()) throw new Exception("数组为空");

            if (index + _headIdx > _tailIdx || index == -1)
            {
                throw new Exception("下标超界：index=" + index + " _headIdx=" + _headIdx + " _tailIdx=" + _tailIdx + "_count=" + Count);
            }

            int tempIdx = index + _headIdx;

            T value = _array[tempIdx];
            tempIdx++;
            while (tempIdx <= _tailIdx)
            {
                _array[tempIdx - 1] = _array[tempIdx];
                tempIdx++;
            }

            _array[_tailIdx] = default;
            _tailIdx--;

            Count--;
            _removeEvent?.Invoke(value);
            return value;
        }
        #endregion

        #region 移除某个特定元素
        public T Remove(T value)
        {
            var idx = 0;
            var hasFind = false;
            for (var i = _headIdx; i <= _tailIdx; i++)
            {
                if (_array[i].Equals(value))
                {
                    idx = i;
                    hasFind = true;
                    break;
                }
            }
            if (hasFind)
            {
                var temp = _array[idx];
                RemoveAt(idx - _headIdx);
                return temp;
            }
            throw new Exception("没有找到该元素:" + nameof(value));
        }
        #endregion

        #region 根据条件移除元素
        public T Remove(Func<T, bool> find)
        {
            var idx = 0;
            var hasFind = false;
            for (var i = _headIdx; i <= _tailIdx; i++)
            {
                if (find(_array[i]))
                {
                    idx = i;
                    hasFind = true;
                    break;
                }
            }
            if (hasFind)
            {
                var temp = _array[idx];
                RemoveAt(idx - _headIdx);
                _removeEvent?.Invoke(temp);
                return temp;
            }
            return default;
        }
        #endregion

        #region 移除满足条件的所有元素
        public QArray<T> RemoveAll(Func<T, bool> find)
        {
            //获取要移除的元素下标集合
            var idxQArray = new QArray<int>(1);
            for (var i = _headIdx; i <= _tailIdx; i++)
            {
                if (find(_array[i]))
                {
                    idxQArray.Add(i);
                }
            }
            var remValuesQArray = new QArray<T>(idxQArray.Count);
            //获取要移除的元素集合
            foreach (int idx in idxQArray)
            {
                remValuesQArray.Add(_array[idx]);
            }
            //开始移除
            foreach (T value in remValuesQArray)
            {
                Remove(value);
            }
            return remValuesQArray;
        }
        #endregion

        #region 数组是否存在该元素
        public bool ContainValue(T t)
        {
            return _array.Contains(t);
        }
        #endregion

        #region 获取数组头部元素
        public T Peek()
        {
            if (IsEmpty()) throw new Exception("数组为空");

            return _array[_headIdx];
        }
        #endregion

        #region 数组是否为空
        public bool IsEmpty()
        {
            return _headIdx > _tailIdx;
        }
        #endregion

        #region 克隆数组
        public QArray<T> Clone()
        {
            var newArray = new QArray<T>(Count);
            for (var i = 0; i < Count; i++)
            {
                newArray.Add(newArray[i]);
            }

            return newArray;
        }
        #endregion

        #region 重置数组
        public void Clear()
        {
            _array = null;
            var newArray = new T[_maxSize];
            _array = newArray;
            Count = 0;
            _headIdx = 0;
            _tailIdx = -1;
        }
        #endregion

        #region 根据条件查询元素
        public T FindValue(Func<T, bool> find)
        {
            for (int i = 0; i < Count; i++)
            {
                var value = _array[i];
                var same = find(value);
                if (same)
                {
                    return value;
                }
            }
            return default;
        }
        #endregion

        #region 返回枚举器
        public IEnumerator GetEnumerator()
        {
            for (int i = _headIdx; i <= _tailIdx; i++)
            {
                yield return _array[i];
            }
        }
        #endregion
    }
}