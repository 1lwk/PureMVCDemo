using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

// TestMeditor 类继承自 Mediator，负责处理视图和业务逻辑之间的交互。
public class TestMeditor : Mediator
{
    // 定义中介者的名称常量。
    public const string meditorName = "TestMeditor";
    // 引用 TestView 视图组件。
    TestView testView;

    // 构造函数，初始化中介者并设置视图组件。
    public TestMeditor(GameObject root) : base(meditorName)
    {
        // 获取根对象上的 TestView 组件。
        testView = root.GetComponent<TestView>();
        // 为按钮点击事件添加监听器。
        testView.btn_GetNowTime.onClick.AddListener(GetTime_btn);
    }

    /// <summary>
    /// 列出中介者感兴趣的通知消息
    /// </summary>
    /// <returns>通知消息列表</returns>
    public override IList<string> ListNotificationInterests()
    {
        // 创建并返回一个包含感兴趣通知的数组。
        string[] list = new string[2];
        list[0] = LWK_Facade.UpdateTimer;
        return list;
    }

    /// <summary>
    /// 处理接收到的通知消息
    /// </summary>
    /// <param name="notification">通知消息</param>
    public override void HandleNotification(INotification notification)
    {
        // 根据通知的名称执行相应的操作。
        switch (notification.Name)
        {
            case LWK_Facade.UpdateTimer:
                // 如果是 UpdateTimer 通知，调用 display 方法更新视图。
                display(notification.Body as TestData);
                break;
        }
    }

    /// <summary>
    /// 更新视图显示
    /// </summary>
    /// <param name="testData">包含时间数据的 TestData 对象</param>
    public void display(TestData testData)
    {
        // 更新视图上的时间文本。
        testView.text_ShowTime.text = testData.now_time;
    }

    /// <summary>
    /// 按钮点击事件的处理方法
    /// </summary>
    public void GetTime_btn()
    {
        // 打印调试信息，并发送请求刷新时间的通知。
        Debug.Log("111");
        SendNotification(LWK_Facade.AskRefTimer);
    }
}
