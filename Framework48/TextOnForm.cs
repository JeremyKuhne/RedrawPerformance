using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RedrawPerformance
{
    public partial class TextOnForm : Form
    {
        private readonly Font _font = new Font("Arial", 18.0f);
        private int _paintCount;
        private readonly StringBuilder _sb = new StringBuilder(60);

        public TextOnForm()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _paintCount++;

            _sb.Clear();
            _sb.AppendFormat("OnPaint Invocation {0}", _paintCount);
#if NET5API
            Span<char> count = stackalloc char[_sb.Length];
            _sb.CopyTo(0, count, _sb.Length);
            Size size = TextRenderer.MeasureText(e, count, _font);
#else
            string count = _sb.ToString();
            Size size = TextRenderer.MeasureText(e.Graphics, count, _font);
#endif
            Point position = new Point(
                (ClientRectangle.Width / 2) - (size.Width / 2),
                (ClientRectangle.Height / 2) - (size.Height / 2));

            TextRenderer.DrawText(
#if NET5API
                e,
#else
                e.Graphics,
#endif
                count,
                _font,
                position,
                Color.Blue);
        }
    }
}
