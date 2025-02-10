namespace Assets.GameSystem.MenuSystem.CharacterChose.Scripts
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    [Serializable]
    public class CharacterData
    {
        public int id;
        public string chaName;
        public int maxHp;
        public int maxActPoint;
        public int maxHeadCardCnt;
        public int skillCardId;
    }

    [CreateAssetMenu(fileName = "角色列表", menuName = "CharactersDataSo", order = 0)]
    public class CharactersDataSo : ScriptableObject
    {
        public List<CharacterData> characterDatas;
    }
}