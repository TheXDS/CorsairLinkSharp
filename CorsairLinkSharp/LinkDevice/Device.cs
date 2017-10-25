//
//  LinkDevice.cs
//
//  Author:
//       César Morgan <xds_xps_ivx@hotmail.com>
//
//  Copyright (c) 2017 César Morgan
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using CorsairLinkSharp.LinkDriver;

namespace CorsairLinkSharp.LinkDevice
{
    /// <summary>
    /// Define a un dispositivo de Corsair Link.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Controlador de acceso a este dispositivo.
        /// </summary>
        readonly Driver driver;

        /// <summary>
        /// Obtiene el Id del dispositivo.
        /// </summary>
        public readonly byte Id;

        /// <summary>
        /// Obtiene el nombre del dispositivo.
        /// </summary>
        public readonly string ProductName;

        /// <summary>
        /// Obtiene el Id del firmware en en dispositivo.
        /// </summary>
        public readonly Version FirmwareId;

        /// <summary>
        /// Obtiene un valor que determina si este dispositivo funciona
        /// correctamente.
        /// </summary>
        public bool IsOk
        {
            get
            {
                var c = driver.Read1(Register.Status);
                driver.Send();
                return driver.GetCommandData(c).ElementAt(0) == 0;
            }
        }

        /// <summary>
        /// Colección de luces Led del dispositivo.
        /// </summary>
        public readonly ReadOnlyCollection<Led> Leds;

        /// <summary>
        /// Colección de sensores de temperatura del dispositivo.
        /// </summary>
        public readonly ReadOnlyCollection<Sensor> Sensors;

        /// <summary>
        /// Colección de motores (ventiladores y bombas) del dispositivo.
        /// </summary>
        public readonly ReadOnlyCollection<Fan> Fans;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LinkDriver"/>.
        /// </summary>
        /// <param name="driver">
        /// Controlador a utilizar para interactuar con el dispositivo.
        /// </param>
        public Device(Driver driver)
        {
            this.driver = driver;

            // Self-test ?
            driver.DirectCommand(Command.Unk1);
            driver.Send();

            // Leer información básica del dispositivo
            byte c1 = driver.Read1(Register.Id);
            byte c2 = driver.Read(Register.Name, 8);
            byte c3 = driver.Read2(Register.FirmwareId);
            byte c4 = driver.Read1(Register.FansCount);
            byte c5 = driver.Read1(Register.LedsCount);
            byte c6 = driver.Read1(Register.SensorsCount);
            // El packet de respuesta a penas es suficiente para todos estos
            // comandos.

            driver.Send();
            Id = driver.GetCommandData(c1).FirstOrDefault();

            var pname = driver.GetCommandData(c2);
            var j = pname.IndexOf(0);
            byte[] nme = new byte[j];
            Array.Copy(pname.ToArray(), nme, j);
            ProductName = System.Text.Encoding.ASCII.GetString(nme) ?? "desconocido";

            nme = driver.GetCommandData(c3).ToArray();
            FirmwareId = new Version((nme[0] >> 4) & 0xf, nme[0] & 0xf, nme[1]);

            byte lcount = driver.GetCommandData(c5).FirstOrDefault();
            byte fcount = driver.GetCommandData(c4).FirstOrDefault();
            byte scount = driver.GetCommandData(c6).FirstOrDefault();

            List<Led> leds = new List<Led>(lcount);
            for (byte k = 0; k < lcount; k++)
            {
                leds.Add(new Led(driver, k));
            }
            Leds = leds.AsReadOnly();

            List<Fan> fans = new List<Fan>(fcount);
            for (byte k = 0; k < fcount; k++)
            {
                fans.Add(new Fan(driver, k));
            }
            Fans = fans.AsReadOnly();

            List<Sensor> sensors = new List<Sensor>(fcount);
            for (byte k = 0; k < fcount; k++)
            {
                sensors.Add(new Sensor(driver, k));
            }
            Sensors = sensors.AsReadOnly();

            driver.Clear();
#if DEBUG
            // Si el Id es 0, el dispositivo podría necesitar un reset
            if (Id == 0)
            {
                System.Diagnostics.Debug.Print(
                    $"El dispositivo {ProductName} está en estado de error y necesita reiniciarse.");
            }
#endif
        }
    }
}