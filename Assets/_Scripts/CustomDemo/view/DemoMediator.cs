#region <<文件说明>>

/*--------------------------------------
//文件名称：DemoMediator
// 创建者：李满堂
//创建时间：2022年09月12日 星期一 10:39
//文件版本：V1.0.0
//=======================================
//功能描述：
//
//
---------------------------------------*/

#endregion


using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
public class DemoMediator : Mediator
{
    public new const string NAME = "demoMediator";

    private DemoUI _demoUI;

    public DemoMediator(GameObject ui) : base(NAME, ui)
    {
        _demoUI = ui.GetComponent<DemoUI>();

        _demoUI.gameStart += GameStartHandler;
        _demoUI.areaClicked += ClickAreahandler;
    }

    private void ClickAreahandler()
    {
        (this.Facade.RetrieveProxy(DemoProy.NAME) as DemoProy).AddScore(1);
    }

    private void GameStartHandler()
    {
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>()
        {
            LMT_Facade.NameChange,
            LMT_Facade.ScoreChange
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case LMT_Facade.NameChange:

                break;

            case LMT_Facade.ScoreChange:
                this._demoUI.showNum((int) notification.Body);
                break;
        }
    }
}