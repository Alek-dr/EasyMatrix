using System;
using System.Windows;

namespace EasyMatrix
{
    class StartUp
    {
        [STAThread]
        void Main()
        {
            Application app = new Application();
            MainWindow window = new MainWindow();
            app.Run(window);
        }
    }
}
