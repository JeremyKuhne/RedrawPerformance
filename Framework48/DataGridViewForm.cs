using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedrawPerformance
{
    public partial class DataGridViewForm : Form
    {
        public DataGridViewForm()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridViewForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = CreateTable(typeof(Control));
        }

        public static DataTable CreateTable(Type type)
        {
            DataTable dataTable = new DataTable(type.FullName);

            var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Static);

            dataTable.Columns.Add("Member Name", typeof(string));
            dataTable.Columns.Add("Member Type", typeof(MemberTypes));
            dataTable.Columns.Add("Binding Flags", typeof(BindingFlags));
            dataTable.Columns.Add("Attributes", typeof(string));
            dataTable.Columns.Add("Calling Conventions", typeof(CallingConventions));

            foreach (var member in members)
            {
                if (member.MemberType != MemberTypes.NestedType)
                {
                    object flags = member.GetType().GetProperty("BindingFlags",
                        BindingFlags.NonPublic | BindingFlags.Instance).GetValue(member);
                    object attributes = member.GetType().GetProperty("Attributes",
                        BindingFlags.Public | BindingFlags.Instance)?.GetValue(member);
                    object callingConvention = member.GetType().GetProperty("CallingConvention",
                        BindingFlags.Public | BindingFlags.Instance)?.GetValue(member);

                    dataTable.Rows.Add(
                        member.Name,
                        member.MemberType,
                        (BindingFlags)flags,
                        attributes,
                        callingConvention);
                }
            }

            return dataTable;
        }
    }
}
