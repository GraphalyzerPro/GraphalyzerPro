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

        public static readonly DependencyProperty DurationPerActualHeightProperty = DependencyProperty.Register(
            "DurationPerActualHeight",
            typeof (double),
            typeof (SequenceDiagram),
            new PropertyMetadata(5000000.0)
            );

        private bool _isControlPressed;
        private Point _position;
        private MultiBinding _orientationLineXBinding;
        private MultiBinding _orientationLineYBinding;

        public SequenceDiagram()
        {
            InitializeComponent();

            ViewModel = new SequenceDiagramViewModel();
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
            Binding diagnoseOutputItemsControlActualHeightBinding = new Binding();
            diagnoseOutputItemsControlActualHeightBinding.ElementName = "SequenceDiagramUserControl";
            diagnoseOutputItemsControlActualHeightBinding.Path = new PropertyPath("ActualHeight");
            _orientationLineYBinding.Bindings.Add(diagnoseOutputItemsControlActualHeightBinding);
            Binding lineYHeightPropertionBinding = new Binding();
            lineYHeightPropertionBinding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
            lineYHeightPropertionBinding.Path = new PropertyPath("Tag");
            _orientationLineYBinding.Bindings.Add(lineYHeightPropertionBinding);
            _orientationLineYBinding.Converter = new YPropertionToHeightConverter();
        }

        public ISequenceDiagramViewModel ViewModel
        {
            get { return (ISequenceDiagramViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public double DurationPerActualHeight
        {
            get { return (double) GetValue(DurationPerActualHeightProperty); }
            set { SetValue(DurationPerActualHeightProperty, value); }
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
                OrientationLineItemsControl.ContextMenu = null;
            }
        }

        private void UserControlOnKeyUp(object sender, KeyEventArgs e)
        {
            if((e.KeyboardDevice.IsKeyUp(Key.LeftCtrl)) && (e.KeyboardDevice.IsKeyUp(Key.RightCtrl)))
            {
                _isControlPressed = false;
                OrientationLineItemsControl.ContextMenu = OrientationLineItemsControlContextMenu;
            }
        }

        private void OrientationLineItemsControlOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _position = e.GetPosition((IInputElement)(sender));
            if((_isControlPressed) &&
                ((!OrientationLineItemsControlContextMenu.IsVisible) ||
                 (!OrientationLineItemsControlContextMenu.IsMouseOver)))
            {
                CreateOrientationLine();
            }
        }

        private void OrientationLineItemsControlOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!_isControlPressed)
            {
                _position = e.GetPosition((IInputElement)(sender));
                OrientationLineItemsControlDeleteContextMenu.IsEnabled = (MainGrid.InputHitTest(_position) is Line);
            }
            else if((!OrientationLineItemsControlContextMenu.IsVisible) ||
                     (!OrientationLineItemsControlContextMenu.IsMouseOver))
            {
                _position = e.GetPosition((IInputElement)(sender));
                DeleteOrientationLine();
            }
        }

        private void CreateOrientationLine()
        {
            Line l = new Line();
            l.Tag = _position.Y / SequenceDiagramUserControl.ActualHeight;
            l.SetBinding(Line.Y1Property, _orientationLineYBinding);
            l.SetBinding(Line.Y2Property, _orientationLineYBinding);
            l.X1 = 0.0;
            l.SetBinding(Line.X2Property, _orientationLineXBinding);
            l.HorizontalAlignment = HorizontalAlignment.Stretch;
            l.VerticalAlignment = VerticalAlignment.Top;
            l.Stroke = Brushes.Blue;
            l.StrokeThickness = 1.0;
            OrientationLineItemsControl.Items.Add(l);
        }

        private void DeleteOrientationLine()
        {
            OrientationLineItemsControl.Items.Remove(MainGrid.InputHitTest(_position));
        }

        private void MainGridContextMenuOnCreateOrientationLine(object sender, RoutedEventArgs e)
        {
            CreateOrientationLine();
        }

        private void MainGridContextMenuOnDeleteOrientationLine(object sender, RoutedEventArgs e)
        {
            DeleteOrientationLine();
        }

        private void MainGridContextMenuOnDeleteAllOrientationLines(object sender, RoutedEventArgs e)
        {
            OrientationLineItemsControl.Items.Clear();
        }

        private void AutoScrollContextMenuOnChecked(object sender, RoutedEventArgs e)
        {
            ScrollViewerBehavior.Attach(ScrollViewer);
        }

        private void AutoScrollContextMenuOnUnchecked(object sender, RoutedEventArgs e)
        {
            ScrollViewerBehavior.Detach();
        }
    }
}