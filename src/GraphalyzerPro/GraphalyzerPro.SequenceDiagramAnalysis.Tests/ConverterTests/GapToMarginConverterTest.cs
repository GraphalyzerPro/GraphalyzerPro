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
using GraphalyzerPro.SequenceDiagramAnalysis.Converter;
using NUnit.Framework;

namespace GraphalyzerPro.SequenceDiagramAnalysis.Tests.ConverterTests
{
    [TestFixture]
    public class GapToMarginConverterTest
    {
        [Test]
        public void Convert_AllValuesOk()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new object[] {(long) (1), (double) (10), (double) (20)}, typeof (double), null,
                                CultureInfo.InvariantCulture))).Top.Should().Be(2.0);
        }

        [Test]
        public void Convert_AllValuesOk_ParameterNull()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new object[] {(long) (1), (double) (10), (double) (20)}, typeof (double), null,
                                CultureInfo.InvariantCulture))).Left.Should().Be(0.0);
        }

        [Test]
        public void Convert_AllValuesOk_ParameterSet()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new object[] {(long) (1), (double) (10), (double) (20)}, typeof (double), (long) (5),
                                CultureInfo.InvariantCulture))).Left.Should().Be(5.0);
        }

        [Test]
        public void Convert_FirstValueDependencyPropertyUnset()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new[] {DependencyProperty.UnsetValue, (double) (10), (double) (20)}, typeof (double),
                                null, CultureInfo.InvariantCulture))).Top.Should().Be(0.0);
        }

        [Test]
        public void Convert_SecondValueDependencyPropertyUnset()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new[] {(long) (1), DependencyProperty.UnsetValue, (double) (20)}, typeof (double), null,
                                CultureInfo.InvariantCulture))).Top.Should().Be(0.0);
        }

        [Test]
        public void Convert_ThirdValueDependencyPropertyUnset()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new[] {(long) (1), (double) (10), DependencyProperty.UnsetValue}, typeof (double), null,
                                CultureInfo.InvariantCulture))).Top.Should().Be(0.0);
        }

        [Test]
        public void Convert_TotalDurationIsNull()
        {
            var converter = new GapToMarginConverter();
            ((Thickness)
             (converter.Convert(new object[] {(long) (1), (double) (0), (double) (20)}, typeof (double), null,
                                CultureInfo.InvariantCulture))).Top.Should().Be(0.0);
        }
    }
}