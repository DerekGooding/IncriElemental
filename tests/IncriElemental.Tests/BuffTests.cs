using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class BuffTests
{
    [Fact]
    public void Buff_Update_DecreasesTime_And_Deactivates()
    {
        var buff = new Buff { 
            Id = "test", 
            Duration = 10, 
            RemainingTime = 10 
        };

        Assert.True(buff.IsActive);
        
        buff.Update(5.0);
        Assert.Equal(5.0, buff.RemainingTime);
        Assert.True(buff.IsActive);

        buff.Update(6.0);
        Assert.Equal(0, buff.RemainingTime);
        Assert.False(buff.IsActive);
    }
}
