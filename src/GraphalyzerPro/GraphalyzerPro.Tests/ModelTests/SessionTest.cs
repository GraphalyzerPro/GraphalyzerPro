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
using GraphalyzerPro.Models;
using Moq;
using NUnit.Framework;

namespace GraphalyzerPro.Tests.ModelTests
{
    [TestFixture]
    public class SessionTest
    {
        [Test]
        public void AddNewAnalysis_Normal_CallsProcessNewDiagnoseOutputEntryFromAnalysis()
        {
            var processNewDiagnoseOutputEntryWasCalled = false;

            var analysisMock = new Mock<IAnalysis>();
            analysisMock.Setup(x => x.ProcessNewDiagnoseOutputEntry(It.IsAny<IDiagnoseOutputEntry>()))
                        .Callback(() => processNewDiagnoseOutputEntryWasCalled = true);

            var receiverMock = new Mock<IReceiver>();
            var defaultanalysisMock = new Mock<IAnalysis>();

            var session = new Session(receiverMock.Object, defaultanalysisMock.Object);

            var diagnoseMock = new Mock<IDiagnoseOutputEntry>();
            session.DiagnoseOutputEntries.Add(diagnoseMock.Object);

            session.Analysis.Add(analysisMock.Object);

            processNewDiagnoseOutputEntryWasCalled.Should().BeTrue();
        }

        [Test]
        public void AddNewDiagnoseOutputEntry_Normal_CallsProcessNewDiagnoseOutputEntryFromAnalysis()
        {
            var processNewDiagnoseOutputEntryWasCalled = false;

            var analysisMock = new Mock<IAnalysis>();
            analysisMock.Setup(x => x.ProcessNewDiagnoseOutputEntry(It.IsAny<IDiagnoseOutputEntry>()))
                        .Callback(() => processNewDiagnoseOutputEntryWasCalled = true);

            var receiverMock = new Mock<IReceiver>();

            var session = new Session(receiverMock.Object, analysisMock.Object);

            var diagnoseMock = new Mock<IDiagnoseOutputEntry>();

            session.DiagnoseOutputEntries.Add(diagnoseMock.Object);

            processNewDiagnoseOutputEntryWasCalled.Should().BeTrue();
        }

        [Test]
        public void Constructor_Normal_CreatsNewGuid()
        {
            var receiverMock = new Mock<IReceiver>();
            var analysisMock = new Mock<IAnalysis>();

            var session = new Session(receiverMock.Object, analysisMock.Object);
            var session2 = new Session(receiverMock.Object, analysisMock.Object);

            session.Id.Should().NotBeEmpty();
            session.Id.Should().NotBe(session2.Id);
        }
    }
}