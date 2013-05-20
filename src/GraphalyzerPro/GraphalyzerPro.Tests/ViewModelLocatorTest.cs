/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
 *
 * This file is part of the GraphalyzerPro application.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public
 * License along with this program. If not, see
 * <http://www.gnu.org/licenses/>.
 */

using System;
using FluentAssertions;
using GraphalyzerPro.ViewModels;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace GraphalyzerPro.Tests
{
    [TestFixture]
    public class ViewModelLocatorTest
    {
        private readonly ViewModelLocator _viewModelLocator = new ViewModelLocator();

        [Test]
        public void GetMainViewModel_ReturnsInstanceOfTheMainViewModel()
        {
            var mainViewModel = ViewModelLocator.MainViewModel;

            mainViewModel.GetType().Should().Be(typeof (MainViewModel));
        }

        [Test]
        public void Resolve_CorrectInterface_ReturnsInstanceOfTheGivenInterface()
        {
            var mainViewModel = ViewModelLocator.Resolve<IMainViewModel>();

            mainViewModel.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof (ResolutionFailedException))]
        public void Resolve_IncorrectInterface_ReturnsInstanceOfTheGivenInterface()
        {
            ViewModelLocator.Resolve<ICloneable>();
        }
    }
}