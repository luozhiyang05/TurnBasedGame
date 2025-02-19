namespace Assets.GameSystem.MenuSystem.CharacterChose.Scripts
{
    using System;
    using System.Collections.Generic;
    using GlobalData;
    using UnityEngine;
    [Serializable]
    public class CharacterData
    {
        public int id;
        public CharacterType characterType;
        public string chaName;
        public int maxHp;
        public int maxActPoint;
        public int maxHeadCardCnt;
        public int skillCardId;
        public int cardDeckId;
    }

    [CreateAssetMenu(fileName = "角色列表", menuName = "CharactersDataSo", order = 0)]
    public class CharactersDataSo : ScriptableObject
    {
        public List<CharacterData> characterDatas;
        public CharacterData GetCharacterDataById(int id)
        {
            if (id<=0 || id>characterDatas.Count)
            {
                throw new Exception($"角色获取id{id}下标错误");
            }
            return characterDatas[id-1];
        }
    }
}