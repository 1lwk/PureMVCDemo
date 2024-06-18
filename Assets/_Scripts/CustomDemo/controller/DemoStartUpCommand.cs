#region <<文件说明>>

/*--------------------------------------
//文件名称：StartUpCommand
// 创建者：李满堂
//创建时间：2022年09月12日 星期一 10:47
//文件版本：V1.0.0
//=======================================
//功能描述：
//
// 启动框架命令，注册必要的 Mediator 和 Proxy。
//
//---------------------------------------*/

#endregion

using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

/// <summary>
/// 启动框架命令类
/// </summary>
public class DemoStartUpCommand : SimpleCommand
{
    /// <summary>
    /// 执行启动命令
    /// </summary>
    /// <param name="notification">通知对象，包含启动时传递的信息</param>
    public override void Execute(INotification notification)
    {
        // 注册 DemoMediator，并传递通知中的 GameObject 作为参数
        this.Facade.RegisterMediator(new DemoMediator(notification.Body as GameObject));

        // 注册 DemoProxy
        this.Facade.RegisterProxy(new DemoProy());
    }
}
