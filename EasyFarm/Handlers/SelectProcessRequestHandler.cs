using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MediatR;
using MemoryAPI.Memory;

namespace EasyFarm.ViewModels
{
    public class SelectProcessRequestHandler : IRequestHandler<SelectProcessRequest>
    {
        private readonly MetroWindow window;

        public SelectProcessRequestHandler(App app)
        {
            this.window = (app.MainWindow as MetroWindow);
        }

        public async Task Handle(SelectProcessRequest message, CancellationToken cancellationToken)
        {
            // Let user select ffxi process
            var selectionView = new SelectProcessDialog();
            var viewModel = new SelectProcessViewModel(selectionView);
            selectionView.DataContext = viewModel;

            // Show window and do not continue until user closes it. 
            await window.ShowMetroDialogAsync(selectionView);
            await selectionView.WaitUntilUnloadedAsync();

            // Get the selected process.
            var process = viewModel.SelectedProcess;

            // User never selected a process.
            if (process == null || !viewModel.IsProcessSelected)
            {
                LogViewModel.Write("Process not found");
                AppServices.InformUser("No valid process was selected.");
                return;
            }

            // Log that a process selected.
            LogViewModel.Write("Process found");

            // Get memory reader set in config file.
            var fface = MemoryWrapper.Create(process.Id);

            // Set the EliteApi Session.
            ViewModelBase.SetSession(fface);

            // Tell the user the program has loaded the player's data
            AppServices.InformUser("Bot Loaded: " + fface.Player.Name);

            // Set the main window's title to the player's name.
            AppServices.UpdateTitle("EasyFarm - " + fface.Player.Name);
        }
    }
}