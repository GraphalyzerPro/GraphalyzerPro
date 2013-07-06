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
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Configuration;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace GraphalyzerPro.ViewModels
{
    public class NewSessionDialogViewModel : ReactiveObject, INewSessionDialogViewModel
    {
        private ReactiveCollection<IAnalysis> _allAnalyses;
        private ReactiveCollection<IReceiver> _allReceiver;
        private IAnalysis _selectedAnalysis;
        private IReceiver _selectedReceiver;
        private bool _isReceiverSelectionEnabled;
        private string _title;

        public NewSessionDialogViewModel()
        {
            IsReceiverSelectionEnabled = true;
            Title = "Neu Session erstellen";

            ApplyCommand =
                new ReactiveCommand(
                    this.ObservableForProperty(x => x.SelectedReceiver, x => x != null)
                        .StartWith(false)
                        .CombineLatest(
                            this.ObservableForProperty(x => x.SelectedAnalysis, x => x != null).StartWith(false),
                            (a, b) => a && b));

            _allReceiver = new ReactiveCollection<IReceiver>();
            _allAnalyses = new ReactiveCollection<IAnalysis>();

            CreateInstanceOfInterfaceImplementations<IReceiver>(
                ConfigurationAccessor.GetAllAssemblyFileNamesBySectionName(ConfigurationAccessor.ReceiversSectionName));

            CreateInstanceOfInterfaceImplementations<IAnalysis>(
                ConfigurationAccessor.GetAllAssemblyFileNamesBySectionName(ConfigurationAccessor.AnalysesSectionName));
        }

        public IReactiveCommand ApplyCommand { get; private set; }

        public ReactiveCollection<IReceiver> AllReceiver
        {
            get { return _allReceiver; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public ReactiveCollection<IAnalysis> AllAnalyses
        {
            get { return _allAnalyses; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public IReceiver SelectedReceiver
        {
            get { return _selectedReceiver; }
            set { this.RaiseAndSetIfChanged(AllReceiver.First(x => x.Name == value.Name)); }
        }

        public IAnalysis SelectedAnalysis
        {
            get { return _selectedAnalysis; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public bool IsReceiverSelectionEnabled
        {
            get { return _isReceiverSelectionEnabled; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        private void CreateInstanceOfInterfaceImplementations<T>(IEnumerable<string> allAnalysisAssemblyFileNames)
            where T : class
        {
            foreach (var analysisAssembly in allAnalysisAssemblyFileNames)
            {
                var assembly = Assembly.LoadFrom(analysisAssembly);

                var allIAnalysisTypesOfTheAssembly =
                    assembly.GetTypes().Where(x => typeof (T).IsAssignableFrom(x)).ToList();

                foreach (var type in allIAnalysisTypesOfTheAssembly)
                {
                    if (typeof (T) == typeof (IAnalysis))
                    {
                        AllAnalyses.Add(Activator.CreateInstance(type) as T);
                    }
                    else if (typeof (T) == typeof (IReceiver))
                    {
                        AllReceiver.Add(Activator.CreateInstance(type) as T);
                    }
                }
            }
        }
    }
}