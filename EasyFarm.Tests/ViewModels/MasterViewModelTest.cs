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
