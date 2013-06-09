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
    internal class ThreadViewModel : ReactiveObject, IThreadViewModel
    {
        private readonly ReactiveCollection<IDiagnoseOutputViewModel> _diagnoseOutputEntries;

        public ThreadViewModel(IDiagnoseOutputEntry diagnoseOutputEntry)
        {
            _diagnoseOutputEntries = new ReactiveCollection<IDiagnoseOutputViewModel>();

            ThreadNumber = diagnoseOutputEntry.ThreadNumber;

            ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry);
        }

        public int ThreadNumber { get; private set; }

        public ReactiveCollection<IDiagnoseOutputViewModel> DiagnoseOutputEntries
        {
            get { return _diagnoseOutputEntries; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry entry)
        {
            var output = DiagnoseOutputEntries.SingleOrDefault(x => x.IsBracketOpen);

            if (output != null)
            {
                output.ProcessNewDiagnoseOutputEntry(entry);
            }
            else
            {
                DiagnoseOutputEntries.Add(new DiagnoseOutputViewModel(entry));
            }
        }
    }
}