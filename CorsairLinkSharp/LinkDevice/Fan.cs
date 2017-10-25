//
//  Fan.cs
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
using System.Linq;
using CorsairLinkSharp.LinkDriver;

namespace CorsairLinkSharp.LinkDevice
{
    /// <summary>
    /// Representa un motor (ventilador o bomba) individual en un dispositivo
    /// CorsairLink.
    /// </summary>
    public class Fan : Component
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Fan"/>.
        /// </summary>
        /// <param name="driver">Controlador de acceso al dispositivo.</param>
        /// <param name="id">
        /// Identificador de este <see cref="Fan"/>.
        /// </param>
        public Fan(Driver driver, byte id) : base(driver, id) { }

        /// <summary>
        /// Obtiene o establece el modo de este <see cref="Fan"/>.
        /// </summary>
        public FanMode Mode
        {
            get
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Read1(Register.FanMode);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return (FanMode)r[0];
            }
            set
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Write1(Register.FanMode, (byte)value);
                driver.Send();
            }
        }

        /// <summary>
        /// Obtiene o establece la velocidad PWM de este <see cref="Fan"/>.
        /// </summary>
        /// <value>
        /// Un valor entre 0 y 255 que representa el porcentaje de PWM.
        /// </value>
        public byte PWM
        {
            get
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Read1(Register.FanPWM);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return r[0];
            }
            set
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Write1(Register.FanPWM, value);
                driver.Send();
            }
        }

        /// <summary>
        /// Obtiene o establece la velocidad objetivo en revoluciones por minuto
        /// de este <see cref="Fan"/>.
        /// </summary>
        public ushort RPM
        {
            get
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Read2(Register.RPM);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return BitConverter.ToUInt16(r, 0);
            }
            set
            {
                driver.Write1(Register.SelectedFan, Id);
                driver.Write2(Register.RPM, value);
                driver.Send();
            }
        }

        /// <summary>
        /// Obtiene o establece la temperatura actual a reportar en este
        /// <see cref="Fan"/>.
        /// </summary>
        public float ReportedTemp
        {
            get
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Read2(Register.ReportedFanTemp);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return BitConverter.ToUInt16(r, 0) / (float)256;
            }
            set
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Write2(Register.ReportedFanTemp, (ushort)(value * 256));
                driver.Send();
            }
        }

        /// <summary>
        /// Obtiene la velocidad actual en RPM de este <see cref="Fan"/>.
        /// </summary>
        public ushort CurrentRPM
        {
            get
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Read2(Register.CurrentRPM);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return BitConverter.ToUInt16(r, 0);
            }
        }
        /// <summary>
        /// Obtiene la velocidad máxima en RPM registrada en este
        /// <see cref="Fan"/> desde el encendido del dispositivo.
        /// </summary>
        public ushort MaxEverRPM
        {
            get
            {
                driver.Write1(Register.SelectedFan, Id);
                byte c1 = driver.Read2(Register.MaxEverRPM);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return BitConverter.ToUInt16(r, 0);
            }
        }
    }
}