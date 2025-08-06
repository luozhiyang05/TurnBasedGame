using System.Transactions;
using Assets.Wights.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using UnityEngine;

namespace Assets.GameSystem.MusicNoteSystem.Main
{
    public class MusicNoteFloatView : BaseView
    {
        #region 自动生成UI组件区域，内部禁止手动更改！
        protected override void AutoInitUI()
        {
        }
        #endregion 自动生成UI组件区域结束！

        #region 遮罩相关
        /// <summary>
        /// 是否启用MaskPanel，启用的话只需要取消注释
        /// </summary>
        /// <returns></returns>
        public override bool MaskPanel()
        {
            return true;
        }

        // /// <summary>
        // /// 是否开启点击遮罩关闭View，启用的话只需要取消注释
        // /// </summary>
        // /// <returns></returns>
        // public override bool ClickMaskPanel()
        // {
        //     return true;
        // }

        // /// <summary>
        // /// 是否重写遮罩事件，重写后不执行父类点击遮罩关闭事件
        // /// </summary>
        // /// <returns></returns>
        // public override void OnClickMaskPanel()
        // {
        //     Debug.Log("点击了遮罩！");
        // }
        #endregion

        /// <summary>
        /// 绑定model回调事件
        /// </summary>
        protected override void BindModelListener()
        {
        }

        public PoolWight poolWight;
        public Transform upCreatNotePoint, downCreateNotePoint;
        private MusicData _musicData;
        private QArray<NoteMove> _upNoteMoveQArray,_downNoteMoveQArray; //存储实体音符

        /// <summary>
        /// 初始化,时机在Awake中
        /// </summary>
        protected override void OnInit()
        {
            SetPath("GameSystem/MusicNoteFloatView");
            _upNoteMoveQArray = new QArray<NoteMove>();
            _downNoteMoveQArray = new QArray<NoteMove>();
        }

        void Update()
        {
            if (null == _musicData)
            {
                _musicData = Model as MusicData;
            }

            //每此peek2个
            CheckCanCreateNote();

            HandleKeyDown();
        }

        private void HandleKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
            {
                if (_upNoteMoveQArray.Count > 0)
                {
                    this.GetSystem<IMusicNoteSystemModule>().PressNote(_musicData.GetNowMusicTime(), true);
                }
            }
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
            {
                if (_downNoteMoveQArray.Count > 0)
                {
                    this.GetSystem<IMusicNoteSystemModule>().PressNote(_musicData.GetNowMusicTime(), true);
                }
            }
        }

        private void CheckCanCreateNote()
        {
            if (_musicData.GetPlayState())
            {
                var _nowTime = _musicData.GetNowMusicTime();

                //上阶音符
                if (_musicData.GetRemainderNoteDataCnt(true) > 0)
                {
                    var tempNoteData = _musicData.PeekOneNoteData(true);
                    if (_nowTime >= tempNoteData.createTime)
                    {
                        CreateNote(true);
                    }
                }

                //下阶音符
                if (_musicData.GetRemainderNoteDataCnt(false) > 0)
                {
                    var tempNoteData = _musicData.PeekOneNoteData(false);
                    if (_nowTime >= tempNoteData.createTime)
                    {
                        CreateNote(false);
                    }
                }
            }
        }

        private void CreateNote(bool isUp)
        {
       

            var note = poolWight.GetFromPool(EWightType.Note);
            var noteMove = note.GetComponent<NoteMove>();
            var noteData = _musicData.GetHeadNoteData(isUp,noteMove);

            note.transform.SetParent(noteData.notePos == ENotePos.Up ? upCreatNotePoint : downCreateNotePoint);
            note.transform.localPosition = Vector3.zero;
            note.transform.localScale = Vector3.one;
            note.GetComponent<NoteMove>().SetNoteData(noteData, (noteData) =>
            {
                //过了屏幕左边，自动从音符实体列表中移除（音符进入池子）
                if (noteData.notePos == ENotePos.Up)
                {
                    var temp = _upNoteMoveQArray.FindValue(value => noteData.id == value.GetId());
                    _upNoteMoveQArray.Remove(temp);
                }
                else
                    _downNoteMoveQArray.FindValue(value => noteData.id == value.GetId());

                //回收
                poolWight.EnterPool(note);
            });

            //添加到移动列表
            if (noteData.notePos == ENotePos.Up)
                _upNoteMoveQArray.Add(note.GetComponent<NoteMove>());
            else
                _downNoteMoveQArray.Add(note.GetComponent<NoteMove>());
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            this.GetSystem<IMusicNoteSystemModule>().LoadMusic(EMusicName.Test2);
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }
    }
}