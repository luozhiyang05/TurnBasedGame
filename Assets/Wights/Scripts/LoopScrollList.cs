using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wights.Utilities
{
    public class LoopScrollList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform viewPort;
        public RectTransform content;
        public RectTransform cell;
        //public int visiableCellCnt;
        private int _initCellCnt;
        public List<int> data = new List<int>();
        void Start()
        {
            //TODO:重新结算初始化cell个数，个数为ViewPort高度向上取整
            _initCellCnt = Mathf.CeilToInt(viewPort.rect.height / cell.rect.height) + 1;
            _cellHeight = cell.rect.height;
            //补偿高度，为了避免ViewPort长于cell高度×个数时无法滑倒底部，用于滑倒底部时对content高度补偿
            //如height=100，viewPort高度为大于525，那content滑倒最底部的高度要补偿_payHeigtt
            //需要有补偿高度时，初始化cell的个数一般比 viewPort.rect.height / cell.rect.height 多 2个
            _payHeigtt = (viewPort.rect.height % cell.rect.height) == 0 ? 0 : _cellHeight - (viewPort.rect.height % cell.rect.height);

            for (int i = 0; i < 25; i++)
            {
                data.Add(i);
            }

            for (int i = 0; i < _initCellCnt; i++)
            {
                GameObject go = Instantiate(cell.gameObject, content);
                go.transform.localPosition = Vector3.zero;
                go.SetActive(true);
            }

            for (int i = 0; i < _initCellCnt; i++)
            {
                RenderCell(content.GetChild(i).gameObject, i);
            }
        }

        public float _moveSpeed = 1f;
        private Vector2 _startPos;
        private float _moveDistance;
        private int _currentCellOffset = 0;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _moveDistance = eventData.position.y - _startPos.y;
            _startPos = eventData.position;
            MoveList();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _startPos = Vector2.zero;
        }


        private float _cellHeight;
        private int _nowDataIndex = 0;
        private float _payHeigtt = 0;
        public void MoveList()
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + _moveDistance * _moveSpeed);

            JudgeEdge();

            var contentY = content.anchoredPosition.y;
            //加上补偿高度限制更新cell，防止需要补偿时，无法滑倒最底部
            if (contentY > _cellHeight + _payHeigtt)
            {
                var firstCell = content.GetChild(0);
                firstCell.SetAsLastSibling();
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y - _cellHeight);
                _currentCellOffset++;
                _nowDataIndex = _currentCellOffset + _initCellCnt - 1;
                RenderCell(firstCell.gameObject, _nowDataIndex);
            }
            else if (contentY < 0)
            {
                var lastCell = content.GetChild(content.childCount - 1);
                lastCell.SetAsFirstSibling();
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + _cellHeight);
                _currentCellOffset--;
                _nowDataIndex = _currentCellOffset;
                RenderCell(lastCell.gameObject, _nowDataIndex);
            }
        }

        public void RenderCell(GameObject cell, int index)
        {
            Debug.Log(index);
            cell.transform.Find("Text").GetComponent<Text>().text = index.ToString();
        }

        public void JudgeEdge()
        {
            if (_nowDataIndex >= data.Count - 1)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, Mathf.Min(_cellHeight + _payHeigtt, content.anchoredPosition.y));
            }
            else if (_nowDataIndex <= 0)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, Mathf.Max(0, content.anchoredPosition.y));
            }
        }

    }
}