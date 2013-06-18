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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GraphalyzerPro.SequenceDiagramAnalysis.ViewModels;

namespace GraphalyzerPro.SequenceDiagramAnalysis
{
    /// <summary>
    ///     Interaction logic for SequenceDiagram.xaml
    /// </summary>
    public partial class SequenceDiagram : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof (ISequenceDiagramViewModel),
            typeof (SequenceDiagram),
            new PropertyMetadata(default(ISequenceDiagramViewModel))
            );

        public SequenceDiagram()
        {
            InitializeComponent();

            ViewModel = new SequenceDiagramViewModel();
        }

        public ISequenceDiagramViewModel ViewModel
        {
            get { return (ISequenceDiagramViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            RefreshCrossairPosition();
        }

        private void ScrollViewerOnMouseEnter(object sender, MouseEventArgs e)
        {
            CrosshairTopLine.Visibility =
                CrosshairBottomLine.Visibility =
                CrosshairRightLine.Visibility = CrosshairLeftLine.Visibility = Visibility.Visible;
        }

        private void ScrollViewerOnMouseLeave(object sender, MouseEventArgs e)
        {
            CrosshairTopLine.Visibility =
                CrosshairBottomLine.Visibility =
                CrosshairRightLine.Visibility = CrosshairLeftLine.Visibility = Visibility.Collapsed;
        }

        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            RefreshCrossairPosition();
        }

        private void RefreshCrossairPosition()
        {
            var position = Mouse.GetPosition(MainGrid);

            CrosshairTopLine.X1 = CrosshairTopLine.X2 = position.X;
            CrosshairTopLine.Y1 = position.Y - 15;

            CrosshairBottomLine.X1 = CrosshairBottomLine.X2 = position.X;
            CrosshairBottomLine.Y1 = position.Y + 15;
            CrosshairBottomLine.Y2 = MainGrid.ActualHeight;

            CrosshairRightLine.Y1 = CrosshairRightLine.Y2 = position.Y;
            CrosshairRightLine.X1 = position.X - 15;

            CrosshairLeftLine.Y1 = CrosshairLeftLine.Y2 = position.Y;
            CrosshairLeftLine.X1 = position.X + 15;
            CrosshairLeftLine.X2 = MainGrid.ActualWidth;
        }
    }
}