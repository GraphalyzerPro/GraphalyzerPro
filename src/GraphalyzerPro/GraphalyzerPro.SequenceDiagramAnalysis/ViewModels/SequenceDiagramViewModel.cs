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

using System.Linq;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    internal class SequenceDiagramViewModel : ReactiveObject, ISequenceDiagramViewModel
    {
        private readonly ReactiveCollection<IProcessViewModel> _processes;
        private long _totalDuration;

        public SequenceDiagramViewModel()
        {
            Processes = new ReactiveCollection<IProcessViewModel>();
            TotalDuration = 0;
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
                process = new ProcessViewModel(diagnoseOutputEntry);
                Processes.Add(process);
            }
            else
            {
                process.ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry);
            }

            if (process.TotalDuration > TotalDuration)
            {
                TotalDuration = process.TotalDuration;
            }
        }
    }
}