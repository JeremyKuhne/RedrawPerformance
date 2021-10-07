using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RedrawPerformance
{
    public partial class MainForm : Form
    {
        private static readonly Stopwatch s_stopwatch = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();
            Text = $"Redraw Peformance: {typeof(MainForm).Assembly.GetName().Name}";
        }

        private static string MeasureFormPainting(Form form, int iterations)
        {
            form.Show();

            long bytes = GC.GetAllocatedBytesForCurrentThread();

            // Measuring time is only moderately useful as times will vary widely due to the system wide async
            // nature of window message handling.

            s_stopwatch.Restart();

            for (int i = 0; i < iterations; i++)
            {
                // Directly hit the Windows API to synchronously invalidate the entire window and redraw it.
                Interop.RedrawWindow(
                    form.Handle,
                    default,
                    default,
                    Interop.RDW.INVALIDATE | Interop.RDW.ERASE | Interop.RDW.ALLCHILDREN);
                Interop.UpdateWindow(form.Handle);
            }

            form.Hide();
            s_stopwatch.Stop();
            long afterBytes = GC.GetAllocatedBytesForCurrentThread() - bytes;
            return $"Iterations: {iterations,-6} Allocations per redraw: {afterBytes / 1024.0 / iterations:F3} KB";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using Form form = new TextOnForm();
            textBox1.Text = MeasureFormPainting(form, 10000);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            using Form form = new DataGridViewForm();
            textBox2.Text = MeasureFormPainting(form, 50);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            using Form form = new ManyControls();
            textBox3.Text = MeasureFormPainting(form, 400);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            using Form form = new MultipleControls();
            textBox4.Text = MeasureFormPainting(form, 400);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            using Form form = new GroupBoxForm();
            textBox5.Text = MeasureFormPainting(form, 10000);
        }
    }
}
