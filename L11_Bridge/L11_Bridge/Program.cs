

public interface IImplementation
{
    bool Status { get; set; }
    void On();
    void Off();
    void PrintStatus();
}

public class SonyTV : IImplementation
{
    public bool Status { get; set; }
    public void On()
    {
        Status = true;
        PrintStatus();
    }
    public void Off()
    {
        Status = false;
        PrintStatus();
    }
    public void PrintStatus()
    {
        Console.WriteLine("Sony TV is " + (Status ? "On" : "Off"));
    }
}

public class SamsungTV : IImplementation
{
    public bool Status { get; set; }
    public void On()
    {
        Status = true;
        PrintStatus();
    }
    public void Off()
    {
        Status = false;
        PrintStatus();
    }
    public void PrintStatus()
    {
        Console.WriteLine("Samsung TV is " + (Status ? "On" : "Off"));
    }
}

public abstract class Abstraction
{
    protected IImplementation _implementation;
    public Abstraction(IImplementation implementation)
    {
        _implementation = implementation;
    }
    public virtual void TurnOn()
    {
        _implementation.On();
    }
    public virtual void TurnOff()
    {
        _implementation.Off();
    }
    public virtual void ChangeTV(IImplementation implementation)
    {
        _implementation = implementation;
        _implementation.PrintStatus();
    }

}

public class RemoteControlTV : Abstraction
{
    public RemoteControlTV(IImplementation implementation) : base(implementation)
    {
    }
    public void SetChannel(int channel)
    {
        Console.WriteLine("Channel set to " + channel);
    }
}

public class Program
{

    public static void Main()
    {
        SonyTV sonyTV = new SonyTV();
        SamsungTV samsungTV = new SamsungTV();
        var remoteControlTV = new RemoteControlTV(sonyTV);
        remoteControlTV.TurnOn();
        remoteControlTV.SetChannel(5);
        remoteControlTV.TurnOff();
        remoteControlTV.ChangeTV(samsungTV);
        remoteControlTV.TurnOn();
        remoteControlTV.SetChannel(5);
        remoteControlTV.TurnOff();


    }
}