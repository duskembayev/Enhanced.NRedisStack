using System.Text.Json.Serialization;

namespace SchemaForPlainJson;

public class Account
{
    public string Iban { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
}