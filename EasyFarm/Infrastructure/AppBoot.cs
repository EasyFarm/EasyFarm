using System;
using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.Views;
using MahApps.Metro.Controls.Dialogs;
using Ninject;
using Ninject.Extensions.Conventions;

namespace EasyFarm.Infrastructure
{
    public class AppBoot
    {
        private readonly IKernel _container;
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

        private IKernel CreateContainer()
        {
            StandardKernel container = new StandardKernel();
            RegisterApp(container);
            RegisterContainer(container);
            return container;
        }

        private void RegisterContainer(IKernel container)
        {
        }

        private void RegisterApp(IKernel container)
        {
            container.Bind<App>().ToMethod(x => _app);
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().EndingWith("ViewModel").BindToSelf());
            container.Bind<TabViewModels>().ToSelf();
            container.Bind<LibraryUpdater>().ToSelf();
            container.Bind<IDialogCoordinator>().To<DialogCoordinator>();
            container.Bind<SelectCharacterRequestHandler>().ToSelf();
            container.Bind<SelectAbilityRequestHandler>().ToSelf();
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