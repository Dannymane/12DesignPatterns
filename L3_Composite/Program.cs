using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace RefactoringGuru.DesignPatterns.Composite.Conceptual
{
    abstract class Component
    {
        //private string type { get; set; }
        public string type;
        public Component() { }

        public abstract void Operation();
        public virtual void Add(Component component) { }

        public virtual void Remove(Component component) { }

        public virtual bool IsComposite()
        {
            return true;
        }
    }

    class Leaf : Component
    {
        //private string type { get; set; }
        private string? name;
        private string? surname;
        
        public Leaf(string type_, string? name_ = null, string? surname_ = null)
        {
            type = type_;
            name = name_;
            surname = surname_;
        }
        public override void Operation()
        {
            switch (type)
            {
                case "Person": 
                    Console.WriteLine($"--- {name} {surname}");
                    break;
                default: Console.WriteLine(type); break;
            }
        }

        public override bool IsComposite()
        {
            return false;
        }

        public override void Add(Component component)
        {
            Console.WriteLine("Error. You can't add components into the leaf.");
        }
    }


    class Composite : Component
    {
        protected List<Component> childrens = new List<Component>();
        //private string type { get; set; }
        protected string? name;
        protected string? street;
        protected string? flatNumber;
        public Composite(string type_, string? field_ = null)
        {
            type = type_;
            switch (type)
            {
                case "City":
                    name = field_;
                    break;

                case "House":
                case "ApartamentBlock":
                    street = field_;
                    break;

                case "Flat":
                    flatNumber = field_;
                    break;
            }
        }
        public override void Add(Component c)
        {
            switch (type)
            {
                case "City":
                    if (c.type.Equals("ApartamentBlock") || c.type.Equals("House"))
                    {
                        childrens.Add(c);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error. Wrong object type");
                        break;
                    }
                case "ApartamentBlock":
                    if (c.type.Equals("Flat"))
                    {
                        childrens.Add(c);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error. Wrong object type");
                        break;
                    }

                case "House":
                case "Flat":
                    if (c.type.Equals("Person"))
                    {
                        childrens.Add(c);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error. Wrong object type");
                        break;
                    }

                case "Person":
                    Console.WriteLine("Error. You can't add objects to Person");
                    break;

                default:
                    Console.WriteLine("Error. Wrong parent type");
                    break;
            }
        }

        public override void Remove(Component c)
        {
            try
            {
                if (childrens.Contains(c))
                {
                    childrens.Remove(c);
                    Console.WriteLine($"The object of {c.type} type has removed successfully.");
                }
                else
                {
                    Console.WriteLine($"The object of {c.type} type not found in the list.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        public override void Operation()
        {
            switch (type){
                case "City":
                    Console.WriteLine($"- {name} ({type})");
                    foreach (var child in childrens) child.Operation();
                    break;

                case "ApartamentBlock": 
                    Console.WriteLine($"-- {street} ({type})"); 
                    foreach (var child in childrens) child.Operation();
                    break;

                case "House":
                    Console.WriteLine($"-- {street} ({type})");
                    foreach (var child in childrens) child.Operation();
                    break;

                case "Flat":
                    Console.WriteLine($"--- {flatNumber} ({type})");
                    foreach (var child in childrens) child.Operation();
                    break;

                default: 
                    Console.WriteLine(type);
                    foreach (var child in childrens) child.Operation(); 
                    break;
            }

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Composite CzCity = new Composite("City", "Częstochowa");

            Composite h1 = new Composite("House", "ul. Zielona 3");
            CzCity.Add(h1);
            Leaf p1 = new Leaf("Person", "Anna", "Nowak");
            Leaf p2 = new Leaf("Person", "Zenon", "Nowak");
            Leaf p3 = new Leaf("Person", "Julia", "Nowak");
            h1.Add(p1);
            h1.Add(p2);
            h1.Add(p3);
  

            Composite A1 = new Composite("ApartamentBlock", "ul. Czerwona 3");
            CzCity.Add(A1);
            Composite A1F1 = new Composite("Flat", "1");
            Composite A1F2 = new Composite("Flat", "2");
            Composite A1F3 = new Composite("Flat", "3");
            A1.Add(A1F1);
            A1.Add(A1F2);
            A1.Add(A1F3);
            Leaf p4 = new Leaf("Person", "Jan", "Kowal");
            Leaf p5 = new Leaf("Person", "Zofia", "Kowal");
            A1F1.Add(p4);
            A1F1.Add(p5);
            Leaf p6 = new Leaf("Person", "Weronika", "Wesoła");
            Leaf p7 = new Leaf("Person", "Michał", "Wesoły");
            A1F3.Add(p6);
            A1F3.Add(p7);

            Composite h2 = new Composite("House", " ul. Czarna 7");
            CzCity.Add(h2);
            Leaf p8 = new Leaf("Person", "Piotr", "Wysoki");
            h2.Add(p8);

            Composite h3 = new Composite("House", "ul. Złota 33");
            CzCity.Add(h3);
            Leaf p9 = new Leaf("Person", "Paweł", "Niski");
            Leaf p10 = new Leaf("Person", "Dorota", "Niska");
            h3.Add(p9);
            h3.Add(p10);

            CzCity.Operation();

            Leaf p11 = new Leaf("NO TYPE", "Dorota", "Niska");
            A1F1.Add(h2); //Error. Wrong parent type
            A1F1.Add(p11); //Error. Wrong parent type
            CzCity.Add(p1); //Error. Wrong parent type
            p1.Add(p2); //Error. You can't add components into the leaf.
            A1F1.Remove(p11); //The object of NO TYPE type not found in the list.
            CzCity.Operation(); //the same

        }
    }
}