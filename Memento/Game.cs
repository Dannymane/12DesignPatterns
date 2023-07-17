using System;
using System.Text;
using System.Threading;
namespace Memento
{
    public class Originator
    {
        private Memento[] _mementos = new Memento[9];
        private int _currentStateIndex = -1;
        private char[] _state = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private int _player = 1;
        private char _choice; //This holds the _choice at which position user want to mark
        // The _flag variable checks who has won if it's value is 1 then someone has won the match
        //if -1 then Match has Draw if 0 then match is still running
        private int _flag = 0;
        private class Memento
        {
            public readonly char[] _state;

            public Memento(char[] state)
            {
                _state = new char[10];
                Array.Copy(state, _state, 10);
            }
        }
        public void StartGame()
        {
            do{
                Console.Clear();
                Console.WriteLine("Player1:X and Player2:O");
                Console.WriteLine("Press z to undo");
                if (_player % 2 == 0)
                {
                    Console.WriteLine("Player 2 Chance");
                }
                else
                {
                    Console.WriteLine("Player 1 Chance");
                }
                Console.WriteLine("\n");
                Board();

                try
                {
                    _choice = Console.ReadLine()[0];
                }catch (System.IndexOutOfRangeException e1)
                {
                    Console.WriteLine("Enter at least 1 character");
                    Thread.Sleep(2000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                if (_choice == 'z')
                {
                    if (_mementos != null && _currentStateIndex != 0)
                    {
                        _currentStateIndex--;
                        //_state = _mementos[_currentStateIndex]._state;
                        _state = new Memento(_mementos[_currentStateIndex]._state)._state;
                        _player--;
                    }
                }
                else if(Char.IsDigit(_choice) && _choice != '0')
                {
                    int choice = (int)_choice - 48;//minus 48 becouse int returns a Unicode code point of character
                    if (_state[choice] != 'X' && _state[choice] != 'O')
                    {
                        if (_player % 2 == 0) //if chance is of _player 2 then mark O else mark X
                        {
                            _state[choice] = 'O';
                            _player++;
                        }
                        else
                        {
                            _state[choice] = 'X';
                            _player++;
                        }
                        _currentStateIndex++;
                        _mementos[_currentStateIndex] = new Memento(_state);
                    }
                    else
                    //If there is any possition where user want to run
                    //and that is already marked then show message and load board again
                    {
                        Console.WriteLine("Sorry the row {0} is already marked with {1}", _choice, _state[_choice]);
                        Console.WriteLine("\n");
                        Console.WriteLine("Please wait 2 second board is loading again.....");
                        Thread.Sleep(2000);
                    }
                }
                
                _flag = CheckWin();// calling of check win
            }while (_flag != 1 && _flag != -1);

            Console.Clear();
            Board();
            if (_flag == 1)
            {
                Console.WriteLine("Player {0} has won", (_player % 2) + 1);
            }else// if _flag value is -1 the match will be draw and no one is winner
                {
                    Console.WriteLine("Draw");
                }
            Console.ReadLine();
        }
        // Board - redraw field
        private void Board()
        {
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", _state[1], _state[2], _state[3]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", _state[4], _state[5], _state[6]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", _state[7], _state[8], _state[9]);
            Console.WriteLine("     |     |      ");
        }
        //Return 1 if somebody win, 0 if there is a draw, otherwise -1
        private int CheckWin()
        {
            #region Horzontal Winning Condtion
            if (_state[1] == _state[2] && _state[2] == _state[3])
            {
                return 1;
            }
            else if (_state[4] == _state[5] && _state[5] == _state[6])
            {
                return 1;
            }
            else if (_state[6] == _state[7] && _state[7] == _state[8])
            {
                return 1;
            }
            #endregion
            #region vertical Winning Condtion
            else if (_state[1] == _state[4] && _state[4] == _state[7])
            {
                return 1;
            }
            else if (_state[2] == _state[5] && _state[5] == _state[8])
            {
                return 1;
            }
            else if (_state[3] == _state[6] && _state[6] == _state[9])
            {
                return 1;
            }
            #endregion
            #region Diagonal Winning Condition
            else if (_state[1] == _state[5] && _state[5] == _state[9])
            {
                return 1;
            }
            else if (_state[3] == _state[5] && _state[5] == _state[7])
            {
                return 1;
            }
            #endregion
            #region Checking For Draw
            else if (_state[1] != '1' && _state[2] != '2' && _state[3] != '3' && _state[4] != '4' && _state[5] != '5' && _state[6] != '6' && _state[7] != '7' && _state[8] != '8' && _state[9] != '9')
            {
                return -1;
            }
            #endregion
            else
            {
                return 0;
            }
        }

    }

    class Game
    {

        static void Main(string[] args)
        {
            Originator o = new Originator();
            while (true)
            {
                o.StartGame();
                o = new Originator();
                Thread.Sleep(3000);
            }

        }
    }
}