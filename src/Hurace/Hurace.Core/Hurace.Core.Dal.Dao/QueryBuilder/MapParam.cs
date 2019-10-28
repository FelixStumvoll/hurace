namespace Hurace.Core.Dal.Dao.QueryBuilder
{
    public class MapParam
    {
        public string ForeignColumn { get; set; }
        public string SelfColumn { get; set; }

        public static implicit operator MapParam((string foreignColumn, string selfColumn) joinParam) => new MapParam()
        {
            ForeignColumn = joinParam.foreignColumn,
            SelfColumn = joinParam.selfColumn
        };
    }
}