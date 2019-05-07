﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

namespace DragonStallion.Common.ServiceLocation
{
    public class ServiceLocatorFactory
    {
        private readonly ContainerBuilder _builder;
        private IEnumerable<Type> _assemblyTypes;
        private IEnumerable<Type> _implementationTypes;
        private IEnumerable<Type> _singletonTypes;

        public ServiceLocatorFactory()
        {
            _builder = new ContainerBuilder();
            _assemblyTypes = Enumerable.Empty<Type>();
            _implementationTypes = Enumerable.Empty<Type>();
            _singletonTypes = Enumerable.Empty<Type>();
        }

        #region Fluent Config

        public ServiceLocatorFactory WithAssemblyTypes(IEnumerable<Type> assemblyTypes)
        {
            _assemblyTypes = assemblyTypes ?? throw new ArgumentNullException(nameof(assemblyTypes));
            return this;
        }

        public ServiceLocatorFactory WithAssemblyTypes(params Type[] assemblyTypes) => WithAssemblyTypes((IEnumerable<Type>)assemblyTypes);

        public ServiceLocatorFactory WithImplementationTypes(IEnumerable<Type> implementationTypes)
        {
            _implementationTypes = implementationTypes ?? throw new ArgumentNullException(nameof(implementationTypes));
            return this;
        }

        public ServiceLocatorFactory WithImplementationTypes(params Type[] implementationTypes) => WithImplementationTypes((IEnumerable<Type>)implementationTypes);

        public ServiceLocatorFactory WithSingletonTypes(IEnumerable<Type> singletonTypes)
        {
            _singletonTypes = singletonTypes ?? throw new ArgumentNullException(nameof(singletonTypes));
            return this;
        }

        public ServiceLocatorFactory WithSingletonTypes(params Type[] singletonTypes) => WithSingletonTypes((IEnumerable<Type>)singletonTypes);

        #endregion

        #region Build

        public IServiceLocator Build()
        {
            RegisterAssemblies();
            RegisterImplementations();
            RegisterSingletons();

            var container = _builder.Build();
            var serviceLocator = new AutofacServiceLocator(container);

            ServiceLocator.SetLocatorProvider(() => serviceLocator);

            return serviceLocator;
        }

        private void RegisterAssemblies()
        {
            var assemblies = _assemblyTypes
                .Select(Assembly.GetAssembly)
                .ToList();

            assemblies.Add(Assembly.GetExecutingAssembly());

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null) assemblies.Add(entryAssembly);

            _builder.RegisterAssemblyTypes(assemblies.ToArray()).AsSelf();
        }

        private void RegisterImplementations()
        {
            var implementationTypes = _implementationTypes
                .ToArray();

            _builder.RegisterTypes(implementationTypes).AsImplementedInterfaces();
        }

        private void RegisterSingletons()
        {
            var singletonTypes = _singletonTypes
                .ToArray();

            _builder.RegisterTypes(singletonTypes).AsImplementedInterfaces().SingleInstance();
            _builder.RegisterTypes(singletonTypes).SingleInstance();
        }

        #endregion
    }
}
