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
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.SequenceDiagramAnalysis.ViewModels;
using Moq;
using NUnit.Framework;

namespace GraphalyzerPro.SequenceDiagramAnalysis.Tests
{
    [TestFixture]
    public class SequenceDiagramViewModelTest
    {
        [Test]
        public void Constructor_Normal_DiagnoseOutputEntriesNotNull()
        {
            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.DiagnoseOutputEntries.Should().NotBeNull();
        }

        [Test]
        public void Constructor_Normal_ProcessesNotNull()
        {
            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.Processes.Should().NotBeNull();
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithNonExistingProcessId_AddsNewEntryAndNewProcess()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.ProcessId).Returns(1234);

            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.ProcessNewDiagnoseOutputEntry(mock.Object);

            sequenceDiagramViewModel.DiagnoseOutputEntries.Should().Contain(x => x.ProcessId == 1234);
            sequenceDiagramViewModel.Processes.Should().Contain(x => x.Id == 1234);
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithExistingProcessId_AddsNewEntryAndNoNewProcess()
        {
            var entryOne = new Mock<IDiagnoseOutputEntry>();
            entryOne.Setup(x => x.ProcessId).Returns(1234);

            var sequenceDiagramViewModel = new SequenceDiagramViewModel();
            sequenceDiagramViewModel.ProcessNewDiagnoseOutputEntry(entryOne.Object);
            sequenceDiagramViewModel.ProcessNewDiagnoseOutputEntry(entryOne.Object);

            sequenceDiagramViewModel.DiagnoseOutputEntries.Count.Should().Be(2);
            sequenceDiagramViewModel.Processes.Count.Should().Be(1);
            sequenceDiagramViewModel.Processes.Should().Contain(x => x.Id == 1234);
        }
    }
}