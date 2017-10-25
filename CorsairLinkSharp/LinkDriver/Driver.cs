//
//  Driver.cs
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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace CorsairLinkSharp.LinkDriver
{
    /// <summary>
    /// Clase base para los controladores de dispositivos CorsairLink.
    /// </summary>
    public abstract class Driver
    {
        #region Campos privados
        /// <summary>
        /// Contador de comandos.
        /// </summary>
        byte cmdId = 0;

        /// <summary>
        /// Lista de bytes con los comandos a enviar.
        /// </summary>
        readonly List<byte> commands = new List<byte>(64);

        /// <summary>
        /// Lista de bytes con la respuesta del dispositivo.
        /// </summary>
        readonly List<byte> response = new List<byte>(32);
        #endregion

        #region Helpers privados
        /// <summary>
        /// Obtiene el tamaño de los datos devueltos por un comando.
        /// </summary>
        /// <returns>Cantidad de bytes que un comando devuelve.</returns>
        /// <param name="c">Comando a comprobar.</param>
        byte CmdSize(Command c)
        {
            switch (c)
            {
                case Command.Read1: return 1;
                case Command.Read2: return 2;
                case Command.Read: return 255; // El comando reporta el tamaño.
                default: return 0;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Realiza la acción de enviar una cadena de comandos al dispositivo.
        /// </summary>
        /// <returns>
        /// <c>true</c> si la operación fue exitosa, <c>false</c> en caso
        /// contrario.
        /// </returns>
        protected abstract bool DoSend(byte[] commands);

        /// <summary>
        /// Realiza la acción de recibir datos desde el dispositivo.
        /// </summary>
        /// <returns>Los datos redibidos.</returns>
        protected abstract Task<byte[]> ListenDevice();

        /// <summary>
        /// Abre el dispositivo para realizar transacciones.
        /// </summary>
        /// <returns><c>true</c>, if device was opened, <c>false</c> otherwise.</returns>
        protected abstract bool OpenDevice();

        /// <summary>
        /// Cierra el dispositivo al finalizar las transacciones.
        /// </summary>
        /// <returns><c>true</c>, if device was closed, <c>false</c> otherwise.</returns>
        protected abstract bool CloseDevice();
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Lee el registro de 1 byte especificado.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="reg">Registro a leer.</param>
        public byte Read1(Register reg)
        {
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)Command.Read1);
            commands.Add((byte)reg);
            return cmdId;
        }

        /// <summary>
        /// Escribe un valor en un registro de 1 byte.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="reg">Registro a escribir.</param>
        /// <param name="value">Valor a escribir en el registro.</param>
        public byte Write1(Register reg, byte value)
        {
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)Command.Write1);
            commands.Add((byte)reg);
            commands.Add(value);
            return cmdId;
        }

        /// <summary>
        /// Lee el registro de 2 bytes especificado.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="reg">Registro a leer.</param>
        public byte Read2(Register reg)
        {
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)Command.Read2);
            commands.Add((byte)reg);
            return cmdId;
        }

        /// <summary>
        /// Escribe un valor en un registro de 2 bytes.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="reg">Registro a escribir.</param>
        /// <param name="value">Valor a escribir en el registro.</param>
        public byte Write2(Register reg, uint value)
        {
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)Command.Read2);
            commands.Add((byte)reg);
            commands.AddRange(BitConverter.GetBytes(value));
            return cmdId;
        }

        /// <summary>
        /// Lee la cantidad de bytes especificada del registro solicitado.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="reg">Registro a leer.</param>
        /// <param name="count">Cantidad de bytes a leer.</param>
        public byte Read(Register reg, byte count)
        {
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)Command.Read);
            commands.Add((byte)reg);
            commands.Add(count);
            return cmdId;
        }

        /// <summary>
        /// Escribe valores en un registro.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="reg">Registro a escribir.</param>
        /// <param name="data">Datos a escribir en el registro.</param>
        public byte Write(Register reg, params byte[] data)
        {
            if (data.Length == 0) return ++cmdId;
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)Command.Write);
            commands.Add((byte)reg);
            commands.Add((byte)data.Length);
            commands.AddRange(data);
            return cmdId;
        }

        /// <summary>
        /// Ejecuta un comando personalizado directamente.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="c">Comando a ejecutar.</param>
        /// <param name="args">Argumentos del comando.</param>
        public byte DirectCommand(Command c, params byte[] args)
        {
            unchecked { commands.Add(++cmdId); }
            commands.Add((byte)c);
            commands.AddRange(args);
            return cmdId;
        }

        /// <summary>
        /// Envía bytes directamente.
        /// </summary>
        /// <returns>El Id asignado a este comando.</returns>
        /// <param name="args">Argumentos del comando.</param>
        public byte DirectData(params byte[] args)
        {
            unchecked { commands.Add(++cmdId); }
            commands.AddRange(args);
            return cmdId;
        }

        /// <summary>
        /// Obtiene la cuenta de bytes libres en el packet actual.
        /// </summary>
        public byte CommandBytesRemaining => (byte)(64 - commands.Count);

        /// <summary>
        /// Envía todos los comandos actualmente en cola al dispositivo.
        /// </summary>
        /// <returns>
        /// <c>true</c> si la operación ha sido exitosa, <c>false</c> en caso
        /// contrario.
        /// </returns>
        public bool Send()
        {
            if (!OpenDevice()) return false;
            commands.Insert(0, (byte)commands.Count);
            var rec = ListenDevice();
            if (!DoSend(commands.ToArray())) return false;

            Clear();

            // Parece haber un problema de sincronización entre hilos...
            Thread.Sleep(5);
            Task.WaitAll(rec);

            response.AddRange(rec.Result);
            CloseDevice();
            return true;
        }

        /// <summary>
        /// Obtiene los datos de respuesta de un comando que ha sido enviado.
        /// </summary>
        /// <returns>Los datos que el comando ha regresado.</returns>
        /// <param name="id">Identificador de comando.</param>
        public ReadOnlyCollection<byte> GetCommandData(byte id)
        {
            List<byte>.Enumerator j = response.GetEnumerator();
            List<byte> r = new List<byte>();
            while (j.MoveNext())
            {
                byte currId = j.Current;
                j.MoveNext();
                byte sze = CmdSize((Command)j.Current);
                if (sze == 255)
                {
                    j.MoveNext();
                    sze = j.Current;
                }
                if (currId == id)
                {
                    for (byte k = 0; k < sze; k++)
                    {
                        if (!j.MoveNext()) break;
                        r.Add(j.Current);
                    }
                    break;
                }
                for (byte k = 0; k < sze; k++) j.MoveNext();
            }
            return r.AsReadOnly();
        }

        /// <summary>
        /// Limpia el estado del controlador.
        /// </summary>
        public void Clear()
        {
            commands.Clear();
            response.Clear();
        }

#if DEBUG
        /// <summary>
        /// Devuelve la información hexadecimal del último paquete recibido.
        /// </summary>
        public string DumpLastPacket() => BitConverter.ToString(response.ToArray());

        /// <summary>
        /// Devuelve el último paquete como información raw.
        /// </summary>
        public byte[] DumpLastPacketRaw() => response.ToArray();
#endif
        #endregion
    }
}