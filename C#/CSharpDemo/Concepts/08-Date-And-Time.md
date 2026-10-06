# 08 - Date and Time in .NET

## Start by asking what the value means

Date/time bugs often come from choosing a type before deciding what the value represents. Ask:

- Is this a **calendar date**, with no time of day?
- Is it a **clock time**, with no date or time zone?
- Is it a **duration**?
- Is it a **specific instant** on the global timeline?
- Is it a **future local appointment** that must follow a named place's time-zone rules?

These are different concepts. Use a type that expresses the intended meaning instead of treating every one as “a timestamp.”

## The main .NET types

### `DateTime`

`DateTime` represents a date and clock time, plus a `Kind` hint: `Utc`, `Local`, or `Unspecified`. It does not store a named time zone or an explicit UTC offset as part of the value.

Use it when the surrounding contract clearly defines how the value is interpreted, such as a UTC-only API contract or a local clock value whose time zone is supplied separately. Be careful when a `DateTime` can be unspecified or mixed between UTC/local conventions.

### `DateTimeOffset`

`DateTimeOffset` represents a date/time with a UTC offset. For example, `09:00` at `-04:00` and `13:00` at `+00:00` represent the same instant. It is usually a good choice for an event timestamp that must identify an instant unambiguously.

An offset is not a time-zone identity. `-04:00` tells the offset at that moment; it does not tell you which region's daylight-saving rules produced it.

### `DateOnly`

`DateOnly` represents a calendar date without a time or time zone. It is suitable for a birthday, due date, or holiday when converting between time zones would make no sense.

### `TimeOnly`

`TimeOnly` represents a clock time within a day, without a date or time zone. It is suitable for a daily opening time. It does not identify an instant until combined with a date and time-zone rule.

### `TimeSpan`

`TimeSpan` represents an interval or duration, such as a timeout or elapsed difference. It is not a time of day and has no time zone.

```csharp
DateOnly birthday = new(1995, 4, 12);
TimeOnly openingTime = new(9, 30);
TimeSpan timeout = TimeSpan.FromSeconds(20);
DateTimeOffset recordedAt = DateTimeOffset.UtcNow;
```

## The global timeline: UTC and offsets

**UTC** (Coordinated Universal Time) is the common reference used to identify instants independently of a machine's local zone. `DateTimeOffset.UtcNow` gives the current instant with a zero offset. `DateTime.UtcNow` gives a UTC `DateTime`. `DateTime.Now` gives the machine's local date/time; that local zone can differ across a developer computer, server, and user device.

For event timestamps crossing machines, a common strategy is to represent or normalize the instant in UTC and convert for display. Keep enough information for the contract: a `DateTimeOffset` preserves the supplied offset, while converting it to UTC produces a zero-offset representation of the same instant.

```csharp
DateTimeOffset eventTime = new(2026, 9, 27, 9, 30, 0, TimeSpan.FromHours(-4));
DateTimeOffset sameInstantInUtc = eventTime.ToUniversalTime();

Console.WriteLine(eventTime.ToString("O"));
Console.WriteLine(sameInstantInUtc.ToString("O"));
```

The clock fields differ, but the represented instant is the same. Compare `DateTimeOffset` values as instants; changing the displayed offset does not change the instant.

## Time zones are rule sets, not just offsets

A **time zone** is a named rule set that maps local clock time to UTC over dates. Rules can include daylight-saving changes and historical changes. `TimeZoneInfo` represents and converts using those rules.

For a future appointment such as “9 AM in this city,” store the intended local date/time and the time-zone identity (or the product's equivalent). A fixed offset alone cannot reliably calculate the correct offset for a future date if the region changes its rules.

Daylight-saving transitions create two important cases:

- A **gap**: some local times do not exist when clocks move forward. A local appointment at a skipped clock time needs a policy: reject it, move it forward, or choose another rule.
- An **overlap**: some local times occur twice when clocks move backward. The local clock time alone is ambiguous; the application must choose which occurrence/offset it means.

Time-zone identifiers and rule databases vary by platform and runtime environment. If an application accepts user-selected zones or deploys across Windows/Linux, validate the identifier and use documented mapping/support for the target framework. Do not hard-code an offset as if it were a named zone.

## `DateTime.Kind`: useful hint, not a time zone

`DateTime.Kind` can be `Utc`, `Local`, or `Unspecified`:

- `Utc`: the value is intended to represent UTC clock fields.
- `Local`: the value is interpreted as machine-local time for conversion operations.
- `Unspecified`: the value has no attached UTC/local interpretation.

`Unspecified` is common when constructing a date from components or parsing text without zone information. It is dangerous to convert it without knowing its meaning. Some conversions interpret unspecified values as local. `DateTime.SpecifyKind` changes the `Kind` metadata without changing the clock fields; it does not convert the instant. `ToUniversalTime` converts according to the value's `Kind` and local-zone rules.

Do not use `SpecifyKind` as a substitute for conversion. First decide whether the clock fields represent UTC, local time, or a wall-clock value in a separately known zone.

## Parsing text into date/time values

Parsing converts text into a date/time value. The meaning of text can depend on culture and format. `03/04/2026` could mean March 4 or April 3 in different cultures.

For a known machine/API format, use a documented invariant format and `TryParseExact`/`ParseExact`. For user-entered text, use an intended culture and handle invalid input. `TryParse` returns success/failure instead of throwing for ordinary invalid input.

```csharp
using System.Globalization;

string input = "2026-09-27T09:30:00-04:00";
bool succeeded = DateTimeOffset.TryParseExact(
    input,
    "yyyy-MM-dd'T'HH:mm:sszzz",
    CultureInfo.InvariantCulture,
    DateTimeStyles.None,
    out DateTimeOffset parsed);

if (succeeded)
{
    Console.WriteLine(parsed.ToUniversalTime());
}
```

The `zzz` custom format component reads a signed UTC offset. `InvariantCulture` prevents machine culture from changing the interpretation of this contract format. `DateTimeStyles` options differ by parsing API and type; use options supported by the exact overload rather than copying a setting blindly.

## Formatting values as text

Formatting converts a date/time value to text. Standard format strings include:

- `O` or `o`: round-trip representation suitable for preserving date/time and offset information.
- `d`: short date pattern, culture-dependent.
- `D`: long date pattern, culture-dependent.
- `t` / `T`: short/long time patterns, culture-dependent.

Use a format provider when the output contract matters:

```csharp
string machineText = parsed.ToString("O", CultureInfo.InvariantCulture);
string userText = parsed.ToString("f", CultureInfo.CurrentCulture);
```

Invariant formatting is suitable for stable machine/log interchange when the format is defined. User-facing display usually should respect the user's culture. Do not store a culture-specific display string as if it were a universally parseable timestamp.

## Date/time arithmetic and measuring elapsed time

Date/time arithmetic returns a `TimeSpan` in common cases:

```csharp
DateTimeOffset start = DateTimeOffset.UtcNow;
DateTimeOffset end = start.AddMinutes(15);
TimeSpan duration = end - start;
```

`DateTimeOffset` subtraction accounts for the represented instants. `DateTime` arithmetic can be misleading when values have inconsistent `Kind` or represent different zones; make the interpretation explicit.

For measuring how long code takes to execute, use `Stopwatch`, not `DateTime.Now` or `DateTime.UtcNow`. Wall clocks can jump because of time synchronization or system changes. `Stopwatch` uses a monotonic timing source intended for elapsed measurements:

```csharp
using System.Diagnostics;

var timer = Stopwatch.StartNew();
DoWork();
timer.Stop();
Console.WriteLine(timer.Elapsed);
```

A timestamp answers “when did this happen?” A stopwatch answers “how long did it take?”

## Complete runnable example

This console program parses an exact timestamp with an offset, converts the same instant to UTC, formats it for a machine-readable value, and separately models a date-only and clock-time concept:

```csharp
using System;
using System.Globalization;

internal class Program
{
    private static void Main()
    {
        const string format = "yyyy-MM-dd'T'HH:mm:sszzz";
        const string text = "2026-09-27T09:30:00-04:00";

        if (!DateTimeOffset.TryParseExact(
                text,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTimeOffset eventTime))
        {
            Console.WriteLine("Timestamp format is invalid.");
            return;
        }

        DateTimeOffset utc = eventTime.ToUniversalTime();
        Console.WriteLine($"Original: {eventTime.ToString("O", CultureInfo.InvariantCulture)}");
        Console.WriteLine($"UTC:      {utc.ToString("O", CultureInfo.InvariantCulture)}");

        DateOnly dueDate = new(2026, 10, 5);
        TimeOnly officeOpens = new(9, 0);
        TimeSpan reminderDelay = TimeSpan.FromHours(2);
        Console.WriteLine($"Due date: {dueDate:yyyy-MM-dd}");
        Console.WriteLine($"Opening time: {officeOpens}");
        Console.WriteLine($"Reminder delay: {reminderDelay}");
    }
}
```

Expected output (the exact `TimeOnly`/`TimeSpan` presentation can depend on formatting conventions):

```text
Original: 2026-09-27T09:30:00.0000000-04:00
UTC:      2026-09-27T13:30:00.0000000+00:00
Due date: 2026-10-05
Opening time: 09:00
Reminder delay: 02:00:00
```

The event timestamp identifies one instant. The due date has no time zone. The opening time is only a clock time and would need a date and zone rules to identify an instant. The delay is a duration.

## Common mistakes

- Using `DateTime.Now` for a globally meaningful event timestamp without recording its zone/offset.
- Assuming a UTC offset is a time-zone identity.
- Treating `DateTimeKind.Unspecified` as UTC without a contract.
- Calling `SpecifyKind` and thinking it converted a time.
- Parsing culture-specific text on a machine with a different culture.
- Storing only a display-formatted local time for an event.
- Using wall-clock timestamps to benchmark elapsed time.
- Modeling a birthday with a timestamp and then shifting its date through time-zone conversion.
- Assuming a future local appointment can be represented by today's offset alone.

## Interview questions with answers

### `DateTime` or `DateTimeOffset` for an event timestamp?

Usually `DateTimeOffset` makes the offset explicit and identifies an instant. Many systems normalize the instant to UTC at boundaries. Follow the application contract and still use a time-zone identifier when future local scheduling rules matter.

### Does an offset identify a time zone?

No. It gives the difference from UTC at a particular time. A time zone is a named set of rules that can change over time.

### When should I use `DateOnly` and `TimeOnly`?

Use `DateOnly` for a date with no meaningful time and `TimeOnly` for a wall-clock time without a date/zone. They prevent accidentally adding semantics the value does not have.

### What does `DateTimeKind.Unspecified` mean?

The value has no UTC/local marker. It does not mean UTC. A separate contract must define its interpretation before conversion.

### What is the difference between `SpecifyKind` and `ToUniversalTime`?

`SpecifyKind` changes the `Kind` label without changing clock fields. `ToUniversalTime` performs a conversion based on the value's interpretation and local-zone rules.

### Why is a time zone more than an offset?

The zone contains historical and future rules such as daylight-saving transitions. An offset represents only one UTC difference and cannot answer how a local appointment should map on another date.

### How do you measure elapsed time?

Use `Stopwatch`, which uses a monotonic timing source. Wall-clock values such as `DateTime.Now` can move due to system clock adjustments.

## Practice

1. Parse an exact timestamp with an explicit offset and convert it to UTC.
2. Parse a date with two different cultures and explain why ambiguous numeric dates are risky.
3. Explain why a birthday should usually be a `DateOnly`, not a UTC instant.
4. Research how your target runtime handles a skipped and repeated local time using `TimeZoneInfo`; decide the product policy for each.
5. Measure a short operation with `Stopwatch` and compare the result with subtracting two wall-clock reads.

## Mental model

Choose the type by meaning: `DateOnly` for a calendar date, `TimeOnly` for a clock reading, `TimeSpan` for a duration, and usually `DateTimeOffset` for an instant. Use `TimeZoneInfo` when named regional rules matter. Treat parsing/formatting culture and daylight-saving ambiguity as part of the data contract, not cosmetic details.
