using PureMVC.Interfaces;
using PureMVC.Patterns;

public class TestCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        TestDataProxy testdataproxy = Facade.RetrieveProxy("TestDatal") as TestDataProxy;
        testdataproxy.UpdateValue();
    }
}