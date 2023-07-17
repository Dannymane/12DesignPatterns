#include <iostream>
#include <list>
#include <algorithm>
#include <fstream>

using namespace std;

struct STATE
{
    string login;
    string status;

    STATE(string login_ = "", string status_ = "")
    {
        login = login_;
        status = status_;
    }

    string toString()
    {
        return login + "\nStatus: " + status + "\n";
    }

    string getLogin()
    {
        return login;
    }
};

class Subject;

class Observer
{
public:
    Observer() { }

    virtual ~Observer() {}

    virtual void Update(Subject* pSubject) = 0;

    STATE m_nObserverState;
};

class Subject
{
public:
    Subject() : m_nSubjectState() { }

    virtual ~Subject()
    {
        list<Observer*>::iterator it1, it2, temp;

        it1 = listaObserwatorow.begin();

        it2 = listaObserwatorow.end();

        for (;it1 != it2;)
        {
            temp = it1;

            ++it1;

            delete(*temp);
        }

        listaObserwatorow.clear();
    }

    void Dolacz(Observer* pObserver)
    {

        listaObserwatorow.push_back(pObserver);
    }

    void Odlacz(Observer* pObserver)
    {
        list<Observer*>::iterator it;

        it = find(listaObserwatorow.begin(), listaObserwatorow.end(), pObserver);

        if (listaObserwatorow.end() != it)
        {
            listaObserwatorow.erase(it);
        }

    }

    void Notyfikuj()
    {
        list<Observer*>::iterator it1, it2;

        it1 = listaObserwatorow.begin();

        it2 = listaObserwatorow.end();

        for (;it1 != it2; ++it1)
        {
            (*it1)->Update(this);
        }
    }

    virtual void SetState(STATE nState)
    {
        m_nSubjectState = nState;
    }

    virtual STATE GetState()
    {
        return m_nSubjectState;
    }

protected:
    STATE m_nSubjectState;
    list<Observer*> listaObserwatorow;
};



class ConcreateSubject : public Subject
{
public:
    ConcreateSubject() : Subject() { }

    virtual ~ConcreateSubject() { }

    virtual void SetState(STATE nState)
    {
        m_nSubjectState = nState;
    }

    virtual STATE GetState()
    {
        return m_nSubjectState;
    }

    void Update(Subject* pSubject);
};

class ConcreateObserver1 : public Observer
{
public:
    string login;
    ofstream file;

    ConcreateObserver1(string login_) : Observer()
    {
        login = login_;
        file.open(login+".txt");
    }

    virtual ~ConcreateObserver1()
    {
        file.close();
    }

    virtual void Update(Subject* pSubject)
    {
        if (NULL == pSubject)
            return;

        m_nObserverState = pSubject->GetState();

        file << m_nObserverState.toString() << endl;
    }
};



int main()
{
    Subject* czat = new ConcreateSubject;
    Observer *observer1 = new ConcreateObserver1("Danny");
    Observer *observer2 = new ConcreateObserver1("Ann");
    Observer *observer3 = new ConcreateObserver1("Cub");
    Observer *observer4 = new ConcreateObserver1("Dro");

    czat->Dolacz(observer1);
    czat->Dolacz(observer2);
    czat->Dolacz(observer3);
    czat->Dolacz(observer4);

    czat->SetState(STATE("Danny", "Online"));
    czat->Notyfikuj();
    czat->SetState(STATE("Ann", "Online"));
    czat->Notyfikuj();
    czat->SetState(STATE("Cub", "Online"));
    czat->Notyfikuj();
    czat->SetState(STATE("Dro", "Online"));
    czat->Notyfikuj();

    czat->Odlacz(observer2);
    czat->SetState(STATE("Ann", "Offline"));
    czat->Notyfikuj();

    czat->Odlacz(observer3);
    czat->SetState(STATE("Cub", "Offline"));
    czat->Notyfikuj();

    czat->SetState(STATE("Danny", "Odszedl 15 minut temu"));
    czat->Notyfikuj();
    czat->SetState(STATE("Dro", "Odszedl 15 minut temu"));
    czat->Notyfikuj();
    
    delete czat;
    return 0;
}
