using System;
using System.Threading.Tasks;
using Ninject;

namespace EasyFarm.Handlers
{
    public class NavigateViewRequestHandler
    {
        private readonly App _app;
        private readonly IKernel _container;

        public NavigateViewRequestHandler(App app, IKernel container)
        {
            _app = app;
            _container = container;
        }

        public Task Handle(Type requestedType)
        {
            if (_app.MainWindow == null) return Task.FromResult(true);
            _app.MainWindow.DataContext = _container.Get(requestedType);
            return Task.FromResult(true);
        }
    }
}