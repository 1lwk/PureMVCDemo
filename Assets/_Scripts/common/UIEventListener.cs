using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 事件监听器类，继承自 EventTrigger，处理各种 UI 事件
/// </summary>
public class UIEventListener : EventTrigger
{
    // 定义一个无参数的委托类型
    public delegate void VoidDelegate(GameObject go);
    // 声明各种 UI 事件的委托
    public event VoidDelegate onClick;
    public event VoidDelegate onDown;
    public event VoidDelegate onEnter;
    public event VoidDelegate onExit;
    public event VoidDelegate onUp;
    public event VoidDelegate onSelect;
    public event VoidDelegate onUpdateSelect;

    // 定义一个带有 PointerEventData 参数的拖动事件委托类型
    public delegate void DragDelegate(GameObject go, PointerEventData eventData);
    public DragDelegate onDrag;
    public DragDelegate onBeginDrag;
    public DragDelegate onEndDrag;

    /// <summary>
    /// 获取 UIEventListener 组件，如果没有则添加一个新的
    /// </summary>
    /// <param name="go">要获取或添加组件的 GameObject</param>
    /// <returns>UIEventListener 组件</returns>
    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    /// <summary>
    /// 当点击指针时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }

    /// <summary>
    /// 当指针按下时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
    }

    /// <summary>
    /// 当指针进入时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }

    /// <summary>
    /// 当指针退出时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }

    /// <summary>
    /// 当指针抬起时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
    }

    /// <summary>
    /// 当选择时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }

    /// <summary>
    /// 当更新选择时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }

    /// <summary>
    /// 当开始拖动时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject, eventData);
    }

    /// <summary>
    /// 当拖动时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject, eventData);
    }

    /// <summary>
    /// 当结束拖动时触发
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject, eventData);
    }
}
