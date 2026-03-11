namespace TenancyService.src.Domain.Enums
{
    public static class DepositTransactionType
    {
        public const string Deposit = "deposit";
        public const string Refund = "refund";
        public const string Deduct = "deduct";

        public static readonly IReadOnlyList<string> All = new[] { Deposit, Refund, Deduct };
    }
}
