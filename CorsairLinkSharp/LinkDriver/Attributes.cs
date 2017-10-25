//
//  Attributes.cs
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

namespace CorsairLinkSharp.LinkDriver
{
    /// <summary>
    /// Indica el tamaño en bytes de un registro o de un comando.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SizeAttribute : Attribute
    {
        /// <summary>
        /// Tamaño del registro, en bytes.
        /// </summary>
        public readonly byte Size;

        /// <summary>
        /// Iniicaliza una nueva instancia del atributo
        /// <see cref="SizeAttribute"/>, indicando el tamaño de un registro.
        /// </summary>
        /// <param name="size">Tamaño del registro, en bytes.</param>
        public SizeAttribute(byte size) { Size = size; }
    }

    /// <summary>
    /// Indica que un registro puede ser escrito.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class WrittableAttribute : Attribute { }
}