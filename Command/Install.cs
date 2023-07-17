using System;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Input;


internal interface ICommand
{
    public string Command { get; set; }
    void Execute();
}
internal class ConcreteCommandSystem : ICommand
{
    public string Command { get; set; }
    private Receiver _receiver;

    public ConcreteCommandSystem(string command, Receiver receiver)
    {
        Command = command;
        _receiver = receiver;

    }
    public void Execute()
    {
        _receiver.Action(Command);
    }
}

internal class Receiver
{
    public void Action(string command)
    {
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/c {command}";
        process.Start();

    }
}
internal class Invoker
{
    private List<ICommand> _commands = new List<ICommand>();

    public void AddCommand(ICommand c)
    {
        _commands.Add(c);
    }
    public void RemoveCommand(ICommand c)
    {
        _commands.Remove(c);
    }
    public void Invoke()
    {
        string logs = "Wywołanie poleceń:";
        foreach (ICommand c in _commands)
        {
            c.Execute();
            logs += "\n"+c.Command;
        }
        //create file with logs
        using FileStream fs = new FileStream("logs.txt", FileMode.Create, FileAccess.Write);
        byte[] bytes = Encoding.UTF8.GetBytes(logs.ToString());
        fs.Write(bytes, 0, bytes.Length);
    }
}

internal class Install {

    static void Main(string[] args)
    {
        Invoker inv = new Invoker();
        Receiver r = new Receiver();

        //installation program
        ConcreteCommandSystem CC1 = new ConcreteCommandSystem("mkdir Podkatalog1", r);
        ConcreteCommandSystem CC2 = new ConcreteCommandSystem("copy Program.exe Podkatalog1\\", r);
        ConcreteCommandSystem CC3 = new ConcreteCommandSystem("cd Podkatalog1 & Program.exe 9", r);

        inv.AddCommand(CC1);
        inv.AddCommand(CC2);
        inv.AddCommand(CC3);
        inv.Invoke();


        ////deinstall
        //inv = new Invoker(); //cleaning
        //ConcreteCommandSystem CC4 = new ConcreteCommandSystem("rmdir /S /Q Podkatalog1", r);
        //inv.AddCommand(CC4);
        //inv.Invoke();
    }
}