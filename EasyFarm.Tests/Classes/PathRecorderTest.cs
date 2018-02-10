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
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class PathRecorderTest
    {
        [Fact]
        public void AddNewPositionWithPositionWillRecordIt()
        {
            Position expected = new Position {H = 1, X = 1, Y = 1, Z = 1};
            Position result = null;

            var recorder = new PathRecorder(null);

            recorder.OnPositionAdded += actual => result = actual;
            recorder.AddNewPosition(expected);

            Assert.Equal(expected, result);
        }
    }
}
