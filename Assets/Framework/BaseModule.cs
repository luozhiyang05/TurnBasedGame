using System.Collections.Generic;
using Framework;
using GameSystem.MVCTemplate;
using Tool.Utilities;
using Tool.Utilities.Events;

public class BaseModule : AbsModule
{
    protected Dictionary<string, BaseCtrl> ctrlDic = new Dictionary<string, BaseCtrl>();  //viewName -> ctrl
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
    protected BaseCtrl GetCtrl(string viewName)
    {
        if (ctrlDic.ContainsKey(viewName))
        {
            return ctrlDic[viewName];
        }
        return null;
    }

    protected void SetViewInfo(string viewName, BaseCtrl ctrl)
    {
        ctrlDic.Add(viewName, ctrl);
    }

    private void BindRelease(string viewName)
    {
        string releaseViewName = "";
        foreach (var ctrl in ctrlDic)
        {
            if (ctrl.Key.Equals(viewName))
            {
                releaseViewName = ctrl.Key;
                break;
            }
        }
        ctrlDic.Remove(releaseViewName);
    }
}