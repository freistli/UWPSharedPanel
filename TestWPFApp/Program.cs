using System;
using System.Collections.Generic;
using System.Text;

namespace TestWPFApp
{
    public class Program
    {
        [System.STAThreadAttribute()]
        public static void Main()
        {
            using (new UWPApp.App())
            {
                TestWPFApp.App app = new TestWPFApp.App();
                app.InitializeComponent();
                app.Run();
            }
        }
    }
}
