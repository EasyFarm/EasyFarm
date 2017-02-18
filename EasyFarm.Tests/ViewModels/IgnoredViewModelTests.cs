using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFarm.ViewModels;
using Xunit;

namespace EasyFarm.Tests.ViewModels
{
    public class IgnoredViewModelTests
    {
        [Fact]
        public void AddCommandWithValueAddsItToValues()
        {
            // Fixture setup
            var sut = new IgnoredViewModel {Value = "Mandragora"};

            // Exercise system
            sut.AddCommand.Execute(null);

            // Verify outcome
            Assert.Equal("Mandragora", sut.Values.Single());

            // Teardown
        }

        [Fact]
        public void DeleteCommandWhenValuesContainsValueRemovesIt()
        {
            // Fixture setup
            var sut = new IgnoredViewModel
            {
                Values = new ObservableCollection<string> {"Mandragora"},
                Value = "Mandragora"
            };

            // Exercise system
            sut.DeleteCommand.Execute(null);

            // Verify outcome
            Assert.Empty(sut.Values);

            // Teardown
        }

        [Fact]
        public void AddCommandWithEmptyValueDoesNotAddIt()
        {
            // Fixture setup
            var sut = new IgnoredViewModel {Value = ""};

            // Exercise system
            sut.AddCommand.Execute(null);

            // Verify outcome
            Assert.Empty(sut.Values);

            // Teardown
        }

        [Fact]
        public void ClearCommandWithValuesClearsThem()
        {
            // Fixture setup
            var sut = new IgnoredViewModel
            {
                Values = new ObservableCollection<string>
                {
                    "Mandragora"
                }
            };

            // Exercise system
            sut.ClearCommand.Execute(null);

            // Verify outcome
            Assert.Empty(sut.Values);

            // Teardown
        }
    }
}
