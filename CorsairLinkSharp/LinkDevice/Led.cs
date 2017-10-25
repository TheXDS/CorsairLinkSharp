//
//  Led.cs
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
    /// Representa un Led individual en un dispositivo CorsairLink.
    /// </summary>
    public class Led : Component
    {
        /// <summary>
        /// Copia en memoria de los colores estáticos.
        /// </summary>
        readonly Color[] colors = new Color[4];

        /// <summary>
        /// Copia en memoria de los colores de temperatura.
        /// </summary>
        readonly Color[] tempColors = new Color[3];

        /// <summary>
        /// Copia en memoria de la tabla de colores.
        /// </summary>
        readonly short[] temp = new short[3];

        /// <summary>
        /// Obtiene el color actual del <see cref="Led"/>.
        /// </summary>
        public Color ActualColor
        {
            get
            {
                driver.Write1(Register.SelectedLed, Id);
                byte c1 = driver.Read(Register.CurrentColor, 3);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return new Color(r[0], r[1], r[2]);
            }
        }

        /// <summary>
        /// Obtiene o establece el modo de este <see cref="Led"/>.
        /// </summary>
        /// <value>The mode.</value>
        public LedMode Mode
        {
            get
            {
                driver.Write1(Register.SelectedLed, Id);
                byte c1 = driver.Read1(Register.LedMode);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return (LedMode)r[0];
            }
            set
            {
                driver.Write1(Register.SelectedLed, Id);
                byte c1 = driver.Write1(Register.LedMode, (byte)value);
                driver.Send();
            }
        }
        /// <summary>
        /// Obtiene o establece el color primario del <see cref="Led"/>.
        /// </summary>
        public Color Color1
        {
            get => colors[0];
            set
            {
                colors[0] = value;
                UpdtColors();
            }
        }

        /// <summary>
        /// Obtiene o establece el color secundario del <see cref="Led"/>.
        /// </summary>
        public Color Color2
        {
            get => colors[1];
            set
            {
                colors[1] = value;
                UpdtColors();
            }
        }

        /// <summary>
        /// Obtiene o establece el color terciario del <see cref="Led"/>.
        /// </summary>
        public Color Color3
        {
            get => colors[2];
            set
            {
                colors[2] = value;
                UpdtColors();
            }
        }

        /// <summary>
        /// Obtiene o establece el cuarto color del <see cref="Led"/>.
        /// </summary>
        public Color Color4
        {
            get => colors[3];
            set
            {
                colors[3] = value;
                UpdtColors();
            }
        }

        /// <summary>
        /// Obtiene o establece la temperatura actual a reportar en este
        /// <see cref="Led"/>.
        /// </summary>
        public float ReportedTemp
        {
            get
            {
                driver.Write1(Register.SelectedLed, Id);
                byte c1 = driver.Read2(Register.ReportedLedTemp);
                driver.Send();
                byte[] r = driver.GetCommandData(c1).ToArray();
                return BitConverter.ToUInt16(r, 0) / (float)256;
            }
            set
            {
                driver.Write1(Register.SelectedLed, Id);
                byte c1 = driver.Write2(Register.ReportedLedTemp, (ushort)(value * 256));
                driver.Send();
            }
        }

        /// <summary>
        /// Obtiene o establece el color de temperatura baja del 
        /// <see cref="Led"/>.
        /// </summary>
        public Color TempLowColor
        {
            get => tempColors[0];
            set
            {
                tempColors[0] = value;
                UpdtTempColors();
            }
        }

        /// <summary>
        /// Obtiene o establece el color de temperatura media del 
        /// <see cref="Led"/>.
        /// </summary>
        public Color TempMedColor
        {
            get => tempColors[1];
            set
            {
                tempColors[1] = value;
                UpdtTempColors();
            }
        }

        /// <summary>
        /// Obtiene o establece el color de temperatura alta del
        /// <see cref="Led"/>.
        /// </summary>
        public Color TempHighColor
        {
            get => tempColors[2];
            set
            {
                tempColors[2] = value;
                UpdtTempColors();
            }
        }

        /// <summary>
        /// Obtiene o establece el valor de temperatura baja del 
        /// <see cref="Led"/>.
        /// </summary>
        public float TempLow
        {
            get => temp[0] / (float)256;
            set
            {
                temp[0] = (short)(value * 256);
                UpdtTempCurve();
            }
        }

        /// <summary>
        /// Obtiene o establece el valor de temperatura media del 
        /// <see cref="Led"/>.
        /// </summary>
        public float TempMed
        {
            get => temp[1] / (float)256;
            set
            {
                temp[1] = (short)(value * 256);
                UpdtTempCurve();
            }
        }

        /// <summary>
        /// Obtiene o establece el valor de temperatura alta del
        /// <see cref="Led"/>.
        /// </summary>
        public float TempHigh
        {
            get => temp[2] / (float)256;
            set
            {
                temp[2] = (short)(value * 256);
                UpdtTempCurve();
            }
        }


        /// <summary>
        /// Inicializa una nueva isntancia de la clase <see cref="Led"/>.
        /// </summary>
        /// <param name="driver">Controlador de acceso al dispositivo.</param>
        /// <param name="id">Identificador de este <see cref="Led"/>.</param>
        internal Led(Driver driver, byte id) : base(driver, id)
        {
            driver.Write1(Register.SelectedLed, id);
            byte c1 = driver.Read(Register.Colors, 12);
            driver.Send();
            byte[] r = driver.GetCommandData(c1).ToArray();
            colors[0] = new Color(r[0], r[1], r[2]);
            colors[1] = new Color(r[3], r[4], r[5]);
            colors[2] = new Color(r[6], r[7], r[8]);
            colors[3] = new Color(r[9], r[10], r[11]);

            c1 = driver.Read(Register.TempVals, 6);
            driver.Send();
            r = driver.GetCommandData(c1).ToArray();
            temp[0] = BitConverter.ToInt16(r, 0);
            temp[1] = BitConverter.ToInt16(r, 2);
            temp[2] = BitConverter.ToInt16(r, 4);

            c1 = driver.Read(Register.TempColors, 9);
            driver.Send();
            r = driver.GetCommandData(c1).ToArray();
            tempColors[0] = new Color(r[0], r[1], r[2]);
            tempColors[1] = new Color(r[3], r[4], r[5]);
            tempColors[2] = new Color(r[6], r[7], r[8]);

        }

        /// <summary>
        /// Actualiza la tabla de colores estáticos.
        /// </summary>
        void UpdtColors()
        {
            driver.Write1(Register.SelectedLed, Id);
            driver.Write(Register.Colors,
                         colors[0].Red, colors[0].Green, colors[0].Blue,
                         colors[1].Red, colors[1].Green, colors[1].Blue,
                         colors[2].Red, colors[2].Green, colors[2].Blue,
                         colors[3].Red, colors[3].Green, colors[3].Blue);
            driver.Send();
        }

        /// <summary>
        /// Actualiza la tabla de temperatura.
        /// </summary>
        void UpdtTempCurve()
        {
            driver.Write1(Register.SelectedLed, Id);
            List<byte> d = new List<byte>();
            d.AddRange(BitConverter.GetBytes(temp[0]));
            d.AddRange(BitConverter.GetBytes(temp[1]));
            d.AddRange(BitConverter.GetBytes(temp[2]));
            driver.Write(Register.TempVals, d.ToArray());
            driver.Send();
        }

        /// <summary>
        /// Actualiza la tabla de colores de temperatura.
        /// </summary>
        void UpdtTempColors()
        {
            driver.Write1(Register.SelectedLed, Id);
            driver.Write(Register.TempColors,
                         tempColors[0].Red, tempColors[0].Green, tempColors[0].Blue,
                         tempColors[1].Red, tempColors[1].Green, tempColors[1].Blue,
                         tempColors[2].Red, tempColors[2].Green, tempColors[2].Blue);
            driver.Send();
        }
    }

    /// <summary>
    /// Estructura básica de color.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Canal de color rojo.
        /// </summary>
        public readonly byte Red;

        /// <summary>
        /// Canal de color verde.
        /// </summary>
        public readonly byte Green;

        /// <summary>
        /// Canal de color azul.
        /// </summary>
        public readonly byte Blue;

        /// <summary>
        /// Inicializa una nueva instancia de la estructura <see cref="Color"/>.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        public Color(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current 
        /// <see cref="Color"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current 
        /// <see cref="Color"/>.</returns>
        public override string ToString()
        {
            return string.Format($"{Red}, {Green}, {Blue}");
        }
    }
}