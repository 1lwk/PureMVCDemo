/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
using System;
using System.Reflection;

using PureMVC.Patterns;

namespace PureMVC.Interfaces
{
    /// <summary>
    /// PureMVC Observer接口定义
    /// </summary>
    /// <remarks>
    ///     <para>在PureMVC中，<c>IObserver</c> 实现者承担以下职责：</para>
    ///     <list type="bullet">
    ///         <item>封装感兴趣对象的通知（回调）方法</item>
    ///         <item>封装感兴趣对象的通知上下文（<c>this</c>）</item>
    ///         <item>提供设置感兴趣对象的通知方法和上下文的方法</item>
    ///         <item>提供通知感兴趣对象的方法</item>
    ///     </list>
    ///     <para>PureMVC不依赖于底层事件模型</para>
    ///     <para>在PureMVC中实现的观察者模式旨在支持应用程序与MVC三元组的参与者之间的事件驱动通信</para>
    ///     <para>观察者是一个对象，它封装了关于一个感兴趣对象的信息，并具有在广播<c>INotification</c>时应该调用的通知方法。然后，观察者作为代理通知感兴趣的对象</para>
    ///     <para>观察者可以通过调用其<c>notifyObserver</c>方法接收<c>Notification</c>，传入实现<c>INotification</c>接口的对象，如<c>Notification</c>的子类</para>
    /// </remarks>
	/// <see cref="PureMVC.Interfaces.IView"/>
	/// <see cref="PureMVC.Interfaces.INotification"/>
    public interface IObserver
    {
        /// <summary>
		/// 感兴趣对象的通知（回调）方法
        /// </summary>
        /// <remarks>通知方法应接受一个<c>INotification</c>类型的参数</remarks>
		string NotifyMethod { set; }

        /// <summary>
		/// 感兴趣对象的通知上下文（this）
        /// </summary>
		object NotifyContext { set; }

        /// <summary>
        /// 通知感兴趣的对象
        /// </summary>
        /// <param name="notification">传递给感兴趣对象的<c>INotification</c></param>
        void NotifyObserver(INotification notification);

        /// <summary>
        /// 将给定对象与通知上下文对象进行比较
        /// </summary>
        /// <param name="obj">要比较的对象</param>
        /// <returns>指示通知上下文和对象是否相同</returns>
        bool CompareNotifyContext(object obj);
    }
}