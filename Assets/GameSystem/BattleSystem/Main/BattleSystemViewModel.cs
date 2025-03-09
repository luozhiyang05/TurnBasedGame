using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tool.Mono;
using Tool.Utilities;
using Tool.Utilities.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GameSystem.BattleSystem.Main
{
    public class BattleSystemViewModel : BaseModel
    {
        private QArray<AbsUnit> _enemyList = new QArray<AbsUnit>(10);
        private AbsUnit _player;

        private CharacterData _characterData;
        private LevelData _levelData;
        private int _nowEnemyIndex; //当前敌人下标
        private int _nowWaveIndex;  //当前波次下标
        private int _nowRoundCnt = 0;

        private UnityAction<int> updateRoundCntTxtDelegate;
        private UnityAction setNextWavaEnemiesDataDelegate;
        private UnityAction passLevelDelegate;
        private UnityAction loseLevelDelegate;
        public override void Init()
        {
            _nowEnemyIndex = 0;
            _nowRoundCnt = 0;
            _nowWaveIndex = 1;
        }

        /// <summary>
        /// 监听某些数据更改事件,可以通知view更新
        /// </summary>
        public override void BindListener()
        {
            //绑定敌人列表死亡事件
            _enemyList.AddListenEvent(IListEventType.Remove, RemoveUnit);

            //绑定单位死亡事件
            EventsHandle.AddListenEvent<AbsUnit>(EventsNameConst.ABSUNIT_DIE, AbsUnitDie);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public override void RemoveListener()
        {
            _enemyList.RemoveListenEvent(IListEventType.Remove, RemoveUnit);

            EventsHandle.RemoveOneEventByEventName<AbsUnit>(EventsNameConst.ABSUNIT_DIE, AbsUnitDie);
        }

        /// <summary>
        /// 回合数+1
        /// </summary>
        public void UpdateRoundCnt()
        {
            _nowRoundCnt++;
            UpdateRoundCntTxt();
        }

        public void SetBattleData(CharacterData characterData,LevelData levelData)
        {
            _characterData = characterData;
            _levelData = levelData;
        }

        public void SetEnemyAbsUnit(AbsUnit absUnit)
        {
            _enemyList.Add(absUnit);
        }

        public void SetPlayerAbsUnit(AbsUnit absUnit)
        {
            _player = absUnit;
        }

        public void EnemyStartRoundSettle()
        {
            _enemyList[_nowEnemyIndex++].StartRoundSettle();
        }

        public void PlayerStartRoundSettle()
        {
            _player.StartRoundSettle();
        }

        public bool IsEnemiesAfterAct()
        {
            var isEnemiesAfterAct = _nowEnemyIndex == _enemyList.Count;
            _nowEnemyIndex = isEnemiesAfterAct ? 0 : _nowEnemyIndex;
            return isEnemiesAfterAct;
        }

        #region 数据获取
        public int GetEnemyCount() => _enemyList.Count;
        public QArray<AbsUnit> GetNowWavaAllEnemies() => _enemyList;
        public WavasData GetNowWava() => _levelData.GetWavaData(_nowWaveIndex);

        public AbsUnit GetPlayerUnit() => _player;
        public CharacterData GetCharacterData() => _characterData;
        public int GetWaveCnt() => _levelData.GetWaveCnt();
        public int GetNowWaveIndex() => _nowWaveIndex;
        #endregion

        #region 单位死亡事件
        private void RemoveUnit(AbsUnit absUnit)
        {
            Debug.Log("死亡单位:" + absUnit.unitName + " id=" + absUnit.id);
            if (_enemyList.Count == 0)
            {
                //判断当前关卡是否已经完成所有波次的推进
                if (_nowWaveIndex == _levelData.waveCnt)
                {
                    //TODO：通关提示
                    PassLevel();
                }
                else
                {
                    _nowWaveIndex++;

                    //开启下一个波次
                    ActionKit.GetInstance().DelayTime(GameManager.nextWaveWaitTime, SetNextWavaEnemiesData);
                }
            }
        }
        private void AbsUnitDie(AbsUnit absUnit)
        {
            Debug.Log("单位死亡");
            UnitDie(absUnit);
        }

        public void UnitDie(AbsUnit unit)
        {
            if (unit is Enemy)
            {
                _enemyList.Remove(unit);
            }
            else
            {
                //TODO：失败提示
                LoseLevel();
            }
        }

        #endregion

        #region 事件回调
        public void SetUpdateRoundCntTxtAction(UnityAction<int> unityAction)
        {
            updateRoundCntTxtDelegate += unityAction;
        }
        public void UpdateRoundCntTxt()
        {
            updateRoundCntTxtDelegate?.Invoke(_nowRoundCnt);
        }
        public void SetNextWavaEnemiesDataAction(UnityAction unityAction)
        {
            setNextWavaEnemiesDataDelegate = unityAction;
        }
        private void SetNextWavaEnemiesData()
        {
            setNextWavaEnemiesDataDelegate?.Invoke();
        }
        public void SetPassLevelAction(UnityAction unityAction)
        {
            passLevelDelegate = unityAction;
        }
        private void PassLevel()
        {
            passLevelDelegate?.Invoke();
        }
        public void SetLoseLevelAction(UnityAction unityAction)
        {
            loseLevelDelegate = unityAction;
        }
        private void LoseLevel()
        {
            loseLevelDelegate?.Invoke();
        }
        #endregion

    }
}