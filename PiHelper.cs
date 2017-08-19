using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using PISDK;
using PISDKCommon;
using PITimeServer;
using System.IO;

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PiUtinity
{
    /// <summary>
    /// PI操作类
    /// </summary>
    static class PiHelper
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new main());
        }

    }
}
