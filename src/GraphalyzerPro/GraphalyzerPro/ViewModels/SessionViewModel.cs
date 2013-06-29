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
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Models;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace GraphalyzerPro.ViewModels
{
    public class SessionViewModel : ReactiveObject, ISessionViewModel
    {
        public SessionViewModel(ISession session)
        {
            SessionId = session.Id;
            Receiver = session.Receiver;
            Analysis = session.Analysis;

            CloseAnalysisCommand = new ReactiveCommand();
            CloseAnalysisCommand.Subscribe(x => Analysis.Remove(x));
        }

        public IReceiver Receiver { get; private set; }

        public ReactiveCollection<IAnalysis> Analysis { get; private set; }

        public ReactiveCommand CloseAnalysisCommand { get; private set; }

        public Guid SessionId { get; private set; }
    }
}