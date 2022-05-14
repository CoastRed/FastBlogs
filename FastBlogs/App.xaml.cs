using FastBlogs.ViewModels;
using FastBlogs.Views;
using MetaWeblogAPI;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FastBlogs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(BlogOperation), option =>
            {
                string url = "https://rpc.cnblogs.com/metaweblog/xcoast";
                return new BlogOperation(url, string.Empty, "尘枫", "2ok5yu1n");
            });

            containerRegistry.RegisterForNavigation<UploadFileView, UploadFileViewModel>();
            containerRegistry.RegisterForNavigation<UploadImgView, UploadImgViewModel>();
            containerRegistry.RegisterForNavigation<SystemSettingView, SystemSettingViewModel>();
        }


    }
}
