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

using System.Globalization;
using System.Windows;
using FluentAssertions;
using GraphalyzerPro.SequenceDiagramAnalysis.ViewModels;
using NUnit.Framework;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.Tests
{
    [TestFixture]
    public class DurationToHeightConverterTest
    {
        [SetUp]
        public void Bootstrapper()
        {
            RxApp.GetFieldNameForPropertyNameFunc = delegate(string name)
            {
                var nameAsArray = name.ToCharArray();
                nameAsArray[0] = char.ToLower(nameAsArray[0], CultureInfo.InvariantCulture);
                return '_' + new string(nameAsArray);
            };
        }

        [Test]
        public void Convert_AllValuesOk()
        {
            var converter = new DurationToHeightConverter();
            ((double)(converter.Convert(new object[] { (long)(1), (long)(10), (double)(20) }, typeof(double), null, CultureInfo.InvariantCulture))).Should().Be(2);
        }

        [Test]
        public void Convert_FirstValueDependencyPropertyUnset()
        {
            var converter = new DurationToHeightConverter();
            ((double)(converter.Convert(new object[] { DependencyProperty.UnsetValue, (long)(10), (double)(20) }, typeof(double), null, CultureInfo.InvariantCulture))).Should().Be(1.0);
        }

        [Test]
        public void Convert_SecondValueDependencyPropertyUnset()
        {
            var converter = new DurationToHeightConverter();
            ((double)(converter.Convert(new object[] { (long)(1), DependencyProperty.UnsetValue, (double)(20) }, typeof(double), null,
                              CultureInfo.InvariantCulture))).Should().Be(1.0);
        }

        [Test]
        public void Convert_ThirdValueDependencyPropertyUnset()
        {
            var converter = new DurationToHeightConverter();
            ((double)(converter.Convert(new object[] { (long)(1), (long)(10), DependencyProperty.UnsetValue }, typeof(double), null, CultureInfo.InvariantCulture))).Should().Be(1.0);
        }

        [Test]
        public void Convert_TotalDurationIsNull()
        {
            var converter = new DurationToHeightConverter();
            ((double)(converter.Convert(new object[] { (long)(1), (long)(0), (double)(20) }, typeof(double), null, CultureInfo.InvariantCulture))).Should().Be(1.0);
        }

        [Test]
        public void Convert_DurationIsNull()
        {
            var converter = new DurationToHeightConverter();
            ((double)(converter.Convert(new object[] { (long)(0), (long)(10), (double)(20) }, typeof(double), null, CultureInfo.InvariantCulture))).Should().Be(1.0);
        }
    }
}