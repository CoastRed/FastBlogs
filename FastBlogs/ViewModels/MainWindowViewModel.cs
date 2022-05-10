using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastBlogs.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
       
        #region 构造函数

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        #endregion

        #region 命令

        public DelegateCommand<string> NavigateCommand
        {
            get
            {
                return new DelegateCommand<string>(viewName =>
                {
                    this.regionManager.Regions["MainWindowRegion"].RequestNavigate(viewName);
                });
            }
        }

        public DelegateCommand ExitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Application.Current.Shutdown();
                });
            }
        }

        #endregion
    }
}
