/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System;

namespace PureMVC.Interfaces
{
    /// <summary>
    /// PureMVC View 的接口定义
    /// </summary>
    /// <remarks>
    ///     <para>在 PureMVC 中，<c>IView</c> 实现者承担以下责任：</para>
    ///     <list type="bullet">
    ///         <item>维护 <c>IMediator</c> 实例的缓存</item>
    ///         <item>提供注册、检索和移除 <c>IMediators</c> 的方法</item>
    ///         <item>管理应用程序中每个 <c>INotification</c> 的观察者列表</item>
    ///         <item>提供一个方法将 <c>IObservers</c> 附加到 <c>INotification</c> 的观察者列表中</item>
    ///         <item>提供一个广播 <c>INotification</c> 的方法</item>
    ///         <item>当广播时，通知给定 <c>INotification</c> 的 <c>IObservers</c></item>
    ///     </list>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IMediator"/>
	/// <see cref="PureMVC.Interfaces.IObserver"/>
	/// <see cref="PureMVC.Interfaces.INotification"/>
    public interface IView
    {
        #region 观察者

        /// <summary>
        /// 注册一个 <c>IObserver</c> 以接收特定名称的 <c>INotifications</c> 的通知
        /// </summary>
        /// <param name="notificationName">要通知该 <c>IObserver</c> 的 <c>INotifications</c> 名称</param>
        /// <param name="observer">要注册的 <c>IObserver</c></param>
        void RegisterObserver(string notificationName, IObserver observer);

        /// <summary>
        /// 从给定通知名称的观察者列表中移除一组观察者
        /// </summary>
        /// <param name="notificationName">要从中移除的观察者列表</param>
        /// <param name="notifyContext">具有此对象作为其 notifyContext 的观察者</param>
        void RemoveObserver(string notificationName, object notifyContext);

        /// <summary>
        /// 通知特定 <c>INotification</c> 的 <c>IObservers</c>
        /// </summary>
        /// <param name="note">要通知 <c>IObservers</c> 的 <c>INotification</c></param>
        /// <remarks>
        ///     <para>所有之前附加到此 <c>INotification</c> 列表的 <c>IObservers</c> 都会被通知，并按注册的顺序传递 <c>INotification</c> 的引用</para>
        /// </remarks>
		void NotifyObservers(INotification note);

        #endregion

        #region 中介者

        /// <summary>
        /// 向 <c>View</c> 注册一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediator">一个 <c>IMediator</c> 实例的引用</param>
        /// <remarks>
        ///     <para>注册 <c>IMediator</c> 以便可以通过名称检索，并进一步查询 <c>IMediator</c> 以了解其 <c>INotification</c> 兴趣</para>
        ///     <para>如果 <c>IMediator</c> 返回任何感兴趣的 <c>INotification</c> 名称，将创建一个封装了 <c>IMediator</c> 实例的 <c>handleNotification</c> 方法的 <c>Observer</c>，并将其注册为所有 <c>IMediator</c> 感兴趣的 <c>INotifications</c> 的 <c>Observer</c></para>
        /// </remarks>
        void RegisterMediator(IMediator mediator);

        /// <summary>
        /// 从 <c>View</c> 中检索一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediatorName">要检索的 <c>IMediator</c> 实例的名称</param>
        /// <returns>之前使用给定 <c>mediatorName</c> 注册的 <c>IMediator</c> 实例</returns>
		IMediator RetrieveMediator(string mediatorName);

        /// <summary>
        /// 从 <c>View</c> 中移除一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediatorName">要移除的 <c>IMediator</c> 实例的名称</param>
        /// <returns>被移除的 <c>IMediator</c> 实例</returns>
		IMediator RemoveMediator(string mediatorName);

        /// <summary>
        /// 检查是否注册了某个 <c>IMediator</c>
        /// </summary>
        /// <param name="mediatorName">要检查的 <c>IMediator</c> 实例的名称</param>
        /// <returns>是否注册了具有给定 <c>mediatorName</c> 的 <c>IMediator</c>。</returns>
        bool HasMediator(string mediatorName);

        #endregion
    }
}
