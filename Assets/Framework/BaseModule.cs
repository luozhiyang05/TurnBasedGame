using System.Collections.Generic;
using Framework;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using Tool.Utilities.Events;

public class BaseModule : AbsModule
{
    protected BaseCtrl ctrl;

    protected override void InitModule()
    {
        OnInit();
    }
    /// <summary>
    /// 具体模块初始化
    /// </summary>
    protected virtual void OnInit()
    {

    }
}