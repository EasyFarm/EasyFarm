using System;
using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using EasyFarm.Infrastructure;
using EasyFarm.Logging;
using MediatR;

namespace EasyFarm.Handlers
{
    public class NavigateViewRequestHandler : IRequestHandler<NavigateViewRequest>
    {
        private readonly App _app;
        private readonly Container _container;

        public NavigateViewRequestHandler(App app, Container container)
        {
            _app = app;
            _container = container;
        }

        public Task Handle(NavigateViewRequest message, CancellationToken cancellationToken)
        {
            if (_app.MainWindow != null)
            {
                _app.MainWindow.DataContext = _container.Resolve(message.Type);
            }

            return Task.FromResult(true);
        }
    }
}