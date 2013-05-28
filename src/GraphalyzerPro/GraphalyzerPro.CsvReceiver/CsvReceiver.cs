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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using Microsoft.Win32;

namespace GraphalyzerPro.CsvReceiver
{
    public class CsvReceiver : IReceiver
    {
        private static readonly Dictionary<char, DiagnoseType> TypeCharToTypeEnumLookUp = new Dictionary
            <char, DiagnoseType>
            {
                {'(', DiagnoseType.StartBracketOutput},
                {')', DiagnoseType.EndBracketOutput},
                {'E', DiagnoseType.Exception},
                {'�', DiagnoseType.SingleOutput},
                {'·', DiagnoseType.SingleOutput},
            };

        private IInformationEngine InformationEngine { get; set; }

        private Guid SessionId { get; set; }

        public string Name
        {
            get { return "CSVReceiver"; }
        }

        public void Initialize(IInformationEngine informationEngine, Guid sessionId)
        {
            InformationEngine = informationEngine;
            SessionId = sessionId;
            ProcessTheCsvFile(GetFilePath());
        }

        public void Deactivate()
        {
            InformationEngine = null;
        }

        private static string GetFilePath()
        {
            var openFileDialog = new OpenFileDialog {Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"};

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : string.Empty;
        }

        private void ProcessTheCsvFile(string filePath)
        {
            var diagnoseOutputEntries = ReadAllEntriesFromFile(filePath);

            foreach (var diagnoseOutputEntry in diagnoseOutputEntries)
            {
                InformationEngine.ProcessNewDiagnoseOutputEntry(diagnoseOutputEntry, SessionId);
            }
        }

        private static IEnumerable<IDiagnoseOutputEntry> ReadAllEntriesFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var allLines = File.ReadAllLines(filePath).ToList();

                // Removing the header line of the csv file.
                allLines.RemoveAt(0);

                var diagnoseOutputEntries = from line in allLines
                                            let dataEntry = line.Split(';')
                                            select ParseDataEntryToDiagnoseOutputEntry(dataEntry);

                return diagnoseOutputEntries.ToList();
            }

            return new List<IDiagnoseOutputEntry>();
        }

        private static IDiagnoseOutputEntry ParseDataEntryToDiagnoseOutputEntry(string[] dataEntry)
        {
            DateTime dateTimeParseBuffer;

            var type = dataEntry[5].Trim();

            return
                new DiagnoseOutputEntry(
                    DateTime.TryParse(dataEntry[0], out dateTimeParseBuffer) ? dateTimeParseBuffer : DateTime.MinValue,
                    Convert.ToInt64(dataEntry[1]),
                    Convert.ToInt64(dataEntry[2]),
                    Convert.ToInt32(dataEntry[3]),
                    Convert.ToInt32(dataEntry[4]),
                    type.Length > 0 ? TypeCharToTypeEnumLookUp[type.Last()] : DiagnoseType.SingleOutput,
                    dataEntry[6],
                    dataEntry[7],
                    dataEntry[8],
                    dataEntry[9],
                    dataEntry[10],
                    dataEntry[13],
                    dataEntry[12]);
        }
    }
}