using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 奖励数据   Value  Object
/// </summary>
public class BonusVO {

    public int Id { get; set; }
    public string Name{get;set;}
    public int Reward { get; set; }

    public BonusVO(int _id,string _name,int _reward)
    {
        Id = _id;
        Name = _name;
        Reward = _reward;
    }

	
}
