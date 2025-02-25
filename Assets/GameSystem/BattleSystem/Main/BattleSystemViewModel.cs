using Assets.GameSystem.BattleSystem.Scripts;
using Assets.GameSystem.MenuSystem;
using Assets.GameSystem.MenuSystem.CharacterChose.Scripts;
using Assets.GameSystem.MenuSystem.LevelChose.Scripts;
using Framework;
using GameSystem.MVCTemplate;
using GlobalData;
using Tips;
using Tool.Mono;
using Tool.UI;
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

        private UnityAction setNextWavaEnemiesDataDelegate;
        public override void Init()
        {
            _nowEnemyIndex = 0;
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


        public void UnitDie(AbsUnit unit)
        {
            if (unit is Enemy)
            {
                _enemyList.Remove(unit);
            }
            else
            {
                Debug.Log("玩家死亡");
            }
        }

        #region 数据获取
        public int GetEnemyCount() => _enemyList.Count;
        public WavasData GetNowWava() => _levelData.GetWavaData(_nowWaveIndex);

        public AbsUnit GetPlayerUnit() => _player;
        public CharacterData GetCharacterData() => _characterData;
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
                    TipsModule.ComfirmTips("tips_1001", "win_1001", () =>
                    {
                        UIManager.GetInstance().CloseAllViewByLayer(EuiLayer.GameUI);
                        this.GetSystem<IMenuSystemModule>().ShowView();
                    });
                }
                else
                {
                    _nowWaveIndex++;
                    //开启下一个波次
                    this.GetSystem<IBattleSystemModule>().BulletScreenTimeDelegate(SetNextWavaEnemiesData, GameManager.GetText("waveTip_1001"));
                }
            }
        }
        private void AbsUnitDie(AbsUnit absUnit)
        {
            Debug.Log("单位死亡");
            UnitDie(absUnit);
        }
        #endregion

        #region 事件回调
        public void SetNextWavaEnemiesDataAction(UnityAction unityAction)
        {
            setNextWavaEnemiesDataDelegate = unityAction;
        }
        private void SetNextWavaEnemiesData()
        {
            setNextWavaEnemiesDataDelegate?.Invoke();
        }
        #endregion

    }
}