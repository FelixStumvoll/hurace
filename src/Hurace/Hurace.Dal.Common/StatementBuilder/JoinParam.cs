namespace Hurace.Dal.Common.StatementBuilder
{
    public class JoinParam
    {
        public string ForeignColumn { get; set; }
        public string SelfColumn { get; set; }

        private JoinParam(string foreignColumn, string selfColumn)
        {
            ForeignColumn = foreignColumn;
            SelfColumn = selfColumn;
        }

        public static implicit operator JoinParam((string selfColumn, string foreignColumn) joinParam) =>
            new JoinParam(joinParam.foreignColumn, joinParam.selfColumn);
    }
}