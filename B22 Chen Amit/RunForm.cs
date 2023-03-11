using System.Windows.Forms;

namespace B22_Chen_Amit
{
    internal class RunForm
    {
        internal static void Run()
        {
            Application.EnableVisualStyles();
            new FormLogin().ShowDialog();
        }
    }
}
