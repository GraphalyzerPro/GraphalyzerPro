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
using System.Windows;
using System.Windows.Threading;
using GraphalyzerPro.Common.Interfaces;
using ReactiveUI;

namespace GraphalyzerPro.Models
{
    /// <summary>
    ///     Provides a common implementation for the information engine.
    /// </summary>
    public class InformationEngine : IInformationEngine
    {
        public InformationEngine()
        {
            Sessions = new ReactiveCollection<Session>();
        }

        public ReactiveCollection<Session> Sessions { get; private set; }

        /// <summary>
        ///     Processes a new <see cref="IDiagnoseOutputEntry" />.
        /// </summary>
        /// <param name="diagnoseOutputEntry">
        ///     An <see cref="IDiagnoseOutputEntry" /> which should be processed by the <see cref="IInformationEngine" />.
        /// </param>
        /// <param name="sessionId">
        ///     The id of the <see cref="ISession" />.
        /// </param>
        public void ProcessNewDiagnoseOutputEntry(IDiagnoseOutputEntry diagnoseOutputEntry, Guid sessionId)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                Sessions.Single(x => x.Id == sessionId).DiagnoseOutputEntries.Add(diagnoseOutputEntry);
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                                           new Action(
                                                               () => Sessions.Single(x => x.Id == sessionId)
                                                                             .DiagnoseOutputEntries.Add(
                                                                                 diagnoseOutputEntry)));
            }
        }

        public void DeleteSession(Guid sessionId)
        {
            Sessions.Remove(Sessions.Single(x => x.Id == sessionId));
        }

        public Guid AddSession(IReceiver receiver, IAnalysis analysis)
        {
            var newSession = new Session(receiver, analysis);

            Sessions.Add(newSession);

            return newSession.Id;
        }
    }
}