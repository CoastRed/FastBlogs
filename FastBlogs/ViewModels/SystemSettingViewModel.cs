using MetaWeblogAPI;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBlogs.ViewModels
{
    public class SystemSettingViewModel : BindableBase
    {
        #region 命令

        public DelegateCommand SetUserInfoCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //BlogOperation blogOperation = new BlogOperation(string.Empty);
                    //BlogInfo[] blogInfo = blogOperation.GetUsersBlogs(string.Empty, "尘枫", "2ok5yu1n");

                });
            }
        }

        #endregion
    }
}
