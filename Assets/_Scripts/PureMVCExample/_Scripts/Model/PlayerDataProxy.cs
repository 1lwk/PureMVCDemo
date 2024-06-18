using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
public class PlayerDataProxy : PureMVC.Patterns.Proxy {

    public new static string NAME = "PlayerData";

    public PlayerDataModel PlayerData;

    public PlayerDataProxy(string name)
        :base(name,null)
    {
        PlayerData = new PlayerDataModel();
    }

    /// <summary>
    /// 对V  或者C提供对外的一些接口
    /// </summary>
    /// <param name="reward"></param>
    /// <param name="info"></param>
	public void GetReward(int reward,string info)
    {
        PlayerData.PlayGameCount++;
        PlayerData.RewardTotal += reward;
	
        //发送消息 更新MainPanelView UI组件 通知订阅者
		SendNotification(MyFacade.UPDATE_PLAYER_DATA,info+reward);
    }


    public override void OnRegister()
    {
        Debug.Log("PlayerDataProxy OnRegister");
    }

    /// <summary>
    /// Called by the Model when the Proxy is removed
    /// </summary>
    public override void OnRemove()
    {
        Debug.Log("PlayerDataProxy OnRemove");
    }
}
