#region <<文件说明>>

/*--------------------------------------
//文件名称：MyFacade
// 创建者：李满堂
//创建时间：2022年09月12日 星期一 10:20
//文件版本：V1.0.0
//=======================================
//功能描述：
//
//
---------------------------------------*/

#endregion

using PureMVC.Patterns;
using UnityEngine;

public class LMT_Facade : Facade
{
    private static LMT_Facade _instance;

    public static LMT_Facade GetInstance()
    {
        if (_instance == null)
            _instance = new LMT_Facade();
        return _instance;
    }

    protected override void InitializeController()
    {
        base.InitializeController();
        this.RegisterCommand(LMT_Facade.StartUP, typeof(DemoStartUpCommand));
    }

    public void Launch(GameObject ui)
    {
        this.SendNotification(LMT_Facade.StartUP, ui);
    }

    public const string ScoreChange = "scoreChange";
    public const string NameChange = "nameChanged";
    public const string StartUP = "StartUP";
}