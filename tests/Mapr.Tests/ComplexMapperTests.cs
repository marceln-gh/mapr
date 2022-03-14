using AutoFixture;
using FluentAssertions;
using NSubstitute;
using System;
using Xunit;

namespace Mapr.Tests;

public class ComplexMapperTests : TestBed
{
    public class TestOutput
    {
        string? Property { get; set; }
    }

    [Fact]
    public void Map_ShouldMap_WhenTypeMapExists()
    {
        var typeMap = Substitute.For<IComplexMap<string, TestOutput>>();
        var locator = Substitute.For<IMapLocator>();
        locator.LocateComplexMapFor<string, TestOutput>().Returns(typeMap);

        var mapper = new Mapper(locator);

        var source = Fixture.Create<string>();
        var expectedResult = Fixture.Create<TestOutput>();

        typeMap.Map(source, expectedResult).Returns(expectedResult);

        var result = mapper.Map(source, expectedResult);

        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Map_ShouldNotThrow_WhenPassedNullSource()
    {
        var locator = Substitute.For<IMapLocator>();

        var mapper = new Mapper(locator);

        var expectedResult = Fixture.Create<TestOutput>();

        Action act = () => mapper.Map<string?, TestOutput>(null!, expectedResult);

        act.Should().NotThrow<ArgumentNullException>();
    }

    [Fact]
    public void Map_ShouldNotThrow_WhenPassedNullTarget()
    {
        var locator = Substitute.For<IMapLocator>();

        var mapper = new Mapper(locator);

        var source = Fixture.Create<string>();

        Action act = () => mapper.Map<string, TestOutput?>(source, null);

        act.Should().NotThrow<ArgumentNullException>();
    }
}