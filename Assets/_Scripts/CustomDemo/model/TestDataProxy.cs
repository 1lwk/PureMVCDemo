using PureMVC.Patterns;
using System;

public class TestDataProxy : Proxy
{
    public const string proxyname = "TestDatal";
    public TestData testData = null;

    public TestDataProxy():base(proxyname)
    {
        testData=new TestData();
    }

    public void UpdateValue()
    {
        DateTime currentTime = DateTime.Now;
        testData.now_time = currentTime.ToString();
        SendNotification(LWK_Facade.UpdateTimer, testData);
    }
}
