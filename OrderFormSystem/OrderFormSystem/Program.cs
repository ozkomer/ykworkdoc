using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OrderFormSystem
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Order od=null;// = GetInputData();
            Output.Output op = new Output.PdfOutput(od);
            //Output.Output op = new Output.TxtOutput(od);
            //op.PrintOut();
            //return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OFSForm());
        }
    }
}
