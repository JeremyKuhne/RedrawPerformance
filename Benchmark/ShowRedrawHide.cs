using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using RedrawPerformance;
using System.Windows.Forms;

namespace Benchmark
{
    [MemoryDiagnoser]
    // Native memory profiler requires running elevated
    // [NativeMemoryProfiler]
    public class ShowRedrawHide
    {
        private static readonly TextOnForm s_textOnForm = new TextOnForm();
        private static readonly DataGridViewForm s_dataGridViewForm = new DataGridViewForm();
        private static readonly ManyControls s_manyControlsForm = new ManyControls();
        private static readonly MultipleControls s_multipleControls = new MultipleControls();
        private static readonly GroupBoxForm s_groupBoxForm = new GroupBoxForm();

        [Benchmark]
        public void TextOnForm() => ShowRedrawAndHide(s_textOnForm);

        [Benchmark]
        public void DataGridView() => ShowRedrawAndHide(s_dataGridViewForm);

        [Benchmark]
        public void ManyControls() => ShowRedrawAndHide(s_manyControlsForm);

        [Benchmark]
        public void MultipleControls() => ShowRedrawAndHide(s_multipleControls);

        [Benchmark]
        public void GroupBoxForm() => ShowRedrawAndHide(s_groupBoxForm);

        private static void ShowRedrawAndHide(Form form)
        {
            form.Show();

            // Directly hit the Windows API to synchronously invalidate the entire window and redraw it.
            Interop.RedrawWindow(
                form.Handle,
                default,
                default,
                Interop.RDW.INVALIDATE | Interop.RDW.ERASE | Interop.RDW.ALLCHILDREN);
            Interop.UpdateWindow(form.Handle);

            form.Hide();
        }
    }
}