using System;
using System.Collections.Generic;
using System.Text;

namespace XunitTestDemo
{
    public delegate void MyEventHandler(int i, int j);
    public interface IFoo
    {
        Bar Bar { get; set; }
        string Name { get; set; }
        int Value { get; set; }
        bool DoSomething(string value);
        bool DoSomething(int number, string value);
        string DoSomethingStringy(string value);
        bool TryParse(string value, out string outputValue);
        bool Submit(ref Bar bar);
        void Submit();
        int GetCount();
        bool Add(int value);
        event MyEventHandler MyEvent;
    }

    public class Bar
    {
        public virtual Baz Baz { get; set; }
        public virtual bool Submit() { return false; }
    }

    public class Baz
    {
        public virtual string Name { get; set; }
    }

    public class FooEventArgs : EventArgs { }
}
