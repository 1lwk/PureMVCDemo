#region <<文件说明>>

/*--------------------------------------
//文件名称：DemoUI
// 创建者：李满堂
//创建时间：2022年09月12日 星期一 10:46
//文件版本：V1.0.0
//=======================================
//功能描述：
//
//
---------------------------------------*/

#endregion


using System;
using UnityEngine;
using UnityEngine.UI;


public class DemoUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;

    [SerializeField] private Text nameTxt;
    [SerializeField] private Text numTxt;

    [SerializeField] private ClickArea _clickArea;
    public event Action gameStart;
    public event Action areaClicked;

    private void OnEnable()
    {
        startBtn.onClick.AddListener(() => gameStart?.Invoke());
        UIEventListener.Get(this._clickArea.gameObject).onClick += clickAreaClickhandler;
    }

    private void clickAreaClickhandler(GameObject go)
    {
        areaClicked?.Invoke();
    }

    private void Start()
    {
    }

    private void Awake()
    {
    }

    private void Update()
    {
    }

    public void showNum(int num)
    {
        this.numTxt.text = num.ToString();
    }

    public void showname(string name)
    {
        this.nameTxt.text = name;
    }
}