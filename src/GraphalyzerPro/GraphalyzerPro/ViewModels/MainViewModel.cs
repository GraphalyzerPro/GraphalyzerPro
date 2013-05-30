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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Models;
using GraphalyzerPro.Views;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace GraphalyzerPro.ViewModels
{
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly InformationEngine _informationEngine;
        private readonly ReactiveDerivedCollection<ISessionViewModel> _sessionViewModels;
        private readonly Action _showCloseSessionErrorMessage;
        private readonly Action _showInitializationErrorMessage;
        private ISessionViewModel _selectedSessionViewModel;

        public MainViewModel()
        {
            _informationEngine = new InformationEngine();
            _sessionViewModels =
                _informationEngine.Sessions.CreateDerivedCollection(x => new SessionViewModel(x) as ISessionViewModel);
            ((IEditableCollectionView)(CollectionViewSource.GetDefaultView(_sessionViewModels))).NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;

            _showInitializationErrorMessage = () => MessageBox.Show(
                "Bei der Initialisierung des Receivers ist ein Fehler aufgetreten.\n" +
                "Bitte schließen Sie die Session und starten den Receiver neu.",
                "Fehler");

            _showCloseSessionErrorMessage = () => MessageBox.Show(
                "Beim Schließen der Session trat ein Fehler auf.",
                "Fehler");

            StartNewSessionCommand = new ReactiveCommand();
            StartNewSessionCommand.Subscribe(_ => StartNewSession());

            InitializeReceiverCommand = new ReactiveAsyncCommand();
            InitializeReceiverCommand.RegisterAsyncAction(x => InitializeReceiver((Tuple<Guid, IReceiver>) x));
            InitializeReceiverCommand.ThrownExceptions.Subscribe(
                ex => _showInitializationErrorMessage());

            CloseSessionCommand = new ReactiveCommand();
            CloseSessionCommand.Subscribe(x => CloseSession((ISessionViewModel) x));
            CloseSessionCommand.ThrownExceptions.Subscribe(ex => _showCloseSessionErrorMessage());
        }

        public string Title
        {
            get { return "GraphalyzerPro"; }
        }

        public IReactiveCommand StartNewSessionCommand { get; private set; }

        public IReactiveCommand CloseSessionCommand { get; private set; }

        public IReactiveAsyncCommand InitializeReceiverCommand { get; private set; }

        public ReactiveDerivedCollection<ISessionViewModel> SessionViewModels
        {
            get { return _sessionViewModels; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        public ISessionViewModel SelectedSessionViewModel
        {
            get { return _selectedSessionViewModel; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        private void StartNewSession()
        {
            var dialog = new NewSessionDialog();

            if (dialog.ShowDialog() == true)
            {
                var newSessionId = _informationEngine.AddSession(dialog.ViewModel.SelectedReceiver,
                                                                 dialog.ViewModel.SelectedAnalysis);

                SelectedSessionViewModel = SessionViewModels.Single(x => x.SessionId == newSessionId);

                InitializeReceiverCommand.Execute(new Tuple<Guid, IReceiver>(SelectedSessionViewModel.SessionId,
                                                                             SelectedSessionViewModel.Receiver));
            }
        }

        private void InitializeReceiver(Tuple<Guid, IReceiver> sessionIdAndReceiver)
        {
            sessionIdAndReceiver.Item2.Initialize(_informationEngine, sessionIdAndReceiver.Item1);
        }

        private void CloseSession(ISessionViewModel sessionViewModel)
        {
            sessionViewModel.Receiver.Deactivate();
            sessionViewModel.Analysis.Deactivate();
            _informationEngine.DeleteSession(sessionViewModel.SessionId);
        }
    }
}