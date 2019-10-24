using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Hurace.Core.Common;

namespace Hurace.Core.Dal.Dao
{
    public static class DbCommandExtensions
    {
        public static void AddParameters(this DbCommand command, IEnumerable<QueryParam> parameters)
        {
            foreach (var param in parameters)
            {
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = param.Name;
                dbParam.Value = param.Value;
                command.Parameters.Add(dbParam);
            }
        }
    }
}