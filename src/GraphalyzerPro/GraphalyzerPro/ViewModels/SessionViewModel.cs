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
using System.Reflection;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Configuration;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace GraphalyzerPro.ViewModels
{
    public class SessionViewModel : ReactiveObject, ISessionViewModel
    {
        private IAnalysis _activatedAnalysis;
        private ReactiveCollection<IAnalysis> _allAnalyses;
        private IAnalysis _selectedAnalysis;

        public SessionViewModel(IReceiver receiver)
        {
            SessionId = Guid.NewGuid();
            Receiver = receiver;
            AllAnalyses = new ReactiveCollection<IAnalysis>();
            CreateInstanceOfAnalysisImplementations(
                ConfigurationAccessor.GetAllAssemblyFileNamesBySectionName(ConfigurationAccessor.AnalysesSectionName));

            SelectAnalysisCommand = new ReactiveCommand();
            SelectAnalysisCommand.Subscribe(_ => SelectAnalysis());
        }

        public IAnalysis ActivatedAnalysis
        {
            get { return _activatedAnalysis; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public ReactiveCollection<IAnalysis> AllAnalyses
        {
            get { return _allAnalyses; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public IAnalysis SelectedAnalysis
        {
            get { return _selectedAnalysis; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public IReceiver Receiver { get; private set; }

        public Guid SessionId { get; private set; }

        public IReactiveCommand SelectAnalysisCommand { get; private set; }

        private void CreateInstanceOfAnalysisImplementations(IEnumerable<string> allAnalysisAssemblyFileNames)
        {
            foreach (var analysisAssembly in allAnalysisAssemblyFileNames)
            {
                var assembly = Assembly.LoadFrom(analysisAssembly);

                var allIAnalysisTypesOfTheAssembly =
                    assembly.GetTypes().Where(x => typeof (IAnalysis).IsAssignableFrom(x)).ToList();

                foreach (var type in allIAnalysisTypesOfTheAssembly)
                {
                    AllAnalyses.Add(Activator.CreateInstance(type) as IAnalysis);
                }
            }
        }

        private void SelectAnalysis()
        {
            if (!SelectedAnalysis.IsInitialized)
            {
                SelectedAnalysis.Initialize();
            }
            ActivatedAnalysis = SelectedAnalysis;
        }
    }
}