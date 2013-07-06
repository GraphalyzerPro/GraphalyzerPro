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
    public class SequenceDiagramViewModelTest
    {
        [Test]
        public void Constructor_Normal_ProcessesNotNull()
        {
            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.Processes.Should().NotBeNull();
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntriesDifferentDurations_TotalDurationIsMaximumDuration()
        {
            var mock1 = new Mock<IDiagnoseOutputEntry>();
            mock1.Setup(x => x.ProcessId).Returns(1);
            mock1.Setup(x => x.ThreadNumber).Returns(2);
            mock1.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock1.Setup(x => x.Gap).Returns(3);
            mock1.Setup(x => x.Duration).Returns(4);
            var mock2 = new Mock<IDiagnoseOutputEntry>();
            mock2.Setup(x => x.ProcessId).Returns(5);
            mock2.Setup(x => x.ThreadNumber).Returns(6);
            mock2.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock2.Setup(x => x.Gap).Returns(7);
            mock2.Setup(x => x.Duration).Returns(8);

            var processViewModel = new ProcessViewModel(mock1.Object, 0);
            processViewModel.ProcessNewDiagnoseOutputEntry(mock2.Object);

            processViewModel.TotalDuration.Should().Be(22);
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithExistingProcessId_AddsNewEntryAndNoNewProcess()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.ProcessId).Returns(1234);
            mock.Setup(x => x.ThreadNumber).Returns(1);
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(2);
            mock.Setup(x => x.Gap).Returns(3);

            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.ProcessNewDiagnoseOutputEntry(mock.Object);
            sequenceDiagramViewModel.ProcessNewDiagnoseOutputEntry(mock.Object);

            sequenceDiagramViewModel.Processes.Count.Should().Be(1);
            sequenceDiagramViewModel.Processes.Should().Contain(x => x.ProcessId == 1234);
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithNonExistingProcessId_AddsNewProcess()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.ProcessId).Returns(1234);
            mock.Setup(x => x.ThreadNumber).Returns(1);
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(2);
            mock.Setup(x => x.Gap).Returns(3);

            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.ProcessNewDiagnoseOutputEntry(mock.Object);
            sequenceDiagramViewModel.Processes.Should().Contain(x => x.ProcessId == 1234);
        }
    }
}