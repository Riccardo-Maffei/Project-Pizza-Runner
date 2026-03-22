using System;
using System.Collections.Generic;


namespace utils
{
    public class Observable<T>
    {
        private T _value;
        private readonly List<Action<T>> _observers;

        public Observable(T value)
        {
            _value = value;
            _observers = new List<Action<T>>();
        }

        public void Subscribe(Action<T> observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(Action<T> observer)
        {
            _observers.Remove(observer);
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer(_value);
            }
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            _value = value;
            NotifyObservers();
        }
    }

    public static class ObservableExtensions
    {
        public static void Increase(this Observable<double> observable, double value)
        {
            observable.SetValue(observable.GetValue() + value);
        }

        public static void Increase(this Observable<float> observable, float value)
        {
            observable.SetValue(observable.GetValue() + value);
        }

        public static void Decrease(this Observable<int> observable, int value)
        {
            observable.SetValue(observable.GetValue() - value);
        }
    }
}