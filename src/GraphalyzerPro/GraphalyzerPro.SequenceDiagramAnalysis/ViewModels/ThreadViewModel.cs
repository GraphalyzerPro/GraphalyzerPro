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
        private long _duration;
        private long _totalDurationWithoutExtraGap;

        public ThreadViewModel(IDiagnoseOutputEntry diagnoseOutputEntry, long totalDuration)
        {
            DiagnoseOutputViewModels = new ReactiveCollection<IDiagnoseOutputViewModel>();
            ThreadNumber = diagnoseOutputEntry.ThreadNumber;
            TotalDuration = totalDuration;
            _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel = 0;
            _totalDurationWithoutExtraGap = 0;

            ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry);
        }

        public int ThreadNumber { get; private set; }

        public long TotalDurationWithoutExtraGap
        {
            get { return _totalDurationWithoutExtraGap; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

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
                output = new DiagnoseOutputViewModel(entry, TotalDuration - TotalDurationWithoutExtraGap);
                DiagnoseOutputViewModels.Add(output);
            }
            else
            {
                output.ProcessNewDiagnoseOutputEntry(entry, TotalDuration - TotalDurationWithoutExtraGap);
            }

            if(output.IsBracketOpen)
            {
                TotalDurationWithoutExtraGap = TotalDuration = _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel + output.GapExtraGapAndTotalDuration;
            }
            else
            {
                _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel += output.GapExtraGapAndTotalDuration;
                TotalDurationWithoutExtraGap = TotalDuration = _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel;
            }
        }

        public void UpdateTotalDuration(long totalDuration)
        {
            if (totalDuration != TotalDuration)
            {
                var output = DiagnoseOutputViewModels.SingleOrDefault(x => x.IsBracketOpen);
                if (output != null)
                {
                    output.AddExtraGapToTotalDuration(totalDuration - TotalDuration);
                }
                TotalDuration = totalDuration;
            }
        }
    }
}