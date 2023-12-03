using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FastCache;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace ZiggyCreatures.Caching.Fusion.Benchmarks
{
	[MemoryDiagnoser]
	[ShortRunJob(RuntimeMoniker.Net70)]
	[ShortRunJob(RuntimeMoniker.Net80)]
	public class HappyPathBenchmark
	{
		const string Key = "test key";
		const string Value = "test value";

		readonly FusionCache Cache = new(new FusionCacheOptions());
		readonly MemoryCache MemoryCache = new(new MemoryCacheOptions());
		readonly CachingService LazyCache = new();

		[GlobalSetup]
		public void Setup()
		{
			Cache.Set(Key, Value);
			MemoryCache.Set(Key, Value);
			LazyCache.Add(Key, Value);
			Cached<string>.Save(Key, Value, TimeSpan.FromDays(1));
		}

		[Benchmark(Baseline = true)]
		public string GetFusionCache()
		{
			var result = Cache.TryGet<string>(Key);
			return result.HasValue ? result.Value : Unreachable();
		}

		[Benchmark]
		public string GetMemoryCache()
		{
			return MemoryCache.TryGetValue<string>(Key, out var value)
				? value : Unreachable();
		}

		[Benchmark]
		public string GetLazyCache()
		{
			return LazyCache.TryGetValue<string>(Key, out var value)
				? value : Unreachable();
		}

		[Benchmark]
		public string GetFastCache()
		{
			return Cached<string>.TryGet(Key, out var value)
				? value : Unreachable();
		}

		[Benchmark]
		public void SetFusionCache() => Cache.Set(Key, Value);

		[Benchmark]
		public void SetMemoryCache() => MemoryCache.Set(Key, Value);

		[Benchmark]
		public void SetLazyCache() => LazyCache.Add(Key, Value);

		[Benchmark]
		public void SetFastCache() => Cached<string>.Save(Key, Value, TimeSpan.FromDays(1));

		[DoesNotReturn]
		static string Unreachable() => throw new Exception("Unreachable code");
	}
}
