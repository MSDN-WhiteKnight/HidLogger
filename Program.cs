using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
/*USB Hid Logger
 Author: MSDN.WhiteKnight (https://github.com/MSDN-WhiteKnight)
 Based on Ruxcon2016ETW KeyloggerPOC (https://github.com/CyberPoint/Ruxcon2016ETW/tree/master/KeyloggerPOC)*/

namespace HidLogger
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
