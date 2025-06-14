using System;
using System.Collections.Generic;
using Tool.Utilities.Events;
using UnityEngine;

namespace Tool.Utilities
{
    //事件基类
    public class EventBase
    {
    }

    //拓展事件
    public class Event : EventBase
    {
        private Action _action;
        public Event(Action action) => _action = action;
        public void AddEvent(Action action) => _action += action;
        public void RemoveEvent(Action action) => _action -= action;
        public void Trigger() => _action?.Invoke();
    }

    public class Event<T> : EventBase
    {
        private Action<T> _action;
        public Event(Action<T> action) => _action = action;
        public void AddEvent(Action<T> action) => _action += action;
        public void RemoveEvent(Action<T> action) => _action -= action;
        public void Trigger(T t) => _action?.Invoke(t);
    }

    public class Event<T, K> : EventBase
    {
        private Action<T, K> _action;
        public Event(Action<T, K> action) => _action = action;
        public void AddEvent(Action<T, K> action) => _action += action;
        public void RemoveEvent(Action<T, K> action) => _action -= action;
        public void Trigger(T t, K k) => _action?.Invoke(t, k);
    }

    public class Event<T, K, Q> : EventBase
    {
        private Action<T, K, Q> _action;
        public Event(Action<T, K, Q> action) => _action = action;
        public void AddEvent(Action<T, K, Q> action) => _action += action;
        public void RemoveEvent(Action<T, K, Q> action) => _action -= action;
        public void Trigger(T t, K k, Q q) => _action?.Invoke(t, k, q);
    }

    public class ReturnValueEvent<V> : EventBase
    {
        private Func<V> _action;
        public ReturnValueEvent(Func<V> action) => _action = action;

        public void RemoveEvent() => _action = null;
        public V Trigger() => _action.Invoke();
    } 
    
    public class ReturnValueEvent<V,T> : EventBase
    {
        private Func<V,T> _action;
        public ReturnValueEvent(Func<V,T> action) => _action = action;

        public void RemoveEvent() => _action = null;
        public T Trigger(V v) => _action.Invoke(v);
    }

    public static class EventsHandle
    {
        private static Dictionary<EventsNameConst, EventBase> _eventBasesDic = new Dictionary<EventsNameConst, EventBase>();

        #region 添加事件

        public static void AddListenEvent(EventsNameConst eventName, Action action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
                (eventBase as Event).AddEvent(action);
            else
                _eventBasesDic.Add(eventName, new Event(action));
        }

        public static void AddListenEvent<T>(EventsNameConst eventName, Action<T> action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
                (eventBase as Event<T>).AddEvent(action);
            else
                _eventBasesDic.Add(eventName, new Event<T>(action));
        }

        public static void AddListenEvent<T, K>(EventsNameConst eventName, Action<T, K> action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
                (eventBase as Event<T, K>).AddEvent(action);
            else
                _eventBasesDic.Add(eventName, new Event<T, K>(action));
        }

        public static void AddListenEvent<T, K, Q>(EventsNameConst eventName, Action<T, K, Q> action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
                (eventBase as Event<T, K, Q>).AddEvent(action);
            else
                _eventBasesDic.Add(eventName, new Event<T, K, Q>(action));
        }

        public static void AddListenEvent<V>(EventsNameConst eventName, Func<V> action)
        {
            _eventBasesDic.Add(eventName, new ReturnValueEvent<V>(action));
        }
        
        public static void AddListenEvent<V,T>(EventsNameConst eventName, Func<V,T> action)
        {
            _eventBasesDic.Add(eventName, new ReturnValueEvent<V,T>(action));
        }

        #endregion

        #region 移除事件

        public static void RemoveReturnValueEventsByName(EventsNameConst eventName)
        {
            if (_eventBasesDic.ContainsKey(eventName))
            {
                _eventBasesDic.Remove(eventName);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        } 

        public static void ClearAllEventByEventName(EventsNameConst eventName)
        {
            if (_eventBasesDic.ContainsKey(eventName))
            {
                _eventBasesDic.Remove(eventName);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void RemoveOneEventByEventName(EventsNameConst eventName, Action action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase value))
            {
                (value as Event).RemoveEvent(action);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void RemoveOneEventByEventName<T>(EventsNameConst eventName, Action<T> action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase value))
            {
                (value as Event<T>).RemoveEvent(action);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void RemoveOneEventByEventName<T, K>(EventsNameConst eventName, Action<T, K> action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase value))
            {
                (value as Event<T, K>).RemoveEvent(action);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void RemoveOneEventByEventName<T, K, Q>(EventsNameConst eventName, Action<T, K, Q> action)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase value))
            {
                (value as Event<T, K, Q>).RemoveEvent(action);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        #endregion

        #region 事件触发

        public static T ReturnValueEventsTrigger<T>(EventsNameConst eventName)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
            {
                return (eventBase as ReturnValueEvent<T>).Trigger();
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
                return default;
            }
        } 
        public static T ReturnValueEventsTrigger<V,T>(EventsNameConst eventName,V v)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
            {
                return (eventBase as ReturnValueEvent<V,T>).Trigger(v);
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
                return default;
            }
        }

        public static void EventTrigger(EventsNameConst eventName)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
            {
                (eventBase as Event).Trigger();
                Debug.Log("派发：" + eventName + "事件");
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void EventTrigger<T>(EventsNameConst eventName, T value)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
            {
                (eventBase as Event<T>)?.Trigger(value);
                Debug.Log("派发：" + eventName + "事件");
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void EventTrigger<T, K>(EventsNameConst eventName, T t, K k)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
            {
                (eventBase as Event<T, K>)?.Trigger(t, k);
                Debug.Log("派发：" + eventName + "事件");
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        public static void EventTrigger<T, K, Q>(EventsNameConst eventName, T t, K k, Q q)
        {
            if (_eventBasesDic.TryGetValue(eventName, out EventBase eventBase))
            {
                (eventBase as Event<T, K, Q>)?.Trigger(t, k, q);
                Debug.Log("派发：" + eventName + "事件");
            }
            else
            {
                Debug.LogWarning("找不到" + eventName + "事件");
            }
        }

        #endregion
    }
}