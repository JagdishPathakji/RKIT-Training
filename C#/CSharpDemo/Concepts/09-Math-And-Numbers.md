# 09 - Math and Numbers

## Begin with the quantity you need to represent

Choosing a numeric type is a design decision. Ask whether the value is whole or fractional, what range is valid, whether decimal exactness matters, and what should happen on overflow. A count, scientific measurement, monetary amount, and cryptographic random value have different needs.

## Integral types and ranges

Integral types store whole numbers. Signed types represent negative and positive values; unsigned types represent zero and positive values only.

| Type | Bits | Approximate range |
|---|---:|---:|
| `sbyte` | 8 | -128 to 127 |
| `byte` | 8 | 0 to 255 |
| `short` | 16 | -32,768 to 32,767 |
| `ushort` | 16 | 0 to 65,535 |
| `int` | 32 | about -2.1 billion to 2.1 billion |
| `uint` | 32 | 0 to about 4.29 billion |
| `long` | 64 | about -9.22 quintillion to 9.22 quintillion |
| `ulong` | 64 | 0 to about 18.44 quintillion |

`int` is conventional for ordinary counts and indexes when its range is sufficient. Use a wider type when the domain can exceed that range. Unsigned types are not automatically better for nonnegative quantities: they can make interoperability and arithmetic less convenient.

Use `BigInteger` from `System.Numerics` when integers may exceed fixed-width limits. It grows as needed, with additional memory and computation costs:

```csharp
using System.Numerics;

BigInteger veryLarge = BigInteger.Parse("123456789012345678901234567890");
```

## Floating-point types and precision

`float` and `double` use binary floating-point representation. They support a wide range and fractional calculations, but most decimal fractions cannot be represented exactly in binary.

| Type | Approximate significant decimal digits | Typical use |
|---|---:|---|
| `float` | 6-9 | memory-sensitive graphics or APIs requiring single precision |
| `double` | 15-17 | general scientific and measurement calculations |
| `decimal` | 28-29 | decimal quantities where base-10 rounding behavior is useful |

These are approximate precision descriptions, not promises that every calculation has that many correct digits. `double` is usually the default floating-point choice. `float` uses less storage but has lower precision. `decimal` has a smaller range than `double` and is not interchangeable with either binary float.

### Why `0.1 + 0.2` may not equal `0.3`

The decimal fractions 0.1 and 0.2 have repeating representations in binary floating point. Their stored approximations are added, so the result can differ slightly from exact decimal 0.3:

```csharp
using System;
using System.Globalization;

internal class Program
{
	private static void Main()
	{
double result = 0.1 + 0.2;
Console.WriteLine(result.ToString("R", CultureInfo.InvariantCulture));
// Common output: 0.30000000000000004
	}
}
```

When approximate equality is intended, compare with a tolerance appropriate to the scale, units, and error budget:

```csharp
bool nearlyEqual = Math.Abs(actual - expected) < tolerance;
```

There is no universal tolerance. Also consider special values such as NaN and infinity; use `double.IsNaN` and `double.IsInfinity` when they are possible.

## `decimal` and money-like quantities

`decimal` is designed for decimal fractions and can avoid many binary-fraction surprises:

```csharp
decimal decimalResult = 0.1m + 0.2m; // Exactly 0.3m for these values.
```

The `m` suffix makes a literal `decimal`; without it, a decimal-looking literal such as `0.1` is a `double`. `decimal` and `double` do not mix freely; convert deliberately.

`decimal` is often useful for prices, but it does not define currency, scale, tax/discount rules, or rounding policy. Currency is not always two fractional digits. Avoid converting through `double` when exact decimal behavior matters.

## Literals, conversions, and type promotion

Numeric suffixes select a literal type, and underscores improve readability without changing the value:

```csharp
long population = 8_000_000_000L;
float scale = 0.75f;
double measurement = 0.75;
decimal price = 12.50m;
```

An **implicit conversion** is allowed without a cast. An **explicit conversion** uses a cast and may lose information or overflow:

```csharp
double measurement = 12; // int converts to double.
int truncated = (int)12.9; // Explicit conversion truncates toward zero.
```

Even an implicit conversion such as a large `int` to `float` can lose precision. An explicit cast does not prove the value fits or is exact.

Arithmetic result types follow C# rules. Converting after an operation does not prevent overflow that already happened. Promote an operand before a calculation that needs a wider intermediate:

```csharp
int left = int.MaxValue;
int right = 1;
long wideResult = (long)left + right;
```

## Division and remainder

When both operands are integral, division returns an integral result and truncates toward zero:

```csharp
int quotient = 7 / 2;       // 3
double ratio = 7.0 / 2.0;   // 3.5
double promoted = 7 / 2.0;  // 3.5
```

`%` is the remainder operator: `7 % 2` is `1`. For negative operands, C# remainder follows the dividend's sign; do not assume it is mathematical modulo for every negative value. Integer division by zero throws `DivideByZeroException`. Floating-point division by zero can produce infinity or NaN instead.

## Overflow: `checked` and `unchecked`

**Overflow** occurs when a result is outside a fixed-width integral type's range. In a checked context, an overflowing integral operation throws `OverflowException`. In an unchecked context, it generally wraps to a representable value. Compiler settings and constant expressions can affect behavior, so be explicit when overflow matters.

```csharp
int maximum = int.MaxValue;

try
{
	int impossible = checked(maximum + 1);
}
catch (OverflowException)
{
	Console.WriteLine("The result does not fit in Int32.");
}

int wrapped = unchecked(maximum + 1);
Console.WriteLine(wrapped); // Commonly -2147483648.
```

Choose a domain policy: reject, use a wider type, saturate, or use arbitrary precision. Floating-point overflow generally produces infinity rather than `OverflowException`.

## `Math`: common operations and edge cases

`System.Math` supplies frequently used operations:

- `Abs(x)`: absolute value.
- `Min(a, b)` / `Max(a, b)`: smaller/larger value.
- `Floor(x)` / `Ceiling(x)`: round toward negative/positive infinity.
- `Truncate(x)`: discard fractional digits toward zero.
- `Round(x)`: round to an integer or a requested number of digits/mode.
- `Sqrt(x)` / `Pow(x, y)`: square root and exponentiation.
- `Clamp(value, min, max)`: constrain a value to a range.

`Floor`, `Ceiling`, and `Truncate` differ for negative values. For `-2.3`, floor is `-3`, ceiling is `-2`, and truncate is `-2`. Read the overload and result type; numeric APIs often have overloads for several primitive types.

Edge cases matter: `Math.Abs(int.MinValue)` cannot be represented as a positive `int` and throws `OverflowException`; `Math.Sqrt(-1)` returns NaN for a floating-point input. Do not assume a mathematically valid result always fits the selected type.

## Rounding is a policy

`Math.Round` uses midpoint-to-even by default: an exact halfway value rounds to the nearest even result. This is sometimes called banker's rounding. Other modes, such as `AwayFromZero`, produce different answers.

```csharp
decimal even2 = Math.Round(2.5m); // 2
decimal even4 = Math.Round(3.5m); // 4
decimal away = Math.Round(2.5m, MidpointRounding.AwayFromZero); // 3
decimal amount = Math.Round(12.345m, 2, MidpointRounding.AwayFromZero);
```

Choose the mode and decimal scale from the domain contract. Avoid rounding after every intermediate operation unless the rule requires it; repeated rounding can accumulate error. Formatting a number to two decimal places changes its text representation, not necessarily the stored numeric value.

## Random values: simulation versus security

`Random` produces pseudo-random values suitable for simulation, simple sampling, and casual variation. It is not a cryptographic security source. Reuse an appropriately scoped instance rather than repeatedly constructing one in a tight loop; modern .NET also supplies `Random.Shared` for convenient thread-safe shared use cases.

For keys, tokens, or values an attacker must not predict, use `RandomNumberGenerator`:

```csharp
using System.Security.Cryptography;

int casualDieRoll = Random.Shared.Next(1, 7); // 1 through 6; upper bound is exclusive.
int secureValue = RandomNumberGenerator.GetInt32(0, 1_000_000);
```

`GetInt32(fromInclusive, toExclusive)` has an exclusive upper bound and is designed for cryptographic randomness. Do not build security tokens from `Random`, timestamps, or simple arithmetic.

## Complete runnable example

This console program demonstrates integer division, floating-point approximation, decimal arithmetic, rounding, overflow handling, and secure versus non-secure randomness:

```csharp
using System;
using System.Globalization;
using System.Security.Cryptography;

internal class Program
{
	private static void Main()
	{
		int integerDivision = 7 / 2;
		double floatingDivision = 7.0 / 2.0;
		double floatingSum = 0.1 + 0.2;
		decimal decimalSum = 0.1m + 0.2m;

		Console.WriteLine($"Integer division: {integerDivision}");
		Console.WriteLine($"Floating division: {floatingDivision}");
		Console.WriteLine($"Double sum: {floatingSum.ToString("R", CultureInfo.InvariantCulture)}");
		Console.WriteLine($"Decimal sum: {decimalSum.ToString(CultureInfo.InvariantCulture)}");
		Console.WriteLine($"2.5 to even: {Math.Round(2.5m)}");
		Console.WriteLine($"2.5 away from zero: {Math.Round(2.5m, MidpointRounding.AwayFromZero)}");

		int maximum = int.MaxValue;
		try
		{
			int overflow = checked(maximum + 1);
			Console.WriteLine(overflow);
		}
		catch (OverflowException)
		{
			Console.WriteLine("Checked overflow detected.");
		}

		Console.WriteLine($"Casual die roll: {Random.Shared.Next(1, 7)}");
		Console.WriteLine($"Secure sample: {RandomNumberGenerator.GetInt32(0, 1_000_000)}");
	}
}
```

The random outputs vary every execution. The other lines are deterministic for these examples. The `checked` expression uses the `int.MaxValue` property, so the overflow is detected at runtime.

## Common mistakes

- Using `int` when valid values exceed its range.
- Assuming `double` stores decimal fractions exactly.
- Comparing approximate floating-point results with `==` without a domain-specific reason.
- Converting to a wider type after an operation that already overflowed.
- Forgetting that integer division truncates toward zero.
- Assuming an explicit cast prevents overflow or precision loss.
- Rounding for display and assuming the underlying value changed.
- Assuming `decimal` supplies currency, scale, or rounding rules.
- Using `Random` for passwords, keys, or security tokens.
- Ignoring NaN or infinity when numerical operations can produce them.

## Interview questions with answers

### Why can `0.1 + 0.2` differ from `0.3` with `double`?

Binary floating point cannot exactly represent many decimal fractions, so the stored operands and result are approximations.

### When use `decimal` instead of `double`?

Use `decimal` when decimal-fraction behavior and decimal rounding are important, often for financial calculations. Use `double` for general numerical work that needs its range and performance. The domain contract decides.

### What does integer division do?

When both operands are integral, division produces an integral result and truncates toward zero. Convert/promote an operand before division if a fractional result is required.

### What is the difference between `checked` and `unchecked`?

For integral arithmetic, checked contexts detect overflow and throw; unchecked contexts permit wrapping behavior. Explicitly choose and test the policy where overflow matters.

### What is midpoint-to-even rounding?

An exact halfway value rounds to the nearest even result, such as 2.5 to 2 and 3.5 to 4. Choose another `MidpointRounding` mode when the rule requires it.

### Is `Random` cryptographically secure?

No. Use `RandomNumberGenerator` for security-sensitive unpredictable values.

### Why can converting an `int` to `float` lose information?

`float` has fewer significant binary digits than the set of possible `int` values, so some integers cannot be represented exactly even though the conversion is implicit.

## Practice

1. Compare `1 / 2`, `1.0 / 2.0`, and `1m / 2m`.
2. Print `0.1 + 0.2` as `double`, then repeat with `decimal`; explain the difference.
3. Make an overflowing integer addition in checked and unchecked contexts.
4. Round `2.5`, `3.5`, `-2.5`, and `-3.5` with two midpoint modes.
5. Explain why `(long)(left + right)` may not prevent overflow if `left` and `right` are `int`, and show how to promote before adding.
6. Generate a casual die roll and a secure random integer; identify which API is appropriate for a login token.

## Mental model

Integral types have fixed whole-number ranges. `float` and `double` trade exact decimal representation for broad range and efficient approximation. `decimal` is useful when base-10 rounding behavior matters but still needs a business policy. Overflow, conversion, division, rounding, and randomness are design decisions, not details to leave to habit.
