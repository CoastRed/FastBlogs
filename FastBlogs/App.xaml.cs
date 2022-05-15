using FastBlogs.ViewModels;
using FastBlogs.Views;
using MetaWeblogAPI;
using Newtonsoft.Json;
using NLog;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastBlogs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public readonly ILogger logger = LogManager.GetCurrentClassLogger();

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            MainWindowViewModel? mainWindowViewModel = App.Current.MainWindow.DataContext as MainWindowViewModel;
            if (mainWindowViewModel != null)
                mainWindowViewModel.Configure();
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(ILogger), options =>
            {
                return this.logger;
            });

            FastBlog? fastBlog = new FastBlog();
            if (!File.Exists("FastBlogs.json"))
            {
                string setting = JsonConvert.SerializeObject(fastBlog);
                File.WriteAllText("FastBlogs.json", setting, Encoding.UTF8);
                fastBlog = JsonConvert.DeserializeObject<FastBlog>(setting);
            }
            fastBlog = JsonConvert.DeserializeObject<FastBlog>(File.ReadAllText("FastBlogs.json", Encoding.UTF8));
            if (fastBlog == null)
            {
                fastBlog = new FastBlog();
            }
            
            BlogOperation blogOperation = new BlogOperation(fastBlog.MetaWeblog ?? string.Empty, string.Empty, fastBlog.UserName ?? string.Empty, fastBlog.PassWord ?? string.Empty);
            containerRegistry.RegisterSingleton(typeof(BlogOperation), option =>
            {
                return blogOperation;
            });

            containerRegistry.RegisterForNavigation<UploadFileView, UploadFileViewModel>();
            containerRegistry.RegisterForNavigation<UploadImgView, UploadImgViewModel>();
            containerRegistry.RegisterForNavigation<SystemSettingView, SystemSettingViewModel>();
        }


    }
}
