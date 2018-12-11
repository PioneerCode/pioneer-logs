using System.Runtime.Serialization;

namespace Pioneer.Logs.Models
{
    [DataContract]
    public class Index
    {
        public string Health { get; set; }
        public string Status { get; set; }
        [DataMember(Name = "index")]
        public string IndexName { get; set; }
        public string UUid { get; set; }
        public string Pri { get; set; }
        public string Rep { get; set; }
        [DataMember(Name = "docs.count")]
        public string DocsCount { get; set; }
        [DataMember(Name = "docs.deleted")]
        public string DocsDeleted { get; set; }
        [DataMember(Name = "store.size")]
        public string StorSize { get; set; }
        [DataMember(Name = "pri.store.size")]
        public string PriStorSize { get; set; }
    }
}
