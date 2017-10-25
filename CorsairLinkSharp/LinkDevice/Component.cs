//
//  Component.cs
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

using CorsairLinkSharp.LinkDriver;

namespace CorsairLinkSharp.LinkDevice
{
    /// <summary>
    /// Clase base de un componente individual de un dispositivo CorsairLink.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Referencia al controlador.
        /// </summary>
        protected readonly Driver driver;

        /// <summary>
        /// Identificador único del componente.
        /// </summary>
        public readonly byte Id;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Component"/>.
        /// </summary>
        /// <param name="driver">Controlador de acceso al dispositivo.</param>
        /// <param name="id">
        /// Identificador de este <see cref="Component"/>.
        /// </param>
        protected Component(Driver driver, byte id)
        {
            this.driver = driver;
            Id = id;
        }
    }
}