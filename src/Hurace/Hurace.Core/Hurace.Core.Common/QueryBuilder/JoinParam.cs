namespace Hurace.Core.Dal.Dao.QueryBuilder
{
    public class JoinParam
    {
        public string ForeignColumn { get; set; }
        public string SelfColumn { get; set; }

        public static implicit operator JoinParam((string selfColumn, string foreignColumn) joinParam) => new JoinParam
        {
            ForeignColumn = joinParam.foreignColumn,
            SelfColumn = joinParam.selfColumn
        };
    }
}