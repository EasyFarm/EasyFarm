using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyFarm.Infrastructure;
using EasyFarm.Parsing;
using EasyFarm.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace EasyFarm.Handlers
{
    public class SelectAbilityRequestHandler
    {
        private readonly MetroWindow _window;

        public SelectAbilityRequestHandler(MetroWindow window)
        {
            _window = window;
        }

        public async Task<Ability> Handle(string abilityName)
        {
            // Retriever all moves with the specified name.
            List<Ability> moves = ViewModelBase.AbilityService.GetAbilitiesWithName(abilityName).ToList();

            // Return Ability if there's only one.
            if (moves.Count <= 1) return moves.FirstOrDefault();

            // Let user select ability if there are multiples.
            var dialog = new SelectActionDialog(moves);
            await _window.ShowMetroDialogAsync(dialog);
            await dialog.WaitUntilUnloadedAsync();
            return dialog.SelectedAbility;
        }
    }
}