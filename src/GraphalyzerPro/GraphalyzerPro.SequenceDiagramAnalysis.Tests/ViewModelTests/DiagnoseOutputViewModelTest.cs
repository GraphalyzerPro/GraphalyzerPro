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
    public class DiagnoseOutputViewModelTest
    {
        [Test]
        public void Constructor_Normal_DiagnoseOutputViewModelsNotNull()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);
            var diagnoseOutputViewModel = new DiagnoseOutputViewModel(mock.Object, 0);
            diagnoseOutputViewModel.DiagnoseOutputViewModels.Should().NotBeNull();
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryStartBracketOutput_IsBracketOpen()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.Type).Returns(DiagnoseType.StartBracketOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);

            var diagnoseViewModel = new DiagnoseOutputViewModel(mock.Object, 0);

            diagnoseViewModel.IsBracketOpen.Should().Be(true);
        }

        [Test]
        public void
            ProcessNewDiagnoseOutputEntry_NewEntryWithLastAddedEntryIsBracketOpen_AddsDurationAndGapToTotalDiagnoseOutputViewModelDuration
            ()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.Type).Returns(DiagnoseType.StartBracketOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);

            var diagnoseViewModel = new DiagnoseOutputViewModel(mock.Object, 0);
            diagnoseViewModel.ProcessNewDiagnoseOutputEntry(mock.Object, 0);

            diagnoseViewModel.TotalDuration.Should().Be(4);
        }

        [Test]
        public void
            ProcessNewDiagnoseOutputEntry_NewEntryWithLastAddedEntryIsNotBracketOpen_AddsDurationAndGapToTotalDiagnoseOutputViewModelDuration
            ()
        {
            var mock = new Mock<IDiagnoseOutputEntry>();
            mock.Setup(x => x.Type).Returns(DiagnoseType.SingleOutput);
            mock.Setup(x => x.Duration).Returns(1);
            mock.Setup(x => x.Gap).Returns(2);

            var diagnoseViewModel = new DiagnoseOutputViewModel(mock.Object, 0);
            diagnoseViewModel.ProcessNewDiagnoseOutputEntry(mock.Object, 0);

            diagnoseViewModel.TotalDuration.Should().Be(4);
        }

        [Test]
        public void ProcessNewDiagnoseOutputEntry_NewEntryWithStartBracketOutputEndBracketOutput_IsNotBracketOpen()
        {
            var mock1 = new Mock<IDiagnoseOutputEntry>();
            mock1.Setup(x => x.Type).Returns(DiagnoseType.StartBracketOutput);
            mock1.Setup(x => x.Duration).Returns(1);
            mock1.Setup(x => x.Gap).Returns(2);
            var mock2 = new Mock<IDiagnoseOutputEntry>();
            mock2.Setup(x => x.Type).Returns(DiagnoseType.EndBracketOutput);
            mock2.Setup(x => x.Duration).Returns(1);
            mock2.Setup(x => x.Gap).Returns(2);

            var diagnoseViewModel = new DiagnoseOutputViewModel(mock1.Object, 0);
            diagnoseViewModel.ProcessNewDiagnoseOutputEntry(mock2.Object, 0);

            diagnoseViewModel.IsBracketOpen.Should().Be(false);
        }
    }
}