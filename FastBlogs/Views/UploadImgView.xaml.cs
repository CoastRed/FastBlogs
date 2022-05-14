using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FastBlogs.Views
{
    /// <summary>
    /// UploadImgView.xaml 的交互逻辑
    /// </summary>
    public partial class UploadImgView : UserControl
    {
        public UploadImgView()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            string? filePath = ((System.Array)e.Data.GetData(DataFormats.FileDrop))?.GetValue(0)?.ToString();
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            // 快捷方式需要获取目标文件路径
            if (filePath.ToLower().EndsWith("lnk"))
            {
                return;
            }
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            if (string.IsNullOrWhiteSpace(file.Extension))
            {
                return;
            }
            this.filePath.Text = file.FullName;
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}
