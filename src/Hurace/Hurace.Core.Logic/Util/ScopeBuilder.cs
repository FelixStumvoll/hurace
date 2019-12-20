using System.Transactions;

namespace Hurace.Core.Logic.Util
{
    public static class ScopeBuilder
    {
        public static TransactionScope BuildTransactionScope() =>
            new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}