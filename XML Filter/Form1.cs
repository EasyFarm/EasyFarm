
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace XML_Filter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(FileNameTextBox.Text))
            {
                MessageBox.Show("File " + FileNameTextBox.Text + " not found.");
                return;
            }

            // List all attributes. 
            if (AttributeNameTextBox.Text.Equals("List", StringComparison.CurrentCultureIgnoreCase))
            {
                // Create and load the abilities xml file. 
                XDocument doc = XDocument.Load(FileNameTextBox.Text);

                IEnumerable<String> names = doc.Root.Descendants()
                    .SelectMany(x => x.Attributes().Select(attribute => attribute.Name.LocalName))
                    .Distinct();

                AttributeContentListBox.Items.AddRange(names.ToArray());
            }
            // Filter by attribute name. 
            else
            {
                // Create and load the abilities xml file. 
                XmlDocument doc = new XmlDocument();
                doc.Load(FileNameTextBox.Text);

                // Get all of the ability xml nodes with the user's specified attribute name. 
                var attributes = doc.SelectNodes("//*[@" + AttributeNameTextBox.Text + "]");

                // Return if the are no attributes with the given name. 
                if (attributes == null) return;

                // Get the inner text for all attributes.
                var values = attributes
                    .Cast<XmlNode>()
                    .Select(x => x.Attributes[AttributeNameTextBox.Text].InnerText)
                    .ToList();

                // Clear all previous data.
                AttributeContentListBox.Items.Clear();

                // Add all the new values under the given attribute name. 
                AttributeContentListBox.Items.AddRange(values.Distinct().ToArray());
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.ShowDialog();
            FileNameTextBox.Text = ofd.FileName;
        }
    }
}
