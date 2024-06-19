/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

#region Using

using System;
using PureMVC.Interfaces;
using PureMVC.Patterns;

#endregion

namespace PureMVC.Patterns
{
    /// <summary>
    /// 一个基本的 <c>IProxy</c> 实现
    /// </summary>
    /// <remarks>
    /// 	<para>在PureMVC中，<c>Proxy</c> 类用于管理应用程序数据模型的一部分</para>
    /// 	<para>一个 <c>Proxy</c> 可能只是管理对本地数据对象的引用，在这种情况下，与它的交互可能涉及以同步方式设置和获取其数据</para>
    /// 	<para><c>Proxy</c> 类还用于封装应用程序与远程服务的交互以保存或检索数据，在这种情况下，我们采用异步模式；在 <c>Proxy</c> 上设置数据（或调用方法），并监听 <c>Proxy</c> 从服务检索数据时发送的 <c>Notification</c></para>
    /// </remarks>
	/// <see cref="PureMVC.Core.Model"/>
    public class Proxy : Notifier, IProxy, INotifier
    {
        #region Constants

        /// <summary>
        /// 默认的代理名称
        /// </summary>
        public static string NAME = "Proxy";

        #endregion

        #region Constructors

        /// <summary>
        /// 构造一个具有默认名称且没有数据的新代理
        /// </summary>
        public Proxy()
            : this(NAME, null)
        {
        }

        /// <summary>
        /// 构造一个具有指定名称且没有数据的新代理
        /// </summary>
        /// <param name="proxyName">代理的名称</param>
        public Proxy(string proxyName)
            : this(proxyName, null)
        {
        }

        /// <summary>
        /// 构造一个具有指定名称和数据的新代理
        /// </summary>
        /// <param name="proxyName">代理的名称</param>
        /// <param name="data">要管理的数据</param>
		public Proxy(string proxyName, object data)
        {
            m_proxyName = (proxyName != null) ? proxyName : NAME;
            if (data != null) m_data = data;
        }

        #endregion

        #region Public Methods

        #region IProxy Members

        /// <summary>
        /// 当代理被注册时由模型调用
        /// </summary>
        public virtual void OnRegister()
        {
        }

        /// <summary>
        /// 当代理被移除时由模型调用
        /// </summary>
        public virtual void OnRemove()
        {
        }

        #endregion

        #endregion

        #region Accessors

        /// <summary>
        /// 获取代理名称
        /// </summary>
        /// <returns></returns>
        public virtual string ProxyName
        {
            get { return m_proxyName; }
        }

        /// <summary>
        /// 设置数据对象
        /// </summary>
        public virtual object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        #endregion

        #region Members

        /// <summary>
        /// 代理的名称
        /// </summary>
        protected string m_proxyName;

        /// <summary>
        /// 要管理的数据对象
        /// </summary>
        protected object m_data;

        #endregion
    }
}
