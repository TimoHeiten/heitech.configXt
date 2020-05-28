namespace heitech.configXt.Core.Queries
{
    public class QueryContext : ConfigurationContext
    {
        public QueryTypes QueryType { get; }

         public QueryContext(string admin, string queryName, QueryTypes types, IStorageModel model)
            : base(admin, model)
        {
            QueryType = types;
            ConfigName = queryName;
        }
    }
}