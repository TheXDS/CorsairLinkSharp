//
//  HidDriver.cs
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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static CorsairLinkSharp.Tools.UdevInterop;

namespace CorsairLinkSharp.LinkDriver.HID
{
    /// <summary>
    /// Controlador estándar USB de Human Interface Device (HID)
    /// </summary>
    public partial class HidDriver : Driver, IDisposable
    {

        FileStream hidraw;
        readonly string devPath;

        /// <summary>
        /// Escanea el sistema en busca de dispositivos HID conocidos.
        /// </summary>
        /// <returns>
        /// Una lista de instancias de todos los dispositivos encontrados.
        /// </returns>
        public static IEnumerable<HidDriver> Scan()
        {
            DirectoryInfo roth = new DirectoryInfo("/sys/class/hidraw");
            foreach (var j in roth.GetDirectories())
            {
                var k = (new DirectoryInfo(DevPath(j))).Parent.Parent;
                foreach (var l in knownDevices)
                {
                    if (k.Name.Contains(l.ProdId))
                    {
#if DEBUG
                        string n = DevName((j));
                        System.Diagnostics.Debug.Print($"Se ha encontrado un {l.Name} en {n}");
                        yield return new HidDriver(n);
#else
                        yield return new HidDriver(DevName(j));
#endif
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HidDriver"/>.
        /// </summary>
        /// <param name="path">Ruta del Stream hidraw.</param>
        public HidDriver(string path)
        {
            devPath = path;
        }

        /// <summary>
        /// Abre el dispositivo para realizar transacciones.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if device was opened, <c>false</c> otherwise.</returns>
        protected override bool OpenDevice()
        {
            try
            {
                hidraw = new FileStream(devPath, FileMode.Open);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Cierra el dispositivo al finalizar las transacciones.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if device was closed, <c>false</c> otherwise.</returns>
        protected override bool CloseDevice()
        {
            try
            {
                hidraw.Close();
                hidraw.Dispose();
                hidraw = null;
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Realiza la acción de recibir datos desde el dispositivo.
        /// </summary>
        /// <returns>Los datos redibidos.</returns>
        protected override async Task<byte[]> ListenDevice()
        {
            byte[] result = new byte[32];
            try
            {
                await hidraw.ReadAsync(result, 0, result.Length);
            }
            catch (Exception ex)
            {
                // La operación falló...
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Realiza la acción de enviar una cadena de comandos al dispositivo.
        /// </summary>
        /// <returns>
        /// <c>true</c> si la operación fue exitosa, <c>false</c> en caso
        /// contrario.
        /// </returns>
        protected override bool DoSend(byte[] commands)
        {
            try
            {
                hidraw.Write(commands, 0, commands.Length);
            }
            catch (Exception ex)
            {
                // La operación falló...
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
            return true;
        }

        #region IDisposable Support
        bool disposedValue;
        /// <summary>
        /// Implementa la interfaz <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    hidraw?.Dispose();
                }
                disposedValue = true;
            }
        }
        /// <summary>
        /// Releases all resource used by the <see cref="HidDriver"/> object.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Dispose()"/> when you are finished using the
        /// <see cref="HidDriver"/>. The <see cref="Dispose()"/> method leaves 
        /// the <see cref="HidDriver"/> in an unusable state. After calling
        /// <see cref="Dispose()"/>, you must release all references to the
        /// <see cref="HidDriver"/> so the garbage collector can reclaim the
        /// memory that the <see cref="HidDriver"/> was occupying.
        /// </remarks>
        public void Dispose() => Dispose(true);

        #endregion
    }
}