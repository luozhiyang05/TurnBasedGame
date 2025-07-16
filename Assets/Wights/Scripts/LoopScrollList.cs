using System;
using System.Collections.Generic;
using Tool.CustomAttribute;
using Tool.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wights.Utilities
{
    public class LoopScrollList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
    {
        public bool isVertical = true;
        public RectTransform cell;
        public RectTransform viewport;
        public RectTransform content;
        private Vector2 _oldMousePos;
        public int DataCnt => data.Count;
        private List<int> data = new List<int>();
        private float _cellHeight;
        private float _viewportHeight;
        public float MoveDis => _moveDis;
        private float _moveDis;
        public int UpIndex => _upIndex;
        public int DownIndex => _downIndex;
        private int _upIndex, _downIndex;
        private int _diffIndex;
        private int _initCellCnt;
        public float PayHeight => _payHeight;
        private float _payHeight;
        private bool _isScrollUp;
        private bool _isScrollDown;
        public bool NeedMinPeyHeight => _needMinPeyHeight;
        private bool _needMinPeyHeight;
        private bool _banScroll;
        private bool _startRender = false;
        [CustomPropertyText("滑动速度")]
        public float moveSpeed = 1;
        private QArray<ListViewCell> _cellList = new QArray<ListViewCell>();
        private UnityAction<GameObject, int> _renderCellAction;
        public LoopScrollList InitDataSize(int dataSize)
        {
            //清除缓存cell
            data.Clear();
            var cellCnt = content.transform.childCount;
            for (int i = 0; i < cellCnt; i++)
            {
                var cell = content.transform.GetChild(i).gameObject;
                GameObject.Destroy(cell);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            content.anchoredPosition = Vector2.zero;

            //数据索引列表
            for (int i = 0; i < dataSize; i++)
            {
                data.Add(i);
            }

            //cell高度和视图高度
            _cellHeight = isVertical ? cell.rect.height : cell.rect.width;
            _viewportHeight = isVertical ? viewport.rect.height : viewport.rect.width;

            //初始化cell的个数，在需要补偿的时候多生成一个cell，防止动画没能显示全
            _initCellCnt = Mathf.Abs(_viewportHeight % _cellHeight) > 0
            ? Mathf.CeilToInt(_viewportHeight / _cellHeight) + 1
            : (int)(_viewportHeight / _cellHeight + 1);

            //初始化cell的个数不能大于数据源的个数
            var _initCellCntBiger = _initCellCnt > data.Count;
            //特殊情况特殊处理，原本应该多初始化2个cell用于循环，但是如果数据源数量过少，只会初始化1个cell（不用循环），所以要使用最低高度补偿限制滑动
            _needMinPeyHeight = _initCellCntBiger;
            _initCellCnt = _initCellCntBiger ? data.Count : _initCellCnt;

            //补偿高度，当viewport的高度不能取余cell的高度==0时，为了让viewport拉到底部能显示全cell，需要计算补偿高度
            _payHeight = Mathf.Abs(_viewportHeight % _cellHeight) > 0
            ? _cellHeight - _viewportHeight % _cellHeight
            : 0;

            //初始化cell个数没有铺满viewport时禁止滑动
            _banScroll = _initCellCnt * _cellHeight <= _viewportHeight;

            //cell索引初始化，_downIndex为视图底部准备渲染的cell索引
            _upIndex = 0;
            _downIndex = Mathf.Abs(_viewportHeight % _cellHeight) > 0
            ? Mathf.CeilToInt(_viewportHeight / _cellHeight)
            : (int)(_viewportHeight / _cellHeight);
            _diffIndex = _downIndex - _upIndex;

            //滑动缓存速度
            _scrollBufferSpeed = scrollBufferSpeed;

            return this;
        }
        public LoopScrollList SetRenderEvent(UnityAction<GameObject, int> renderCellAction)
        {
            _renderCellAction = renderCellAction;
            return this;
        }
        public LoopScrollList UpdateList()
        {
            //初始化渲染这些cell
            InitCells();
            _startRender = true;
            return this;
        }
        private void InitCells()
        {
            for (int i = 0; i < _initCellCnt; i++)
            {
                var cellGo = Instantiate(cell, content).gameObject;
                var listViewCell = cellGo.transform.GetComponent<ListViewCell>();
                listViewCell.Init(this);
                listViewCell.SetStopBufferCallback(() => _scrollBuffer = false);
                listViewCell.CliclkAction = SelectCellCallback;
                listViewCell.UpdateCell(i);
                _cellList.Add(listViewCell);
                cellGo.SetActive(true);
                UpdateItem(cellGo, i);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        private void Update()
        {
            //cell定位
            Moving();
            //滑动缓冲
            ScrollBuffer();
            //边界锁定
            JudgeEdge();
            //视图循环刷新
            ListViewUpdate();
        }

        #region 拖拽
        public void OnBeginDrag(PointerEventData eventData)
        {
            _scrollBuffer = false;
            _scrollBufferSpeed = scrollBufferSpeed;
            _oldMousePos = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (_banScroll || !_startRender) return;

            //content移动
            _moveDis = isVertical ? (eventData.position.y - _oldMousePos.y) : (eventData.position.x - _oldMousePos.x) * moveSpeed;
            if (_moveDis != 0)
            {
                var signal = _moveDis / Mathf.Abs(_moveDis);
                _moveDis = Mathf.Min(_viewportHeight, Mathf.Abs(_moveDis)) * signal;
            }
            _isScrollUp = _moveDis > 0;
            _isScrollDown = _moveDis < 0;
            if (isVertical)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + _moveDis);
            }
            else
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x + _moveDis, content.anchoredPosition.y);
            }
            _oldMousePos = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            _oldMousePos = Vector2.zero;
            _scrollBufferSpeed = Mathf.Abs(scrollBufferSpeed * _moveDis);
            _scrollBufferSpeed = Mathf.Min(_scrollBufferSpeed, 1000);
            _scrollBuffer = true;
        }
        #endregion

        #region 移动列表
        public void JudgeEdge()
        {
            //滑到底部的边界限制
            if (_downIndex >= data.Count - 1 && ((isVertical ? _isScrollUp : _isScrollDown) || _isMoving))
            {
                var lockPos = isVertical ?
                new Vector2(0, Mathf.Min(_needMinPeyHeight ? _payHeight : (_cellHeight + _payHeight), content.anchoredPosition.y)) :
                new Vector2(Mathf.Max(_needMinPeyHeight ? -_payHeight : -(_cellHeight + _payHeight), content.anchoredPosition.x), 0);
                content.anchoredPosition = lockPos;
                var isEdge = isVertical ? (content.anchoredPosition.y == (_needMinPeyHeight ? _payHeight : (_cellHeight + _payHeight))) : (content.anchoredPosition.x == (_needMinPeyHeight ? -_payHeight : -(_cellHeight + _payHeight)));
                if (_isMoving && isEdge)
                {
                    _isMoving = false;
                    if (_moveFinishNeedSelect)
                    {
                        _moveFinishNeedSelect = false;
                        SelectCell(_moveToIndex);
                        _moveToIndex = -1;
                    }
                }

                if (_scrollBuffer && isEdge)
                {
                    _scrollBuffer = false;
                    _scrollBufferSpeed = scrollBufferSpeed;
                }
            }
            //滑倒顶部的边界限制
            else if (_upIndex <= 0 && ((isVertical ? _isScrollDown : _isScrollUp) || _isMoving))
            {
                var lockPos = isVertical ? new Vector2(0, Mathf.Max(0, content.anchoredPosition.y)) : new Vector2(Mathf.Min(0, content.anchoredPosition.x), 0);
                var isEdge = isVertical ? content.anchoredPosition.y == 0 : content.anchoredPosition.x == 0;
                content.anchoredPosition = lockPos;
                if (_isMoving && isEdge)
                {
                    _isMoving = false;
                    if (_moveFinishNeedSelect)
                    {
                        _moveFinishNeedSelect = false;
                        SelectCell(_moveToIndex);
                        _moveToIndex = -1;
                    }
                }

                if (_scrollBuffer && isEdge)
                {
                    _scrollBuffer = false;
                    _scrollBufferSpeed = scrollBufferSpeed;
                }
            }
        }
        public void ListViewUpdate()
        {
            if (isVertical)
            {
                var contentY = content.anchoredPosition.y;
                //有补偿高度时，为了让viewprot拉到最底部全部完全显示cell，则需要把限制高度加上补偿高度，不然无法限制继续往上滑动渲染
                if (contentY > _cellHeight + _payHeight)
                {
                    var firstCell = content.GetChild(0);
                    firstCell.SetAsLastSibling();
                    content.anchoredPosition = new Vector2(0, content.anchoredPosition.y - _cellHeight);
                    _downIndex++;
                    _upIndex++;
                    UpdateItem(firstCell.gameObject, _downIndex);
                    var listViewCell = _cellList.RemoveAt(0);
                    _cellList.Add(listViewCell);
                }
                else if (contentY < 0)
                {
                    var lastCell = content.GetChild(content.childCount - 1);
                    lastCell.SetAsFirstSibling();
                    content.anchoredPosition = new Vector2(0, content.anchoredPosition.y + _cellHeight);
                    _downIndex--;
                    _upIndex--;
                    UpdateItem(lastCell.gameObject, _upIndex);
                    var listViewCell = _cellList.RemoveAt(_cellList.Count - 1);
                    _cellList.Insert(listViewCell, 0);
                }
            }
            else
            {
                var contentX = content.anchoredPosition.x;
                if (contentX < -(_cellHeight + _payHeight))
                {
                    var firstCell = content.GetChild(0);
                    firstCell.SetAsLastSibling();
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x + _cellHeight, 0);
                    _downIndex++;
                    _upIndex++;
                    UpdateItem(firstCell.gameObject, _downIndex);
                    var listViewCell = _cellList.RemoveAt(0);
                    _cellList.Add(listViewCell);
                }
                else if (contentX > 0)
                {
                    var lastCell = content.GetChild(content.childCount - 1);
                    lastCell.SetAsFirstSibling();
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x - _cellHeight, 0);
                    _downIndex--;
                    _upIndex--;
                    UpdateItem(lastCell.gameObject, _upIndex);
                    var listViewCell = _cellList.RemoveAt(_cellList.Count - 1);
                    _cellList.Insert(listViewCell, 0);
                }
            }

        }
        private void UpdateItem(GameObject cell, int index)
        {
            cell.transform.GetComponent<ListViewCell>().UpdateCell(index);
            if (_selectIndex != -1) _selectChangeStyle?.Invoke(index, cell, index == _selectIndex);
            _renderCellAction?.Invoke(cell, index);
        }
        #endregion

        #region cell点击
        private int _selectIndex = -1;
        private GameObject _selectCell = null;
        public void SelectCellCallback(int index, GameObject cell, bool click)
        {
            if (_selectIndex != -1 && _selectCell != null)
            {
                _selectChangeStyle?.Invoke(_selectIndex, _selectCell, false);
            }
            _selectIndex = index;
            _selectCell = cell;

            _selectChangeStyle?.Invoke(index, cell, click);
            _selectCellCallback?.Invoke(index, cell, click);
        }

        private UnityAction<int, GameObject, bool> _selectChangeStyle;
        private UnityAction<int, GameObject, bool> _selectCellCallback;
        public LoopScrollList SetSelectChangeStyle(UnityAction<int, GameObject, bool> callback)
        {
            _selectChangeStyle = callback;
            return this;
        }
        public LoopScrollList SetSelectCellCallback(UnityAction<int, GameObject, bool> callback)
        {
            _selectCellCallback = callback;
            return this;
        }
        #endregion

        #region 选择cell
        public void SelectCell(int index, bool isExcuteCallback = true)
        {
            var cell = _cellList.FindValue(x => x.GetIndex() == index);
            if (cell == null)
            {
                return;
            }

            if (_selectIndex != -1 && _selectCell != null)
            {
                _selectChangeStyle?.Invoke(_selectIndex, _selectCell, false);
            }

            _selectIndex = index;
            _selectCell = cell.gameObject;
            _selectChangeStyle?.Invoke(index, cell.gameObject, true);

            if (isExcuteCallback)
            {
                _selectCellCallback?.Invoke(index, cell.gameObject, true);
            }
        }
        #endregion

        #region cell定位
        [CustomPropertyText("移动定位速度")]
        public float moveToSpeed = 2000f;
        private bool _isMoving = false;
        private int _moveToIndex = -1;
        private int _moveDir = 0;
        private bool _moveFinishNeedSelect = false;
        public void MoveToIndex(int index, bool select = false)
        {
            _moveToIndex = index;
            _isMoving = true;
            _moveDir = index > _upIndex ? 1 : -1;
            _moveFinishNeedSelect = select;
        }

        private void Moving()
        {
            if (!_isMoving) return;
            var oldXorY = isVertical ? content.anchoredPosition.y : content.anchoredPosition.x;
            var targetXorY = oldXorY + Time.deltaTime * moveToSpeed * (isVertical ? _moveDir : -_moveDir);
            var targetPos = isVertical ? new Vector2(0, targetXorY) : new Vector2(targetXorY, 0);
            content.anchoredPosition = targetPos;

            if (_upIndex == _moveToIndex)
            {
                _isMoving = false;
                content.anchoredPosition = new Vector2(0, 0);
                if (_moveFinishNeedSelect)
                {
                    _moveFinishNeedSelect = false;
                    SelectCell(_moveToIndex);
                }
                _moveToIndex = -1;
            }
        }
        #endregion

        #region cell跳转
        public void JumpToIndex(int index, bool select = false)
        {
            if (index < 0 || index >= data.Count)
            {
                throw new Exception("跳转index超出范围");
            }
            var tempIndex = index;
            _scrollBuffer = false;

            //视图可展示的cell的数量比要跳转的index下所有的cell数量都多时，就要滑到底部，避免留空
            var needJumpToListBottom = (_viewportHeight / _cellHeight) > data.Count - index;
            _downIndex = data.Count - 1;
            _upIndex = _downIndex - _diffIndex;
            if (needJumpToListBottom)
            {
                content.anchoredPosition = isVertical ? new Vector2(0, _cellHeight + _payHeight) : new Vector2(-(_cellHeight + _payHeight), 0);

                int cellIndex = 0;
                for (int i = _upIndex; i <= _downIndex; i++)
                {
                    UpdateItem(_cellList[cellIndex++].gameObject, i);
                }
            }
            else
            {
                bool needPay = false;
                var remainCellCnt = data.Count - index;
                if (remainCellCnt < _initCellCnt)
                {

                    //特殊情况，需要content往上移补偿，避免向下滑动时渲染到不存在的数据
                    needPay = true;
                    _upIndex = --index;
                    _downIndex = data.Count - 1;
                }
                else
                {
                    needPay = false;
                    _upIndex = index;
                    _downIndex = _upIndex + _initCellCnt - 1;
                }
                content.anchoredPosition = isVertical ? new Vector2(0, needPay ? _cellHeight : 0) : new Vector2(needPay ? -_cellHeight : 0, 0);
                for (int i = 0; i < _initCellCnt; i++)
                {
                    UpdateItem(_cellList[i].gameObject, index++);
                }
            }

            if (select)
            {
                SelectCell(tempIndex);
            }
        }
        #endregion

        #region 拖动缓冲
        [CustomPropertyText("缓冲速度")]
        public float scrollBufferSpeed = 10;
        [CustomPropertyText("缓冲衰落速度")]
        public float bufferDeclineSpeed = 40;
        [CustomPropertyText("滚轮缓冲速度")]
        public float scrollSpeed = 50;
        [CustomPropertyText("是否开启缓冲")]
        public bool useBuffer = true;
        public float ScrollBufferSpeed => _scrollBufferSpeed;
        private float _scrollBufferSpeed;
        private bool _scrollBuffer = false;
        public void ScrollBuffer()
        {
            if (useBuffer == false)
            {
                return;
            }
            if (_scrollBuffer)
            {
                var dir = _isScrollUp ? 1 : (_isScrollDown ? -1 : 0);
                if (isVertical)
                {
                    var targetY = content.anchoredPosition.y + dir * _scrollBufferSpeed * Time.deltaTime;
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, targetY);
                }
                else
                {
                    var targetX = content.anchoredPosition.x + dir * _scrollBufferSpeed * Time.deltaTime;
                    content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
                }
                _scrollBufferSpeed -= scrollBufferSpeed * Time.deltaTime * bufferDeclineSpeed;
                if (_scrollBufferSpeed < 0.1)
                {
                    _scrollBufferSpeed = scrollBufferSpeed;
                    _scrollBuffer = false;
                    return;
                }
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            _scrollBuffer = false;
            _scrollBufferSpeed = scrollBufferSpeed * scrollSpeed;
            _isScrollUp = eventData.scrollDelta.y > 0;
            _isScrollDown = eventData.scrollDelta.y < 0;
            _scrollBuffer = true;
        }
        #endregion
    }
}