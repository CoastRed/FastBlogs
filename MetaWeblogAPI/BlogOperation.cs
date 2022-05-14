using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookComputing.XmlRpc;

namespace MetaWeblogAPI
{
    public class BlogOperation
    {
        private readonly IMetaWeblog proxy;
        public string? url;
        public string? blogid;
        public string? username;
        public string? password;

        public BlogOperation(string url, string blogid, string username, string password)
        {
            this.url = url;
            this.blogid = blogid;
            this.username = username;
            this.password = password;
            this.proxy = (IMetaWeblog)XmlRpcProxyGen.Create(typeof(IMetaWeblog));
            XmlRpcClientProtocol cp = (XmlRpcClientProtocol)proxy;
            cp.Url = url;
            //cp.Url = "https://rpc.cnblogs.com/metaweblog/xcoast";
            this.blogid = this.GetUsersBlogs(string.Empty, this.username, this.password)[0].blogid;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="appKey">博客园此参数为空</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public BlogInfo[] GetUsersBlogs(string appKey, string username, string password)
        {
            return proxy.GetUsersBlogs(appKey, username, password);
        }

        //public UrlData NewMediaObject(string filePath)
        //{
        //    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中
        //    return this.NewMediaObject(filePath, imgBytesIn);
        //}

        public UrlData NewMediaObject(string filePath)
        {
            if (string.IsNullOrEmpty(this.blogid) || string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.password))
            {
                throw new ArgumentException("博客Id、用户名或密码无效");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string name = new FileInfo(filePath).Name;
            string type = MimeMapping.GetMimeMapping(filePath);
            
            FileData data = new FileData()
            {
                bits = fileBytes,
                name = name,
                type = type,
            };
            return this.proxy.newMediaObject(this.blogid, this.username, this.password, data);
        }

    }
}
