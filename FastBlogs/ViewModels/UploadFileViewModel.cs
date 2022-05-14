using MetaWeblogAPI;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FastBlogs.ViewModels
{
    public class UploadFileViewModel : BindableBase
    {

        private readonly BlogOperation blogOperation;

        #region 属性

        private int _Progress;

        public int Progress
        {
            get { return _Progress; }
            set
            {
                _Progress = value;
                base.RaisePropertyChanged();
                if (value == 0)
                {
                    this.Visibility = Visibility.Hidden;
                    return;
                }
                if (value == 100)
                {
                    Task.Delay(1000).Wait();
                    this.Visibility = Visibility.Hidden;
                    return;
                }
                this.Visibility = Visibility.Visible;
            }
        }

        private Visibility _Visibility = Visibility.Hidden;

        /// <summary>
        /// 进度条是否显示
        /// </summary>
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; base.RaisePropertyChanged(); }
        }

        private string? _FilePath;
        /// <summary>
        /// 拖拽进来的文件的路径
        /// </summary>
        public string? FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                base.RaisePropertyChanged();
                if (!string.IsNullOrEmpty(value))
                {
                    _ = this.UploadFile(value);
                }
            }
        }

        private string? _Message;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string? Message
        {
            get { return _Message; }
            set { _Message = value; base.RaisePropertyChanged(); }
        }


        #endregion

        #region 构造函数

        public UploadFileViewModel(BlogOperation blogOperation)
        {
            this.blogOperation = blogOperation;
        }

        #endregion


        #region 命令

        public DelegateCommand UploadImgCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()
                    {
                        Filter = "md文件 |*.md"
                    };
                    var result = openFileDialog.ShowDialog();
                    openFileDialog.Multiselect = false;
                    await this.UploadFile(openFileDialog.FileName);
                });
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath"></param>
        public async Task UploadFile(string filePath)
        {
            this.Message = string.Empty;
            this.Progress = 50;
            await Task.Delay(100);
            //string url = this.blogOperation.NewMediaObject(filePath).url;
            //System.Windows.Clipboard.SetText(url);
            MessageBox.Show(filePath);
            this.Progress = 100;
            this.Message = "上传成功";
        }

        #endregion
    }
}
