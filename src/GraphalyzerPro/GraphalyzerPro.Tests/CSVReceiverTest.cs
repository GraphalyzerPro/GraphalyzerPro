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
using GraphalyzerPro.Receivers;
using GraphalyzerPro.Common.Interfaces;
using System.IO;
using System.Text;

namespace GraphalyzerPro.Tests
{
    [TestFixture]
    public class CSVReceiverTest
    {
		private CSVReceiver _receiver=new CSVReceiver("name");
		private	InformationEngine _informationEngine=new InformationEngine();
		private ASCIIEncoding _encoder=new ASCIIEncoding();

        [Test]
        public void Name_Get_ReturnsName()
        {
            _receiver.Name.Should().Be("name");
        }

		[Test]
		public void ReadLine_CorrectLine()
		{
			IDiagnoseOutputEntry entry=_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;6;7;8;9;(;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
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
		public void ReadLine_IncorrectLine_NoCorrectCountOfColumns()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;6;7;8;9;T;Domain;Application;Component;Module;Code;Text;Meta-Informationen;error"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadLine_IncorrectLine_Timestamp()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("error;6;7;8;9;T;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadLine_IncorrectLine_Gap()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;error;7;8;9;T;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadLine_IncorrectLine_Duration()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;6;error;8;9;T;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadLine_IncorrectLine_ProcessId()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;6;7;error;9;T;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadLine_IncorrectLine_ThreadNumber()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;6;7;8;error;T;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadLine_IncorrectLine_Type()
		{
			_receiver.ReadLine(new StreamReader(new MemoryStream(_encoder.GetBytes("01.02.2003 03:04:05;6;7;8;9;error;Domain;Application;Component;Module;Code;Text;Meta-Informationen"))));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ReadFile_IncorrectFile_Header()
		{
			_receiver.ReadFile(new StreamReader(new MemoryStream(_encoder.GetBytes("error"))));
		}

		[Test]
		public void ReadFile_CorrectFile_ZeroLinesData()
		{
			_receiver.ReadFile(new StreamReader(new MemoryStream(_encoder.GetBytes("Timestamp;Gap;Duration;PID;Thread;Type;Domain;Application;Component;Module;Code;Text;Meta-Informationen")))).Count.Should().Be(0);
		}

		[Test]
		public void ReadFile_CorrectFile_OneLinesData()
		{
			_receiver.ReadFile(new StreamReader(new MemoryStream(_encoder.GetBytes("Timestamp;Gap;Duration;PID;Thread;Type;Domain;Application;Component;Module;Code;Text;Meta-Informationen\n01.02.2003 03:04:05;6;7;8;9;(;Domain;Application;Component;Module;Code;Text;Meta-Informationen")))).Count.Should().Be(1);
		}

		[Test]
		public void ReadFile_CorrectFile_TwoLinesData()
		{
			_receiver.ReadFile(new StreamReader(new MemoryStream(_encoder.GetBytes("Timestamp;Gap;Duration;PID;Thread;Type;Domain;Application;Component;Module;Code;Text;Meta-Informationen\n01.02.2003 03:04:05;6;7;8;9;(;Domain;Application;Component;Module;Code;Text;Meta-Informationen\n01.02.2003 03:04:05;6;7;8;9;(;Domain;Application;Component;Module;Code;Text;Meta-Informationen")))).Count.Should().Be(2);
		}
    }
}
