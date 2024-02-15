using System.Windows;
using CETAP_LOB.Mapping;
using GalaSoft.MvvmLight.Threading;

namespace CETAP_LOB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
            Maps.Initialize();
        }
    }
}
