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
        //添加视图回收事件
        EventsHandle.AddListenEvent<string>(EventsNameConst.RELEASE_VIEW, BindRelease);
        OnInit();
    }
    /// <summary>
    /// 具体模块初始化
    /// </summary>
    protected virtual void OnInit()
    {

    }

    private void BindRelease(string viewName)
    {
        if (null != ctrl && ctrl.MainViewName == viewName)
        {
            ctrl = null;
        }
    }
}