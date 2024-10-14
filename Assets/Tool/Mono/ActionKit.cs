using System.Runtime.InteropServices;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using Tool.Single;
using Tool.Utilities;
using UnityEngine;

namespace Tool.Mono
{
    public enum ETimerState
    {
        Finish,
        Pause,
        Start
    }

    public class Timer
    {
        //计时完成事件
        private Action _onFinishEvent;

        //计时器名字
        private string _timerName;

        //延迟时间
        private float _delayTime;

        //完成时间
        private float _finishTime;

        //暂停时计时剩余时间
        private float _remainderFinishTime;

        //是否循环
        private bool _isLoop;

        private ETimerState _timerState = ETimerState.Start;

        public void Stop() => _timerState = ETimerState.Finish;

        public void Pause()
        {
            _remainderFinishTime = _finishTime - Time.time;
            _timerState = ETimerState.Pause;
        }

        public void DontPause() => _timerState = ETimerState.Start;
        public string GetName() => _timerName;
        public ETimerState GetTimerState() => _timerState;

        public Timer SetupInfo(Action onFinishEvent, float delayTime, bool isLoop, string timerName)
        {
            _onFinishEvent = onFinishEvent;
            _finishTime = Time.time + delayTime;
            _delayTime = delayTime;
            _isLoop = isLoop;
            _timerName = timerName;
            _remainderFinishTime = 0;

            _timerState = ETimerState.Start;

            return this;
        }

        public void Update()
        {
            //如果已经完成了，还没开始，则不需要继续计时
            if (_timerState == ETimerState.Finish) return;
            //判断是否暂停,已经暂停则无限延迟计时完成时间
            if (_timerState == ETimerState.Pause) _finishTime = Time.time + _remainderFinishTime;
            //如果没到计时时间，则继续计时
            if (Time.time < _finishTime) return;
            //计时结束，执行方法
            _onFinishEvent?.Invoke();
            //如果计时到了，不需要循环,则标记计时完成
            if (!_isLoop) _timerState = ETimerState.Finish;
            //需要循环，就重新设置完成时间
            else _finishTime = Time.time + _delayTime;
        }
    }

    public class ActQueue
    {
        public class ActInfo
        {
            public Action Action;
            public float durationTime;
        }
        private string _queName;
        private QArray<ActInfo> _actQueue = new QArray<ActInfo>();
        private Action _killCallback;
        private bool _killAuto = true;
        private bool _isExecute = false;
        private float _time = 0f;
        private int _nowIdx = 0;
        
        public void Init(Action kill,string queName)
        {
             _killCallback = kill;
            _queName = queName;
        }

        public string GetQueName()
        {
            return _queName;
        }

        /// <summary>
        /// 从队尾添加一个动作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="durationTime"></param>
        /// <returns></returns>
        public ActQueue Append(Action action, float durationTime)
        {
            _actQueue.Add(new ActInfo
            {
                Action = action,
                durationTime = durationTime
            });
            return this;
        }
        
        /// <summary>
        /// 执行队列
        /// </summary>
        public void Execute()
        {
            _isExecute = true;
            PublicMonoKit.GetInstance().OnRegisterUpdate(Invoke);
        }

        /// <summary>
        /// 设置是否自动销毁
        /// </summary>
        /// <param name="atuo"></param>
        /// <returns></returns>
        public ActQueue KillAuto(bool atuo)
        {
            _killAuto = atuo;
            return this;
        }
        
        /// <summary>
        /// 销毁
        /// </summary>
        public void Kill() => _killCallback();

        /// <summary>
        /// 判断是否执行
        /// </summary>
        /// <returns></returns>
        public bool IsExecute() => _isExecute;

        private void Invoke()
        {
            if (_nowIdx == _actQueue.Count)
            {
                _time = 0;
                _nowIdx = 0;
                _isExecute = false;
                PublicMonoKit.GetInstance().OnUnRegisterUpdate(Invoke);
                if(_killAuto) _killCallback();
                return;
            }

            _time += Time.deltaTime;
            var peekActInfo = _actQueue[_nowIdx];
            if (_time < peekActInfo.durationTime)
            {
                peekActInfo.Action?.Invoke();
            }
            else
            {
                _nowIdx++;
                _time = 0;
            }
        }
    }

    public class ActionKit : Singleton<ActionKit>
    {
        //计时器列表，用于在公共mono内更新计时器
        private readonly List<Timer> _timerUpdateList = new List<Timer>();
        private readonly Queue<Timer> _timersPoolQueue = new Queue<Timer>();    
        private readonly QArray<ActQueue> _actQueueList = new QArray<ActQueue>();

        protected override void OnInit()
        {
            PublicMonoKit.GetInstance().OnRegisterUpdate(UpdateTimer);
        }

        public Timer AddTimer(Action onFinishEvent, float delayTime, string timerName, bool isLoop = false,
            bool isInvokeNow = false)
        {
            var timer = _timersPoolQueue.Count == 0 ? new Timer() : _timersPoolQueue.Dequeue();
            timer.SetupInfo(onFinishEvent, delayTime, isLoop, timerName);
            _timerUpdateList.Add(timer);
            if (isInvokeNow) onFinishEvent?.Invoke();
            return timer;
        }

        public void SetupTimerStop(string timerName) =>
            _timerUpdateList.Find(timer => timer.GetName() == timerName)?.Stop();

        public void SetupTimerPause(string timerName) =>
            _timerUpdateList.Find(timer => timer.GetName() == timerName)?.Pause();

        public void SetupTimerDontPause(string timerName) =>
            _timerUpdateList.Find(timer => timer.GetName() == timerName)?.DontPause();

        public void DelayTime(float delayTime, Action action) => AddTimer(action, delayTime, action.Method.Name);
        public void DelayTime(float delayTime,string timerName, Action action) => AddTimer(action, delayTime, timerName);

        public void RemoveTimer(string timerName)
        {
            _timerUpdateList.RemoveAll(timer => timer.GetName() == timerName);
        }

        private void UpdateTimer()
        {
            if (_timerUpdateList.Count == 0) return;
            for (var i = 0; i < _timerUpdateList.Count; i++)
            {
                if (_timerUpdateList[i].GetTimerState() == ETimerState.Finish)
                {
                    _timersPoolQueue.Enqueue(_timerUpdateList[i]);
                    _timerUpdateList.RemoveAt(i);
                    continue;
                }

                _timerUpdateList[i].Update();
            }
        }

        public ActQueue CreateActQue(string queName, Action action, float durationTime)
        {
            var actQue = new ActQueue();
            actQue.Init(() => _actQueueList.Remove(actQue), queName);
            actQue.Append(action, durationTime);
            _actQueueList.Add(actQue);
            return actQue;
        }
    }
}