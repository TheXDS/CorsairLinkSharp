//
//  KnownDevices.cs
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

namespace CorsairLinkSharp.LinkDriver.HID
{
    public partial class HidDriver
    {
        /// <summary>
        /// Estructura para representar dispositivos conocidos.
        /// </summary>
        struct KnownDevice
        {
            /// <summary>
            /// Identificador (IdVendor:IdProduct) del dispositivo
            /// </summary>
            internal readonly string ProdId;

            /// <summary>
            /// Nombre del dispositivo.
            /// </summary>
            internal readonly string Name;

            /// <summary>
            /// Inicializa una nueva instancia de la estructura
            /// <see cref="KnownDevice"/>.
            /// </summary>
            /// <param name="prodId">
            /// Identificador de producto. Debe estar formateado como 
            /// ":XXXX:YYYY.".
            /// </param>
            /// <param name="name">Nombre amigable del dispositivo.</param>
            internal KnownDevice(string prodId, string name)
            {
                ProdId = prodId;
                Name = name;
            }
        }

        /// <summary>
        /// Colección de dispositivos conocidos que pueden utilizar el
        /// controlador <see cref="HidDriver"/>.
        /// </summary>
        /// <remarks>
        /// No se deben agregar dispositivos USB Asetek, porque los
        /// mismos no ofrecen interfaz HID.
        /// </remarks>
        static KnownDevice[] knownDevices =
        {
            new KnownDevice(":1B1C:0C04.", "Corsair Hydro Series (HID)")

            // TODO: agregar más dispositivos conocidos aquí...

        };
    }
}