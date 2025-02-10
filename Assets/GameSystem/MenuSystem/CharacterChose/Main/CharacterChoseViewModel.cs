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

        public void SetChoseCharacter(int id)
        {
            _nowCharacterId = id;
        }
        public int GetChoseCharacterId()
        {
            return _nowCharacterId;
        }
        public CharacterData GetCharacterDataById(int id)
        {
            return _charactersDataSo.GetCharacterDataById(id);
        }
    }
}