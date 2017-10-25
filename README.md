# CorsairLinkSharp
Librería y utilidades para interactuar con dispositivos Corsair Link.
## Acerca de este repositorio
CorsairLinkSharp es una implementación simple del protocolo utilizado por los dispositivos Corsair para administrar efectos de iluminación, ventiladores y sensores. Es compatible con dispositivos que utilicen HID para comunicarse (básicamente solo las unidades de enfriamiento líquido H80i/H100i/H110i y similares cuya bomba no sea Asetek).

Este software utiliza acceso RAW a la interfaz HID expuesta por udev en Linux, y requiere Mono para funcionar.
Irónicamente, **ESTE SOFTWARE ACTUALMENTE NO FUNCIONA EN WINDOWS**. Falta crear un controlador USB que funcione en ese sistema operativo.

Admisiblemente, la implementación del software es algo sucia, pero al menos funciona para mis propósitos.
