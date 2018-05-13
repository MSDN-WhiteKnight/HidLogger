# HidLogger
This Example demonstrates logging USB Keyboard/Mouse events using Event Tracing for Windows mechanism. Based on the code from [Ruxcon2016ETW KeyloggerPOC](https://github.com/CyberPoint/Ruxcon2016ETW) project.

**License:** GPL v3

**Requirements:**
Windows 7 (USB 2.0), Windows 8 or newer (USB 2.0 and 3.0)
.NET Framework 4.0
[Microsoft.Diagnostics.Tracing.TraceEvent](https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.TraceEvent/) library.

**Details:**
The program can start/stop ETW sessions with USB-UCX and USBPORT providers, saving events into Event Log File (.etl). Then it can parse this file and display the list of mouse events (button press, move, wheel scroll etc.) and keystrokes on the form or save it into the text file.
