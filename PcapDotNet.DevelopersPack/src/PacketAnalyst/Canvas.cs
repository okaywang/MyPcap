using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace PacketAnalyst
{
    public class Canvas
    {
        private Label _label;
        public Canvas(Label label)
        {
            this._label = label;
        }
        public void WriteLine(PacketDigest pd)
        {
            //this._label.Text += Environment.NewLine + line;
        }
        public void Clear()
        {
            this._label.Text = string.Empty;
        }
    }

    public class Canvas2
    {
        private DataGridView _dgv;
        public Canvas2(DataGridView dgv)
        {
            this._dgv = dgv;
        }
        public void WriteLine(PacketDigest pd)
        {
            if (pd == null)
            {
                return;
            }
            var row = (DataGridViewRow)this._dgv.Rows[0].Clone();

            var properties = pd.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                row.Cells[i].Value = properties[i].GetValue(pd, null);
            } 

            _dgv.Rows.Add(row);
        }
        public void Clear()
        {
            this._dgv.Rows.Clear();
        }
    }
}
