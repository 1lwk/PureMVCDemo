/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using UnityEngine;

#endregion

namespace PureMVC.Interfaces
{
    /// <summary>
    /// PureMVC Facade的接口定义
    /// </summary>
    /// <remarks>
    ///     <para>外观模式建议提供一个单一类作为子系统通信的中心点</para>
    ///     <para>在PureMVC中，Facade作为核心MVC参与者（Model、View、Controller）与应用程序其余部分之间的接口</para>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IModel"/>
	/// <see cref="PureMVC.Interfaces.IView"/>
	/// <see cref="PureMVC.Interfaces.IController"/>
	/// <see cref="PureMVC.Interfaces.ICommand"/>
	/// <see cref="PureMVC.Interfaces.INotification"/>
    public interface IFacade : INotifier
    {
        #region Proxy

        /// <summary>
        /// 通过名称向<c>Model</c>注册<c>IProxy</c>
        /// </summary>
        /// <param name="proxy">要向<c>Model</c>注册的<c>IProxy</c></param>
        void RegisterProxy(IProxy proxy);

        /// <summary>
        /// 通过名称从<c>Model</c>检索<c>IProxy</c>
        /// </summary>
        /// <param name="proxyName">要检索的<c>IProxy</c>实例的名称</param>
        /// <returns>之前通过<c>proxyName</c>注册的<c>IProxy</c></returns>
		IProxy RetrieveProxy(string proxyName);

        /// <summary>
        /// 通过名称从<c>Model</c>移除<c>IProxy</c>实例
        /// </summary>
        /// <param name="proxyName">要从<c>Model</c>移除的<c>IProxy</c></param>
        IProxy RemoveProxy(string proxyName);

        /// <summary>
        /// 检查是否注册了一个Proxy
        /// </summary>
        /// <param name="proxyName">要检查的<c>IProxy</c>实例的名称</param>
        /// <returns>是否已经注册了具有给定<c>proxyName</c>的Proxy</returns>
        bool HasProxy(string proxyName);

        #endregion

        #region Command

        /// <summary>
        /// 向<c>Controller</c>注册<c>ICommand</c>
        /// </summary>
        /// <param name="notificationName">与<c>ICommand</c>关联的<c>INotification</c>的名称</param>
        /// <param name="commandType">与<c>ICommand</c>关联的<c>Type</c>引用</param>
        void RegisterCommand(string notificationName, Type commandType);

        /// <summary>
        /// 从Controller中移除先前注册的<c>ICommand</c>与<c>INotification</c>的映射。
        /// </summary>
        /// <param name="notificationName">从Controller中移除先前注册的<c>ICommand</c>与<c>INotification</c>的映射。</param>
		void RemoveCommand(string notificationName);

        /// <summary>
        /// 检查是否为给定的Notification注册了Command
        /// </summary>
        /// <param name="notificationName">要检查的<c>INotification</c>的名称。</param>
        /// <returns>是否为给定的<c>notificationName</c>注册了Command。</returns>
        bool HasCommand(string notificationName);

        #endregion

        #region Mediator

        /// <summary>
        /// 向<c>View</c>注册<c>IMediator</c>实例
        /// </summary>
        /// <param name="mediator">对<c>IMediator</c>实例的引用</param>
        void RegisterMediator(IMediator mediator);

        /// <summary>
        /// 从<c>View</c>检索<c>IMediator</c>实例
        /// </summary>
        /// <param name="mediatorName">要检索的<c>IMediator</c>实例的名称</param>
        /// <returns>之前通过给定<c>mediatorName</c>注册的<c>IMediator</c></returns>
		IMediator RetrieveMediator(string mediatorName);

        /// <summary>
        /// 从<c>View</c>移除<c>IMediator</c>实例
        /// </summary>
        /// <param name="mediatorName">要移除的<c>IMediator</c>实例的名称</param>
        IMediator RemoveMediator(string mediatorName);

        /// <summary>
        /// 检查是否注册了Mediator
        /// </summary>
        /// <param name="mediatorName">要检查的<c>IMediator</c>实例的名称</param>
        /// <returns>是否注册了具有给定<c>mediatorName</c>的Mediator</returns>
        bool HasMediator(string mediatorName);

        #endregion

        #region Observer

        /// <summary>
        /// 通知特定<c>INotification</c>的<c>IObservers</c>。
        /// <para>所有先前为该<c>INotification</c>列表附加的<c>IObservers</c>都会被通知，并按它们注册的顺序传递一个<c>INotification</c>引用。</para>
        /// <para>注意：仅在发送自定义通知时使用此方法。否则使用不需要创建通知实例的sendNotification方法。</para>
        /// </summary>
        /// <param name="note">要通知<c>IObservers</c>的<c>INotification</c>。</param>
        void NotifyObservers(INotification note);

        #endregion

        // SimpleFramework 代码由 Jarjin lee 编写
        void AddManager(string typeName, object obj);

        T AddManager<T>(string typeName) where T : Component;

        T GetManager<T>(string typeName) where T : class;

        void RemoveManager(string typeName);
    }
}
