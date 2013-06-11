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
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    internal class ThreadViewModel : ReactiveObject, IThreadViewModel
    {
        private readonly ReactiveCollection<IDiagnoseOutputViewModel> _diagnoseOutputViewModels;
        private long _totalDuration;
        private long _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel;

        public ThreadViewModel(IDiagnoseOutputEntry diagnoseOutputEntry)
        {
            DiagnoseOutputViewModels = new ReactiveCollection<IDiagnoseOutputViewModel>();
            ThreadNumber = diagnoseOutputEntry.ThreadNumber;
            TotalDuration = 0;
            _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel = 0;

            ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry);
        }

        public int ThreadNumber { get; private set; }

        public long TotalDuration
        {
            get { return _totalDuration; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public ReactiveCollection<IDiagnoseOutputViewModel> DiagnoseOutputViewModels
        {
            get { return _diagnoseOutputViewModels; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry entry)
        {
            var output = DiagnoseOutputViewModels.SingleOrDefault(x => x.IsBracketOpen);

            if (output == null)
            {
                output = new DiagnoseOutputViewModel(entry);
                DiagnoseOutputViewModels.Add(output);
            }
            else
            {
                output.ProcessNewDiagnoseOutputEntry(entry);
            }
            if(output.IsBracketOpen)
            {
                TotalDuration = _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel + output.GapAndTotalDuration;
            }
            else
            {
                _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel += output.GapAndTotalDuration;
                TotalDuration = _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel;
            }
        }
    }
}