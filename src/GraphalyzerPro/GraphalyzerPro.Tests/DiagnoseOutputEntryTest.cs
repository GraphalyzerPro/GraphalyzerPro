/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Carl-Clemens Ebinger, Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
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
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Practices.Unity;
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using System.IO;
using System.Text;

namespace GraphalyzerPro.Tests
{
    [TestFixture]
    public class DiagnoseOutputEntryTest
    {
        [Test]
        public void Constructor_CorrectParameters()
        {
			DiagnoseOutputEntry entry=new DiagnoseOutputEntry(new DateTime(2003, 2, 1, 3, 4, 5), 6, 7, 8, 9, '(', "Domain", "Application", "Component", "Module", "Code", "Text", "Meta-Informationen");
		 	entry.TimeStamp.Should().Be(new DateTime(2003, 2, 1, 3, 4, 5));
			entry.Gap.Should().Be(6);
			entry.Duration.Should().Be(7);
			entry.ProcessId.Should().Be(8);
			entry.ThreadNumber.Should().Be(9);
			entry.Type.Should().Be('(');
			entry.Domain.Should().Be("Domain");
			entry.Application.Should().Be("Application");
			entry.Component.Should().Be("Component");
			entry.Module.Should().Be("Module");
			entry.Code.Should().Be("Code");
			entry.Text.Should().Be("Text");
			entry.MetaInformation.Should().Be("Meta-Informationen");
		}

		
        [Test]
		[ExpectedException(typeof(FormatException))]
        public void Constructor_IncorrectParameters()
		{
			DiagnoseOutputEntry entry=new DiagnoseOutputEntry(new DateTime(2003, 2, 1, 3, 4, 5), 6, 7, 8, 9, 'a', "Domain", "Application", "Component", "Module", "Code", "Text", "Meta-Informationen");
		}
    }
}
