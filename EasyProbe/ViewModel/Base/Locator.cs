using Autofac;
using EasyProbe.Service.Bluetooth;
using EasyProbe.Service.Dialog;
using EasyProbe.Service.Navigation;
using EasyProbe.ViewModel.Popups;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyProbe.ViewModel.Base
{
    public class Locator
    {
        Autofac.IContainer container;
        ContainerBuilder containerBuilder;

        public static Locator Instance { get; } = new Locator();

        public Locator()
        {
            containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<DialogService>().As<IDialogService>();
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
            containerBuilder.RegisterType<BluetoothService>().As<IBluetoothService>();

            containerBuilder.RegisterType<MainViewModel>();
            containerBuilder.RegisterType<ListTagsViewModel>();


        }

        public T Resolve<T>() => container.Resolve<T>();

        public object Resolve(Type type) => container.Resolve(type);

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface => containerBuilder.RegisterType<TImplementation>().As<TInterface>();

        public void Register<T>() where T : class => containerBuilder.RegisterType<T>();

        public void Build() => container = containerBuilder.Build();
    }

}
