using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QE.Models
{
    public partial class Main
    {
        public async Task UpdateDateTime(StackPanel panel)
        {
            _prevState = _nextState;
            DateTime now = DateTime.Now;
            var day = _schedulesDto.FirstOrDefault(f => f.SDayWeekId == (int)now.DayOfWeek);
            if (day == null)
            {
                _isActiveWorkTime = false;
                _nextState = false;
            }
            else if (day.StartTime > now.TimeOfDay)
            {
                _isActiveWorkTime = false;
                _nextState = false;
            }
            else if (day.StopTime < now.TimeOfDay)
            {
                _isActiveWorkTime = false;
                _nextState = false;
            }
            else
            {
                _isActiveWorkTime = true;
                _nextState = true;
            }

            if (_isActiveStart||_prevState != _nextState)
            {
                await ActivePages();
            }

            HeaderDatePages(panel, now);

            _isActiveStart = false;
        }
    }
}
