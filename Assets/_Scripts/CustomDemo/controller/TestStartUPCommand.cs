using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartUPCommand : PureMVC.Patterns.SimpleCommand
{
    public override void Execute(INotification notification)
    {
        // 注册 TestMediator，并传递通知中的 GameObject 作为参数
        this.Facade.RegisterMediator(new TestMeditor(notification.Body as GameObject));

        // 注册 TestDataProxy
        this.Facade.RegisterProxy(new TestDataProxy());
    }
}
