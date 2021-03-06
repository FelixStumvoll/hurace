﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hurace.Dal.Domain.Attributes;

namespace Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder
{
    public abstract class AbstractStatementBuilder
    {
        private readonly string _schemaName;

        internal AbstractStatementBuilder(string schemaName) => _schemaName = schemaName;

        protected static string AddParamSymbol(string str) => $"@{str}";
        
        protected static QueryParam AddParamSymbol(QueryParam queryParam)
        {
            queryParam.Name = AddParamSymbol(queryParam.Name);
            return queryParam;
        }
        
        protected string WithSchema(string append) => $"{_schemaName}.{append}";

        protected static IEnumerable<(string name, object value)> GetNonNavigationalProps(object obj, bool withKey) =>
            obj.GetType()
               .GetProperties()
               .Where(pi => !Attribute.IsDefined(pi, typeof(NavigationalAttribute)) &&
                            (withKey || !Attribute.IsDefined(pi, typeof(KeyAttribute))) && pi.GetValue(obj) != null)
               .Select(pi => (pi.Name , pi.GetValue(obj)!));
    }
}