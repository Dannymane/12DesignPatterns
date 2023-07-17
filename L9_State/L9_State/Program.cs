using System;

//Napisać program we ktorym będzie sterowany samochód
//Samochód może być w różnych stanach: 
//jazda do tyłu, postój, jazda na 1,2,3,4,5 biegu
//można przełączać tylko liniowo jdt-p-1-2-3-4-5
// 1,2,3,4,5 to jeden stan (concreteState)
//przełączenie NIE zajmuje czasu, przełącza się natychmiast
public class Program
{
   class Context
   {
      public State _state;
      public Context() {
         _state = new ConcreteStateStand(this);
      }

      public Context(State state)
      {
         ChangeStateTo(state);
      }

      public void ChangeStateTo(State state)
      {
         _state = state;
         _state.SetContext(this);
      }

      public void RequestGoBack()
      {
         _state.HandleGoBack();
      }
      public void RequestStopCar()
      {
         _state.HandleStopCar();
      }

      public void RequestGoForward(int gear = 0)
      {
         _state.HandleGoForward(gear);
      }
   }

   abstract class State
   {
      protected int _gear;
      protected Context _context;
      public State(Context context, int gear = 0)
      {
         _context = context;
         _gear = gear;
      }

      public void SetContext(Context context)
      {
         this._context = context;
      }
      public abstract void HandleGoBack();

      public abstract void HandleStopCar();

      public abstract void HandleGoForward(int gear = 1);
   }

   class ConcreteStateBackward : State
   {
      public ConcreteStateBackward(Context context) : base(context) { }

      public override void HandleGoBack()
      {
            Console.WriteLine("The car is already moving backward");
      }

      public override void HandleStopCar()
      {

         _context.ChangeStateTo(new ConcreteStateStand(_context));
         Console.WriteLine("The car is standing");
      }

      public override void HandleGoForward(int gear = 1)
      {
         Console.WriteLine("Denied. You can't go ahead while car is going backward");
      }
   }

   class ConcreteStateStand : State
   {
      public ConcreteStateStand(Context context) : base(context) { }



      public override void HandleGoBack()
      {
         _context.ChangeStateTo(new ConcreteStateBackward(_context));
         Console.WriteLine("The car is moving backward");
      }

      public override void HandleStopCar()
      {
         Console.WriteLine("The car is already standing");
      }
      public override void HandleGoForward(int gear = 1)
      {
         if (gear != 1)
         {
            Console.WriteLine($"You can't choose gear {gear} from standing position");
            return;
         }
         _context.ChangeStateTo(new ConcreteStateForward(_context, gear));
         Console.WriteLine($"The car is moving forward on gear {gear}");
      }
   }
   class ConcreteStateForward : State
   {
      public ConcreteStateForward(Context context, int gear) : base(context, gear) { }

      public override void HandleGoBack()
      {
            Console.WriteLine("Denied. You can't go backward when car is moving forward");
      }

      public override void HandleStopCar()
      {
         _context.ChangeStateTo(new ConcreteStateStand(_context));
         Console.WriteLine("The car is standing");
      }
      public override void HandleGoForward(int gear = 1)
      {
         if (gear == _gear)
         {
            Console.WriteLine("The car is alreday moving on this gear");
            return;
         }
         if (gear < 1 || gear > 5)
         {
            Console.WriteLine($"There is no gear {gear}");
            return;
         }

         if(gear == (_gear - 1) || gear == (_gear + 1))
         {
            _context.ChangeStateTo(new ConcreteStateForward(_context, gear));
            Console.WriteLine($"The car is moving forward on gear {gear}");
            return;
         }

         Console.WriteLine($"You can't change gear from {_gear} to {gear}");

      }

   }
   static void Main(string[] args)
   {




      Context car = new Context();
      State standingCar = new ConcreteStateStand(car);
      car.ChangeStateTo(standingCar);

      char command;
      //console loop to drive car (by context methods)
      while (true)
      {
                  Console.WriteLine("Drive a car: ");
                  Console.WriteLine("b - go back");
                  Console.WriteLine("s - stop");
                  Console.WriteLine("1-5 - drive on chosen gear\n");

         command = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (command)
         {
            case 'b':
               car.RequestGoBack();
               break;
            case 's':
               car.RequestStopCar();
               break;
            case '1':
               car.RequestGoForward(1);
               break;
            case '2':
               car.RequestGoForward(2);
               break;
            case '3':
               car.RequestGoForward(3);
               break;
            case '4':
               car.RequestGoForward(4);
               break;
            case '5':
               car.RequestGoForward(5);
               break;
            default:
               Console.WriteLine("Wrong command");
               break;
         }

            Console.WriteLine();
        }


      /*
       State pStateA = new ConcreateStateA();
       State pStateB = new ConcreateStateB();
       Context pContext = new Context(pStateA);
       pContext->Request();

       pContext->ChangeState(pStateB);
       pContext->Request();
       */

   }
}