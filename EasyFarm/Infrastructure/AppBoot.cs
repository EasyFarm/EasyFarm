using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DryIoc;
using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.ViewModels;
using EasyFarm.Views;
using MahApps.Metro.Controls.Dialogs;
using MediatR;

namespace EasyFarm.Infrastructure
{
    public class AppBoot
    {
        private readonly Container _container;
        private readonly App _app;
        private readonly IMediator _mediatr;

        public AppBoot(App app)
        {
            _app = app;
            _container = CreateContainer();
            _mediatr = _container.Resolve<IMediator>();
            _app.MainWindow = new MasterView();
        }

        public void Initialize()
        {
            SystemTray.ConfigureTray(_app.MainWindow);
        }

        public object ViewModel => _app.MainWindow?.DataContext;

        private Container CreateContainer()
        {
            var container = new Container();
            RegisterMediatr(container);
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
            container.Register<IRequestHandler<NavigateViewRequest>, NavigateViewRequestHandler>();
            container.Register<IRequestHandler<SelectProcessRequest>, SelectProcessRequestHandler>();
            container.Register<LibraryUpdater>();
            container.Register<IDialogCoordinator, DialogCoordinator>();
        }

        private void RegisterMediatr(Container container)
        {
            container.RegisterDelegate<SingleInstanceFactory>(r => r.Resolve);
            container.RegisterDelegate<MultiInstanceFactory>(r => serviceType => r.ResolveMany(serviceType));
            container.RegisterMany(
                new[] {typeof(IMediator).GetAssembly()},
                type => type.GetTypeInfo().IsInterface);
        }

        public void Navigate<TViewModel>()
        {
            _mediatr.Send(NavigateViewRequest.Create<TViewModel>()).GetAwaiter();
        }

        public void Show()
        {
            _app.MainWindow?.Show();
        }
    }
}