# Fun 🐱

[![Build Status](https://img.shields.io/travis/sungiant/fun)][status]
[![License](https://img.shields.io/github/license/sungiant/fun)][mit]
[![Nuget Version](https://img.shields.io/nuget/v/Fun)][nuget]
[![Nuget Downloads](https://img.shields.io/nuget/dt/Fun)][nuget]

## Overview

Fun is simple functional programming library written in C#, inspired by Scala.

## Getting started

Fun is available as a stand-alone library via [nuget][nuget].  Here's an example nuget `packages.config` file that pulls in Fun:

```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Fun" version="1.0.0" targetFramework="net45" />
</packages>
```


## Example usage:

Fluent function application:

```cs
"945bd525-d80e-4055-a762-7256b4b38403".Apply (Guid.Parse);
```

Immutable `Unit` data type, the empty tuple `()`:

```cs
Func<String, Unit> print = (z) => {
	Console.WriteLine (z);
	return Unit.Value;
};
```

Fluent `if` - `else` statements:

```cs
Int32 x = 42;

If.Else (	
	x % 2 == 0, () => "x is even",
	/* else */  () => "x is odd")
	.Apply (print);

If.Else (
	x % 3 == 0, () => "x is a multiple of three",
	x % 2 == 0, () => "x is even",
	/* else */  () => "x is odd")
	.Apply (print);
```

Immutable `Option` data type:

```cs
var oh = Option.Apply ("hello");
var ow = Option.Apply ("world");

oh.FlatMap ((h) => ow.Map ((w) => h + " " + w))
    .ValueOrElse ("foobar")
	.Apply (print);

Option.Apply (Console.ReadLine ())
	.Match (
	    (z) => z,
	    () => "Read line returned null!")
	.Apply (print);
```

Immutable `Either` data type:

```cs
Either.Left <String, Int32>("foobar")
	.MapLeft (z => z.ToUpper ())
	.Left
	.ValueOrElse ("!")
	.Apply (print);

Either.Right <String, Int32> (42)
	.Match (
		(left) => left.Length,
		(right) => right * 2)
	.ToString ()
	.Apply (print);
```

Immutable `Try` data type:

```cs
Try.Apply (() => Environment.GetEnvironmentVariable ("HOME"))
	.Match (
		(success) => success,
		(failure) => "foobar")
	.Apply (print);


Try.Apply (() => Environment.GetEnvironmentVariable ("HOME"))
	.Flatten ((failure) => "foobar")
	.Apply (print);
	
```

## License

Fun is licensed under the **[MIT License][mit]**; you may not use this software except in compliance with the License.

[status]: https://travis-ci.org/sungiant/fun
[mit]: https://github.com/sungiant/fun/blob/master/LICENSE
[nuget]: https://www.nuget.org/packages/Fun/
