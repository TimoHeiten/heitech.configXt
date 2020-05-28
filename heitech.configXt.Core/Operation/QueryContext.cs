using System;
using System.Linq;
using heitech.configXt.Core.Entities;

namespace heitech.configXt.Core.Operation
{
    public class QueryContext : ConfigurationOperationContext
    {
        public QueryTypes QueryType { get; }
        public ConfigRequest Request { get; }

         public QueryContext(AdministratorEntity admin, QueryTypes types, ConfigRequest request, IStorageModel model)
            : base(admin, model)
        {
            Check(request);
            QueryType = types;
            Request = request;
        }
    }
}