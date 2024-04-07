using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Bogus;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;
using Redis.OM;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace Benchmarks;

[MemoryDiagnoser]
public class SearchBenchmark
{
    private RedisConnectionProvider? _provider;
    private ConnectionMultiplexer? _redis;
    private string? _redisConnectionString;
    private RedisContainer? _redisContainer;

    [GlobalCleanup(Targets = [nameof(NRedisStack), nameof(RedisOM)])]
    public void Cleanup()
    {
        if (_redisContainer != null)
        {
            _redisContainer.StopAsync().Wait();
            _redisContainer.DisposeAsync().GetAwaiter().GetResult();
        }
    }

    [Params(2000, 10000)] public int DataAmount { get; set; }
    [Params(100, 1000)] public int PageSize { get; set; }

    [GlobalSetup(Target = nameof(NRedisStack))]
    public void NRedisStackSetup()
    {
        StartRedis();
        GenerateData();

        _redis!.GetDatabase().FT()
            .Create("idx:dto",
                FTCreateParams.CreateParams()
                    .On(IndexDataType.JSON)
                    .AddPrefix("dto:"),
                new Schema()
                    .AddTextField(FieldName.Of("$.Name").As("name"), sortable: true)
                    .AddNumericField(FieldName.Of("$.Balance").As("balance")));
    }

    [GlobalSetup(Target = nameof(RedisOM))]
    public void RedisOMSetup()
    {
        StartRedis();
        GenerateData();

        _provider = new RedisConnectionProvider(_redis!);
        _provider.Connection.CreateIndex(typeof(Dto));
    }

    [Benchmark]
    public List<Dto> NRedisStack()
    {
        var list = _redis!.GetDatabase().FT()
            .Search("idx:dto", new Query("@balance:[500,1000]").SetSortBy("name", true).Limit(0, PageSize)).Documents
            .Select(document => JsonSerializer.Deserialize<Dto>(document["json"]!)!)
            .ToList();
        return list;
    }

    [Benchmark]
    public List<Dto> RedisOM()
    {
        var list = _provider!.RedisCollection<Dto>()
            .Where(dto => dto.Balance >= 500 && dto.Balance <= 1000)
            .OrderBy(dto => dto.Name)
            .Take(PageSize)
            .ToList();
        return list;
    }

    private void StartRedis()
    {
        _redisContainer = new RedisBuilder()
            .WithImage("redis/redis-stack:7.2.0-v10")
            .Build();
        _redisContainer.StartAsync().Wait();

        _redisConnectionString = _redisContainer.GetConnectionString();
        _redis = ConnectionMultiplexer.Connect(_redisConnectionString!);
    }

    private void GenerateData()
    {
        var list = new Faker<Dto>()
            .UseSeed(1337)
            .RuleFor(x => x.Id, f => ++f.IndexVariable)
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.AccountNumber, f => f.Finance.Account())
            .RuleFor(x => x.Iban, f => f.Finance.Iban())
            .RuleFor(x => x.IsActive, f => f.Random.Bool())
            .RuleFor(x => x.Balance, f => f.Finance.Amount())
            .Generate(DataAmount);

        var db = _redis!.GetDatabase();

        foreach (var dto in list)
        {
            db.JSON().Set("dto:" + dto.Id, "$", dto);
        }
    }
}
