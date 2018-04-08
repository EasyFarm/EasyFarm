using System;
using System.Threading.Tasks;
using DryIoc;

namespace EasyFarm.Handlers
{
    public class NavigateViewRequestHandler
    {
        private readonly App _app;
        private readonly Container _container;

        public NavigateViewRequestHandler(App app, Container container)
        {
            _app = app;
            _container = container;
        }

        public Task Handle(Type requestedType)
        {
            if (_app.MainWindow != null)
            {
                _app.MainWindow.DataContext = _container.Resolve(requestedType);
            }

            return Task.FromResult(true);
        }
    }
}