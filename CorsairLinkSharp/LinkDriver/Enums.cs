//
//  Enums.cs
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

namespace CorsairLinkSharp.LinkDriver
{
    /// <summary>
    /// Enumera los comandos aceptados por un dispositivo CorsairLink.
    /// </summary>
    public enum Command : byte
    {
        /// <summary>
        /// Escribir 1 byte.
        /// </summary>
        [Size(0)] Write1 = 0x06,
        /// <summary>
        /// Leer 1 byte.
        /// </summary>
        [Size(1)] Read1 = 0x07,
        /// <summary>
        /// Escribir 2 bytes.
        /// </summary>
        [Size(0)] Write2 = 0x08,
        /// <summary>
        /// Leer 2 bytes.
        /// </summary>
        [Size(2)] Read2 = 0x09,
        /// <summary>
        /// Escribir bytes.
        /// </summary>
        [Size(0)] Write = 0x0a,
        /// <summary>
        /// Leer bytes.
        /// </summary>
        [Size(3)] Read = 0x0b,

        // Comandos desconocidos

        /// <summary>
        /// Self-test?
        /// </summary>
        /// <remarks>
        /// Sin argumentos, devuelve 0F-05-FF-FF-FF-FF-FF-FF-FF
        /// </remarks>
        Unk1 = 0x4f
    }

    /// <summary>
    /// Enumera los registros disponibles en los dispositivos CorsairLink.
    /// </summary>
    public enum Register : byte
    {
        /// <summary>
        /// Identificador de producto.
        /// </summary>
        [Size(1)] Id = 0x00,
        /// <summary>
        /// Id de firmware.
        /// </summary>
        [Size(2)] FirmwareId = 0x01,
        /// <summary>
        /// Nombre del dispositivo.
        /// </summary>
        [Size(8)] Name = 0x02,
        /// <summary>
        /// Estado del dispositivo.
        /// </summary>
        [Size(1)] Status = 0x03,
        /// <summary>
        /// Obtiene o establece el Led actualmente seleccionado para configurar.
        /// </summary>
        [Writtable] [Size(1)] SelectedLed = 0x04,
        /// <summary>
        /// Cuenta de leds del dispositivo.
        /// </summary>
        [Size(1)] LedsCount = 0x05,
        /// <summary>
        /// Modo del led seleccionado. Debe interpretarse como un valor
        /// <see cref="CorsairLinkSharp.LinkDriver.LedMode"/>.
        /// </summary>
        [Writtable] [Size(1)] LedMode = 0x06,
        /// <summary>
        /// Color actual del led seleccionado.
        /// </summary>
        [Size(3)] CurrentColor = 0x07,
        /// <summary>
        /// Color base de temperatura
        /// </summary>
        [Writtable] [Size(2)] ReportedLedTemp = 0x08,
        /// <summary>
        /// Tabla de valores de temperatura para determinar el color del led
        /// seleccionado.
        /// </summary>
        [Writtable] [Size(6)] TempVals = 0x09,
        /// <summary>
        /// Tabla de colores para establecer en el led seleccionado basado en la
        /// tabla <see cref="TempVals"/> 
        /// </summary>
        [Writtable] [Size(9)] TempColors = 0x0a,
        /// <summary>
        /// Tabla de colores para modos estático y multicolor.
        /// </summary>
        /// <remarks>
        /// Siempre 4 entradas de 3 bytes (RGB) cada una.
        /// </remarks>
        [Writtable] [Size(12)] Colors = 0x0b,
        /// <summary>
        /// Obtiene o establece el sensor actualmente seleccionado para
        /// configurar.
        /// </summary>
        [Writtable] [Size(1)] SelectedSensor = 0x0c,
        /// <summary>
        /// Cuenta de sendores del dispositivo.
        /// </summary>
        [Size(1)] SensorsCount = 0x0d,
        /// <summary>
        /// Temperatura del sensor actualmente seleccionado.
        /// </summary>
        [Size(2)] CurrentTemp = 0x0e,
        /// <summary>
        /// Temperatura límite del dispositivo para el sensor actual.
        /// </summary>
        [Writtable] [Size(2)] TempLimit = 0x0f,
        /// <summary>
        /// Obtiene o establece el motor (ventilador o bomba) actualmente
        /// seleccionado para configurar.
        /// </summary>
        [Writtable] [Size(1)] SelectedFan = 0x10,
        /// <summary>
        /// Cuenta de motores (ventiladores o bombas) del dispositivo.
        /// </summary>
        [Size(1)] FansCount = 0x11,
        /// <summary>
        /// Modo del motor seleccionado. Debe interpretarse como un valor
        /// <see cref="CorsairLinkSharp.LinkDriver.FanMode"/>.
        /// </summary>
        [Writtable] [Size(1)] FanMode = 0x12,
        /// <summary>
        /// Velocidad PWM del motor seleccionado.
        /// </summary>
        [Writtable] [Size(1)] FanPWM = 0x13,
        /// <summary>
        /// Revoluciones por minuto objetivo del motor seleccionado.
        /// </summary>
        [Writtable] [Size(2)] RPM = 0x14,
        /// <summary>
        /// Generalmente de solo escritura. Temperatura a reportar al
        /// controlador del motor seleccionado.
        /// </summary>
        [Writtable] [Size(2)] ReportedFanTemp = 0x15,
        /// <summary>
        /// Revoluciones por minuto actuales del motor seleccionado.
        /// </summary>
        [Size(2)] CurrentRPM = 0x16,
        /// <summary>
        /// Obtiene el valor de RPM más alto leído del motor seleccionado desde
        /// el encendido.
        /// </summary>
        [Size(2)] MaxEverRPM = 0x17,
        /// <summary>
        /// Punto de infravelocidad del motor seleccionado.
        /// </summary>
        [Size(2)] FanUST = 0x18,
        /// <summary>
        /// Tabla de RPM con respecto a la temperatura del motor seleccionado.
        /// </summary>
        /// <remarks>
        /// 5 entradas de 2 bytes cada una.
        /// </remarks>
        [Writtable] [Size(10)] FanRPMTable = 0x19,
        /// <summary>
        /// Tabla de temperaturas para el control automático de RPM del motor
        /// seleccionado.
        /// </summary>
        /// <remarks>
        /// 5 entradas de 2 bytes cada una.
        /// </remarks>
        [Writtable] [Size(10)] FanTempTable = 0x1a
    }

    /// <summary>
    /// Enumera los distintos modos en los que puede establecerse un Led.
    /// </summary>
    public enum LedMode : byte
    {
        /// <summary>
        /// Modo estático.
        /// </summary>
        Static = 0x00,
        /// <summary>
        /// Multicolor, 2 colores.
        /// </summary>
        Multi2 = 0x4b,
        /// <summary>
        /// Multicolor, 4 colores.
        /// </summary>
        Multi4 = 0x8b,
        /// <summary>
        /// Modo de temperatura (3 colores).
        /// </summary>
        Temp = 0xc0
    }

    /// <summary>
    /// Enumera los distintos modos para los ventiladores y bombas de agua.
    /// </summary>
    public enum FanMode : byte
    {
        /// <summary>
        /// PWM Fijo.
        /// </summary>
        FixedPWM = 0x02,
        /// <summary>
        /// RPM Fija.
        /// </summary>
        FixedRPM = 0x04,
        /// <summary>
        /// Modo predeterminado.
        /// </summary>
        Default = 0x06,
        /// <summary>
        /// Modo silencioso.
        /// </summary>
        Quiet = 0x08,
        /// <summary>
        /// Modo balanceado.
        /// </summary>
        Balanced = 0x0a,
        /// <summary>
        /// Modo de alto rendimiento.
        /// </summary>
        Performance = 0x0c,
        /// <summary>
        /// Modo personalizado. Se utilizan las curvas definidas en el
        /// dispositivo.
        /// </summary>
        Custom = 0x0e
    }
}