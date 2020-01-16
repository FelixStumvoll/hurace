using System.Transactions;

namespace Hurace.Core.Service.Util
{
    public static class ScopeBuilder
    {
        public static TransactionScope BuildTransactionScope() =>
            new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}