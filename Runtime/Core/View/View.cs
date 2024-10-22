using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace GameFramework
{
    public class View : IView
    {
        protected readonly ConcurrentDictionary<string, IMediator> mediatorMap;
        protected readonly ConcurrentDictionary<string, IList<IObserver>> observerMap;
        protected static View instance;
        protected const string SingletonMsg = "View Singleton already constructed!";

        public View()
        {
            if (instance != null) throw new Exception(SingletonMsg);
            instance = this;
            mediatorMap = new ConcurrentDictionary<string, IMediator>();
            observerMap = new ConcurrentDictionary<string, IList<IObserver>>();
        }

        public static View GetInstance(Func<View> factory = null)
        {
            if (instance == null)
            {
                instance = factory();
            }
            return instance;
        }

        public void ASD()
        {
            //new ViewMediator();
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if (observerMap.TryGetValue(notificationName, out var observers))
            {
                observers.Add(observer);
            }
            else
            {
                observerMap.TryAdd(notificationName, new List<IObserver> { observer });
            }
        }

        public virtual void NotifyObservers(INotification notification)
        {
            if (observerMap.TryGetValue(notification.Name, out var observersRef))
            {
                var observers = new List<IObserver>(observersRef);

                foreach (var observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }

        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            if (observerMap.TryGetValue(notificationName, out var observers))
            {
                for (var i = 0; i < observers.Count; i++)
                {
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                if (observers.Count == 0)
                    observerMap.TryRemove(notificationName, out _);
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            if (mediatorMap.TryAdd(mediator.MediatorName, mediator))
            {
                var interests = mediator.ListNotificationInterests();

                if (interests.Length > 0)
                {
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);

                    foreach (var interest in interests)
                    {
                        RegisterObserver(interest, observer);
                    }
                }
                mediator.OnRegister();
            }
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out var mediator) ? mediator : null;
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            if (mediatorMap.TryRemove(mediatorName, out var mediator))
            {
                var interests = mediator.ListNotificationInterests();
                foreach (var interest in interests)
                {
                    RemoveObserver(interest, mediator);
                }

                mediator.OnRemove();
            }
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }
    }
}
