#region <<文件说明>>

/*--------------------------------------
//文件名称：DemoProy
// 创建者：李满堂
//创建时间：2022年09月12日 星期一 10:27
//文件版本：V1.0.0
//=======================================
//功能描述：
//
//
---------------------------------------*/

#endregion

using PureMVC.Patterns;
using UnityEngine;

public class DemoProy : Proxy
{
    public new const string NAME = "DemoProxy";

    private GameVO _gameVo;

    public GameVO GameVo
    {
        get { return _gameVo; }
    }


    public override void OnRegister()
    {
        
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }

    public DemoProy() : base(NAME)
    {
        _gameVo = new GameVO();
    }

    public void AddScore(int score)
    {
        if (this._gameVo.score + score > 0)
        {
            this._gameVo.score += score;
            this.SendNotification(LMT_Facade.ScoreChange, this._gameVo.score);
        }
        else
        {
            Debug.Log("分数不可以为负值");
        }
    }

    public void Setname(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        this._gameVo.playername = name;
        this.SendNotification(LMT_Facade.NameChange, this._gameVo.playername);
    }

    public string getName()
    {
        return _gameVo.playername;
    }

    public int getScore()
    {
        return _gameVo.score;
    }
}