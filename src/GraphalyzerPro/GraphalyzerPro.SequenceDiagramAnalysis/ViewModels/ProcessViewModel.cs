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
using System.Threading;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    internal class ProcessViewModel : ReactiveObject, IProcessViewModel
    {
        private readonly ReactiveCollection<IThreadViewModel> _threads;
        private long _totalDuration;

        public ProcessViewModel(IDiagnoseOutputEntry entry)
        {
            Threads = new ReactiveCollection<IThreadViewModel>();
            ProcessId = entry.ProcessId;
            TotalDuration = 0;

            ProcessNewDiagnoseOutputEntry(entry);
        }

        public int ProcessId { get; private set; }

        public long TotalDuration
        {
            get { return _totalDuration; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public ReactiveCollection<IThreadViewModel> Threads
        {
            get { return _threads; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry diagnoseOutputEntry)
        {
            var thread = Threads.SingleOrDefault(x => x.ThreadNumber == diagnoseOutputEntry.ThreadNumber);

            if (thread == null)
            {
                thread = new ThreadViewModel(diagnoseOutputEntry);
                Threads.Add(thread);
            }
            else
            {
                thread.ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry);
            }

            if (thread.TotalDuration > TotalDuration)
            {
                TotalDuration = thread.TotalDuration;
            }
        }
    }
}