using EasyFarm.Classes;
using EasyFarm.ViewModels;
using Xunit;

namespace EasyFarm.Tests.ViewModels
{
    public class MasterViewModelTests
    {
        [Fact]
        public void UpdatesTitleAndStatusBarOnLoad()
        {
            // Fixture setup
            EventMessenger events = new EventMessenger();
            MasterViewModel sut = new MasterViewModel(null, events);
            // Exercise system
            sut.LoadedCommand.Execute(null);
            // Verify outcome
            Assert.Equal("EasyFarm",sut.MainWindowTitle);
            Assert.Empty(sut.StatusBarText);
            // Teardown	
        }
    }
}
