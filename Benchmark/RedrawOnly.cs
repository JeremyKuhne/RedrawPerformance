using BenchmarkDotNet.Attributes;
using RedrawPerformance;
using System.Windows.Forms;

namespace Benchmark
{
    [MemoryDiagnoser]
    // Native memory profiler requires running elevated
    //[NativeMemoryProfiler]
    public class RedrawOnly
    {
        private static readonly TextOnForm s_textOnForm = new TextOnForm();
        private static readonly DataGridViewForm s_dataGridViewForm = new DataGridViewForm();
        private static readonly ManyControls s_manyControlsForm = new ManyControls();
        private static readonly MultipleControls s_multipleControls = new MultipleControls();
        private static readonly GroupBoxForm s_groupBoxForm = new GroupBoxForm();

        [GlobalSetup(Target = nameof(TextOnForm))]
        public void GlobalSetupTextOnForm() => s_textOnForm.Show();

        [GlobalCleanup(Target = nameof(TextOnForm))]
        public void GlobalCleanupTextOnForm() => s_textOnForm.Hide();

        [Benchmark]
        public void TextOnForm() => Redraw(s_textOnForm);

        [GlobalSetup(Target = nameof(DataGridView))]
        public void GlobalSetupDataGridView() => s_dataGridViewForm.Show();

        [GlobalCleanup(Target = nameof(DataGridView))]
        public void GlobalCleanupDataGridView() => s_dataGridViewForm.Hide();

        [Benchmark]
        public void DataGridView() => Redraw(s_dataGridViewForm);

        [GlobalSetup(Target = nameof(ManyControls))]
        public void GlobalSetupManyControls() => s_manyControlsForm.Show();

        [GlobalCleanup(Target = nameof(ManyControls))]
        public void GlobalCleanupManyControls() => s_manyControlsForm.Hide();

        [Benchmark]
        public void ManyControls() => Redraw(s_manyControlsForm);

        [GlobalSetup(Target = nameof(MultipleControls))]
        public void GlobalSetupMultipleControls() => s_multipleControls.Show();

        [GlobalCleanup(Target = nameof(MultipleControls))]
        public void GlobalCleanupMultipleControls() => s_multipleControls.Hide();

        [Benchmark]
        public void MultipleControls() => Redraw(s_multipleControls);

        [GlobalSetup(Target = nameof(GroupBoxForm))]
        public void GlobalSetupGroupBoxForm() => s_groupBoxForm.Show();

        [GlobalCleanup(Target = nameof(GroupBoxForm))]
        public void GlobalCleanupGroupBoxForm() => s_groupBoxForm.Hide();

        [Benchmark]
        public void GroupBoxForm() => Redraw(s_groupBoxForm);

        private static void Redraw(Form form)
        {
            // Directly hit the Windows API to synchronously invalidate the entire window and redraw it.
            Interop.RedrawWindow(
                form.Handle,
                default,
                default,
                Interop.RDW.INVALIDATE | Interop.RDW.ERASE | Interop.RDW.ALLCHILDREN);
            Interop.UpdateWindow(form.Handle);
        }
    }
}