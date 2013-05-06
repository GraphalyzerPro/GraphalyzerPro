/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Carl-Clemens Ebinger, Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
 *
 * This file is part of the GraphalyzerPro rating application.
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

using System.IO;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Globalization;
using System.Windows;
using GraphalyzerPro.Properties;
using System.Reflection;
using System;
using System.Collections.Generic;
using GraphalyzerPro.Common.Interfaces;

[assembly: InternalsVisibleTo("GraphalyzerPro.Tests")]

namespace GraphalyzerPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
	{
		/// <summary>
		/// The available <see cref="IReceiver"/>-types.
		/// </summary>
		private List<Type> _receiverTypes;

		/// <summary>
		/// The constructor
		/// </summary>
		public App() : base()
		{
			_receiverTypes=new List<Type>();
		}

		/// <summary>
		/// The available <see cref="IReceiver"/>-types
		/// </summary>
		public Type[] ReceiverTypes
		{
			get
			{
				return _receiverTypes.ToArray();
			}
		}

        private static void InitializeRxBackingFieldNameConventions()
        {
            RxApp.GetFieldNameForPropertyNameFunc = delegate(string name)
            {
                var nameAsArray = name.ToCharArray();
                nameAsArray[0] = char.ToLower(nameAsArray[0], CultureInfo.InvariantCulture);
                return '_' + new string(nameAsArray);
            };
        }

		/// <summary>
		/// Initializes _receiverTypes with the receiver types
		/// </summary>
		private void InitializeReceiverTypes()
		{
			Settings settings=new Settings();
			foreach(string path in settings.ReceiverAssemblies)
			{
				try
				{
					_receiverTypes.AddRange(LoadReceiverTypes(path));
				}
				catch(FileLoadException)
				{
					MessageBoxShow("Die Empfänger-Programmbibliothek \""+path+"\" konnte nicht geladen werden.", "GraphalyzerPro", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		/// <summary>
		/// Loads each type which implements the <see cref="IReceiver"/>-interface from the assembly which is saved at the specified path and then returns all.
		/// </summary>
		/// <param name="path">The path of the assembly file</param>
		/// <returns>The loaded <see cref="IReceiver"/>-types.</returns>
		internal List<Type> LoadReceiverTypes(string path)
		{
			List<Type> result=new List<Type>();
			try
			{
				foreach(Type type in Assembly.LoadFrom(path).GetExportedTypes())
				{
					if((type.IsClass)&&(type.FindInterfaces(TypeFilter.Equals, typeof(IReceiver)).Length>0))
					{
						result.Add(type);
					}
				}
			}
			catch
			{
				throw new FileLoadException();
			}
			return result;
		}

	    protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeRxBackingFieldNameConventions();
			InitializeReceiverTypes();
        }

		/// <summary>
		/// Shows a <see cref="MessageBox"/>-object and can be used for mocking
		/// </summary>
		/// <param name="messageBox">The text of the <see cref="MessageBox"/></param>
		/// <param name="caption">The caption of the <see cref="MessageBox"/></param>
		/// <param name="button">The <see cref="MessageBoxButton"/> of the <see cref="MessageBox"/></param>
		/// <param name="icon">The <see cref="MessageBoxImage"/> of the <see cref="MessageBox"/></param>
		internal void MessageBoxShow(string messageBox, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			MessageBox.Show(messageBox, caption, button, icon);
		}
    }
}
