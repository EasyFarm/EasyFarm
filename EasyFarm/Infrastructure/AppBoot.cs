using EasyFarm.Classes;
using EasyFarm.Handlers;
using EasyFarm.Views;
using Ninject;

namespace EasyFarm.Infrastructure
{
    public class AppBoot
    {
        public IKernel Container { get; set; }
        private readonly App _app;

        public AppBoot(App app)
        {
            _app = app;
            Container = CreateContainer();
            _app.MainWindow = new MasterView();
        }

        public void Initialize()
        {
            SystemTray.ConfigureTray(_app.MainWindow);
        }

        public object ViewModel => _app.MainWindow?.DataContext;

        private IKernel CreateContainer()
        {
            StandardKernel container = new StandardKernel(
                new ApplicationBindingsModule());
            return container;
        }

        public void Navigate<TViewModel>()
        {
            NavigateViewRequestHandler handler = new NavigateViewRequestHandler(_app, Container);
            handler.Handle(typeof(TViewModel)).GetAwaiter();
        }

        public void Show()
        {
            _app.MainWindow?.Show();
        }
    }
}