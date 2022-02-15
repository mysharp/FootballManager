namespace Restful.ViewModels
{
    public class LinkResource
    {
        public string Link { get; set; }

        public string Group { get; set; }

        public string Method { get; set; }

        public LinkResource() { }

        public LinkResource(string link, string group, string method) : this()
        { 
            this.Link = link;
            this.Group = group;
            this.Method = method;
        }
    }
}
