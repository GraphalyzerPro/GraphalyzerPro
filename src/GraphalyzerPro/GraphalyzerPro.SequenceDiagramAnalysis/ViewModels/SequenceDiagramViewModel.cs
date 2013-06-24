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
using System.Linq;
using System.Reactive.Linq;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    internal class SequenceDiagramViewModel : ReactiveObject, ISequenceDiagramViewModel
    {
        private readonly ReactiveCollection<IProcessViewModel> _processes;
        private readonly ReactiveCollection<IProcessViewModel> _shownProcesses;
        private IDiagnoseOutputViewModel _selectedDiagnoseOutputViewModel;
        private long _totalDuration;

        public SequenceDiagramViewModel()
        {
            Processes = new ReactiveCollection<IProcessViewModel>();
            ShownProcesses = new ReactiveCollection<IProcessViewModel>();
            TotalDuration = 0;
            ProcessIsShownChangedCommand = new ReactiveCommand();
            ProcessIsShownChangedCommand.Subscribe(x => ProcessIsShownChanged((IProcessViewModel)(x)));
            SelectDiagnoseOutputViewModel = new ReactiveCommand();
            SelectDiagnoseOutputViewModel.Throttle(TimeSpan.FromSeconds(1))
                                         .Subscribe(x => SelectedDiagnoseOutputViewModel = (IDiagnoseOutputViewModel) x);
        }

        public ReactiveCollection<IProcessViewModel> Processes
        {
            get { return _processes; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public long TotalDuration
        {
            get { return _totalDuration; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry diagnoseOutputEntry)
        {
            var process = Processes.SingleOrDefault(x => x.ProcessId == diagnoseOutputEntry.ProcessId);

            if (process == null)
            {
                process = new ProcessViewModel(diagnoseOutputEntry, TotalDuration);
                Processes.Add(process);
                ProcessIsShownChanged(process);
            }
            else
            {
                process.ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry);
            }

            TotalDuration = process.TotalDuration;
            foreach (ProcessViewModel p in Processes)
            {
                p.UpdateTotalDuration(TotalDuration);
            }
        }

        public ReactiveCommand SelectDiagnoseOutputViewModel { get; private set; }

        public IDiagnoseOutputViewModel SelectedDiagnoseOutputViewModel
        {
            get { return _selectedDiagnoseOutputViewModel; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public IReactiveCommand ProcessIsShownChangedCommand { get; private set; }

        public ReactiveCollection<IProcessViewModel> ShownProcesses
        {
            get { return _shownProcesses; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessIsShownChanged(IProcessViewModel pvm)
        {
            if(ShownProcesses.Contains(pvm))
            {
                ShownProcesses.Remove(pvm);
            }
            else
            {
                int processesIndex = Processes.IndexOf(pvm);
                bool found;
                while((!(found = ShownProcesses.Contains(Processes[processesIndex]))) && (processesIndex > 0))
                {
                    processesIndex--;
                }
                int shownProcessesIndex;
                if(found)
                {
                    shownProcessesIndex = ShownProcesses.IndexOf(Processes[processesIndex]) + 1;
                }
                else
                {
                    shownProcessesIndex = 0;
                }
                if(shownProcessesIndex == ShownProcesses.Count)
                {
                    ShownProcesses.Add(pvm);
                }
                else
                {
                    ShownProcesses.Insert(shownProcessesIndex, pvm);
                }
            }
        }
    }
}