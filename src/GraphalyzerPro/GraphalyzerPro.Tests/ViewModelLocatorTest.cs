using System;
using NUnit.Framework;
using GraphalyzerPro.ViewModels;
using FluentAssertions;
using Microsoft.Practices.Unity;

namespace GraphalyzerPro.Tests
{
    [TestFixture]
    public class ViewModelLocatorTest
    {
        private readonly ViewModelLocator _viewModelLocator = new ViewModelLocator();

        [Test]
        public void Resolve_CorrectInterface_ReturnsInstanceOfTheGivenInterface()
        {
            var mainViewModel = ViewModelLocator.Resolve<IMainViewModel>();

            mainViewModel.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void Resolve_IncorrectInterface_ReturnsInstanceOfTheGivenInterface()
        {
            ViewModelLocator.Resolve<ICloneable>();
        }
    }
}
