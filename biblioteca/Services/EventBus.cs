using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace biblioteca.Services
{
    public static class EventBus
    {
        public static event Action? NewLoan;

        public static void NotifyNewLoan()
        {
            NewLoan?.Invoke();
        }
    }
}
