using System;
using System.Collections.Generic;
using System.Data;

public interface IBuilder
{
    void SetMeal1();
    void SetMeal2();
    void SetDessert();
    void Construct();
    ConcreteComponent_ClientProduct GetClientProduct();
}
public abstract class Component 
{
    public abstract void PrintAll();
    //trzeba będzie dodać tu metodę klonowania głębokiego aby można
    //było stworzyć kopię zamówienia
    public abstract Component Clone(); 
}
public class ConcreteComponent_ClientProduct : Component
{
    public List<string> _meals1 = new List<string>();
    public List<string> Meals1
    {
        get { return _meals1; }
        set { _meals1 = value; }
    }
    public List<string> _meals2 = new List<string>();
    public List<string> Meals2
    {
        get { return _meals2; }
        set { _meals2 = value; }
    }
    public List<string> _desserts = new List<string>();
    public List<string> Desserts
    {
        get { return _desserts; }
        set { _desserts = value; }
    }
    public void AddMeal1(string m1) => Meals1.Add(m1);
    public void AddMeal2(string m2) => Meals2.Add(m2);
    public void AddDessert(string d) => Desserts.Add(d);

    public override void PrintAll()
    {
        if(Meals1.Count > 0)
        {
            Console.WriteLine("First meal: " + string.Join(", ", Meals1));
        }
        if(Meals2.Count > 0)
        {
            Console.WriteLine("Second meal: " + string.Join(", ", Meals2));

        }
        if(Desserts.Count > 0)
        {
            Console.WriteLine("Dessert: " + string.Join(", ", Desserts));
        }
    }
    
    //konstruktor dla głębokiego kopiowania
    public ConcreteComponent_ClientProduct(ConcreteComponent_ClientProduct c)
    {
        Meals1 = new List<string>(c.Meals1);
        Meals2 = new List<string>(c.Meals2);
        Desserts = new List<string>(c.Desserts);

    }

    public ConcreteComponent_ClientProduct()
    {
    }

    public override Component Clone()  //sprawdzono w testach - jest to głębokie kopiowanie
    {
        var client = new ConcreteComponent_ClientProduct(this);
        return client;
    }
}

public class Director
{
    public IBuilder Builder { get; set; }
    public Director(IBuilder builder)
    {
        Builder = builder;
    }
    
}

public class UkrainianBuilder : IBuilder
{
    private ConcreteComponent_ClientProduct _clientProduct = new ConcreteComponent_ClientProduct();
    public void SetMeal1()
    {
        _clientProduct.AddMeal1("Borsch");
        _clientProduct.AddMeal1("Bread");
    }
    public void SetMeal2()
    {
        _clientProduct.AddMeal2("Mashed potatoes");
        _clientProduct.AddMeal2("Сhop");
    }

    public void SetDessert()
    {
        _clientProduct.AddDessert("Yabluchnyk");
        _clientProduct.AddDessert("Tea");
    }
    public void Construct()
    {

        SetMeal1();
        SetMeal2();
        SetDessert();
    }
    public ConcreteComponent_ClientProduct GetClientProduct()
    {
        return _clientProduct;
    }

}

public class ChineseBuilder : IBuilder
{
    private ConcreteComponent_ClientProduct _clientProduct = new ConcreteComponent_ClientProduct();
    public void SetMeal1()
    {
        _clientProduct.AddMeal1("Noodle soup");
    }
    public void SetMeal2()
    {
        _clientProduct.AddMeal2("Lo mein");
        _clientProduct.AddMeal2("White rice");
    }

    public void SetDessert()
    {
        _clientProduct.AddDessert("Banana roll");
    }
    public void Construct()
    {

        SetMeal1();
        SetMeal2();
        //SetDessert();
    }

    public ConcreteComponent_ClientProduct GetClientProduct()
    {
        return _clientProduct;
    }

}

public class PolishBuilder : IBuilder
{
    private ConcreteComponent_ClientProduct _clientProduct = new ConcreteComponent_ClientProduct();
    public void SetMeal1()
    {
        _clientProduct.AddMeal1("Krupnik");
    }
    public void SetMeal2()
    {
        _clientProduct.AddMeal2("buckwheat groats");
        _clientProduct.AddMeal2("Pork breaded cutlet");
    }

    public void SetDessert()
    {
        _clientProduct.AddDessert("Babka");
        _clientProduct.AddDessert("Green tea");
    }

    public void Construct()
    {

        //SetMeal1();
        SetMeal2();
        SetDessert();
    }

    public ConcreteComponent_ClientProduct GetClientProduct()
    {
        return _clientProduct;
    }

}

abstract class Decorator : Component
{
    protected Component _component;

    public Decorator(Component component)
    {
        this._component = component;
    }

    public void SetComponent(Component component)
    {
        this._component = component;
    }

    public override void PrintAll()
    {
        if (this._component != null)
        {
            this._component.PrintAll();
        }
    }
}

class ConcreteDecoratorAddAttribute : Decorator
{
    public string AdditionalAttribute { get; set; }
    public ConcreteDecoratorAddAttribute(Component comp, string additionalAttribute) : base(comp)
    {
        AdditionalAttribute = additionalAttribute;
    }

    public override void PrintAll()
    {
        if (this._component != null)
        {
            Console.WriteLine($"Additional attribute: {AdditionalAttribute}");
            this._component.PrintAll();
        }
    }
    //konstruktor definiujący
    public override Component Clone() //sprawdzono w testach - jest to kopiowanie głębokie
    {
            Component c = this._component.Clone();
            ConcreteDecoratorAddAttribute cd = new ConcreteDecoratorAddAttribute(c, AdditionalAttribute);
            return cd;
    }
}

public class Program
{
    static void Main()
    {
        //Builder
        Console.WriteLine("----- Builder -----\n");
        List<Component> orderList = new List<Component>();

        Director director = new Director(new UkrainianBuilder());
        director.Builder.Construct();
        orderList.Add(director.Builder.GetClientProduct());

        director = new Director(new ChineseBuilder());
        director.Builder.Construct();
        orderList.Add(director.Builder.GetClientProduct());

        director = new Director(new PolishBuilder());
        director.Builder.Construct();
        orderList.Add(director.Builder.GetClientProduct());

        for(int i = 0; i < orderList.Count(); i++)
        {
            Console.WriteLine($"Client {i+1}");
            orderList[i].PrintAll();
        }

                    /* Output:
                      ----- Builder -----

                        Client 1
                        First meal: Borsch, Bread
                        Second meal: Mashed potatoes, Сhop
                        Dessert: Yabluchnyk, Tea
                        Client 2
                        First meal: Noodle soup
                        Second meal: Lo mein, White rice
                        Client 3
                        Second meal: buckwheat groats, Pork breaded cutlet
                        Dessert: Babka, Green tea
                     */

        //Decorator
        Console.WriteLine("\n----- Decorator -----\n");

        orderList = new List<Component>();//lista zamówień

        director = new Director(new UkrainianBuilder());
        director.Builder.Construct();
        orderList.Add(director.Builder.GetClientProduct());

        director = new Director(new ChineseBuilder());
        director.Builder.Construct();
        orderList.Add(director.Builder.GetClientProduct());

        director = new Director(new PolishBuilder());
        director.Builder.Construct();
        orderList.Add(director.Builder.GetClientProduct());

        for (int i = 0; i < orderList.Count(); i++)
        {
            Console.WriteLine($"Client {i + 1}");
            orderList[i].PrintAll();
        }
                    /* Output:
                      ----- Decorator -----

                        Client 1
                        First meal: Borsch, Bread
                        Second meal: Mashed potatoes, Сhop
                        Dessert: Yabluchnyk, Tea
                        Client 2
                        First meal: Noodle soup
                        Second meal: Lo mein, White rice
                        Client 3
                        Second meal: buckwheat groats, Pork breaded cutlet
                        Dessert: Babka, Green tea
                     */


        Console.WriteLine("\n----- 'Opakowanie' zamówienia nr 2 -----\n");

        ConcreteDecoratorAddAttribute cd1 = new ConcreteDecoratorAddAttribute(orderList[1], "Child Safety Seat");
        ConcreteDecoratorAddAttribute cd2 = new ConcreteDecoratorAddAttribute(cd1, "2 forks");
        orderList[1] = cd2;

        for (int i = 0; i < orderList.Count(); i++)
        {
            Console.WriteLine($"Client {i + 1}");
            orderList[i].PrintAll();
        }

                    /* Output:
                      ----- 'Opakowanie' zamowienia nr 2 -----

                        Client 1
                        First meal: Borsch, Bread
                        Second meal: Mashed potatoes, Сhop
                        Dessert: Yabluchnyk, Tea
                        Client 2
                        Additional attribute: 2 forks
                        Additional attribute: Child Safety Seat
                        First meal: Noodle soup
                        Second meal: Lo mein, White rice
                        Client 3
                        Second meal: buckwheat groats, Pork breaded cutlet
                        Dessert: Babka, Green tea
                     */


        //Prototype
        // Całkowicie sklonować zamóienie 1 z listy i spakowane zamówienie 2 i
        // doddać ich na koniec listy zamówień
        Console.WriteLine("\n----- Prototype -----\n");

        orderList.Add(orderList[0].Clone());
        orderList.Add(orderList[1].Clone());

        for (int i = 0; i < orderList.Count(); i++)
        {
            Console.WriteLine($"Client {i + 1}");
            orderList[i].PrintAll();
        }

                    /* Output:
                     * ----- Prototype -----

                        Client 1
                        First meal: Borsch, Bread
                        Second meal: Mashed potatoes, Сhop
                        Dessert: Yabluchnyk, Tea
                        Client 2
                        Additional attribute: 2 forks
                        Additional attribute: Child Safety Seat
                        First meal: Noodle soup
                        Second meal: Lo mein, White rice
                        Client 3
                        Second meal: buckwheat groats, Pork breaded cutlet
                        Dessert: Babka, Green tea
                        Client 4
                        First meal: Borsch, Bread
                        Second meal: Mashed potatoes, Сhop
                        Dessert: Yabluchnyk, Tea
                        Client 5
                        Additional attribute: 2 forks
                        Additional attribute: Child Safety Seat
                        First meal: Noodle soup
                        Second meal: Lo mein, White rice
                     */


        //Sprawdzenie czy jest głębokie klonowanie
        Console.WriteLine("\n----- Sprawdzenie czy dokonano glebokie klonowanie -----\n");

        var zeroComponent = orderList[0];                   //stworzone referencję na 0 component listy
        var firstComponent = orderList[1];                  //stworzone referencję na 1 component listy
        var secondComponent = orderList[2];                 //stworzone referencję na 2 component listy

        //lista już zawiera Klony 0 i 1 komponentu, zostało dodać referencję na 3 (bez głębokiego klonowania)
        orderList.Add(orderList[2]);

        orderList.Remove(orderList[2]);                     //usunięto 2 component
        orderList.Remove(orderList[1]);                     //usunięto 1 component
        orderList.Remove(orderList[0]);                     //usunięto 0 component

        Console.WriteLine("Czy isntieje w liscie komponent który był zerowym(lista zawiera jego klon): " + 
            orderList.Contains(zeroComponent) + "\n"); //False

        Console.WriteLine("Czy isntieje w liscie komponent który był pierwszym(lista zawiera jego klon): " + 
            orderList.Contains(firstComponent) + "\n"); //False

        Console.WriteLine("Czy isntieje w liscie komponent który był trzecim?(lista zawiera obiekt który jest referencją): " +
            orderList.Contains(secondComponent) + "\n"); //True

    }
}
