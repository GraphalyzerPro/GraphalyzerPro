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
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphalyzerPro.SequenceDiagramAnalysis.Converter;
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

        private bool _isControlPressed;
        private Point _position;

        private MultiBinding _orientationLineXBinding;
        private MultiBinding _orientationLineYBinding;
        private List<Line> _orientationLines;
        private Line _currentOrientationLine;

        public SequenceDiagram()
        {
            InitializeComponent();

            ViewModel = new SequenceDiagramViewModel();
            _orientationLines = new List<Line>();
            _currentOrientationLine = null;
            _isControlPressed = false;
            _orientationLineXBinding = new MultiBinding();
            Binding diagnoseOutputItemsControlActualWidthBinding = new Binding();
            diagnoseOutputItemsControlActualWidthBinding.ElementName = "DiagnoseOutputItemsControl";
            diagnoseOutputItemsControlActualWidthBinding.Path = new PropertyPath("ActualWidth");
            _orientationLineXBinding.Bindings.Add(diagnoseOutputItemsControlActualWidthBinding);
            Binding scrollViewerViewportWidthBinding = new Binding();
            scrollViewerViewportWidthBinding.ElementName = "ScrollViewer";
            scrollViewerViewportWidthBinding.Path = new PropertyPath("ViewportWidth");
            _orientationLineXBinding.Bindings.Add(scrollViewerViewportWidthBinding);
            _orientationLineXBinding.Converter = new MaxConverter();
            _orientationLineYBinding = new MultiBinding();
            Binding lineDurationBinding = new Binding();
            lineDurationBinding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
            lineDurationBinding.Path = new PropertyPath("Tag");
            _orientationLineYBinding.Bindings.Add(lineDurationBinding);
            Binding lineZoomBinding = new Binding();
            lineZoomBinding.ElementName = "ZoomSlider";
            lineZoomBinding.Path = new PropertyPath("Value");
            _orientationLineYBinding.Bindings.Add(lineZoomBinding);
            Binding sequenceDiagramUserControlHeightBinding = new Binding();
            sequenceDiagramUserControlHeightBinding.ElementName = "SequenceDiagramUserControl";
            sequenceDiagramUserControlHeightBinding.Path = new PropertyPath("ActualHeight");
            _orientationLineYBinding.Bindings.Add(sequenceDiagramUserControlHeightBinding);
            _orientationLineYBinding.Converter = new DurationToHeightConverter();

            MainGrid.ContextMenu = null;
            DiagnoseOutputItemsControl.LayoutUpdated += new EventHandler(DiagnoseOutputItemsControlOnLayoutUpdated);
        }

        public ISequenceDiagramViewModel ViewModel
        {
            get { return (ISequenceDiagramViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (ScrollViewer.IsMouseOver)
            {
                RefreshCrossairPosition();
            }
        }

        private void ScrollViewerOnMouseEnter(object sender, MouseEventArgs e)
        {
            EnableCrosshair();
        }

        private void ScrollViewerOnMouseLeave(object sender, MouseEventArgs e)
        {
            DisableCrosshair();
        }

        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ScrollViewer.IsMouseOver)
            {
                RefreshCrossairPosition();
            }
        }

        private void RefreshCrossairPosition()
        {
            var position = Mouse.GetPosition(MainGrid);

            if (position.Y > MainGrid.ActualHeight - 2 || position.X > MainGrid.ActualWidth - 2)
            {
                DisableCrosshair();
            }
            else
            {
                EnableCrosshair();
                
                if (position.Y < MainGrid.ActualHeight - 15)
                {
                    CrosshairBottomLine.Visibility = Visibility.Visible;

                    CrosshairBottomLine.X1 = CrosshairBottomLine.X2 = position.X;
                    CrosshairBottomLine.Y1 = position.Y + 15;
                    CrosshairBottomLine.Y2 = MainGrid.ActualHeight - 15;
                }
                else
                {
                    CrosshairBottomLine.Visibility = Visibility.Collapsed;
                }

                if (position.X < MainGrid.ActualWidth - 15)
                {
                    CrosshairRightLine.Visibility = Visibility.Visible;

                    CrosshairRightLine.Y1 = CrosshairRightLine.Y2 = position.Y;
                    CrosshairRightLine.X1 = position.X + 15;
                    CrosshairRightLine.X2 = MainGrid.ActualWidth - 15;
                }
                else
                {
                    CrosshairRightLine.Visibility = Visibility.Collapsed;
                }

                CrosshairTopLine.X1 = CrosshairTopLine.X2 = position.X;
                CrosshairTopLine.Y1 = position.Y - 15;

                CrosshairLeftLine.Y1 = CrosshairLeftLine.Y2 = position.Y;
                CrosshairLeftLine.X1 = position.X - 15;
            }
        }

        private void EnableCrosshair()
        {
            CrosshairTopLine.Visibility =
                CrosshairBottomLine.Visibility =
                CrosshairLeftLine.Visibility = CrosshairRightLine.Visibility = Visibility.Visible;
        }

        private void DisableCrosshair()
        {
            CrosshairTopLine.Visibility =
                CrosshairBottomLine.Visibility =
                CrosshairLeftLine.Visibility = CrosshairRightLine.Visibility = Visibility.Collapsed;
        }

        private void UserControlOnKeyDown(object sender, KeyEventArgs e)
        {
            if((e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) || (e.KeyboardDevice.IsKeyDown(Key.RightCtrl)))
            {
                _isControlPressed = true;
                DiagnoseOutputItemsControl.ContextMenu = null;
            }
        }

        private void UserControlOnKeyUp(object sender, KeyEventArgs e)
        {
            if((e.KeyboardDevice.IsKeyUp(Key.LeftCtrl)) && (e.KeyboardDevice.IsKeyUp(Key.RightCtrl)))
            {
                _isControlPressed = false;
               DiagnoseOutputItemsControl.ContextMenu = OrientationLinesContextMenu;
            }
        }

        private void MainGridOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _position = e.GetPosition((IInputElement)(sender));
            if((_isControlPressed) &&
                ((!OrientationLinesContextMenu.IsVisible) ||
                 (!OrientationLinesContextMenu.IsMouseOver)))
            {
                CreateOrientationLine();
            }
        }

        private void OrientationLineOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentOrientationLine = null;
        }

        private void OrientationLineOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            OrientationLinesDeleteContextMenu.IsEnabled = true;
            _currentOrientationLine = (Line) (sender);
            if((_isControlPressed) &&
                ((!OrientationLinesContextMenu.IsVisible) ||
                 (!OrientationLinesContextMenu.IsMouseOver)))
            {
                MainGrid.Children.Remove(_currentOrientationLine);
                _orientationLines.Remove(_currentOrientationLine);
            }
        }

        private void CreateOrientationLine()
        {
            Line l = new Line();
            l.Tag = (_position.Y - ((double)(_orientationLineYBinding.ConverterParameter)))*ZoomSlider.Value/SequenceDiagramUserControl.ActualHeight;
            l.SetBinding(Line.Y1Property, _orientationLineYBinding);
            l.SetBinding(Line.Y2Property, _orientationLineYBinding);
            l.X1 = 0.0;
            l.SetBinding(Line.X2Property, _orientationLineXBinding);
            l.HorizontalAlignment = HorizontalAlignment.Stretch;
            l.VerticalAlignment = VerticalAlignment.Top;
            l.Stroke = Brushes.Blue;
            l.StrokeThickness = 1.0;
            l.ContextMenu = OrientationLinesContextMenu;
            l.MouseLeftButtonDown += new MouseButtonEventHandler(OrientationLineOnMouseLeftButtonDown);
            l.MouseRightButtonDown += new MouseButtonEventHandler(OrientationLineOnMouseRightButtonDown);
            MainGrid.Children.Add(l);
            _orientationLines.Add(l);
        }

        private Grid SearchForThreadEntriesGridInVisualTree(DependencyObject targetElement)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);
            Grid result = null;
            for(int i = 0; (i < count)&&(result==null); i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if(child is Grid)
                {
                    Grid targetItem = (Grid)(child);

                    if(targetItem.Name == "ThreadEntries")
                    {
                        result = targetItem;
                    }
                }
                else
                {
                    result=SearchForThreadEntriesGridInVisualTree(child);
                }
            }
            return result;
        }

        private void OrienationLinesContextMenuOnCreateOrientationLine(object sender, RoutedEventArgs e)
        {
            CreateOrientationLine();
        }

        private void OrientationLinesContextMenuOnDeleteOrientationLine(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Remove(_currentOrientationLine);
            _orientationLines.Remove(_currentOrientationLine);
        }

        private void OrientationLinesContextMenuOnDeleteAllOrientationLines(object sender, RoutedEventArgs e)
        {
            _orientationLines.ForEach(x => MainGrid.Children.Remove(x));
            _orientationLines.Clear();
        }

        private void MainGridOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            OrientationLinesDeleteContextMenu.IsEnabled = false;
            _position = e.GetPosition((IInputElement)(sender));
        }

        private void AutoScrollContextMenuOnChecked(object sender, RoutedEventArgs e)
        {
            ScrollViewerBehavior.Attach(ScrollViewer);
        }

        private void AutoScrollContextMenuOnUnchecked(object sender, RoutedEventArgs e)
        {
            ScrollViewerBehavior.Detach();
        }

        private void DiagnoseOutputItemsControlOnLayoutUpdated(object sender, EventArgs e)
        {
            Grid g = SearchForThreadEntriesGridInVisualTree(DiagnoseOutputItemsControl);
            if (g != null)
            {
                DiagnoseOutputItemsControl.LayoutUpdated -= new EventHandler(DiagnoseOutputItemsControlOnLayoutUpdated);
                MainGrid.ContextMenu = OrientationLinesContextMenu;
                _orientationLineYBinding.ConverterParameter = g.TranslatePoint(new Point(), DiagnoseOutputItemsControl).Y;
                MainGrid.MouseLeftButtonDown +=
                    new MouseButtonEventHandler(MainGridOnMouseLeftButtonDown);
                MainGrid.MouseRightButtonDown +=
                    new MouseButtonEventHandler(MainGridOnMouseRightButtonDown);
            }
        }
    }
}