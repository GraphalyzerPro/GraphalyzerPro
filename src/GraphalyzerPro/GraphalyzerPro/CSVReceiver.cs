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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Common;
using System.Windows;
using Microsoft.Win32;

namespace GraphalyzerPro
{
	/// <summary>
	/// Provides a common interface for all receiver implementations.
	/// </summary>
	public class CSVReceiver : Receiver
	{
		/// <summary>
		/// The header of a CSV-file in the correct format
		/// </summary>
		private const string TABLE_HEADER="Timestamp;Gap;Duration;PID;Thread;Type;Domain;Application;Component;Module;Code;Text;Meta-Informationen";
		/// <summary>
		/// Column seperators of a CSV-file in the correct format
		/// </summary>
		private static readonly char[] SEPARATORS=new char[] { ';' };
		/// <summary>
		/// Culture containing the datetime format of a CSV-file in the correct format
		/// </summary>
		private static readonly CultureInfo DATETIME_FORMAT_CULTURE=CultureInfo.CreateSpecificCulture("de");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">The name of the <see cref="CSVReceiver"/></param>
		public CSVReceiver(string name) : base(name)
		{
		}

		/// <summary>
		/// Initializes the <see cref="IReceiver"/>
		/// </summary>
		/// <param name="informationEngine">
		/// The <see cref="IInformationEngine"/> which handels the requests of the <see cref="IReceiver"/>.
		/// </param>
		public override void Initialize(IInformationEngine informationEngine)
		{
			base.Initialize(informationEngine);
			StreamReader reader;
			bool combatible=false;
			string[] cols;
			List<IDiagnoseOutputEntry> entries;
			OpenFileDialog dialog=new OpenFileDialog();
			dialog.Title="Öffnen";
			dialog.Filter="CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
			bool? result;
			while((!combatible)&&((result=dialog.ShowDialog()).HasValue)&&(result.Value))
			{
				try
				{
					reader=new StreamReader(dialog.OpenFile());
					combatible=(!reader.EndOfStream)&&(reader.ReadLine()==TABLE_HEADER);
					entries=new List<IDiagnoseOutputEntry>();
					while((!reader.EndOfStream)&&(combatible))
					{
						cols=reader.ReadLine().Split(SEPARATORS);
						if((cols.Length==13)&&(cols[5].Length==1))
						{
							try
							{
								entries.Add((IDiagnoseOutputEntry)(new DiagnoseOutputEntry(
									DateTime.Parse(cols[0], DATETIME_FORMAT_CULTURE),
									long.Parse(cols[1], NumberStyles.None),
									long.Parse(cols[2], NumberStyles.None),
									int.Parse(cols[3], NumberStyles.None),
									int.Parse(cols[4], NumberStyles.None),
									cols[5].ToCharArray()[0],
									cols[6],
									cols[7],
									cols[8],
									cols[9],
									cols[10],
									cols[11],
									cols[12]
									)));
							}
							catch
							{
								combatible=false;
							}
						}
						else
						{
							combatible=false;
						}
					}
					reader.Close();
					if(combatible)
					{
						foreach(IDiagnoseOutputEntry entry in entries)
						{
							InformationEngine.ProcessNewDiagnoseOutputEntry(entry);
						}
					}
					else
					{
						MessageBox.Show("Leider konnte die angegebene Datei '"+dialog.FileName+"' aufgrund eines fehlerhaften Formats nicht geöffnet werden.", "Fehlerhaftes Format", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				catch
				{
					MessageBox.Show("Leider konnte die angegebene Datei '"+dialog.FileName+"' aufgrund eines E/A-Fehlers nicht geöffnet werden.", "E/A-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			if(!combatible)
			{
				throw new OperationCanceledException();
			}
		}

		/// <summary>
		/// The <see cref="IReceiver"/> cleanup everything to be destructed without errors.
		/// </summary>
		public override void Deactivate()
		{
		}
	}
}

