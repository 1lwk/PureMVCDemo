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
    /// 一个基础 <c>ICommand</c> 实现，用于执行其他 <c>ICommand</c>。
    /// </summary>
    /// <remarks>
    ///     <para><c>MacroCommand</c> 维护一个 <c>ICommand</c> 类引用的列表，称为 <i>SubCommands</i>。</para>
    ///     <para>当调用 <c>execute</c> 时，<c>MacroCommand</c> 会实例化并依次调用每个 <i>SubCommands</i> 的 <c>execute</c> 方法。每个 <i>SubCommand</i> 都会被传递一个引用，指向最初传递给 <c>MacroCommand</c> 的 <c>INotification</c>。</para>
    ///     <para>与 <c>SimpleCommand</c> 不同，你的子类不应该重写 <c>execute</c>，而是应该重写 <c>initializeMacroCommand</c> 方法，并调用 <c>addSubCommand</c> 一次，以便每个 <i>SubCommand</i> 都被执行。</para>
    /// </remarks>
	/// <see cref="PureMVC.Core.Controller"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
	/// <see cref="PureMVC.Patterns.SimpleCommand"/>
    public class MacroCommand : Notifier, ICommand, INotifier
    {
        #region 构造函数

        /// <summary>
        /// 构造一个新的宏命令
        /// </summary>
        /// <remarks>
        ///     <para>你不需要定义构造函数，而是重写 <c>initializeMacroCommand</c> 方法。</para>
        ///     <para>如果你的子类确实定义了构造函数，请确保调用 <c>super()</c>。</para>
        /// </remarks>
        public MacroCommand()
        {
            m_subCommands = new List<Type>();
            InitializeMacroCommand();
        }

        #endregion

        #region 公共方法

        #region ICommand 成员

        /// <summary>
        /// 执行这个 <c>MacroCommand</c> 的 <i>SubCommands</i>
        /// </summary>
        /// <param name="notification">要传递给每个 <i>SubCommand</i> 的 <c>INotification</c> 对象</param>
        /// <remarks>
        ///     <para><i>SubCommands</i> 将按先进先出（FIFO）的顺序调用。</para>
        /// </remarks>
        public virtual void Execute(INotification notification)
        {
            while (m_subCommands.Count > 0)
            {
                Type commandType = m_subCommands[0];
                object commandInstance = Activator.CreateInstance(commandType);

                if (commandInstance is ICommand)
                {
                    ((ICommand)commandInstance).Execute(notification);
                }

                m_subCommands.RemoveAt(0);
            }
        }

        #endregion

        #endregion

        #region 受保护的 & 内部方法

        /// <summary>
        /// 初始化 <c>MacroCommand</c>
        /// </summary>
        /// <remarks>
        ///     <para>在你的子类中，重写此方法以初始化 <c>MacroCommand</c> 的 <i>SubCommand</i> 列表，像这样：</para>
        ///     <example>
        ///         <code>
        ///             // 初始化 MyMacroCommand
        ///             protected override initializeMacroCommand()
        ///             {
        ///                 addSubCommand( com.me.myapp.controller.FirstCommand );
        ///                 addSubCommand( com.me.myapp.controller.SecondCommand );
        ///                 addSubCommand( com.me.myapp.controller.ThirdCommand );
        ///             }
        ///         </code>
        ///     </example>
        ///     <para>注意 <i>SubCommand</i> 可以是任何 <c>ICommand</c> 实现者，<c>MacroCommand</c> 或 <c>SimpleCommand</c> 都可以接受。</para>
        /// </remarks>
        protected virtual void InitializeMacroCommand()
        {
        }

        /// <summary>
        /// 添加一个 <i>SubCommand</i>
        /// </summary>
        /// <param name="commandType">一个指向 <c>ICommand</c> 类型的引用</param>
        /// <remarks>
        ///     <para><i>SubCommands</i> 将按先进先出（FIFO）的顺序调用。</para>
        /// </remarks>
        protected void AddSubCommand(Type commandType)
        {
            m_subCommands.Add(commandType);
        }

        #endregion

        #region 成员

        private IList<Type> m_subCommands;

        #endregion
    }
}
