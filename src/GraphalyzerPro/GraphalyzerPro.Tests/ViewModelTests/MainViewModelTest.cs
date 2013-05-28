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
using System.Linq;
using System.Reflection;
using FluentAssertions;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Models;
using GraphalyzerPro.ViewModels;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using ReactiveUI.Testing;

namespace GraphalyzerPro.Tests.ViewModelTests
{
    [TestFixture]
    public class MainViewModelTest
    {
        private static void SetField(object obj, object value, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName,
                                                   BindingFlags.NonPublic | BindingFlags.Public |
                                                   BindingFlags.Instance |
                                                   BindingFlags.Static);

            // ReSharper disable PossibleNullReferenceException
            fieldInfo.SetValue(obj, value);
            // ReSharper restore PossibleNullReferenceException
        }

        private static object GetField(object obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName,
                                                   BindingFlags.NonPublic | BindingFlags.Public |
                                                   BindingFlags.Instance |
                                                   BindingFlags.Static);

            // ReSharper disable PossibleNullReferenceException
            return fieldInfo.GetValue(obj);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        public void CloseSessionCommand_OneSessionThrowsException_ExceptionShouldBeHandled()
        {
            var receiverMock = new Mock<IReceiver>();
            receiverMock.Setup(m => m.Deactivate()).Throws<Exception>();

            var analysisMock = new Mock<IAnalysis>();

            var mainViewModel = new MainViewModel();
            var closeSessionErrorMessageWasShown = false;

            SetField(mainViewModel, new Action(() => { closeSessionErrorMessageWasShown = true; }),
                     "_showCloseSessionErrorMessage");

            var informationEngine = GetField(mainViewModel, "_informationEngine") as InformationEngine;

            // ReSharper disable PossibleNullReferenceException
            informationEngine.AddSession(receiverMock.Object, analysisMock.Object);
            // ReSharper restore PossibleNullReferenceException

            mainViewModel.CloseSessionCommand.Execute(mainViewModel.SessionViewModels.First());

            closeSessionErrorMessageWasShown.Should().BeTrue();
        }

        [Test]
        public void CloseSessionCommand_OneSession_SessionShouldBeDeleted()
        {
            var receiverMock = new Mock<IReceiver>();
            var analysisMock = new Mock<IAnalysis>();

            var mainViewModel = new MainViewModel();
            var closeSessionErrorMessageWasShown = false;

            SetField(mainViewModel, new Action(() => { closeSessionErrorMessageWasShown = true; }),
                     "_showCloseSessionErrorMessage");

            var informationEngine = GetField(mainViewModel, "_informationEngine") as InformationEngine;

            // ReSharper disable PossibleNullReferenceException
            informationEngine.AddSession(receiverMock.Object, analysisMock.Object);
            // ReSharper restore PossibleNullReferenceException


            mainViewModel.CloseSessionCommand.Execute(mainViewModel.SessionViewModels.First());

            mainViewModel.SessionViewModels.Count.Should().Be(0);
            closeSessionErrorMessageWasShown.Should().BeFalse();
        }

        [Test]
        public void Constructor_InitialisesSessionViewModlesWithEmptyCollection()
        {
            var mainViewModel = new MainViewModel();

            mainViewModel.SessionViewModels.Any().Should().BeFalse();
        }

        [Test]
        public void InitializeReceiverCommand_EveryThingOk_NothingShouldBeHappen()
        {
            (new TestScheduler()).With(scheduler =>
                {
                    var mock = new Mock<IReceiver>();
                    mock.Setup(m => m.Initialize(It.IsAny<IInformationEngine>(), It.IsAny<Guid>()));

                    var mainViewModel = new MainViewModel();
                    var initializationErrorMessageWasShown = false;
                    SetField(mainViewModel, new Action(() => { initializationErrorMessageWasShown = true; }),
                             "_showInitializationErrorMessage");

                    mainViewModel.InitializeReceiverCommand.Execute(new Tuple<Guid, IReceiver>(Guid.NewGuid(),
                                                                                               mock.Object));

                    scheduler.AdvanceToMs(100);

                    initializationErrorMessageWasShown.Should().BeFalse();
                });
        }

        [Test]
        public void
            InitializeReceiverCommand_ExceptionThrownWithinInitialization_ExceptionShouldBeCatchedAndTheUserNotified()
        {
            (new TestScheduler()).With(scheduler =>
                {
                    var mock = new Mock<IReceiver>();
                    mock.Setup(m => m.Initialize(It.IsAny<IInformationEngine>(), It.IsAny<Guid>())).Throws<Exception>();

                    var mainViewModel = new MainViewModel();
                    var initializationErrorMessageWasShown = false;
                    SetField(mainViewModel, new Action(() => { initializationErrorMessageWasShown = true; }),
                             "_showInitializationErrorMessage");

                    mainViewModel.InitializeReceiverCommand.Execute(mock.Object);

                    scheduler.AdvanceToMs(100);

                    initializationErrorMessageWasShown.Should().BeTrue();
                }
                );
        }

        [Test]
        public void Title_ReturnsCorrectTitle()
        {
            var mainViewModel = new MainViewModel();

            mainViewModel.Title.Should().Be("GraphalyzerPro");
        }
    }
}