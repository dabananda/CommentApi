namespace CommentApi.Common
{
    public class GlobalSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string SqlServer { get; set; }
    }
}
