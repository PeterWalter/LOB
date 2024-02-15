using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Shouldly;
using CETAP_LOB.Helper;

namespace UnitTestLOB
{
    
    public class UnitTest
    {
        [Fact]
        public void PassingTest1()
        {
           // Assert.Equal(4, Add(2, 2));
            Add(2, 2).ShouldBe(4);
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
    public class HelperUtilsTest
    {
        [Fact]
        public void CheckIfStringHasANumber()
        {
            string x = "aby12nb";
           // HelperUtils.StringHasANumber(x).ShouldBeTrue;
            Assert.True(HelperUtils.StringHasANumber(x));
            x = "abyMMnb";
            Assert.False(HelperUtils.StringHasANumber(x));

        }
    }
}
