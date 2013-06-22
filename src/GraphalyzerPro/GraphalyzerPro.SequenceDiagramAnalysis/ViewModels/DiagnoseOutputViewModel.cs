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
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    public class DiagnoseOutputViewModel : ReactiveObject, IDiagnoseOutputViewModel
    {
        private readonly ReactiveCollection<IDiagnoseOutputViewModel> _diagnoseOutputViewModels;
        private long _duration;
        private DateTime _endTimeStamp;
        private long _extraGap;
        private long _gap;
        private bool _isBracketOpen;
        private long _totalDuration;
        private long _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel;

        public DiagnoseOutputViewModel(IDiagnoseOutputEntry diagnoseOutputEntry, long extraGap)
        {
            _diagnoseOutputViewModels = new ReactiveCollection<IDiagnoseOutputViewModel>();

            StartTimeStamp = diagnoseOutputEntry.TimeStamp;
            Gap = diagnoseOutputEntry.Gap;
            Duration = diagnoseOutputEntry.Duration;
            ProcessId = diagnoseOutputEntry.ProcessId;
            ThreadNumber = diagnoseOutputEntry.ThreadNumber;
            Type = diagnoseOutputEntry.Type;
            Domain = diagnoseOutputEntry.Domain;
            Application = diagnoseOutputEntry.Application;
            Component = diagnoseOutputEntry.Component;
            Module = diagnoseOutputEntry.Module;
            Code = diagnoseOutputEntry.Code;
            Text = diagnoseOutputEntry.Text;
            MetaInformation = diagnoseOutputEntry.MetaInformation;

            ExtraGap = extraGap;
            TotalDuration = diagnoseOutputEntry.Duration;
            _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel = diagnoseOutputEntry.Duration;

            if (diagnoseOutputEntry.Type == DiagnoseType.StartBracketOutput)
            {
                IsBracketOpen = true;
            }
        }

        public long GapAndExtraGap
        {
            get { return Gap + ExtraGap; }
        }

        public DateTime StartTimeStamp { get; private set; }

        public DateTime EndTimeStamp
        {
            get { return _endTimeStamp; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public long Gap
        {
            get { return _gap; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public long ExtraGap
        {
            get { return _extraGap; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public long Duration
        {
            get { return _duration; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public long GapExtraGapAndDuration
        {
            get { return Gap + ExtraGap + Duration; }
        }

        public long GapExtraGapAndTotalDuration
        {
            get { return Gap + ExtraGap + TotalDuration; }
        }

        public long TotalDuration
        {
            get { return _totalDuration; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public int ProcessId { get; private set; }

        public int ThreadNumber { get; private set; }

        public DiagnoseType Type { get; private set; }

        public string Domain { get; private set; }

        public string Application { get; private set; }

        public string Component { get; private set; }

        public string Module { get; private set; }

        public string Code { get; private set; }

        public string Text { get; private set; }

        public string MetaInformation { get; private set; }

        public ReactiveCollection<IDiagnoseOutputViewModel> DiagnoseOutputViewModels
        {
            get { return _diagnoseOutputViewModels; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public bool IsBracketOpen
        {
            get { return _isBracketOpen; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry entry, long extraGap)
        {
            var output = DiagnoseOutputViewModels.SingleOrDefault(x => x.IsBracketOpen);

            if (output == null)
            {
                output = new DiagnoseOutputViewModel(entry, extraGap);
                if (entry.Type == DiagnoseType.EndBracketOutput)
                {
                    IsBracketOpen = false;
                    EndTimeStamp = entry.TimeStamp;
                }
                else
                {
                    DiagnoseOutputViewModels.Add(output);
                }
            }
            else
            {
                output.ProcessNewDiagnoseOutputEntry(entry, extraGap);
            }
            if (output.IsBracketOpen)
            {
                TotalDuration = _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel +
                                output.GapExtraGapAndTotalDuration;
            }
            else
            {
                _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel += output.GapExtraGapAndTotalDuration;
                TotalDuration = _totalDurationWithoutLastOpenBracketDiagnoseOutputViewModel;
            }
        }
    }
}