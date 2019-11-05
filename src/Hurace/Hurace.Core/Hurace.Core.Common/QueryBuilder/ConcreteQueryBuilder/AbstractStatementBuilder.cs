using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder
{
    public abstract class AbstractStatementBuilder
    {
        private readonly string _schemaName;

        internal AbstractStatementBuilder(string schemaName) => _schemaName = schemaName;

        protected string WithSchema(string append) => $"{_schemaName}.{append}";

        protected static IEnumerable<(string name,object value)> GetNonNavigationalProps(object obj, bool withKey) =>
            obj.GetType()
               .GetProperties()
               .Where(pi => !Attribute.IsDefined(pi, typeof(NavigationalAttribute)) &&
                            (withKey || !Attribute.IsDefined(pi, typeof(KeyAttribute))))
               .Select(pi => (pi.Name, pi.GetValue(obj)));
    }
}