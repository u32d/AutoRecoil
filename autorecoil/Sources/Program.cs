
using System;
using System.Windows.Forms;
using autorecoil.Sources.Forms;

namespace autorecoil
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AutoRecoil());
        }
        
    }
}
