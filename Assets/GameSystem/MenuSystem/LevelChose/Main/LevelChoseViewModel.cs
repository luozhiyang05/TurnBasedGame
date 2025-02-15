using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using GameSystem.MVCTemplate;
using Tool.ResourceMgr;

namespace Assets.GameSystem.MenuSystem.LevelChose.Main
{
    public class LevelChoseViewModel : BaseModel
    {
        private int _nowChooseLevelId;
        private LevelsSo _levelsSo;
        public override void Init()
        {
            _nowChooseLevelId = -1;
            _levelsSo = ResMgr.GetInstance().SyncLoad<LevelsSo>("关卡设置");
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
        }
        public void SetChooseLevelId(int id)
        {
            _nowChooseLevelId = id;
        }
        public int GetChooseLevelId()
        {
            return _nowChooseLevelId;
        }
        public LevelData GetChooseLevel()
        {
            return GetLevelById(_nowChooseLevelId);
        }
        public LevelData GetLevelById(int id)
        {
            return _levelsSo.GetLevelById(id);
        }
    }
}