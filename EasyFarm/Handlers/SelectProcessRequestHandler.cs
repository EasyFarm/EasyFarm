using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.Views;
using MediatR;
using MemoryAPI.Memory;

namespace EasyFarm.ViewModels
{
    public class SelectProcessRequestHandler : IRequestHandler<SelectProcessRequest>
    {
        public SelectProcessRequestHandler(App app)
        {
        }

        public Task Handle(SelectProcessRequest message, CancellationToken cancellationToken)
        {
            // Let user select ffxi process
            var selectionView = new ProcessSelectionView();
            selectionView.ShowDialog();

            // Grab the view model with the game sessions.
            var viewModel = selectionView.DataContext as ProcessSelectionViewModel;

            // If the view has a process selection view model binded to it.
            if (viewModel == null) return Task.FromResult(false);

            // Get the selected process.
            var process = viewModel.SelectedProcess;

            // User never selected a process.
            if (process == null || !viewModel.IsProcessSelected)
            {
                LogViewModel.Write("Process not found");
                AppServices.InformUser("No valid process was selected.");
                return Task.FromResult(false);
            }

            // Log that a process selected.
            LogViewModel.Write("Process found");

            // Get memory reader set in config file.
            var fface = MemoryWrapper.Create(process.Id);

            // Set the fface Session.
            ViewModelBase.SetSession(fface);

            // Tell the user the program has loaded the player's data
            AppServices.InformUser("Bot Loaded: " + fface.Player.Name);

            // Set the main window's title to the player's name.
            AppServices.UpdateTitle("EasyFarm - " + fface.Player.Name);

            return Task.FromResult(true);
        }
    }
}