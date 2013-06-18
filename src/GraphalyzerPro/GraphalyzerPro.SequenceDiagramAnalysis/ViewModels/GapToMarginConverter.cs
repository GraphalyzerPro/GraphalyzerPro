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
    internal class GapToMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness result;
            double marginLeft;
            if (parameter != null)
            {
                marginLeft = double.Parse(parameter.ToString());
            }
            else
            {
                marginLeft = 0.0;
            }
            if ((values[0] != DependencyProperty.UnsetValue) && (values[1] != DependencyProperty.UnsetValue) &&
                (values[2] != DependencyProperty.UnsetValue))
            {
                double totalDuration = (double) ((long)(values[1]));
                if(totalDuration == 0.0)
                {
                    result = new Thickness(marginLeft, 0.0, 0.0, 0.0);
                }
                else
                {
                    double gap = (double)((long)(values[0]));
                    double totalHeight = (double)(values[2]);
                    result = new Thickness(marginLeft, gap/totalDuration*totalHeight, 0.0, 0.0);
                }
            }
            else
            {
                result = new Thickness(marginLeft,0.0,0.0,0.0);
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
