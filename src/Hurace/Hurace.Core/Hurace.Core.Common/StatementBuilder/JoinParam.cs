namespace Hurace.Core.Common.StatementBuilder
{
    public class JoinParam
    {
        public string ForeignColumn { get; set; } = default!;
        public string SelfColumn { get; set; } = default!;

        public static implicit operator JoinParam((string selfColumn, string foreignColumn) joinParam) => new JoinParam
        {
            ForeignColumn = joinParam.foreignColumn,
            SelfColumn = joinParam.selfColumn
        };
    }
}