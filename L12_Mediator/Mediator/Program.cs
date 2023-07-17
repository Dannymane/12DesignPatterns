
namespace Mediator;

//Chat group app
public interface IMediator
{
    public void Send(string message, Colleague colleague)
    {
    }
}

public abstract class Colleague
{
    protected IMediator mediator;
    public string Name { get; set; }
    public Colleague(IMediator mediator, string name)
    {
        this.mediator = mediator;
        this.Name = name;
    }
    public abstract void Notify(string message, string senderName);

}

public class ConcreteColleagueGroupUser : Colleague
{
    public ConcreteColleagueGroupUser(IMediator mediator, string name) : base(mediator, name)
    {
    }
    public void Send(string message)
    {
        mediator.Send(message, this);
    }
    public override void Notify(string message, string senderName)
    {
        Console.WriteLine($"User {Name} gets message: \"" + message + $"\" From user {senderName}");
    }
}

public class ConcreteMediatorGroup : IMediator
{
    private List<Colleague> _users = new List<Colleague>();

    public void addColleague(Colleague colleague)
    {
        _users.Add(colleague);
    }
    
    public void Send(string message, Colleague colleague)
    {
        foreach(var user in  _users)
        {
            if (user != colleague)
            {
                user.Notify(message, colleague.Name);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var mediator = new ConcreteMediatorGroup();
        var user1 = new ConcreteColleagueGroupUser(mediator, "U1");
        var user2 = new ConcreteColleagueGroupUser(mediator, "U2");
        var user3 = new ConcreteColleagueGroupUser(mediator, "U3");
        var user4 = new ConcreteColleagueGroupUser(mediator, "U4");
        var user5 = new ConcreteColleagueGroupUser(mediator, "U5");

        mediator.addColleague(user1);
        mediator.addColleague(user2);
        mediator.addColleague(user3);
        mediator.addColleague(user4);
        mediator.addColleague(user5);

        user1.Send("Hi everyone");
        user4.Send("Hi");

    }
}





