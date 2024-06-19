#region Using

using System;

#endregion

namespace PureMVC.Interfaces
{
    /// <summary>
    /// PureMVC 控制器接口定义
    /// </summary>
    /// <remarks>
    /// 在 PureMVC 中，<c>IController</c> 实现者遵循 'Command 和 Controller' 策略，并承担以下职责：
    /// <list type="bullet">
    ///     <item>记住哪些 <c>ICommand</c> 处理哪些 <c>INotification</c></item>
    ///     <item>为每个有 <c>ICommand</c> 映射的 <c>INotification</c> 注册自己作为 <c>View</c> 的 <c>IObserver</c></item>
    ///     <item>在收到 <c>View</c> 的通知时，为给定的 <c>INotification</c> 创建一个新的 <c>ICommand</c> 实例</item>
    ///     <item>调用 <c>ICommand</c> 的 <c>execute</c> 方法，传递 <c>INotification</c></item>
    /// </list>
    /// </remarks>
    /// <see cref="PureMVC.Interfaces.INotification"/>
    /// <see cref="PureMVC.Interfaces.ICommand"/>
    public interface IController
    {
        /// <summary>
        /// 注册特定的 <c>ICommand</c> 类来处理特定的 <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">通知名称</param>
        /// <param name="commandType">命令类型</param>
        void RegisterCommand(string notificationName, Type commandType);

        /// <summary>
        /// 执行先前注册为处理具有给定通知名称的 <c>INotification</c> 的 <c>ICommand</c>
        /// </summary>
        /// <param name="notification">要为其执行关联 <c>ICommand</c> 的通知</param>
        void ExecuteCommand(INotification notification);

        /// <summary>
        /// 移除先前注册的 <c>ICommand</c> 到 <c>INotification</c> 的映射
        /// </summary>
        /// <param name="notificationName">要移除 <c>ICommand</c> 映射的通知名称</param>
        void RemoveCommand(string notificationName);

        /// <summary>
        /// 检查是否为给定的通知名称注册了命令
        /// </summary>
        /// <param name="notificationName">要检查 <c>ICommand</c> 映射的通知名称</param>
        /// <returns>是否已为给定的通知名称注册命令</returns>
        bool HasCommand(string notificationName);
    }
}
