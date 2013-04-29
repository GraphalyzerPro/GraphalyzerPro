/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Carl-Clemens Ebinger, Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
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

namespace GraphalyzerPro.Common.Interfaces
{
    /// <summary>
    ///  Provides a common interface for all diagnose output entries.
    /// </summary>
    public interface IDiagnoseOutputEntry
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
        /// Duration of the bracket in microseconds.
        /// </summary>
        long Duration { get; }

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
        char Type { get; }

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
    }
}
