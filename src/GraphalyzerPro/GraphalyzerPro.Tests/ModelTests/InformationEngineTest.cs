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
using FluentAssertions;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Models;
using Moq;
using NUnit.Framework;

namespace GraphalyzerPro.Tests.ModelTests
{
    [TestFixture]
    public class InformationEngineTest
    {
        [Test]
        public void AddSession_Normal_AddsTheSessionAndReturnsTheSessionId()
        {
            var receiverMock = new Mock<IReceiver>();
            var analysisMock = new Mock<IAnalysis>();

            var informationEngine = new InformationEngine();

            var newSessionId = informationEngine.AddSession(receiverMock.Object, analysisMock.Object);

            informationEngine.Sessions.Any().Should().BeTrue();
            newSessionId.Should().NotBeEmpty();
        }

        [Test]
        public void DeleteSession_ExistingGuid_DeletesTheSession()
        {
            var receiverMock = new Mock<IReceiver>();
            var analysisMock = new Mock<IAnalysis>();

            var informationEngine = new InformationEngine();

            var newSessionId = informationEngine.AddSession(receiverMock.Object, analysisMock.Object);

            informationEngine.DeleteSession(newSessionId);

            informationEngine.Sessions.Any().Should().BeFalse();
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void DeleteSession_NonExistingGuid_Throws()
        {
            var informationEngine = new InformationEngine();

            informationEngine.DeleteSession(Guid.NewGuid());
        }
    }
}