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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using GraphalyzerPro.Common.Interfaces;
using Moq;
using NUnit.Framework;

namespace GraphalyzerPro.CsvReceiver.Tests
{
    [TestFixture]
    public class CsvReceiverTest
    {
        private static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            return type.GetMethod(methodName,
                                  BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                                  BindingFlags.Static);
        }

        private static void SetProperty(object obj, object value, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName,
                                                                  BindingFlags.NonPublic | BindingFlags.Public |
                                                                  BindingFlags.Instance |
                                                                  BindingFlags.Static);

            propertyInfo.SetValue(obj, value, null);
        }

        private static object GetProperty(object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName,
                                                                  BindingFlags.NonPublic | BindingFlags.Public |
                                                                  BindingFlags.Instance |
                                                                  BindingFlags.Static);

            return propertyInfo.GetValue(obj);
        }

        [Test]
        public void Deactivate_SetsTheInformationEngineToNull()
        {
            var csvReceiver = new CsvReceiver();

            var mock = new Mock<IInformationEngine>();
            mock.Setup(m => m.ProcessNewDiagnoseOutputEntry(It.IsAny<IDiagnoseOutputEntry>(), It.IsAny<Guid>()));

            SetProperty(csvReceiver, mock.Object, "InformationEngine");

            csvReceiver.Deactivate();

            GetProperty(csvReceiver, "InformationEngine").Should().BeNull();
        }

        [Test]
        public void ProcessTheCsvFile_ExistingFile_CallsTheProcessNewDiagnoseOutputEntryMethodSixTimes()
        {
            var numberOfCalls = 0;

            var csvReceiver = new CsvReceiver();

            var mock = new Mock<IInformationEngine>();
            mock.Setup(m => m.ProcessNewDiagnoseOutputEntry(It.IsAny<IDiagnoseOutputEntry>(), It.IsAny<Guid>()))
                .Callback(() => numberOfCalls++);

            SetProperty(csvReceiver, mock.Object, "InformationEngine");

            var processTheCsvFileMethod = GetMethodInfo(typeof (CsvReceiver), "ProcessTheCsvFile");

            processTheCsvFileMethod.Invoke(
                csvReceiver,
                new object[] {Directory.GetCurrentDirectory() + "\\TestData\\TestData.csv"});

            numberOfCalls.Should().Be(6);
        }

        [Test]
        public void ProcessTheCsvFile_NonExistingFile_CallsTheProcessNewDiagnoseOutputEntryMethodZeroTimes()
        {
            var numberOfCalls = 0;

            var csvReceiver = new CsvReceiver();

            var mock = new Mock<IInformationEngine>();
            mock.Setup(m => m.ProcessNewDiagnoseOutputEntry(It.IsAny<IDiagnoseOutputEntry>(), It.IsAny<Guid>()))
                .Callback(() => numberOfCalls++);

            SetProperty(csvReceiver, mock.Object, "InformationEngine");

            var processTheCsvFileMethod = GetMethodInfo(typeof (CsvReceiver), "ProcessTheCsvFile");

            processTheCsvFileMethod.Invoke(csvReceiver, new object[] {string.Empty});

            numberOfCalls.Should().Be(0);
        }

        [Test]
        public void ReadAllEntriesFromFile_ExistingFile_RetrunsAllEntries()
        {
            var csvReceiver = new CsvReceiver();

            var readAllEntriesFromFileMethod = GetMethodInfo(typeof (CsvReceiver), "ReadAllEntriesFromFile");

            var diagnoseOutputEntries =
                (IEnumerable<IDiagnoseOutputEntry>) readAllEntriesFromFileMethod.Invoke(
                    csvReceiver,
                    new object[] {Directory.GetCurrentDirectory() + "\\TestData\\TestData.csv"});

            diagnoseOutputEntries.Should().NotBeNull();

            diagnoseOutputEntries.Count().Should().Be(6);
        }

        [Test]
        public void ReadAllEntriesFromFile_NonExistingFile_RetrunsEmptyList()
        {
            var csvReceiver = new CsvReceiver();

            var readAllEntriesFromFileMethod = GetMethodInfo(typeof (CsvReceiver), "ReadAllEntriesFromFile");

            var diagnoseOutputEntries =
                (IEnumerable<IDiagnoseOutputEntry>)
                readAllEntriesFromFileMethod.Invoke(csvReceiver, new object[] {string.Empty});

            diagnoseOutputEntries.Should().NotBeNull();

            diagnoseOutputEntries.Any().Should().Be(false);
        }
    }
}