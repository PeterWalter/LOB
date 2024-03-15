using System.Windows;
using CETAP_LOB.Mapping;
using GalaSoft.MvvmLight.Threading;
using Syncfusion.Licensing;

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
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXxfcXVVRWRdVURxXEc=");

        }
    }
}
