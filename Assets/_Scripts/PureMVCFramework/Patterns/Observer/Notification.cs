/*
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved.
 Your reuse is governed by the Creative Commons Attribution 3.0 License
*/

#region Using

using System;

using PureMVC.Interfaces;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// 一个基本的 <c>INotification</c> 实现
    /// </summary>
    /// <remarks>
    ///     <para>PureMVC 不依赖于底层事件模型</para>
    ///     <para>在 PureMVC 中实现的观察者模式旨在支持应用程序与 MVC 三元组的参与者之间的事件驱动通信</para>
    ///     <para>通知不是事件的替代品。通常，<c>IMediator</c> 实现者会在其视图组件上放置事件处理程序，然后以通常方式处理这些事件。这可能会导致广播 <c>Notification</c> 以触发 <c>ICommand</c> 或与其他 <c>IMediator</c> 通信。<c>IProxy</c> 和 <c>ICommand</c> 实例通过广播 <c>INotification</c> 相互通信以及与 <c>IMediator</c> 通信</para>
    /// </remarks>
    /// <see cref="PureMVC.Patterns.Observer"/>
    public class Notification : INotification
    {
        #region Constructors

        /// <summary>
        /// 使用指定名称、默认主体和类型构造一个新的通知
        /// </summary>
        /// <param name="name"><c>Notification</c> 实例的名称</param>
        public Notification(string name)
            : this(name, null, null)
        { }

        /// <summary>
        /// 使用指定名称和主体、默认类型构造一个新的通知
        /// </summary>
        /// <param name="name"><c>Notification</c> 实例的名称</param>
        /// <param name="body"><c>Notification</c> 的主体</param>
        public Notification(string name, object body)
            : this(name, body, null)
        { }

        /// <summary>
        /// 使用指定名称、主体和类型构造一个新的通知
        /// </summary>
        /// <param name="name"><c>Notification</c> 实例的名称</param>
        /// <param name="body"><c>Notification</c> 的主体</param>
        /// <param name="type"><c>Notification</c> 的类型</param>
        public Notification(string name, object body, string type)
        {
            m_name = name;
            m_body = body;
            m_type = type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取 <c>Notification</c> 实例的字符串表示形式
        /// </summary>
        /// <returns><c>Notification</c> 实例的字符串表示形式</returns>
        public override string ToString()
        {
            string msg = "Notification Name: " + Name;
            msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
            msg += "\nType:" + ((Type == null) ? "null" : Type);
            return msg;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// <c>Notification</c> 实例的名称
        /// </summary>
        public virtual string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// <c>Notification</c> 实例的主体
        /// </summary>
        /// <remarks>该访问器是线程安全的</remarks>
        public virtual object Body
        {
            get
            {
                // 引用类型的设置和获取是原子的，这里不需要锁
                return m_body;
            }
            set
            {
                // 引用类型的设置和获取是原子的，这里不需要锁
                m_body = value;
            }
        }

        /// <summary>
        /// <c>Notification</c> 实例的类型
        /// </summary>
        /// <remarks>该访问器是线程安全的</remarks>
        public virtual string Type
        {
            get
            {
                // 引用类型的设置和获取是原子的，这里不需要锁
                return m_type;
            }
            set
            {
                // 引用类型的设置和获取是原子的，这里不需要锁
                m_type = value;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// 通知实例的名称
        /// </summary>
        private string m_name;

        /// <summary>
        /// 通知实例的类型
        /// </summary>
        private string m_type;

        /// <summary>
        /// 通知实例的主体
        /// </summary>
        private object m_body;

        #endregion
    }
}
