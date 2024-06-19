/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/
/*
   这个代码展示了一个基于PureMVC框架的单例`Facade`类的实现，并且包含了一些Unity相关的管理器代码。我们将详细解释每个部分，包括中文注释和设计模式的应用
   这个 `Facade` 类实现了 PureMVC 框架的核心功能，同时扩展了 Unity 的管理器功能。通过单例模式和 MVC 设计模式的结合，实现了对应用程序各个部分的统一管理。
 */
#region Using

using System;
using System.Collections.Generic;
using PureMVC.Core;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// A base Singleton <c>IFacade</c> implementation
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, the <c>Facade</c> class assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Initializing the <c>Model</c>, <c>View</c> and <c>Controller</c> Singletons</item>
    ///         <item>Providing all the methods defined by the <c>IModel, IView, &amp; IController</c> interfaces</item>
    ///         <item>Providing the ability to override the specific <c>Model</c>, <c>View</c> and <c>Controller</c> Singletons created</item>
    ///         <item>Providing a single point of contact to the application for registering <c>Commands</c> and notifying <c>Observers</c></item>
    ///     </list>
    ///     <example>
    ///         <code>
    ///	using PureMVC.Patterns;
    /// 
    ///	using com.me.myapp.model;
    ///	using com.me.myapp.view;
    ///	using com.me.myapp.controller;
    /// 
    ///	public class MyFacade : Facade
    ///	{
    ///		// Notification constants. The Facade is the ideal
    ///		// location for these constants, since any part
    ///		// of the application participating in PureMVC 
    ///		// Observer Notification will know the Facade.
    ///		public static const string GO_COMMAND = "go";
    /// 
    ///     // we aren't allowed to initialize new instances from outside this class
    ///     protected MyFacade() {}
    /// 
    ///     // we must specify the type of instance
    ///     static MyFacade()
    ///     {
    ///         instance = new MyFacade();
    ///     }
    /// 
    ///		// Override Singleton Factory method 
    ///		public new static MyFacade getInstance() {
    ///			return instance as MyFacade;
    ///		}
    /// 		
    ///		// optional initialization hook for Facade
    ///		public override void initializeFacade() {
    ///			base.initializeFacade();
    ///			// do any special subclass initialization here
    ///		}
    ///	
    ///		// optional initialization hook for Controller
    ///		public override void initializeController() {
    ///			// call base to use the PureMVC Controller Singleton. 
    ///			base.initializeController();
    /// 
    ///			// Otherwise, if you're implmenting your own
    ///			// IController, then instead do:
    ///			// if ( controller != null ) return;
    ///			// controller = MyAppController.getInstance();
    /// 		
    ///			// do any special subclass initialization here
    ///			// such as registering Commands
    ///			registerCommand( GO_COMMAND, com.me.myapp.controller.GoCommand )
    ///		}
    ///	
    ///		// optional initialization hook for Model
    ///		public override void initializeModel() {
    ///			// call base to use the PureMVC Model Singleton. 
    ///			base.initializeModel();
    /// 
    ///			// Otherwise, if you're implmenting your own
    ///			// IModel, then instead do:
    ///			// if ( model != null ) return;
    ///			// model = MyAppModel.getInstance();
    /// 		
    ///			// do any special subclass initialization here
    ///			// such as creating and registering Model proxys
    ///			// that don't require a facade reference at
    ///			// construction time, such as fixed type lists
    ///			// that never need to send Notifications.
    ///			regsiterProxy( new USStateNamesProxy() );
    /// 			
    ///			// CAREFUL: Can't reference Facade instance in constructor 
    ///			// of new Proxys from here, since this step is part of
    ///			// Facade construction!  Usually, Proxys needing to send 
    ///			// notifications are registered elsewhere in the app 
    ///			// for this reason.
    ///		}
    ///	
    ///		// optional initialization hook for View
    ///		public override void initializeView() {
    ///			// call base to use the PureMVC View Singleton. 
    ///			base.initializeView();
    /// 
    ///			// Otherwise, if you're implmenting your own
    ///			// IView, then instead do:
    ///			// if ( view != null ) return;
    ///			// view = MyAppView.Instance;
    /// 		
    ///			// do any special subclass initialization here
    ///			// such as creating and registering Mediators
    ///			// that do not need a Facade reference at construction
    ///			// time.
    ///			registerMediator( new LoginMediator() ); 
    /// 
    ///			// CAREFUL: Can't reference Facade instance in constructor 
    ///			// of new Mediators from here, since this is a step
    ///			// in Facade construction! Usually, all Mediators need 
    ///			// receive notifications, and are registered elsewhere in 
    ///			// the app for this reason.
    ///		}
    ///	}
    ///         </code>
    ///     </example>
    /// </remarks>
	/// <see cref="PureMVC.Core.Model"/>
	/// <see cref="PureMVC.Core.View"/>
	/// <see cref="PureMVC.Core.Controller"/>
	/// <see cref="PureMVC.Patterns.Notification"/>
	/// <see cref="PureMVC.Patterns.Mediator"/>
	/// <see cref="PureMVC.Patterns.Proxy"/>
	/// <see cref="PureMVC.Patterns.SimpleCommand"/>
	/// <see cref="PureMVC.Patterns.MacroCommand"/>
    public class Facade : IFacade
    {
        #region 构造函数

        /// <summary>
        /// 初始化 Facade 的构造函数
        /// </summary>
        /// <remarks>
        ///     <para>这个 <c>IFacade</c> 实现是一个单例，因此你不应该直接调用构造函数，而是调用静态单例工厂方法 <c>Facade.Instance</c></para>
        /// </remarks>
        protected Facade()
        {
            InitializeFacade();
        }

        #endregion

        #region 公共方法

        #region IFacade 成员

        #region Proxy

        /// <summary>
        /// 按名称在 <c>Model</c> 中注册一个 <c>IProxy</c>
        /// </summary>
        /// <param name="proxy">要在 <c>Model</c> 中注册的 <c>IProxy</c></param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void RegisterProxy(IProxy proxy)
        {
            m_model.RegisterProxy(proxy);
        }

        /// <summary>
        /// 按名称从 <c>Model</c> 中检索一个 <c>IProxy</c>
        /// </summary>
        /// <param name="proxyName">要检索的 <c>IProxy</c> 实例的名称</param>
        /// <returns>之前通过 <c>proxyName</c> 在 <c>Model</c> 中注册的 <c>IProxy</c></returns>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return m_model.RetrieveProxy(proxyName);
        }

        /// <summary>
        /// 按名称从 <c>Model</c> 中移除一个 <c>IProxy</c> 实例
        /// </summary>
        /// <param name="proxyName">要从 <c>Model</c> 中移除的 <c>IProxy</c></param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual IProxy RemoveProxy(string proxyName)
        {
            return m_model.RemoveProxy(proxyName);
        }

        /// <summary>
        /// 检查是否注册了一个 Proxy
        /// </summary>
        /// <param name="proxyName">要检查的 <c>IProxy</c> 实例的名称</param>
        /// <returns>当前是否注册了给定 <c>proxyName</c> 的 Proxy。</returns>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual bool HasProxy(string proxyName)
        {
            return m_model.HasProxy(proxyName);
        }

        #endregion

        #region Command

        /// <summary>
        /// 在 <c>Controller</c> 中注册一个 <c>ICommand</c>
        /// </summary>
        /// <param name="notificationName">与 <c>ICommand</c> 关联的 <c>INotification</c> 的名称。</param>
        /// <param name="commandType"> <c>ICommand</c> 的类型引用</param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void RegisterCommand(string notificationName, Type commandType)
        {
            m_controller.RegisterCommand(notificationName, commandType);
        }

        /// <summary>
        /// 从控制器中移除先前注册的 <c>ICommand</c> 到 <c>INotification</c> 的映射。
        /// </summary>
        /// <param name="notificationName">从控制器中移除先前注册的 <c>ICommand</c> 到 <c>INotification</c> 的映射。</param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void RemoveCommand(string notificationName)
        {
            m_controller.RemoveCommand(notificationName);
        }

        /// <summary>
        /// 检查是否为给定的通知注册了命令
        /// </summary>
        /// <param name="notificationName">要检查的 <c>INotification</c> 的名称。</param>
        /// <returns>当前是否为给定的 <c>notificationName</c> 注册了命令。</returns>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual bool HasCommand(string notificationName)
        {
            return m_controller.HasCommand(notificationName);
        }

        #endregion

        #region Mediator

        /// <summary>
        /// 在 <c>View</c> 中注册一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediator">对 <c>IMediator</c> 实例的引用</param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void RegisterMediator(IMediator mediator)
        {
            m_view.RegisterMediator(mediator);
        }

        /// <summary>
        /// 从 <c>View</c> 中检索一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediatorName">要检索的 <c>IMediator</c> 实例的名称</param>
        /// <returns>之前通过给定的 <c>mediatorName</c> 注册的 <c>IMediator</c></returns>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return m_view.RetrieveMediator(mediatorName);
        }

        /// <summary>
        /// 从 <c>View</c> 中移除一个 <c>IMediator</c> 实例
        /// </summary>
        /// <param name="mediatorName">要移除的 <c>IMediator</c> 实例的名称</param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return m_view.RemoveMediator(mediatorName);
        }

        /// <summary>
        /// 检查是否注册了一个 Mediator
        /// </summary>
        /// <param name="mediatorName">要检查的 <c>IMediator</c> 实例的名称</param>
        /// <returns>当前是否为给定的 <code>mediatorName</code> 注册了 Mediator。</returns>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual bool HasMediator(string mediatorName)
        {
            return m_view.HasMediator(mediatorName);
        }

        #endregion

        #region Observer

        /// <summary>
        /// 通知 <c>Observer</c> 一个 <c>INotification</c>
        /// </summary>
        /// <remarks>此方法主要为了向后兼容，并允许你使用 Facade 发送自定义的通知类。</remarks>
        /// <remarks>通常你应该直接调用 sendNotification 并传递参数，而不需要自己构建通知实例。</remarks>
        /// <param name="notification">要通知观察者的 <c>INotification</c></param>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void NotifyObservers(INotification notification)
        {
            m_view.NotifyObservers(notification);
        }

        #endregion

        #endregion

        #region INotifier 成员

        /// <summary>
        /// 发送一个 <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">要发送的通知名称</param>
        /// <remarks>使我们在实现代码中不必构建新的通知实例</remarks>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void SendNotification(string notificationName)
        {
            NotifyObservers(new Notification(notificationName));
        }

        /// <summary>
        /// 发送一个 <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">要发送的通知名称</param>
        /// <param name="body">通知的主体</param>
        /// <remarks>使我们在实现代码中不必构建新的通知实例</remarks>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void SendNotification(string notificationName, object body)
        {
            NotifyObservers(new Notification(notificationName, body));
        }

        /// <summary>
        /// 发送一个 <c>INotification</c>
        /// </summary>
        /// <param name="notificationName">要发送的通知名称</param>
        /// <param name="body">通知的主体</param>
        /// <param name="type">通知的类型</param>
        /// <remarks>使我们在实现代码中不必构建新的通知实例</remarks>
        /// <remarks>此方法是线程安全的，并且在所有实现中都需要是线程安全的。</remarks>
        public virtual void SendNotification(string notificationName, object body, string type)
        {
            NotifyObservers(new Notification(notificationName, body, type));
        }

        #endregion

        #region 访问器

        /// <summary>
        /// Facade 单例工厂方法。此方法是线程安全的。
        /// </summary>
        public static IFacade Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new Facade();
                    }
                }

                return m_instance;
            }
        }

        #endregion

        #region 受保护的 & 内部方法

        /// <summary>
        /// 显式静态构造函数，告诉 C# 编译器不要将类型标记为 beforefieldinit
        ///</summary>
        static Facade()
        {
        }

        /// <summary>
        /// 初始化单例 <c>Facade</c> 实例
        /// </summary>
        /// <remarks>
        /// <para>由构造函数自动调用。在你的子类中重写以进行任何子类特定的初始化。但要确保调用 <c>base.initializeFacade()</c></para>
        /// </remarks>
        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }

        /// <summary>
        /// 初始化 <c>Controller</c>
        /// </summary>
        /// <remarks>
        ///     <para>由 <c>initializeFacade</c> 方法调用。如果以下情况之一为真，请在 <c>Facade</c> 的子类中重写此方法：</para>
        ///     <list type="bullet">
        ///         <item>你希望初始化不同的 <c>IController</c></item>
        ///         <item>你有 <c>Commands</c> 需要在启动时向 <c>Controller</c> 注册</item>
        ///     </list>
        ///     <para>如果你不想初始化不同的 <c>IController</c>，请在你的方法开头调用 <c>base.initializeController()</c>，然后注册 <c>Command</c></para>
        /// </remarks>
        protected virtual void InitializeController()
        {
            if (m_controller != null) return;
            m_controller = Controller.Instance;
        }

        /// <summary>
        /// 初始化 <c>Model</c>
        /// </summary>
        /// <remarks>
        ///     <para>由 <c>initializeFacade</c> 方法调用。如果以下情况之一为真，请在 <c>Facade</c> 的子类中重写此方法：</para>
        ///     <list type="bullet">
        ///         <item>你希望初始化不同的 <c>IModel</c></item>
        ///         <item>你有 <c>Proxy</c> 需要在构造时不获取 Facade 引用的情况下向 <c>Model</c> 注册</item>
        ///     </list>
        ///     <para>如果你不想初始化不同的 <c>IModel</c>，请在你的方法开头调用 <c>base.initializeModel()</c>，然后注册 <c>Proxy</c></para>
        ///     <para>注意：此方法很少被重写；实际上，你更有可能使用 <c>Command</c> 来创建和注册 <c>Proxy</c> 到 <c>Model</c>，因为具有可变数据的 <c>Proxy</c> 可能需要发送 <c>INotification</c>，因此可能希望在其构造期间获取 <c>Facade</c> 的引用</para>
        /// </remarks>
        protected virtual void InitializeModel()
        {
            if (m_model != null) return;
            m_model = Model.Instance;
        }

        /// <summary>
        /// 初始化 <c>View</c>
        /// </summary>
        /// <remarks>
        ///     <para>由 <c>initializeFacade</c> 方法调用。如果以下情况之一为真，请在 <c>Facade</c> 的子类中重写此方法：</para>
        ///     <list type="bullet">
        ///         <item>你希望初始化不同的 <c>IView</c></item>
        ///         <item>你有需要向 <c>View</c> 注册的 <c>Observers</c></item>
        ///     </list>
        ///     <para>如果你不想初始化不同的 <c>IView</c>，请在你的方法开头调用 <c>base.initializeView()</c>，然后注册 <c>IMediator</c> 实例</para>
        ///     <para>注意：此方法很少被重写；实际上，你更有可能使用 <c>Command</c> 来创建和注册 <c>Mediator</c> 到 <c>View</c>，因为 <c>IMediator</c> 实例需要发送 <c>INotification</c>，因此可能希望在其构造期间获取 <c>Facade</c> 的引用</para>
        /// </remarks>
        protected virtual void InitializeView()
        {
            if (m_view != null) return;
            m_view = View.Instance;
        }

        #endregion

        #region 成员

        /// <summary>
        /// 私有引用到 Controller
        /// </summary>
        protected IController m_controller;

        /// <summary>
        /// 私有引用到 Model
        /// </summary>
        protected IModel m_model;

        /// <summary>
        /// 私有引用到 View
        /// </summary>
        protected IView m_view;

        /// <summary>
        /// 单例 Facade 实例
        /// </summary>
        protected static volatile IFacade m_instance;

        /// <summary>
        /// 用于锁定实例调用
        /// </summary>
        protected static readonly object m_staticSyncRoot = new object();

        #endregion

        // SimpleFramework 代码由 Jarjin Lee 编写
        static GameObject m_GameManager;
        static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

        GameObject AppGameManager
        {
            get
            {
                if (m_GameManager == null)
                {
                    m_GameManager = GameObject.Find("GameManager");
                }
                return m_GameManager;
            }
        }

        /// <summary>
        /// 添加管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <param name="obj"></param>
        public void AddManager(string typeName, object obj)
        {
            if (!m_Managers.ContainsKey(typeName))
            {
                m_Managers.Add(typeName, obj);
            }
        }

        /// <summary>
        /// 添加 Unity 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T AddManager<T>(string typeName) where T : Component
        {
            object result = null;
            m_Managers.TryGetValue(typeName, out result);
            if (result != null)
            {
                return (T)result;
            }
            Component c = AppGameManager.AddComponent<T>();
            m_Managers.Add(typeName, c);
            return default(T);
        }

        /// <summary>
        /// 获取系统管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public T GetManager<T>(string typeName) where T : class
        {
            if (!m_Managers.ContainsKey(typeName))
            {
                return default(T);
            }
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            return (T)manager;
        }

        /// <summary>
        /// 删除管理器
        /// </summary>
        /// <param name="typeName"></param>
        public void RemoveManager(string typeName)
        {
            if (!m_Managers.ContainsKey(typeName))
            {
                return;
            }
            object manager = null;
            m_Managers.TryGetValue(typeName, out manager);
            Type type = manager.GetType();
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                GameObject.Destroy((Component)manager);
            }
            m_Managers.Remove(typeName);
        }
    }
    #endregion
}