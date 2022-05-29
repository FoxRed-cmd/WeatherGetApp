using System;
using System.Threading.Tasks;

namespace WeatherGetApp.HelperClasses
{
    internal class Timer : IDisposable
    {
        private bool _enabled = false;
        public bool Enabled 
        { 
            get => _enabled; 
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (_enabled == true)
                    {
                        TickInvoke();
                    }
                }
            }
        }
        public int Interval { get; set; }

        public event Action<object, EventArgs>? Tick;
        private EventArgs _plug = new EventArgs();

        public void Start() => Enabled = true;
        public void Stop() => Enabled = false;
        public void Dispose()
        {
            if (Tick != null)
                Tick = null;
        }
        private async void TickInvoke()
        {
            while (Enabled)
            {
                await Task.Delay(Interval);
                Tick?.Invoke(this, _plug);
            }
        }
    }
}
