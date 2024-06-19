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

namespace PureMVC.Core
{
    /// <summary>
    /// 单例模式的 <c>IController</c> 实现。
    /// </summary>
    /// <remarks>
    /// 	<para>在 PureMVC 中，<c>Controller</c> 类遵循 'Command 和 Controller' 策略，并承担以下职责：</para>
    /// 	<list type="bullet">
    /// 		<item>记住哪些 <c>ICommand</c> 处理哪些 <c>INotification</c>。</item>
    /// 		<item>为每个有 <c>ICommand</c> 映射的 <c>INotification</c> 注册自己作为 <c>View</c> 的 <c>IObserver</c>。</item>
    /// 		<item>在收到 <c>View</c> 的通知时，为给定的 <c>INotification</c> 创建一个新的 <c>ICommand</c> 实例。</item>
    /// 		<item>调用 <c>ICommand</c> 的 <c>execute</c> 方法，传递 <c>INotification</c>。</item>
    /// 	</list>
    /// 	<para>应用程序必须在 <c>Controller</c> 中注册 <c>ICommand</c>。</para>
    /// 	<para>最简单的方法是继承 <c>Facade</c>，并使用其 <c>initializeController</c> 方法来添加注册。</para>
    /// </remarks>
    /// <see cref="PureMVC.Core.View"/>
    /// <see cref="PureMVC.Patterns.Observer"/>
    /// <see cref="PureMVC.Patterns.Notification"/>
    /// <see cref="PureMVC.Patterns.SimpleCommand"/>
    /// <see cref="PureMVC.Patterns.MacroCommand"/>
    public class Controller : IController
    {
        #region 构造函数

        /// <summary>
        /// 构造并初始化一个新的控制器
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         这个 <c>IController</c> 实现是一个单例，
        ///         所以不应该直接调用构造函数，
        ///         而是调用静态单例工厂方法 <c>Controller.Instance</c>
        ///     </para>
        /// </remarks>
        protected Controller()
        {
            m_commandMap = new Dictionary<string, Type>();
            InitializeController();
        }

        #endregion

        #region 公共方法

        #region IController 成员

        /// <summary>
        /// 如果已注册了 <c>ICommand</c> 以处理给定的 <c>INotification</c>，则执行该命令。
        /// </summary>
        /// <param name="note">一个 <c>INotification</c></param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void ExecuteCommand(INotification note)
        {
            // 定义一个变量用于存储命令类型
            Type commandType = null;

            // 锁定代码块，以确保线程安全
            lock (m_syncRoot)
            {
                // 如果命令映射表中不包含通知名称，直接返回
                if (!m_commandMap.ContainsKey(note.Name)) return;

                // 从命令映射表中获取对应通知名称的命令类型
                commandType = m_commandMap[note.Name];
            }

            // 使用 反射机制 动态创建命令类型的实例
            object commandInstance = Activator.CreateInstance(commandType);

            // 检查创建的实例是否实现了 ICommand 接口
            if (commandInstance is ICommand)
            {
                // 如果是 ICommand 实例，则调用其 Execute 方法并传递通知对象
                ((ICommand)commandInstance).Execute(note);
            }
        }


        /// <summary>
        /// 注册特定的 <c>ICommand</c> 类来处理特定的 <c>INotification</c>。
        /// </summary>
        /// <param name="notificationName">通知名称</param>
        /// <param name="commandType">命令类型</param>
        /// <remarks>
        ///     <para>
        ///         如果已经注册了 <c>ICommand</c> 来处理具有该名称的 <c>INotification</c>，
        ///         则不再使用旧的 <c>ICommand</c>，而是使用新的 <c>ICommand</c>。
        ///     </para>
        /// </remarks>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void RegisterCommand(string notificationName, Type commandType)
        {
            lock (m_syncRoot)
            {
                if (!m_commandMap.ContainsKey(notificationName))
                {
                    // 需要仔细监控此调用。必须确保 RegisterObserver 不会回调到控制器，否则可能会发生死锁。
                    m_view.RegisterObserver(notificationName, new Observer("ExecuteCommand", this));
                }

                m_commandMap[notificationName] = commandType;
            }
        }

        /// <summary>
        /// 检查是否为给定的通知注册了命令
        /// </summary>
        /// <param name="notificationName">通知名称</param>
        /// <returns>是否已为给定的通知名称注册了命令。</returns>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual bool HasCommand(string notificationName)
        {
            lock (m_syncRoot)
            {
                return m_commandMap.ContainsKey(notificationName);
            }
        }

        /// <summary>
        /// 移除先前注册的 <c>ICommand</c> 到 <c>INotification</c> 的映射。
        /// </summary>
        /// <param name="notificationName">要移除 <c>ICommand</c> 映射的通知名称</param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void RemoveCommand(string notificationName)
        {
            lock (m_syncRoot)
            {
                if (m_commandMap.ContainsKey(notificationName))
                {
                    // 移除观察者

                    // 需要仔细监控此调用。必须确保 RemoveObserver 不会回调到控制器，否则可能会发生死锁。
                    m_view.RemoveObserver(notificationName, this);
                    m_commandMap.Remove(notificationName);
                }
            }
        }

        #endregion

        #endregion

        #region 访问器

        /// <summary>
        /// 单例工厂方法。此方法是线程安全的。
        /// </summary>
        public static IController Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new Controller();
                    }
                }

                return m_instance;
            }
        }

        #endregion

        #region 保护和内部方法

        /// <summary>
        /// 显式静态构造函数告诉 C# 编译器不要将类型标记为 beforefieldinit
        /// </summary>
        static Controller()
        {
        }

        /// <summary>
        /// 初始化单例 <c>Controller</c> 实例
        /// </summary>
        /// <remarks>
        ///     <para>由构造函数自动调用</para>
        ///     
        ///     <para>
        ///         请注意，如果在应用程序中使用的是 <c>View</c> 的子类，
        ///         还应该继承 <c>Controller</c> 并在以下方式中重写 <c>initializeController</c> 方法：
        ///     </para>
        /// 
        ///     <c>
        ///         // 确保 Controller 与我的 IView 实现进行通信
        ///         public override void initializeController()
        ///         {
        ///             view = MyView.Instance;
        ///         }
        ///     </c>
        /// </remarks>
        protected virtual void InitializeController()
        {
            m_view = View.Instance;
        }

        #endregion

        #region 成员

        /// <summary>
        /// 本地对 View 的引用
        /// </summary>
        protected IView m_view;

        /// <summary>
        /// 通知名称到命令类引用的映射
        /// </summary>
        protected IDictionary<string, Type> m_commandMap;

        /// <summary>
        /// 单例实例，可以被子类化....
        /// </summary>
        protected static volatile IController m_instance;

        /// <summary>
        /// 用于锁定
        /// </summary>
        protected readonly object m_syncRoot = new object();

        /// <summary>
        /// 用于锁定实例调用
        /// </summary>
        protected static readonly object m_staticSyncRoot = new object();

        #endregion
    }
}
