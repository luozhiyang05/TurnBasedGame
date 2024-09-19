using System;
using System.Linq;

namespace Tool.Utilities
{
    public class QArray<T>
    {
        private T[] _array;
        private int _maxSize;
        private int _headIdx;
        private int _tailIdx;
        public int Count;

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
            set => _array[_headIdx + index] = value;
        }

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
        }

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

            return value;
        }

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

            return value;
        }

        /// <summary>
        /// 根据下标移除元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Remove(int index)
        {
            if (IsEmpty()) throw new Exception("数组为空");

            if (index < _headIdx || index > _tailIdx) throw new Exception("下标超界");

            int tempIdx = _headIdx;
            while (tempIdx != index)
            {
                tempIdx++;
            }

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

            return value;
        }

        /// <summary>
        /// 数组中是否存在该元素
        /// </summary>
        /// <returns></returns>
        public bool ContainValue(T t)
        {
            return _array.Contains(t);
        }

        /// <summary>
        /// 获取数组头部元素
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (IsEmpty()) throw new Exception("数组为空");

            return _array[_headIdx];
        }

        /// <summary>
        /// 数组是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _headIdx > _tailIdx;
        }

        /// <summary>
        /// 克隆数组
        /// </summary>
        /// <returns></returns>
        public QArray<T> Clone()
        {
            var newArray = new QArray<T>(Count);
            for (var i = 0; i < Count; i++)
            {
                newArray.Add(newArray[i]);
            }

            return newArray;
        }

        /// <summary>
        /// 重置数组
        /// </summary>
        public void Clear()
        {
            _array = null;
            var newArray = new T[_maxSize];
            _array = newArray;
            Count = 0;
            _headIdx = 0;
            _tailIdx = -1;
        }


        public T GetValue(Func<T,bool> find)
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
    }
}