using System;
using DryIoc;
using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.Views;
using MahApps.Metro.Controls.Dialogs;

namespace EasyFarm.Infrastructure
{
    public class AppBoot
    {
        private readonly Container _container;
        private readonly App _app;

        public AppBoot(App app)
        {
            _app = app;
            _container = CreateContainer();
            _app.MainWindow = new MasterView();
        }

        public void Initialize()
        {
            SystemTray.ConfigureTray(_app.MainWindow);
        }

        public Object ViewModel => _app.MainWindow?.DataContext;

        private Container CreateContainer()
        {
            var container = new Container();
            RegisterApp(container);
            RegisterContainer(container);
            return container;
        }

        private void RegisterContainer(Container container)
        {
            container.RegisterInstance(container);
        }

        private void RegisterApp(Container container)
        {
            container.RegisterInstance(_app);
            container.RegisterMany(new []{ typeof(App).GetAssembly() },
                type => type.Name.EndsWith("ViewModel"));
            container.Register<TabViewModels>();
            container.Register<LibraryUpdater>();
            container.Register<IDialogCoordinator, DialogCoordinator>();
        }

        public void Navigate<TViewModel>()
        {
            NavigateViewRequestHandler handler = new NavigateViewRequestHandler(_app, _container);
            handler.Handle(typeof(TViewModel)).GetAwaiter();
        }

        public void Show()
        {
            _app.MainWindow?.Show();
        }
    }
}