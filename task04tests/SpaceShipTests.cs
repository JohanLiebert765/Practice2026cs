using Xunit;
using Moq;
using task04;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldHaveGreaterFirePowerThanFighter()
{
    var cruiser = new Cruiser();
    var fighter = new Fighter();
    Assert.True(cruiser.FirePower > fighter.FirePower);
}
    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Cruiser_MoveForward_ChangePosition()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        Assert.Equal(50, cruiser.Position);
    }

    [Fact]
    public void Fighter_MoveForward_ChangePosition()
    {
        var fighter = new Fighter();
        fighter.MoveForward();
        Assert.Equal(100, fighter.Position);
    }

    [Fact]
    public void Cruiser_Rotate_ChangeAngle()
    {
        var cruiser = new Cruiser();
        cruiser.Rotate(90);
        Assert.Equal(90, cruiser.Angle);
    }

    [Fact]
    public void Fighter_Rotate_ChangeAngle()
    {
        var fighter = new Fighter();
        fighter.Rotate(-90);
        Assert.Equal(270, fighter.Angle);
    }

    [Fact]
    public void Fighter_Fire_Count()
    {
        var fighter = new Fighter();
        fighter.Fire();
        fighter.Fire();
        Assert.Equal(2, fighter.FireCount);
    }

    [Fact]
    public void Cruiser_MoreMoves_ChangesPosition()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        cruiser.MoveForward();
        Assert.Equal(100, cruiser.Position);
    }
}

