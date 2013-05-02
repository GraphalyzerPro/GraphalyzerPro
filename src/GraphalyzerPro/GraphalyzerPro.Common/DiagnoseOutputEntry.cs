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
using GraphalyzerPro.Common.Interfaces;

namespace GraphalyzerPro.Common
{
    /// <summary>
    ///  Provides a standard implementation for all diagnose output entries of the <see cref="IDiagnoseOutputEntry"/> interface.
    /// </summary>
    public class DiagnoseOutputEntry : IDiagnoseOutputEntry
    {
		private DateTime m_timeStamp;
		private long m_gap;
		private long m_duration;
		private int m_processId;
		private int m_threadNumber;
		private char m_type;
		private string m_domain;
		private string m_application;
		private string m_component;
		private string m_module;
		private string m_code;
		private string m_text;
		private string m_metaInformation;

		/// <summary>
		/// Creates a standard DiagnoseOutputEntry
		/// </summary>
		/// <param name="timeStamp"></param>
		/// <param name="gap"></param>
		/// <param name="duration"></param>
		/// <param name="processId"></param>
		/// <param name="threadNumber"></param>
		/// <param name="type"></param>
		/// <param name="domain"></param>
		/// <param name="application"></param>
		/// <param name="component"></param>
		/// <param name="module"></param>
		/// <param name="code"></param>
		/// <param name="text"></param>
		/// <param name="metaInformation"></param>
		public DiagnoseOutputEntry(DateTime timeStamp, long gap, long duration, int processId, int threadNumber, char type, string domain, string application, string component, string module, string code, string text, string metaInformation)
		{
			m_timeStamp=timeStamp;
			m_gap=gap;
			m_duration=duration;
			m_processId=processId;
			m_threadNumber=threadNumber;
			m_type=type;
			m_domain=domain;
			m_application=application;
			m_component=component;
			m_module=module;
			m_code=code;
			m_text=text;
			m_metaInformation=metaInformation;
		}

        /// <summary>
        /// Represents the time stamp of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
		public DateTime TimeStamp
		{
			get
			{
				return m_timeStamp;
			}
		}

        /// <summary>
        /// Time between the previous <see cref="IDiagnoseOutputEntry"/> and the actual <see cref="IDiagnoseOutputEntry"/> in microseconds.
        /// </summary>
		public long Gap
		{
			get
			{
				return m_gap;
			}
		}

        /// <summary>
        /// Duration of the bracket in microseconds.
        /// </summary>
		public long Duration
		{
			get
			{
				return m_duration;
			}
		}

        /// <summary>
        /// The process id to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public int ProcessId
		{
			get
			{
				return m_processId;
			}
		}

        /// <summary>
        /// The thread number to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public int ThreadNumber
		{
			get
			{
				return m_threadNumber;
			}
		}

        /// <summary>
        /// The type of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
		public char Type
		{
			get
			{
				return m_type;
			}
		}

        /// <summary>
        /// The description of the domain to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Domain
		{
			get
			{
				return m_domain;
			}
		}

        /// <summary>
        /// The description of the application to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Application
		{
			get
			{
				return m_application;
			}
		}

        /// <summary>
        /// The description of the component to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Component
		{
			get
			{
				return m_component;
			}
		}
        
        /// <summary>
        /// The description of the module to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Module
		{
			get
			{
				return m_module;
			}
		}

        /// <summary>
        /// The description of the code to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Code
		{
			get
			{
				return m_code;
			}
		}
        
        /// <summary>
        /// The description of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
		public string Text
		{
			get
			{
				return m_text;
			}
		}

        /// <summary>
        /// Optional information.
        /// </summary>
		public string MetaInformation
		{
			get
			{
				return m_metaInformation;
			}
		}
    }
}
