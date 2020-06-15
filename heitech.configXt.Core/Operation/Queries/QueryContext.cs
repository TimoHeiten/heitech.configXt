namespace heitech.configXt.Core.Queries
{
    ///<summary>
    /// QueryContext class, subtype to ConfigurationContext
    ///</summary>
    public class QueryContext : ConfigurationContext
    {
        public QueryTypes QueryType { get; }

        public const string QUERY_ALL = "";
        public QueryContext(string queryName, QueryTypes types, IStorageModel model)
            : base(model)
        {
            QueryType = types;
            ConfigName = queryName;
        }
    }
}