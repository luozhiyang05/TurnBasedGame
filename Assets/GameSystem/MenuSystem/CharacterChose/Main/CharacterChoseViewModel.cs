using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using GameSystem.MVCTemplate;
using Tool.ResourceMgr;
using UnityEngine;

namespace GameSystem.MenuSystem.CharacterChose.Main
{
    public class CharacterChoseViewModel : BaseModel
    {
        private int _nowCharacterId;
        private CharactersDataSo _charactersDataSo;
        protected override void OnInit()
        {
            _nowCharacterId = -1;
            _charactersDataSo = ResMgr.GetInstance().SyncLoad<CharactersDataSo>("角色列表");
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
        /// <summary>
        /// 保存当前勾选的角色Id
        /// </summary>
        /// <param name="id"></param>
        public void SetChoseCharacter(int id)
        {
            _nowCharacterId = id;
        }
        /// <summary>
        /// 获取当前勾选的角色Id，用于View检测是否可以进入下一步
        /// </summary>
        /// <returns></returns>
        public int GetChoseCharacterId()
        {
            return _nowCharacterId;
        }
        /// <summary>
        /// 获取当前选择了的角色信息，用于在进入关卡时，初始化里面的角色
        /// </summary>
        /// <returns></returns>
        public CharacterData GetChoseCharacter()
        {
            return GetCharacterDataById(_nowCharacterId);
        }
        /// <summary>
        /// 获取角色信息，用于在选角面板根据id展示信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CharacterData GetCharacterDataById(int id)
        {
            return _charactersDataSo.GetCharacterDataById(id);
        }
    }
}