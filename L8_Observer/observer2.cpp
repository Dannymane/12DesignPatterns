#include <iostream>
#include <list>
#include <algorithm>

using namespace std;
//typedef int STATE;
struct STATE
{
  //nazwa ("daniel", "warca za 15 minut")
  //status
  STATE(string n, string st);
};


class Observer;

class Subject
{
 public:
  Subject() : m_nSubjectState(-1) {}
  virtual ~Subject();

  void Notify();
  void Attach(Observer* pOberver);
  void Detach(Observer* pOberver);

  virtual void SetState(STATE nState)=0;
  virtual STATE GetState();

 protected:
  STATE m_nSubjectState;
  std::list<Observer*> m_ListObserver;
};

class Observer
{
 public:
  Observer() : m_nObserverState(-1) {}
  virtual ~Observer() {}
  virtual void Update(Subject* pSubject) = 0;

 protected:
  STATE m_nObserverState;
};

class ConcreateSubject : public Subject
{
 public:
  ConcreateSubject() : Subject() {}
  virtual ~ConcreateSubject() {}

  virtual void SetState(STATE nState);
  virtual STATE GetState();
  void Update(Subject* pSubject);
};

class ConcreateObserver : public Observer
{
 public:
  ofstream file;
  string imie;
  ConcreateObserver(string imie_) : Observer() {
    imie = imie_;
    file.open (imie_ + ".txt");
  }//konstruktor - otwiera sie plik
  virtual ~ConcreateObserver() {}//destruktor - zamkniecie pliku
  virtual void Update(Subject* pSubject);
};


void Subject::Attach(Observer* pObserver)
{
 std::cout << "Attach an Observer\n";
 m_ListObserver.push_back(pObserver);
}

void Subject::Detach(Observer* pOberver)
{
 std::list<Observer*>::iterator iter;
 iter = std::find(m_ListObserver.begin(),m_ListObserver.end(),pOberver);
 if (m_ListObserver.end() != iter)
 {
  m_ListObserver.erase(iter);
 }
 std::cout << "Detach an Observer\n";
}

void Subject::Notify()
{
 std::cout << "Notify Observer's State\n";
 std::list<Observer*>::iterator iter1,iter2;
 iter1 = m_ListObserver.begin();
 iter2 = m_ListObserver.end();
 for (;iter1 != iter2; ++iter1)
 {
  (*iter1)->Update(this);
 }
}

void Subject::SetState(STATE nState)
{
 std::cout << "SetState By Subject\n";
 m_nSubjectState = nState;
}

STATE Subject::GetState()
{
 std::cout << "GetState By Subject\n";
 return m_nSubjectState;
}

Subject::~Subject()
{
 std::list<Observer*>::iterator iter1,iter2,temp;
 iter1 = m_ListObserver.begin();
 iter2 = m_ListObserver.end();
 for (;iter1 != iter2;)
 {
  temp = iter1;
  ++iter1;
  delete(*temp);
 }
 m_ListObserver.clear();
}

void ConcreateSubject::SetState(STATE nState)
{
 std::cout << "SetState By ConcreateSubject\n";
 m_nSubjectState = nState;
}

STATE ConcreateSubject::GetState()
{
 std::cout << "GetState By ConcreateSubject\n";
 return m_nSubjectState;
}

void ConcreateObserver::Update(Subject* pSubject)
{
 if (NULL == pSubject)
 {
  return;
 }
 m_nObserverState = pSubject->GetState();
 std::cout << "The ObserverState is " << m_nObserverState << std::endl;
}

int main()
{//rozbudować, powiększyć ilość
 Observer *co1 = new ConcreateObserver("Kuba");
 Observer *co2 = new ConcreateObserver("Michal");
 Subject* p = new ConcreateSubject("Stan sieciowy");
 p->Attach(co1);
 p->Attach(co2);
 p->SetState(STATE(co1->GetNick(),"Byl 15 minut temu"));
 p->Notify();
 p->Detach(co2);
 p->SetState(STATE(co1->GetNick(),"Online"));
 p->Notify();
 delete p;
 //każdy ConcreateObserver tworzy i otwiera przy kazdym uruchomieniu programu
 //nowy czysty plik, nazwa pliku jest imie.txt (imie - imie ConcreateObserver)
 //pod konuiec programu zamyka sie plik (w destruktorze)
 //przy każdym notify ConcreateObserver dopisuje do swojego pliku linijke ze
 // statusem

 return 0;
}
