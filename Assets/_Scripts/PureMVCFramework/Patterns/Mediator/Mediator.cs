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
    /// 一个基础的 <c>IMediator</c> 实现
    /// </summary>
    /// <see cref="PureMVC.Core.View"/>
    public class Mediator : Notifier, IMediator, INotifier
    {
        #region 常量

        /// <summary>
        /// <c>Mediator</c> 的名称
        /// </summary>
        /// <remarks>
        ///     <para>通常，<c>Mediator</c> 会为一个特定的控件或控件组服务，因此不需要动态命名。</para>
        /// </remarks>
        public const string NAME = "Mediator";

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用默认名称和无视图组件构造新的 Mediator
        /// </summary>
        public Mediator()
            : this(NAME, null)
        {
        }

        /// <summary>
        /// 使用指定名称和无视图组件构造新的 Mediator
        /// </summary>
        /// <param name="mediatorName">Mediator 的名称</param>
        public Mediator(string mediatorName)
            : this(mediatorName, null)
        {
        }

        /// <summary>
        /// 使用指定名称和视图组件构造新的 Mediator
        /// </summary>
        /// <param name="mediatorName">Mediator 的名称</param>
        /// <param name="viewComponent">要中介的视图组件</param>
        public Mediator(string mediatorName, object viewComponent)
        {
            m_mediatorName = (mediatorName != null) ? mediatorName : NAME;
            m_viewComponent = viewComponent;
        }

        #endregion

        #region 公共方法

        #region IMediator 成员

        /// <summary>
        /// 列出此 <c>Mediator</c> 感兴趣的 <c>INotification</c> 名称
        /// </summary>
        /// <returns>感兴趣的 <c>INotification</c> 名称列表</returns>
        public virtual IList<string> ListNotificationInterests()
        {
            return new List<string>();
        }

        /// <summary>
        /// 处理 <c>INotification</c>
        /// </summary>
        /// <param name="notification">要处理的 <c>INotification</c> 实例</param>
        /// <remarks>
        ///     <para>
        ///        通常会在 switch 语句中处理，每个 <c>Mediator</c> 感兴趣的 <c>INotification</c> 会有一个 'case' 条目。
        ///     </para>
        /// </remarks>
        public virtual void HandleNotification(INotification notification)
        {
        }

        /// <summary>
        /// 在 Mediator 注册时由 View 调用
        /// </summary>
        public virtual void OnRegister()
        {
        }

        /// <summary>
        /// 在 Mediator 移除时由 View 调用
        /// </summary>
        public virtual void OnRemove()
        {
        }

        #endregion

        #endregion

        #region 访问器

        /// <summary>
        /// <c>Mediator</c> 的名称
        /// </summary>
        /// <remarks><para>应在子类中重写此属性</para></remarks>
        public virtual string MediatorName
        {
            get { return m_mediatorName; }
        }

        /// <summary>
        /// <code>IMediator</code> 的视图组件
        /// </summary>
        /// <remarks>
        ///     <para>此外，通常会在子类中定义一个隐式的 getter，将视图对象强制转换为特定类型，如下所示：</para>
        ///     <example>
        ///         <code>
        ///             private System.Windows.Forms.ComboBox comboBox {
        ///                 get { return viewComponent as ComboBox; }
        ///             }
        ///         </code>
        ///     </example>
        /// </remarks>
        public virtual object ViewComponent
        {
            get { return m_viewComponent; }
            set { m_viewComponent = value; }
        }

        #endregion

        #region 成员

        /// <summary>
        /// Mediator 的名称
        /// </summary>
        protected string m_mediatorName;

        /// <summary>
        /// 被中介的视图组件
        /// </summary>
        protected object m_viewComponent;

        #endregion
    }
}
