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

using FluentAssertions;
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.SequenceDiagramAnalysis.ViewModels;
using Moq;
using NUnit.Framework;

namespace GraphalyzerPro.SequenceDiagramAnalysis.Tests.ViewModelTests
{
    [TestFixture]
    public class ProcessViewModelTest
    {
        [Test]
        public void Constructor_Normal_ThreadsNotNull()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);
            var processViewModel = new ProcessViewModel(mock.Object, 0);
            processViewModel.Threads.Should().NotBeNull();
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntriesDifferentDurations_TotalDurationIsMaximumDuration()
        {
            var mock1 = new Mock<IDiagnoseOutputEntry>();
            mock1.Setup(x => x.ThreadNumber).Returns(1);
            mock1.Setup(x => x.Gap).Returns(2);
            mock1.Setup(x => x.Duration).Returns(3);
            var mock2 = new Mock<IDiagnoseOutputEntry>();
            mock2.Setup(x => x.ThreadNumber).Returns(4);
            mock2.Setup(x => x.Gap).Returns(5);
            mock2.Setup(x => x.Duration).Returns(6);

            var processViewModel = new ProcessViewModel(mock1.Object, 0);
            processViewModel.ProcessNewDiagnoseOutputEntry(mock2.Object);

            processViewModel.TotalDuration.Should().Be(16);
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithExistingThreadNumber_AddsNewEntryAndNoNewThread()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.ThreadNumber).Returns(1234);
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);

            var processViewModel = new ProcessViewModel(mock.Object, 0);
            processViewModel.ProcessNewDiagnoseOutputEntry(mock.Object);

            processViewModel.Threads.Count.Should().Be(1);
            processViewModel.Threads.Should().Contain(x => x.ThreadNumber == 1234);
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithNonExistingThreadNumber_AddsNewThread()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.ThreadNumber).Returns(1234);
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);

            var processViewModel = new ProcessViewModel(mock.Object, 0);

            processViewModel.Threads.Should().Contain(x => x.ThreadNumber == 1234);
        }
    }
}