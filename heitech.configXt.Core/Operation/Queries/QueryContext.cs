using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core
{
    public class QueryContext : ConfigurationContext
    {
        public QueryTypes QueryType { get; }
        public ConfigRequest Request { get; }

         public QueryContext(string admin, QueryTypes types, ConfigRequest request, IStorageModel model)
            : base(admin, model)
        {
            Check(request);
            QueryType = types;
            Request = request;
        }
    }
}