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
        private readonly ReactiveCollection<IDiagnoseOutputEntry> _diagnoseOutputEntries;
        private readonly ReactiveDerivedCollection<IProcessViewModel> _processes;

        public SequenceDiagramViewModel()
        {
            _diagnoseOutputEntries = new ReactiveCollection<IDiagnoseOutputEntry>();

            _processes =
                DiagnoseOutputEntries.CreateDerivedCollection(
                    x => new ProcessViewModel(x.ProcessId) as IProcessViewModel, entry =>
                        {
                            if (Processes.Any(x => x.Id == entry.ProcessId))
                            {
                                Processes.Single(x => x.Id == entry.ProcessId).ProcessNewDiagnoseOutputEntry(entry);
                                return false;
                            }
                            return true;
                        });
        }

        public ReactiveDerivedCollection<IProcessViewModel> Processes
        {
            get { return _processes; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public ReactiveCollection<IDiagnoseOutputEntry> DiagnoseOutputEntries
        {
            get { return _diagnoseOutputEntries; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry diagnoseOutputEntry)
        {
            DiagnoseOutputEntries.Add(diagnoseOutputEntry);
        }
    }
}