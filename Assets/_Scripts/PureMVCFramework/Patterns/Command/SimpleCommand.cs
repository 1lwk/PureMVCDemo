/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// 一个基础的 <c>ICommand</c> 实现
    /// </summary>
    /// <remarks>
    ///     <para>你的子类应重写 <c>execute</c> 方法，在该方法中你的业务逻辑将处理 <c>INotification</c></para>
    /// </remarks>
	/// <see cref="PureMVC.Core.Controller"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
	/// <see cref="PureMVC.Patterns.MacroCommand"/>
    public class SimpleCommand : Notifier, ICommand, INotifier
    {
        #region 公共方法

        #region ICommand 成员

        /// <summary>
        /// 完成由给定的 <c>INotification</c> 启动的用例
        /// </summary>
        /// <param name="notification">要处理的 <c>INotification</c></param>
        /// <remarks>
        ///     <para>在命令模式中，应用程序用例通常从某些用户操作开始，这会导致广播一个 <c>INotification</c>，该通知由 <c>ICommand</c> 的 <c>execute</c> 方法中的业务逻辑处理</para>
        /// </remarks>
        public virtual void Execute(INotification notification)
        {
        }

        #endregion

        #endregion
    }
}
