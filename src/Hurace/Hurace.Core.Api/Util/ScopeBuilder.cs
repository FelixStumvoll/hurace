using System.Transactions;

namespace Hurace.Core.Api.Util
{
    public static class ScopeBuilder
    {
        public static TransactionScope BuildTransactionScope() =>
            new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}