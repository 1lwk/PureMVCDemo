/*
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using System.Diagnostics;
using System.Reflection;

using PureMVC.Interfaces;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// 基础的 <c>IObserver</c> 实现
    /// </summary>
    /// <remarks>
    ///     <para><c>Observer</c> 是一个对象，封装了关于有兴趣对象的信息，这些对象的方法应在特定 <c>INotification</c> 广播时被调用</para>
    ///     <para>在 PureMVC 中，<c>Observer</c> 类承担以下职责：</para>
    ///     <list type="bullet">
    ///         <item>封装有兴趣对象的通知（回调）方法</item>
    ///         <item>封装有兴趣对象的通知上下文（this）</item>
    ///         <item>提供设置通知方法和上下文的方法</item>
    ///         <item>提供通知有兴趣对象的方法</item>
    ///     </list>
    /// </remarks>
    /// <see cref="PureMVC.Core.View"/>
    /// <see cref="PureMVC.Patterns.Notification"/>
    public class Observer : IObserver
    {
        #region 构造函数

        /// <summary>
        /// 使用指定的通知方法和上下文构造一个新的观察者
        /// </summary>
        /// <param name="notifyMethod">有兴趣对象的通知方法</param>
        /// <param name="notifyContext">有兴趣对象的通知上下文</param>
        /// <remarks>
        ///     <para>有兴趣对象的通知方法应接受一个类型为 <c>INotification</c> 的参数</para>
        /// </remarks>
        public Observer(string notifyMethod, object notifyContext)
        {
            m_notifyMethod = notifyMethod;
            m_notifyContext = notifyContext;
        }

        #endregion

        #region 公共方法

        #region IObserver 成员

        /// <summary>
        /// 通知有兴趣的对象
        /// </summary>
        /// <remarks>此方法是线程安全的</remarks>
        /// <param name="notification">传递给有兴趣对象的通知方法的 <c>INotification</c></param>
        public virtual void NotifyObserver(INotification notification)
        {
            object context;
            string method;

            // 检索对象的当前状态，然后在我们的线程安全块之外通知
            lock (m_syncRoot)
            {
                context = NotifyContext;
                method = NotifyMethod;
            }

            ///通过反射机制在context对象上查找名称为method的方法，并使用notification参数调用该方法。
            ///通过这种方式，可以动态调用对象的方法，而无需在编译时确定具体的方法名和参数。
            Type t = context.GetType();
            BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            UnityEngine.Debug.Log(method);
            //通过method获取到方法
            MethodInfo mi = t.GetMethod(method, f);
            //调用将context传输过去这个 方法就是controller中的ExecuteCommand
            mi.Invoke(context, new object[] { notification });
        }

        /// <summary>
        /// 将一个对象与通知上下文进行比较
        /// </summary>
        /// <remarks>此方法是线程安全的</remarks>
        /// <param name="obj">要比较的对象</param>
        /// <returns>指示对象和通知上下文是否相同</returns>
        public virtual bool CompareNotifyContext(object obj)
        {
            lock (m_syncRoot)
            {
                // 比较当前状态
                return NotifyContext.Equals(obj);
            }
        }

        #endregion

        #endregion

        #region 访问器

        /// <summary>
        /// 有兴趣对象的通知（回调）方法
        /// </summary>
        /// <remarks>通知方法应接受一个类型为 <c>INotification</c> 的参数</remarks>
        /// <remarks>此访问器是线程安全的</remarks>
        public virtual string NotifyMethod
        {
            private get
            {
                // 引用类型的设置和获取是原子的，这里不需要锁定
                return m_notifyMethod;
            }
            set
            {
                // 引用类型的设置和获取是原子的，这里不需要锁定
                m_notifyMethod = value;
            }
        }

        /// <summary>
        /// 有兴趣对象的通知上下文（this）
        /// </summary>
        /// <remarks>此访问器是线程安全的</remarks>
        public virtual object NotifyContext
        {
            private get
            {
                // 引用类型的设置和获取是原子的，这里不需要锁定
                return m_notifyContext;
            }
            set
            {
                // 引用类型的设置和获取是原子的，这里不需要锁定
                m_notifyContext = value;
            }
        }

        #endregion

        #region 成员

        /// <summary>
        /// 保存通知方法名称
        /// </summary>
        private string m_notifyMethod;

        /// <summary>
        /// 保存通知上下文
        /// </summary>
        private object m_notifyContext;

        /// <summary>
        /// 用于锁定
        /// </summary>
        protected readonly object m_syncRoot = new object();

        #endregion
    }
}
