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
using EasyFarm.Classes;
using EasyFarm.ViewModels;
using Moq;
using Xunit;

namespace EasyFarm.Tests.ViewModels
{
    public class MasterViewModelTest
    {
        public class OnLoad
        {
            [Fact]
            public void TitleBarContainsText()
            {
                // Fixture setup
                var sut = CreateSut();

                // Exercise system
                sut.OnLoad();

                // Verify outcome
                Assert.Equal("EasyFarm", sut.MainWindowTitle);

                // Teardown
            }

            [Fact]
            public void StatusBarTextIsBlank()
            {
                // Fixture setup
                var sut = CreateSut();

                // Exercise system
                sut.OnLoad();

                // Verify outcome
                Assert.Equal("", sut.StatusBarText);
                // Teardown
            }

            private static MasterViewModel CreateSut()
            {
                return new MasterViewModel(Mock.Of<ISystemTray>());
            }
        }        
    }
}
