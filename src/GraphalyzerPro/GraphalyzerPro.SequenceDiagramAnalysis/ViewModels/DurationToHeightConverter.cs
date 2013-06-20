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
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    internal class DurationToHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            if((values[0] != DependencyProperty.UnsetValue) && (values[1] != DependencyProperty.UnsetValue) &&
                (values[2] != DependencyProperty.UnsetValue))
            {
                double duration = (double)((long)(values[0]));
                double totalDuration = (double)(values[1]);
                if((totalDuration == 0.0) || (duration == 0.0))
                {
                    result = 1.0;
                }
                else
                {
                    double totalHeight = (double)(values[2]);
                    result = duration / totalDuration * totalHeight;
                    if (result < 1.0)
                    {
                        result = 1.0;
                    }
                }
            }
            else
            {
                result = 1.0;
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
