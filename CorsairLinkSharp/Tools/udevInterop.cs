//
//  udevInterop.cs
//
//  Author:
//       César Andrés Morgan <xds_xps_ivx@hotmail.com>
//
//  Copyright (c) 2017 Copyright © 2017 César Andrés Morgan
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Diagnostics;
using System.IO;

namespace CorsairLinkSharp.Tools
{
    /// <summary>
    /// Interoperatividad con udev. 
    /// </summary>
    /// <remarks>
    /// Esta clase es exclusivamente para sistemas GNU/Linux que utilicen udev.
    /// </remarks>
    public static class UdevInterop
    {
        /// <summary>
        /// Obtiene la ruta del Stream a utilizar para comunicarse con un 
        /// dispositivo.
        /// </summary>
        /// <returns>Ruta del Raw Stream del dispositivo.</returns>
        /// <param name="sysPath">Sys path.</param>
        public static string DevName(DirectoryInfo sysPath) => $"/dev/{Query_udevadm($"name {sysPath.FullName}")}";
        /// <summary>
        /// Obtiene la ruta de sysfs del dispositivo especificado.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="sysPath">Sys path.</param>
        public static string DevPath(DirectoryInfo sysPath) => Query_udevadm($"path {sysPath.FullName}");
        static string Query_udevadm(string query)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("udevadm", $"info -q {query}")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd().Replace("\n", "");
        }
    }
}
