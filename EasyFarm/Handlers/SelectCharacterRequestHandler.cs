using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.ViewModels;
using EasyFarm.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MemoryAPI.Memory;

namespace EasyFarm.Handlers
{
    public partial class SelectCharacterRequestHandler
    {
        private readonly MetroWindow _window;

        public SelectCharacterRequestHandler(MetroWindow window)
        {
            _window = window;
        }

        public async Task Handle()
        {
            // Let user select ffxi process
            SelectProcessDialog selectionView = new SelectProcessDialog();
            SelectProcessViewModel viewModel = new SelectProcessViewModel(selectionView);
            selectionView.DataContext = viewModel;

            // Show window and do not continue until user closes it.
            await _window.ShowMetroDialogAsync(selectionView);
            await selectionView.WaitUntilUnloadedAsync();

            // Get the selected process.
            Process process = viewModel.SelectedProcess;

            ChangeCharacter(new SelectCharacterResult
            {
                Process = process,
                IsSelected = viewModel.IsProcessSelected
            });
        }

        private void ChangeCharacter(SelectCharacterResult result)
        {
            Process process = result.Process;
            Boolean isProcessSelected = result.IsSelected;

            // User never selected a process.
            if (result.Process == null || !isProcessSelected)
            {
                LogViewModel.Write("Process not found");
                AppServices.InformUser("No valid process was selected.");
                return;
            }

            // Log that a process selected.
            LogViewModel.Write("Process found");

            // Get memory reader set in config file.
            MemoryWrapper fface = MemoryWrapper.Create(process.Id);

            // Set the EliteApi Session.
            ViewModelBase.SetSession(fface);

            // Tell the user the program has loaded the player's data
            AppServices.InformUser("Bot Loaded: " + fface.Player.Name);

            // Set the main window's title to the player's name.
            AppServices.UpdateTitle("EasyFarm - " + fface.Player.Name);
        }
    }
}