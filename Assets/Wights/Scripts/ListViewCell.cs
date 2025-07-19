using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Wights.Scripts
{
    public class ListViewCell : MonoBehaviour, IPointerClickHandler
    {
        private LoopScrollList _LoopScrollList;
        private UnityAction _stopBufferCallback;
        private int _index;
        public void Init(LoopScrollList loopScrollList)
        {
            _LoopScrollList = loopScrollList;
        }

        public UnityAction<int, GameObject, bool> CliclkAction { set => _cliclkAction = value; }
        private UnityAction<int, GameObject, bool> _cliclkAction;

        public void OnPointerClick(PointerEventData eventData)
        {
            _stopBufferCallback?.Invoke();
            _cliclkAction?.Invoke(_index, gameObject, true);
        }

        public void SetStopBufferCallback(UnityAction callback)
        {
            _stopBufferCallback = callback;
        }

        public void UpdateCell(int index)
        {
            _index = index;
        }

        public int GetIndex()
        {
            return _index;
        }
    }
}