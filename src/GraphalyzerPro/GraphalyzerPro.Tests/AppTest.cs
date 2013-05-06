using System.IO;
using FluentAssertions;
using GraphalyzerPro.Common.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace GraphalyzerPro.Tests
{
	[TestFixture]
	public class AppTest
	{
		private App _app=new App();

		[Test]
		public void LoadReceiverTypes_Correct()
		{
			List<Type> list=_app.LoadReceiverTypes(".\\GraphalyzerPro.Receivers.CSVReceiver.dll");
			list.Count.Should().Be(1);
		}

		[Test]
		[ExpectedException(typeof(FileLoadException))]
		public void LoadReceiverTypes_Incorrect()
		{
			_app.LoadReceiverTypes("");
		}
	}
}
