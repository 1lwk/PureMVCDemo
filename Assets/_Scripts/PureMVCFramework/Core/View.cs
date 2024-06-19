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
    /// 一个单例 <c>IView</c> 的实现。
    /// </summary>
    /// <remarks>
    ///     <para>在 PureMVC 中，<c>View</c> 类承担以下责任：</para>
    ///     <list type="bullet">
    ///         <item>维护 <c>IMediator</c> 实例的缓存</item>
    ///         <item>提供注册、检索和移除 <c>IMediators</c> 的方法</item>
    ///         <item>管理应用程序中每个 <c>INotification</c> 的观察者列表</item>
    ///         <item>提供一个方法将 <c>IObservers</c> 附加到 <c>INotification</c> 的观察者列表中</item>
    ///         <item>提供一个广播 <c>INotification</c> 的方法</item>
    ///         <item>当广播时，通知给定 <c>INotification</c> 的 <c>IObservers</c></item>
    ///     </list>
    /// </remarks>
	/// <see cref="PureMVC.Patterns.Mediator"/>
	/// <see cref="PureMVC.Patterns.Observer"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
    public class View : IView
    {
        #region 构造函数

        /// <summary>
        /// 构造并初始化一个新的视图
        /// </summary>
        /// <remarks>
        /// <para>这个 <c>IView</c> 实现是一个单例，因此不应该直接调用构造函数，而是调用静态单例工厂方法 <c>View.Instance</c></para>
        /// </remarks>
        protected View()
        {
            m_mediatorMap = new Dictionary<string, IMediator>();
            m_observerMap = new Dictionary<string, IList<IObserver>>();
            InitializeView();
        }

        #endregion

        #region 公共方法

        #region IView 成员

        #region 观察者

        /// <summary>
        /// 注册一个 <c>IObserver</c> 以接收特定名称的 <c>INotifications</c> 的通知
        /// </summary>
        /// <param name="notificationName">要通知该 <c>IObserver</c> 的 <c>INotifications</c> 名称</param>
        /// <param name="observer">要注册的 <c>IObserver</c></param>
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            lock (m_syncRoot)
            {
                if (!m_observerMap.ContainsKey(notificationName))
                {
                    m_observerMap[notificationName] = new List<IObserver>();
                }

                m_observerMap[notificationName].Add(observer);
            }
        }

        /// <summary>
        /// 通知特定 <c>INotification</c> 的 <c>IObservers</c>
        /// </summary>
        /// <param name="notification">要通知 <c>IObservers</c> 的 <c>INotification</c></param>
        /// <remarks>
        /// <para>所有之前附加到此 <c>INotification</c> 列表的 <c>IObservers</c> 都会被通知，并按注册的顺序传递 <c>INotification</c> 的引用</para>
        /// </remarks>
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual void NotifyObservers(INotification notification)
        {
            IList<IObserver> observers = null;

            lock (m_syncRoot)
            {
                if (m_observerMap.ContainsKey(notification.Name))
                {
                    // 获取观察者列表的引用
                    IList<IObserver> observers_ref = m_observerMap[notification.Name];
                    // 将观察者从引用数组复制到工作数组，
                    // 因为在通知循环期间引用数组可能会发生变化
                    observers = new List<IObserver>(observers_ref);
                }
            }

            // 在锁外通知
            if (observers != null)
            {
                // 从工作数组中通知观察者				
                for (int i = 0; i < observers.Count; i++)
                {
                    IObserver observer = observers[i];
                    observer.NotifyObserver(notification);
                }
            }
        }

        /// <summary>
        /// 从给定通知名称的观察者列表中移除一个特定的观察者
        /// </summary>
        /// <param name="notificationName">要从中移除的观察者列表</param>
        /// <param name="notifyContext">作为其通知上下文的观察者</param>
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            lock (m_syncRoot)
            {
                // 检查通知名称的观察者列表
                if (m_observerMap.ContainsKey(notificationName))
                {
                    IList<IObserver> observers = m_observerMap[notificationName];

                    // 查找特定的观察者
                    for (int i = 0; i < observers.Count; i++)
                    {
                        if (observers[i].CompareNotifyContext(notifyContext))
                        {
                            // 在任何给定的观察者列表中，notifyContext 只能有一个观察者
                            // 移除该观察者并终止循环
                            observers.RemoveAt(i);
                            break;
                        }
                    }

                    // 当通知的观察者列表长度减少到零时，
                    // 从观察者映射中删除该通知键
                    if (observers.Count == 0)
                    {
                        m_observerMap.Remove(notificationName);
                    }
                }
            }
        }

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
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual void RegisterMediator(IMediator mediator)
        {
            // 使用锁来确保多线程环境下的线程安全
            lock (m_syncRoot)
            {
                // 如果 Mediator 已经被注册过了，则不允许重复注册
                // 你必须先调用 removeMediator 进行移除
                if (m_mediatorMap.ContainsKey(mediator.MediatorName)) return;

                // 根据 Mediator 的名字进行注册，以便后续通过名字进行检索
                m_mediatorMap[mediator.MediatorName] = mediator;

                // 获取 Mediator 关注的通知列表
                // 在注册 Mediator 时，可以从被注册的 Mediator 里获取其关注的通知名字列表
                IList<string> interests = mediator.ListNotificationInterests();

                // 如果 Mediator 有关注的通知
                if (interests.Count > 0)
                {
                    // 创建一个 Observer，用于处理这些通知
                    IObserver observer = new Observer("handleNotification", mediator);

                    // 为 Mediator 关注的每一个通知注册这个 Observer
                    for (int i = 0; i < interests.Count; i++)
                    {
                        RegisterObserver(interests[i].ToString(), observer);
                    }
                }
            }

            // 通知 Mediator 它已经被注册
            mediator.OnRegister();
        }


        /// <summary>
        /// 从 <c>View</c> 中检索一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediatorName">要检索的 <c>IMediator</c> 实例的名称</param>
        /// <returns>之前使用给定 <c>mediatorName</c> 注册的 <c>IMediator</c> 实例</returns>
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            lock (m_syncRoot)
            {
                if (!m_mediatorMap.ContainsKey(mediatorName)) return null;
                return m_mediatorMap[mediatorName];
            }
        }

        /// <summary>
        /// 从 <c>View</c> 中移除一个 <c>IMediator</c>
        /// </summary>
        /// <param name="mediatorName">要移除的 <c>IMediator</c> 实例的名称</param>
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            IMediator mediator = null;

            lock (m_syncRoot)
            {
                // 检索指定名称的中介者
                if (!m_mediatorMap.ContainsKey(mediatorName)) return null;
                mediator = (IMediator)m_mediatorMap[mediatorName];

                // 获取中介者感兴趣的每个通知...
                IList<string> interests = mediator.ListNotificationInterests();

                for (int i = 0; i < interests.Count; i++)
                {
                    // 移除将中介者与通知兴趣链接起来的观察者
                    RemoveObserver(interests[i], mediator);
                }

                // 从映射中移除中介者		
                m_mediatorMap.Remove(mediatorName);
            }

            // 通知中介者它已经被移除
            if (mediator != null) mediator.OnRemove();
            return mediator;
        }

        /// <summary>
        /// 检查是否已注册某个中介者
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns>是否注册了具有给定 <code>mediatorName</code> 的中介者。</returns>
        /// <remarks>这个方法是线程安全的，所有实现中都需要是线程安全的。</remarks>
        public virtual bool HasMediator(string mediatorName)
        {
            lock (m_syncRoot)
            {
                return m_mediatorMap.ContainsKey(mediatorName);
            }
        }

        #endregion

        #endregion

        #endregion

        #region 访问器

        /// <summary>
        /// 视图单例工厂方法。 这个方法是线程安全的。
        /// </summary>
        public static IView Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new View();
                    }
                }

                return m_instance;
            }
        }

        #endregion

        #region 受保护的 & 内部方法

        /// <summary>
        /// 显式静态构造函数告诉 C# 编译器不要将类型标记为 beforefieldinit
        /// </summary>
        static View()
        {
        }

        /// <summary>
        /// 初始化单例 View 实例
        /// </summary>
        /// <remarks>
        /// <para>由构造函数自动调用，这是您在不覆盖构造函数的情况下在子类中初始化单例实例的机会</para>
        /// </remarks>
        protected virtual void InitializeView()
        {
        }

        #endregion

        #region 成员

        /// <summary>
        /// 中介者名称到中介者实例的映射
        /// </summary>
        protected IDictionary<string, IMediator> m_mediatorMap;

        /// <summary>
        /// 通知名称到观察者列表的映射
        /// </summary>
		protected IDictionary<string, IList<IObserver>> m_observerMap;

        /// <summary>
        /// 单例实例
        /// </summary>
        protected static volatile IView m_instance;

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
