using MetaWeblogAPI;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace FastBlogs.ViewModels
{
    public class UploadImgViewModel : BindableBase
    {

        private readonly BlogOperation blogOperation;
        private readonly ILogger logger;


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
                    _ = this.UploadImage(value);
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

        public UploadImgViewModel(BlogOperation blogOperation, ILogger logger)
        {
            this.blogOperation = blogOperation;
            this.logger = logger;
        }

        #endregion


        #region 命令

        public DelegateCommand UploadImgCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog()
                    {
                        Filter = "图片文件 |*.gif;*.jpg;*.png"
                    };
                    var result = openFileDialog.ShowDialog();
                    openFileDialog.Multiselect = false;
                    await this.UploadImage(openFileDialog.FileName);
                });
            }
        }

        /// <summary>
        /// 剪贴板图片上传
        /// </summary>
        public DelegateCommand ClipboardUploadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    //System.Windows.IDataObject iData = System.Windows.Clipboard.GetDataObject();
                    //if (iData.GetDataPresent(System.Windows.DataFormats.Tiff)
                    //|| iData.GetDataPresent(System.Windows.DataFormats.MetafilePicture)
                    //|| iData.GetDataPresent(System.Windows.DataFormats.EnhancedMetafile))
                    //{
                    //    //textBox2.Text = (String)iData.GetData(System.Windows.DataFormats.Text);
                    //}
                    System.Windows.Media.Imaging.BitmapSource? bmap = System.Windows.Clipboard.GetImage();
                    if (bmap == null)
                    {
                        return;
                    }
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        BitmapEncoder encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bmap));
                        encoder.Save(ms);
                        System.Drawing.Bitmap? bitmap = new System.Drawing.Bitmap(ms);
                        bitmap.Save(Path.Combine("Image", DateTime.Now.ToString(), ".png"));
                    }
                });
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="filePath"></param>
        public async Task UploadImage(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            List<string> imgTypes = new List<string>() { ".jpg", ".png", ".gif", ".icon" };
            if (!imgTypes.Contains(fileInfo.Extension.ToLower()))
            {
                this.logger.Info($"{filePath}文件不是图片类型");
                this.Progress = 0;
                return;
            }
            this.Message = string.Empty;
            this.Progress = 50;
            await Task.Delay(100);
            string url = this.blogOperation.NewMediaObject(filePath).url;
            System.Windows.Clipboard.SetText(url);
            this.logger.Info($"上传{filePath}图片成功，URL为{url}");
            this.Progress = 100;
            this.Message = "上传成功，已复制到剪贴板";
        }

        #endregion
    }
}
