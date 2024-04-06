using Redis.OM.Modeling;

namespace Benchmarks;

[Document(StorageType = StorageType.Json, Prefixes = ["dto:"], IndexName = "idx:dto")]
public class Dto
{
    [RedisIdField] public int Id { get; set; }

    [Indexed(Sortable = true)] public string Name { get; set; } = string.Empty;

    public string AccountNumber { get; set; }
    public string Iban { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    [Indexed] public decimal Balance { get; set; }
}
