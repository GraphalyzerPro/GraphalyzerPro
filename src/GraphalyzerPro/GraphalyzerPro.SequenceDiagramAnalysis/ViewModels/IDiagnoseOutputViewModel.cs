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
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.ViewModels
{
    public interface IDiagnoseOutputViewModel
    {
        /// <summary>
        /// Represents the time stamp of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// Time between the previous <see cref="IDiagnoseOutputEntry"/> and the actual <see cref="IDiagnoseOutputEntry"/> in microseconds.
        /// </summary>
        long Gap { get; }

        /// <summary>
        /// Duration of the <see cref="IDiagnoseOutputEntry"/> in microseconds.
        /// </summary>
        long Duration { get; }

        /// <summary>
        /// Total duration of the <see cref="IDiagnoseOutputEntry"/> in microseconds. If it is a subelements containing <see cref="IDiagnoseOutputEntry"/>, the total duration comprises the gap and total duration of all subelements.
        /// </summary>
        long TotalDuration { get; }

        /// <summary>
        /// The process id to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        int ProcessId { get; }

        /// <summary>
        /// The thread number to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        int ThreadNumber { get; }

        /// <summary>
        /// The type of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
        DiagnoseType Type { get; }

        /// <summary>
        /// The description of the domain to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        string Domain { get; }

        /// <summary>
        /// The description of the application to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        string Application { get; }

        /// <summary>
        /// The description of the component to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        string Component { get; }

        /// <summary>
        /// The description of the module to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        string Module { get; }

        /// <summary>
        /// The description of the code to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
        string Code { get; }

        /// <summary>
        /// The description of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Optional information.
        /// </summary>
        string MetaInformation { get; }

        ReactiveCollection<IDiagnoseOutputViewModel> DiagnoseOutputViewModels { get; }

        void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry entry, long extraGap);

        bool IsBracketOpen { get; }

        long GapExtraGapAndDuration { get; }

        long GapExtraGapAndTotalDuration { get; }

        long ExtraGap { get; }
    }
}