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

using System.Reflection;
using FluentAssertions;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.ViewModels;
using Moq;
using NUnit.Framework;

namespace GraphalyzerPro.Tests.ViewModelTests
{
    [TestFixture]
    public class NewSessionDialogViewModelTest
    {
        private static void SetProperty(object obj, object value, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName,
                                                         BindingFlags.NonPublic | BindingFlags.Public |
                                                         BindingFlags.Instance |
                                                         BindingFlags.Static);

            propertyInfo.SetValue(obj, value, null);
        }

        [Test]
        public void ApplyCommand_AnalysisSelected_NotExecutable()
        {
            var analysisMock = new Mock<IAnalysis>();

            var newSessionDialogViewModel = new NewSessionDialogViewModel();

            SetProperty(newSessionDialogViewModel, analysisMock.Object, "SelectedAnalysis");

            newSessionDialogViewModel.ApplyCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void ApplyCommand_NothingSelected_NotExecutable()
        {
            var newSessionDialogViewModel = new NewSessionDialogViewModel();

            newSessionDialogViewModel.ApplyCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void ApplyCommand_ReceiverAndAnalysisSelected_Executable()
        {
            var receiverMock = new Mock<IReceiver>();
            var analysisMock = new Mock<IAnalysis>();

            var newSessionDialogViewModel = new NewSessionDialogViewModel();
            newSessionDialogViewModel.AllReceiver.Add(receiverMock.Object);
            newSessionDialogViewModel.AllAnalyses.Add(analysisMock.Object);

            SetProperty(newSessionDialogViewModel, receiverMock.Object, "SelectedReceiver");
            SetProperty(newSessionDialogViewModel, analysisMock.Object, "SelectedAnalysis");

            newSessionDialogViewModel.ApplyCommand.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void ApplyCommand_ReceiverSelected_NotExecutable()
        {
            var receiverMock = new Mock<IReceiver>();

            var newSessionDialogViewModel = new NewSessionDialogViewModel();
            newSessionDialogViewModel.AllReceiver.Add(receiverMock.Object);

            SetProperty(newSessionDialogViewModel, receiverMock.Object, "SelectedReceiver");

            newSessionDialogViewModel.ApplyCommand.CanExecute(null).Should().BeFalse();
        }
    }
}