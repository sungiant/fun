## Fun

[![Build Status](https://travis-ci.org/sungiant/fun.png?branch=master)](https://travis-ci.org/sungiant/fun)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg)](https://github.com/sungiant/fun/blob/master/LICENSE)

## Overview

Fun is simple functional programming library written in C#, inspired by Scala.

## Why?

For fun, of course!

## Example usage:

```cs

/**
 * Fluent function application:
 */

"945bd525-d80e-4055-a762-7256b4b38403".Apply (Guid.Parse);


/**
 * Immutable `Unit` data type:
 */

Int32 x = 42;

Func<String, Unit> print = (z) => {
	Console.WriteLine (z);
	return Unit.Value;
};

x.ToString ().Apply (print);


/**
 * Fluent `if` - `else` statments:
 */

If.Else (	
	x % 2 == 0, () => "x is even",
	/* else */  () => "x is odd")
	.Apply (print);

If.Else (
	x % 3 == 0, () => "x is a multiple of three",
	x % 2 == 0, () => "x is even",
	/* else */  () => "x is odd")
	.Apply (print);


/**
 * Immutable `Option` data type:
 */

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


/**
 * Immutable `Either` data type:
 */

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


/**
 * Immutable `Try` data type:
 */

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

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

[mit]: https://github.com/sungiant/fun/blob/master/LICENSE
