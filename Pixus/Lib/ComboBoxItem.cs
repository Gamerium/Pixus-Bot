using System;
using System.Collections.Generic;
using System.Text;

namespace Pixus.Lib
{
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboBoxItem(string Text, object Value)
        {
            this.Text = Text;
            this.Value = Value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
