using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaWeblogAPI
{
    public struct BlogInfo
    {
        public string blogid;
        public string url;
        public string blogName;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public int length;
        public string type;
        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public string name;
        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public DateTime dateCreated;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string description;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string title;

        public string[] categories;
        public Enclosure enclosure;
        public string link;
        public string permalink;
        [XmlRpcMember(
           Description = "Not required when posting. Depending on server may "
           + "be either string or integer. "
           + "Use Convert.ToInt32(postid) to treat as integer or "
           + "Convert.ToString(postid) to treat as string")]
        public object postid;
        public Source source;
        public string userid;

        public object mt_allow_comments;
        public object mt_allow_pings;
        public object mt_convert_breaks;
        public string mt_text_more;
        public string mt_excerpt;

        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString());
            sb.Append(" dateCreated=" + dateCreated);
            sb.Append(" userid=" + userid);
            sb.Append(" title=" + title);
            sb.AppendLine(" description=" + description.Truncate(200));
            sb.AppendLine(" categories=" + string.Join(",", categories));

            return sb.ToString();
        }
    }

    public struct CategoryInfo
    {
        public string description;
        public string htmlUrl;
        public string rssUrl;
        public string title;
        public string categoryid;
    }

    public struct Category
    {
        public string categoryId;
        public string categoryName;
    }

    public struct FileData
    {
        public byte[] bits;
        public string name;
        public string type;
    }

    public struct UrlData
    {
        public string url;
    }
}
