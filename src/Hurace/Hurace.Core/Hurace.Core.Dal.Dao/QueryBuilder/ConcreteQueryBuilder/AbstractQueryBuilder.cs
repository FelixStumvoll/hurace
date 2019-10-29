using System;
using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Common;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public abstract class AbstractQueryBuilder
    {
        private readonly string _schemaName;

        internal AbstractQueryBuilder(string schemaName) => _schemaName = schemaName;

        protected string WithSchema(string append) => $"{_schemaName}.{append}";
        
        protected static IEnumerable<(string name, object value)> GetCrudProperties(object obj) =>
            obj.GetType()
                .GetProperties()
                .Where(pi => pi.Name != "Id" && !Attribute.IsDefined(pi, typeof(NavigationalAttribute)))
                .Select(pi => (pi.Name, pi.GetValue(obj)));
    }
}