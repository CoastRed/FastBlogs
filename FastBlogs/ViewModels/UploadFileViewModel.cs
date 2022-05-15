using FastBlogs.Commom;
using MetaWeblogAPI;
using NLog;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FastBlogs.ViewModels
{
    public class UploadFileViewModel : BindableBase
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

        public UploadFileViewModel(BlogOperation blogOperation, ILogger logger)
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
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            this.Message = string.Empty;
            this.Progress = 20;
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Extension != ".md")
            {
                this.Progress = 0;
                return;
            }
            string fileContent = File.ReadAllText(filePath);
            string? fileDir = fileInfo.DirectoryName;
            this.Progress = 30;
            //提取文件中的图片
            List<string> imgList = new List<string>();
            string MatchRule = @"!\[.*?\]\((.*?)\)";
            MatchCollection? matchResult = Regex.Matches(fileContent, MatchRule, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            foreach (Match match in matchResult) imgList.Add(match.Groups[1].Value);
            this.Progress = 50;
            //循环上传图片，如果已经是网络图片则不上传
            Dictionary<string, string> ReplaceDic = new Dictionary<string, string>();
            foreach (string img in imgList)
            {
                if (img.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    this.logger.Info($"网络图片跳过：{img} ");
                    continue;
                }
                try
                {
                    var imgPhyPath = Path.Combine(fileDir!, img);
                    if (File.Exists(imgPhyPath))
                    {
                        string imgUrl = this.blogOperation.NewMediaObject(imgPhyPath).url;
                        if (!ReplaceDic.ContainsKey(img)) ReplaceDic.Add(img, imgUrl);
                        this.logger.Info($"{img} 上传成功. {imgUrl}");
                    }
                    else
                    {
                        this.logger.Info($"{img} 未发现文件.");
                    }
                }
                catch (Exception e)
                {
                    this.logger.Error(e);
                    return;
                }
            }
            this.Progress = 70;
            //替换文件中的本地链接为网络链接
            fileContent = ReplaceDic.Keys.Aggregate(fileContent, (current, key) => current.Replace(key, ReplaceDic[key]));

            var newFileName = filePath.Substring(0, filePath.LastIndexOf('.')) + "-cnblog" + Path.GetExtension(filePath);
            File.WriteAllText(newFileName, fileContent, FileEncodingType.GetType(filePath));

            this.logger.Info($"处理完成！文件保存在：{newFileName}");

            await Task.Delay(100);
            this.Progress = 100;
            this.Message = "上传成功";
        }

        #endregion
    }
}
