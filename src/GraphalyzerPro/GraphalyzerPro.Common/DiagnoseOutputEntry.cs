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
    public class DiagnoseOutputEntry : IDiagnoseOutputEntry
    {
        public DiagnoseOutputEntry(DateTime timeStamp, long gap, long duration, int processId, int threadNumber,
                                   char type, string domain, string application, string component, string module,
                                   string code, string text, string metaInformation)
        {
            TimeStamp = timeStamp;
            Gap = gap;
            Duration = duration;
            ProcessId = processId;
            ThreadNumber = threadNumber;
            Type = type;
            Domain = domain;
            Application = application;
            Component = component;
            Module = module;
            Code = code;
            Text = text;
            MetaInformation = metaInformation;
        }

        public DateTime TimeStamp { get; private set; }
        public long Gap { get; private set; }
        public long Duration { get; private set; }
        public int ProcessId { get; private set; }
        public int ThreadNumber { get; private set; }
        public char Type { get; private set; }
        public string Domain { get; private set; }
        public string Application { get; private set; }
        public string Component { get; private set; }
        public string Module { get; private set; }
        public string Code { get; private set; }
        public string Text { get; private set; }
        public string MetaInformation { get; private set; }
    }
}