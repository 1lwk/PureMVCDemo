using UnityEngine;



public class App : MonoBehaviour
{
    private static App _instance;

    public static App GetInstance()
    {
        return _instance;
    }
    void Awake()
    {
        _instance = this;
        //启动PureMVC，完成Controller，Proxies，Mediators的初始化工作
        //MyFacade.GetInstance().Launch();
    }

}