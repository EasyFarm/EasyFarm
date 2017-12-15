// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013-2017 Mykezero
// 
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using EasyFarm.Classes;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class RangeTests
    {
        [Fact]
        public void NotSetWhenBothHighAndLowAreZero()
        {
            var range = new Range(0, 0);
            Assert.True(range.NotSet());
        }
        
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void SetWhenEitherHighOrLowIsNotZero(int low, int high)
        {
            var range = new Range(low, high);
            Assert.False(range.NotSet());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InRangeWhenValueBetweenLowAndHigh(int value)
        {
            var range = new Range(0, 2);
            Assert.True(range.InRange(value));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void NotInRangeWhenValueNotBetweenLowAndHigh(int value)
        {
            var range = new Range(0, 2);
            Assert.False(range.InRange(value));
        }
    }
}
