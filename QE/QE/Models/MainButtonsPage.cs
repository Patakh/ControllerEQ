using System.Threading.Tasks;
using System.Windows.Controls;

namespace QE.Models
{
    public partial class Main
    {
        public async Task MainPages(Grid panel)
        {
            await InitButtons(panel);
        }
    }
}
