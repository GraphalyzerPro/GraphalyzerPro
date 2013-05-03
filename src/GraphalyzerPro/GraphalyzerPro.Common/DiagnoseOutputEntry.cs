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
		private DateTime _timeStamp;
		private long _gap;
		private long _duration;
		private int _processId;
		private int _threadNumber;
		private char _type;
		private string _domain;
		private string _application;
		private string _component;
		private string _module;
		private string _code;
		private string _text;
		private string _metaInformation;

		/// <summary>
		/// Creates a standard DiagnoseOutputEntry
		/// </summary>
		/// <param name="timeStamp">The timestamp</param>
		/// <param name="gap">The gap</param>
		/// <param name="duration">The duration</param>
		/// <param name="processId">The process id</param>
		/// <param name="threadNumber">The thread number</param>
		/// <param name="type">The type</param>
		/// <param name="domain">The domain</param>
		/// <param name="application">The application</param>
		/// <param name="component">The component</param>
		/// <param name="module">The module</param>
		/// <param name="code">The code</param>
		/// <param name="text">The text</param>
		/// <param name="metaInformation">The meta information</param>
		public DiagnoseOutputEntry(DateTime timeStamp, long gap, long duration, int processId, int threadNumber, char type, string domain, string application, string component, string module, string code, string text, string metaInformation)
		{
			if((type=='(')||(type=='E')||(type=='·')||(type==')'))
			{
				_timeStamp=timeStamp;
				_gap=gap;
				_duration=duration;
				_processId=processId;
				_threadNumber=threadNumber;
				_type=type;
				_domain=domain;
				_application=application;
				_component=component;
				_module=module;
				_code=code;
				_text=text;
				_metaInformation=metaInformation;
			}
			else
			{
				throw new FormatException();
			}
		}

        /// <summary>
        /// Represents the time stamp of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
		public DateTime TimeStamp
		{
			get
			{
				return _timeStamp;
			}
		}

        /// <summary>
        /// Time between the previous <see cref="IDiagnoseOutputEntry"/> and the actual <see cref="IDiagnoseOutputEntry"/> in microseconds.
        /// </summary>
		public long Gap
		{
			get
			{
				return _gap;
			}
		}

        /// <summary>
        /// Duration of the bracket in microseconds.
        /// </summary>
		public long Duration
		{
			get
			{
				return _duration;
			}
		}

        /// <summary>
        /// The process id to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public int ProcessId
		{
			get
			{
				return _processId;
			}
		}

        /// <summary>
        /// The thread number to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public int ThreadNumber
		{
			get
			{
				return _threadNumber;
			}
		}

        /// <summary>
        /// The type of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
		public char Type
		{
			get
			{
				return _type;
			}
		}

        /// <summary>
        /// The description of the domain to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Domain
		{
			get
			{
				return _domain;
			}
		}

        /// <summary>
        /// The description of the application to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Application
		{
			get
			{
				return _application;
			}
		}

        /// <summary>
        /// The description of the component to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Component
		{
			get
			{
				return _component;
			}
		}
        
        /// <summary>
        /// The description of the module to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Module
		{
			get
			{
				return _module;
			}
		}

        /// <summary>
        /// The description of the code to which the <see cref="IDiagnoseOutputEntry"/> belongs to.
        /// </summary>
		public string Code
		{
			get
			{
				return _code;
			}
		}
        
        /// <summary>
        /// The description of the <see cref="IDiagnoseOutputEntry"/>.
        /// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
		}

        /// <summary>
        /// Optional information.
        /// </summary>
		public string MetaInformation
		{
			get
			{
				return _metaInformation;
			}
		}
    }
}
