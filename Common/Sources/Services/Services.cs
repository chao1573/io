using System;
using System.Collections.Generic;

namespace Common
{
    public interface IService
    {
    }

    public static class Services
    {
        private static readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        public static void Add<T>(T service) where T : IService
        {
            services.Add(typeof(T), service);
        }

        public static T Get<T>() where T : IService
        {
            IService service;
            if (services.TryGetValue(typeof(T), out service))
            {
                return (T) service;
            }

            return default(T);
        }
    }
}