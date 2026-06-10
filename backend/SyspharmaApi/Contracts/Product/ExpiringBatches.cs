namespace SyspharmaApi.Contracts.Product
{
    public sealed record ExpiringBatches(
        int IdProduct,
        int IdBatch,
        string ProductInternalCode,
        string? Description,
        string BatchCode,
        int ExpiresInDays
        );
}
