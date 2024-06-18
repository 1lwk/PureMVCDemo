#region <<文件说明>>

/*--------------------------------------
//文件名称：MyFacade
// 创建者：LWK
//创建时间：2024年06月18日 星期二
//文件版本：V1.0.0
//功能描述：启动MVC
---------------------------------------*/

#endregion

using PureMVC.Patterns;
using UnityEngine;

// LWK_Facade 类是一个单例模式，继承了 PureMVC 的 Facade 模式。
// 它负责初始化和管理应用程序中的命令。
public class LWK_Facade : Facade
{
    // LWK_Facade 的静态实例，用于实现单例模式。
    private static LWK_Facade _instance;

    // 获取 LWK_Facade 的单例实例。
    public static LWK_Facade GetInstance()
    {
        // 如果实例不存在，则创建一个新的实例。
        if (_instance == null)
            _instance = new LWK_Facade();
        return _instance;
    }

    // 初始化控制器，注册命令。
    protected override void InitializeController()
    {
        // 调用基类的 InitializeController 方法。
        base.InitializeController();
        // 注册启动命令，关联到 TestStartUPCommand 类。
        this.RegisterCommand(LWK_Facade.StartUP, typeof(TestStartUPCommand));
        // 注册询问计时器命令，关联到 TestCommand 类。
        this.RegisterCommand(LWK_Facade.AskRefTimer, typeof(TestCommand));
    }

    // 启动方法，发送启动通知。
    public void Launch(GameObject ui)
    {
        // 发送启动通知，并附带一个 GameObject 参数。
        this.SendNotification(LWK_Facade.StartUP, ui);
    }

    // 定义一些常量字符串，用作通知的名称。
    public const string StartUP = "StartUPTest";
    public const string UpdateTimer = "UpDateTimer";
    public const string AskRefTimer = "Get_NowTimer";
}
