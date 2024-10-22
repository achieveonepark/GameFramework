using PureMVC.Interfaces;
using PureMVC.Patterns.Proxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameFramework
{
    public class Controller
    {
        protected readonly ConcurrentDictionary<string, IProxy> proxyMap;
        protected static Controller instance;
        protected const string SingletonMsg = "Model Singleton already constructed!";

        public Controller()
        {
            if (instance != null) throw new Exception(SingletonMsg);
            instance = this;
            proxyMap = new ConcurrentDictionary<string, IProxy>();
        }

        public static Controller GetInstance(Func<Controller> factory = null)
        {
            if (factory == null && instance == null)
            {
                throw new Exception("An instance does not exist. Please provide the parameters for creation.");
            }

            instance ??= factory();
            return instance;
        }

        public void RegistData<TKey, TValue>(string proxyName, Dictionary<TKey, TValue> value)
        {
            if (instance == null)
            {
                throw new Exception("An instance does not exist. Please provide the parameters for creation.");
            }

            RegisterProxy(new Proxy(proxyName, value));
        }

        public T GetData<T>(string proxyName, int id)
        {
            var proxy = RetrieveProxy(proxyName);
            var data = (Dictionary<int, T>)proxy.Data;

            if (data.TryGetValue(id, out var value))
            {
                return value;
            }

            return default;
        }

        public bool HasData(string proxyName)
        {
            return HasProxy(proxyName);
        }

        public void RemoveData(string proxyName)
        {
            RemoveProxy(proxyName);
        }

        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c>.
        /// </summary>
        /// <param name="proxy">proxy an <c>IProxy</c> to be held by the <c>Model</c>.</param>
        public virtual void RegisterProxy(IProxy proxy)
        {
            proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }

        private IProxy RetrieveProxy(string proxyName)
        {
            return proxyMap.TryGetValue(proxyName, out var proxy) ? proxy : null;
        }

        private IProxy RemoveProxy(string proxyName)
        {
            if (proxyMap.TryRemove(proxyName, out var proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        private bool HasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }
    }
}