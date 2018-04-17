using System.Windows;
using EasyFarm.Classes;
using EasyFarm.Handlers;
using MahApps.Metro.Controls.Dialogs;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace EasyFarm.Infrastructure
{
    public class ApplicationBindingsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<App>().ToMethod(x => Application.Current as App);
            Bind<TabViewModels>().ToSelf();
            Bind<LibraryUpdater>().ToSelf();
            Bind<SelectCharacterRequestHandler>().ToSelf();
            Bind<SelectAbilityRequestHandler>().ToSelf();
            Bind<IDialogCoordinator>().To<DialogCoordinator>();

            this.Bind(x => x.FromThisAssembly().SelectAllClasses().EndingWith("ViewModel").BindToSelf());
        }
    }
}
