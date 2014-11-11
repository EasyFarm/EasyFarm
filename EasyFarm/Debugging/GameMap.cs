
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

public class GameMap : Form
{
    public GameMap()
    {
        this.Height = 500;
        this.Width = 500;

        this.Graphics = this.CreateGraphics();

        this.Graphics.FillRectangle(Brushes.Black, 250, 250, 1, 1);

        System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
            50, 100, 150, 150);
        this.Graphics.DrawEllipse(System.Drawing.Pens.Black, rectangle);
        this.Graphics.DrawRectangle(System.Drawing.Pens.Red, rectangle);
        this.Show();
    }

    public Graphics Graphics { get; set; }
}

public class MapPoint
{
    private float x, y, z;

    public MapPoint()
    {
        this.x = 0;
        this.y = 0;
        this.z = 0;
    }

    public MapPoint(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float X
    {
        get { return this.x; }
        set { this.x = value; }
    }

    public float Y
    {
        get { return this.y; }
        set { this.y = value; }
    }

    public float Z
    {
        get { return this.z; }
        set { this.z = value; }
    }

    public bool IsEqual(MapPoint point)
    {
        return IsEqual(point.x, point.y, point.z);
    }

    public bool IsEqual(float x, float y, float z)
    {
        return this.x == x && this.y == y && this.z == z;
    }
}
