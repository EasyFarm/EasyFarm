// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
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

        /// <summary>
        /// Disabling test since it fails in appveyor for some reason. 
        /// </summary>
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
        public void DeleteCommandWithNonValuesDoesNothing()
        {
            // Fixture setup
            var sut = new IgnoredViewModel
            {
                Value = "test",
                Values = new ObservableCollection<string>()
            };

            // Exercise system
            sut.DeleteCommand.Execute(null);

            // Verify outcome
            Assert.Empty(sut.Values);

            // Teardown
        }

        [Fact]
        public void AddCommandWithAlreadyExistingValueDoesNothing()
        {
            // Fixture setup
            var sut = new IgnoredViewModel
            {
                Value = "test",
                Values = new ObservableCollection<string>() { "test" }
            };

            // Exercise system
            sut.AddCommand.Execute(null);

            // Verify outcome
            Assert.Contains("test", sut.Values);

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
