//
//  Sensor.cs
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
using System.Linq;
using CorsairLinkSharp.LinkDriver;

namespace CorsairLinkSharp.LinkDevice
{
    /// <summary>
    /// Representa un sensor individual en un dispositivo CorsairLink.
    /// </summary>
    public class Sensor : Component
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Sensor"/>.
        /// </summary>
        /// <param name="driver">Controlador de acceso al dispositivo.</param>
        /// <param name="id">
        /// Identificador de este <see cref="Sensor"/>.
        /// </param>
        public Sensor(Driver driver, byte id) : base(driver, id) { }

        /// <summary>
        /// Obtiene la temperatura en grados centígrados actualmente registrada
        /// por el sensor.
        /// </summary>
        /// <value>Temperatura, en grados centígrados.</value>
        public float CurrentTemp
        {
            get
            {
                driver.Write1(Register.SelectedSensor, Id);
                byte c1 = driver.Read2(Register.CurrentTemp);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return (BitConverter.ToUInt16(r, 0) / 256);
            }
        }

        /// <summary>
        /// Obtiene la temperatura límite de este sensor. Si la temperatura
        /// sobrepasa este valor, la unidad supondrá que ha fallado.
        /// </summary>
        /// <value>Temperatura, en grados centígrados.</value>
        public float TempLimit
        {
            get
            {
                driver.Write1(Register.SelectedSensor, Id);
                byte c1 = driver.Read2(Register.TempLimit);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return (BitConverter.ToUInt16(r, 0) / 256);
            }
            set
            {
                driver.Write1(Register.SelectedSensor, Id);
                byte c1 = driver.Write2(Register.TempLimit, (ushort)(value * 256));
                driver.Send();
            }
        }
    }
}