using System.Collections.Generic;
using System;

namespace Tool.Utilities.Bindery
{
    public class ListBindery<T>
    {
        private List<T> _list;
        private Action<T> _addEvent;
        private Action<T> _removeEvent;
        private Action<T> _modifyEvent;

        public ListBindery()
        {
            _list = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                var oldValue = _list[index];
                _list[index] = value;
                if (_modifyEvent != null && !oldValue.Equals(value))
                    _modifyEvent(value);
            }
        }

        public void OnRegister(IListEventType type, Action<T> action)
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
                    throw new Exception("没有找到该类型事件");
            }
        }

        public void UnRegister(IListEventType type, Action<T> action)
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
                    throw new Exception("没有找到该类型事件");
            }
        }

        public void ClearEvent(IListEventType type)
        {
            switch (type)
            {
                case IListEventType.Add:
                    _addEvent = null;
                    break;
                case IListEventType.Remove:
                    _removeEvent = null;
                    break;
                case IListEventType.Modify:
                    _modifyEvent = null;
                    break;
                default:
                    throw new Exception("没有找到该类型事件");
            }
        }

        public void Add(T t)
        {
            _list.Add(t);
            if (_addEvent != null)
                _addEvent(t);
        }

        public void Remove(T t)
        {
            _list.Remove(t);
            if (_removeEvent != null)
                _removeEvent(t);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public List<T> GetList() => _list;
    }
}