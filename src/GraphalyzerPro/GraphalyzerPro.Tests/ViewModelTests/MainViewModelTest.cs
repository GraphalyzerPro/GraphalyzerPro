using FluentAssertions;
using GraphalyzerPro.ViewModels;
using NUnit.Framework;

namespace GraphalyzerPro.Tests.ViewModelTests
{
    [TestFixture]
    public class MainViewModelTest
    {
        [Test]
        public void Title_ReturnsCorrectTitle()
        {
            var mainViewModel = new MainViewModel();

            mainViewModel.Title.Should().Be("GraphalyzerPro");
        }
    }
}
