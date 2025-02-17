﻿# FusionCache

![FusionCache logo](https://raw.githubusercontent.com/ZiggyCreatures/FusionCache/main/docs/logo-256x256.png)

| 🙋‍♂️ Updating from before `v0.24.0` ? please [read here](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Update_v0_24_0.md). |
|:-------|

## FusionCache is an easy to use, fast and robust cache with advanced resiliency features and an optional distributed 2nd layer.

It was born after years of dealing with all sorts of different types of caches: memory caching, distributed caching, http caching, CDNs, browser cache, offline cache, you name it. So I've tried to put together these experiences and came up with FusionCache.

![FusionCache diagram](https://raw.githubusercontent.com/ZiggyCreatures/FusionCache/main/docs/images/diagram.png)

It uses a memory cache (any impl of the standard `IMemoryCache` interface) as the **primary** backing store and optionally a distributed, 2nd level cache (any impl of the standard `IDistributedCache` interface) as a **secondary** backing store for better resilience and higher performance, for example in a multi-node scenario or to avoid the typical effects of a cold start (initial empty cache, maybe after a restart).

Optionally, it can also use a **backplane**: in a multi-node scenario this will send notifications to the other nodes to keep each node's memory cache perfectly synchronized, without any additional work.

FusionCache also includes some advanced resiliency features like a **fail-safe** mechanism, **cache stampede** prevention, fine grained **soft/hard timeouts** with **background factory completion**, customizable **extensive logging** and more (see below).

## 🏆 Award

On August 2021, FusionCache received the [Google Open Source Peer Bonus Award](https://twitter.com/jodydonetti/status/1422550932433350666): here is the [official blogpost](https://opensource.googleblog.com/2021/09/announcing-latest-open-source-peer-bonus-winners.html).

## 📕 Getting Started

With [🦄 A Gentle Introduction](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/AGentleIntroduction.md) you'll get yourself comfortable with the overall concepts.

Want to start using it immediately? There's a [⭐ Quick Start](https://github.com/ZiggyCreatures/FusionCache/blob/main/README.md#-quick-start) for you.

Curious about what you can achieve from start to finish? There's a [👩‍🏫 Step By Step ](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/StepByStep.md) guide.

In search of all the docs? There's a [page](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/README.md) for that, too.

More into videos? The great Anna Hoffman has been so nice to listen to me mumble random stuff on [Data Exposed](https://learn.microsoft.com/en-us/shows/data-exposed/caching-made-easy-in-azure-sql-db-with-fusioncache-data-exposed).

[![Data Exposed Talk](https://raw.githubusercontent.com/ZiggyCreatures/FusionCache/main/docs/images/talk-data-exposed.jpg)](https://www.youtube.com/watch?v=V2fCUoJgVAo)

## ✔ Features
These are the **key features** of FusionCache:

- [**🛡️ Cache Stampede prevention**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/CacheStampede.md): automatic protection from the Cache Stampede problem
- [**🔀 Optional 2nd level**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/CacheLevels.md): an optional 2nd level handled transparently, with any implementation of `IDistributedCache`
- [**💣 Fail-Safe**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/FailSafe.md): a mechanism to avoids transient failures, by reusing an expired entry as a temporary fallback
- [**⏱ Soft/Hard timeouts**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Timeouts.md): a slow factory (or distributed cache) will not slow down your application, and no data will be wasted
- [**📢 Backplane**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Backplane.md): in a multi-node scenario, it can notify the other nodes about changes in the cache, so all will be in-sync
- [**↩️ Auto-Recovery**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/AutoRecovery.md): automatic handling of transient issues with retries and sync logic
- [**🧙‍♂️ Adaptive Caching**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/AdaptiveCaching.md): for when you don't know upfront the cache duration, as it depends on the value being cached itself
- [**🔂 Conditional Refresh**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/ConditionalRefresh.md): like HTTP Conditional Requests, but for caching
- [**🦅 Eager Refresh**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/EagerRefresh.md): start a non-blocking background refresh before the expiration occurs
- [**🔃 Dependency Injection**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/DependencyInjection.md): native support for Dependency Injection, with a nice fluent interface including a Builder support
- [**📛 Named Caches**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/NamedCaches.md): easily work with multiple named caches, even if differently configured
- [**💫 Natively sync/async**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/CoreMethods.md): native support for both the synchronous and asynchronous programming model
- [**📞 Events**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Events.md): a comprehensive set of events, both at a high level and at lower levels (memory/distributed)
- [**🧩 Plugins**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Plugins.md): extend FusionCache with additional behavior like adding support for metrics, statistics, etc...
- [**📜 Logging**](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Logging.md): comprehensive, structured and customizable, via the standard `ILogger` interface

## ⭐ Quick Start

FusionCache can be installed via the nuget UI (search for the `ZiggyCreatures.FusionCache` package) or via the nuget package manager console:

```PowerShell
PM> Install-Package ZiggyCreatures.FusionCache
```

As an example, imagine having a method that retrieves a product from your database:

```csharp
Product GetProductFromDb(int id) {
	// YOUR DATABASE CALL HERE
}
```

💡 This is using the **sync** programming model, but it would be equally valid with the newer **async** one for even better performance.

To start using FusionCache the first thing is create a cache instance:

```csharp
var cache = new FusionCache(new FusionCacheOptions());
```

If instead you are using **DI (Dependency Injection)** use this:

```csharp
services.AddFusionCache();
```

We can also specify some global options, like a default `FusionCacheEntryOptions` object to serve as a default for each call we'll make, with a duration of `2 minutes` and a `Low` priority:

```csharp
var cache = new FusionCache(new FusionCacheOptions() {
	DefaultEntryOptions = new FusionCacheEntryOptions {
		Duration = TimeSpan.FromMinutes(2),
		Priority = CacheItemPriority.Low
	}
});
```

Or, using DI, like this:

```csharp
services.AddFusionCache()
	.WithDefaultEntryOptions(new FusionCacheEntryOptions {
		Duration = TimeSpan.FromMinutes(2),
		Priority = CacheItemPriority.Low
	})
;
```

Now, to get the product from the cache and, if not there, get it from the database in an optimized way and cache it for `30 sec` simply do this:

```csharp
var id = 42;

cache.GetOrSet<Product>(
	$"product:{id}",
	_ => GetProductFromDb(id),
	TimeSpan.FromSeconds(30)
);
```

That's it 🎉

## 🖥️ Simulator

Distributed systems are, in general, quite complex to understand.

When using FusionCache with the [distributed cache](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/CacheLevels.md), the [backplane](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/Backplane.md) and [auto-recovery](https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/AutoRecovery.md) the Simulator can help us **seeing** the whole picture.

[![FusionCache Simulator](https://raw.githubusercontent.com/ZiggyCreatures/FusionCache/main/docs/images/fusioncache-simulator-autorecovery.png)](docs/Simulator.md)

## 🧰 Supported Platforms

FusionCache targets `.NET Standard 2.0` so any compatible .NET implementation is fine: this means `.NET Framework` (the old one), `.NET Core 2+` and `.NET 5/6+` (the new ones), `Mono` 5.4+ and more (see [here](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support) for a complete rundown).

**NOTE**: if you are running on **.NET Framework 4.6.1** and want to use **.NET Standard** packages Microsoft suggests to upgrade to .NET Framework 4.7.2 or higher (see the [.NET Standard Documentation](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-implementation-support)) to avoid some known dependency issues.

## 💼 Is it Production Ready :tm: ?
Yes!

Even though the current version is `0.X` for an excess of caution, FusionCache is already used **in production** on multiple **real world projects** happily handling millions of requests per day, or at least these are the projects I'm aware of.

Considering that the FusionCache packages have been downloaded more than **2 million times** (thanks everybody!) it may very well be used even more.

And again, if you are using it please [**✉ drop me a line**](https://twitter.com/jodydonetti), I'd like to know!
