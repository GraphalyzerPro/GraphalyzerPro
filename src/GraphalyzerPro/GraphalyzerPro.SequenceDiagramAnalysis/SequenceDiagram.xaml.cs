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
    }
}