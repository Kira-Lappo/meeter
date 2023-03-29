namespace Meeter.Tests;

public class DateTimeRangeHelperTests
{
    public static object[] OverlapsPositiveCases =
    {
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            new DateTime(2022, 01, 01, 0, 5,  0),
            new DateTime(2022, 01, 01, 0, 15,  0),
            "First starts before and ends inside of the second",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 5,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            "Second starts before and ends inside of the first",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            "Same Ranges",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            new DateTime(2022, 01, 01, 0, 5,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            "First includes the second range",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            "First includes the second range with the same start",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            new DateTime(2022, 01, 01, 0, 5,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            "First includes the second range with the same end",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 5,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 15, 0),
            "Second includes the first range",
        },
    };

    [TestCaseSource(nameof(OverlapsPositiveCases))]
    [Test]
    public void Overlaps_PositiveCases(
        DateTime firstStart,
        DateTime firstEnd,
        DateTime secondStart,
        DateTime secondEnd,
        string reason)
    {
        var hasOverlap = DateTimeRangeHelper.HasOverlap(firstStart, firstEnd, secondStart, secondEnd);

        Assert.That(hasOverlap, Is.True, $"Must have an overlap because: {reason}");
    }

    public static object[] OverlapNegativeCases =
    {
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            new DateTime(2022, 01, 01, 0, 15,  0),
            new DateTime(2022, 01, 01, 0, 20, 0),
            "First starts and ends before the second",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 15, 0),
            new DateTime(2022, 01, 01, 0, 20, 0),
            new DateTime(2022, 01, 01, 0, 0,  0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            "Second starts and ends before the first",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 0, 0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            new DateTime(2022, 01, 01, 0, 20, 0),
            "Second starts right after the first range",
        },
        new object[]
        {
            new DateTime(2022, 01, 01, 0, 10,  0),
            new DateTime(2022, 01, 01, 0, 20, 0),
            new DateTime(2022, 01, 01, 0, 0, 0),
            new DateTime(2022, 01, 01, 0, 10, 0),
            "First starts right after the second range",
        },
    };

    [TestCaseSource(nameof(OverlapNegativeCases))]
    [Test]
    public void Overlaps_NegativeCases(
        DateTime firstStart,
        DateTime firstEnd,
        DateTime secondStart,
        DateTime secondEnd,
        string reason)
    {
        var hasOverlap = DateTimeRangeHelper.HasOverlap(firstStart, firstEnd, secondStart, secondEnd);

        Assert.That(hasOverlap, Is.False, $"Must NOT have an overlap because: {reason}");
    }
}
