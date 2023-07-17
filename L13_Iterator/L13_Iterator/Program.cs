using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

interface IIterator
{
    public virtual void First() { }
    public virtual void Next(int step) { }
    public virtual MyObject CurrentItem() { return null; }
}

class ConcreteIterator : IIterator
{
    private Aggregate _ag;
    private int _index;

    public ConcreteIterator(Aggregate ag, int index = 0)
    {
        _ag = ag;
        _index = index;
    }

    public void First()
    {
        _index = 0;
    }

    public void Next(int step)
    {
        if (step < 0){
          if (_index >= -step)
          {
            _index += step;
            Console.WriteLine("Current index: " + _index);
          }else
            Console.WriteLine("Too small step. Index has not changed");
        }
        else if (_ag.GetSize() > step + _index)
        {
            _index += step;
            Console.WriteLine("Current index: " + _index);
        }else
          Console.WriteLine("Too big step. Index has not changed");
        
    }

    public MyObject CurrentItem()
    {
        return _ag.GetItem(_index);
    }
}

class MyObject
{
    public string postalCode;
    public string city;
}

abstract class Aggregate
{
    public abstract IIterator CreateIterator();
    public abstract MyObject GetItem(int idx);
    public abstract int GetSize();
}

class ConcreteAggregate : Aggregate
{
    private MyObject[] _objs;
    private int SIZE;
    private string path = Path.Combine(Directory.GetCurrentDirectory(), "kody_pocztowe.txt");

  public ConcreteAggregate()
  {
    StreamReader file = new StreamReader(path);
    if (file == null)
    {
      Console.WriteLine("Unable to open file");
    }

    SIZE = int.Parse(file.ReadLine());
    Console.WriteLine("Size of database:\t" + SIZE);

    _objs = new MyObject[SIZE];
    for (int i = 0; i < SIZE; i++)
    {
      string address = file.ReadLine();
      if (address.Length > 8)
      {
        _objs[i] = new MyObject
        {
           postalCode = address.Substring(0, 6),
           city = address.Substring(8)
        };
      } else {
        _objs[i] = new MyObject
        {
           postalCode = address.Substring(0, 6),
           city = "-"
        };
      };
    }
  }
    public override IIterator CreateIterator()
    {
        return new ConcreteIterator(this);
    }

    public override MyObject GetItem(int idx)
    {
      return _objs[idx];
    }

    public override int GetSize()
    {
        return SIZE;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Aggregate ag = new ConcreteAggregate();
        IIterator it = ag.CreateIterator(); //it is an object of ConcreteIterator, which implements IIterator

        int step = 10;
        Console.WriteLine("Step: " + step);
        it.Next(step);
        Console.WriteLine("Postal code: " + it.CurrentItem().postalCode + " City: " + it.CurrentItem().city);
        Console.WriteLine();

        step = 100000;
        Console.WriteLine("Step: " + step);
        it.Next(step);
        Console.WriteLine("Postal code: " + it.CurrentItem().postalCode + " City: " + it.CurrentItem().city);
        Console.WriteLine();

        step = -10;
        Console.WriteLine("Step: " + step);
        it.Next(step);
        Console.WriteLine("Postal code: " + it.CurrentItem().postalCode + " City: " + it.CurrentItem().city);

        Console.WriteLine();

        step = -100000;
        Console.WriteLine("Step: " + step);
        it.Next(step);
        Console.WriteLine("Postal code: " + it.CurrentItem().postalCode + " City: " + it.CurrentItem().city);

      /*Output
            Size of database: 23259
            Step: 10
            Current index: 10
            Postal code: 23 - 408 City: leksandr? w k.Bi? goraja

            Step: 100000
            Too big step.Index has not changed
            Postal code: 23 - 408 City: leksandr? w k.Bi? goraja

            Step: -10
            Current index: 0
            Postal code: 21 - 143 City: bram? w

            Step: -100000
            Too small step.Index has not changed
            Postal code: 21 - 143 City: bram? w
      */
  }
}
