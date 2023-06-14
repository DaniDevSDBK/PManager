using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Utils
{
    using System;
    using System.Threading;

    public class ThreadSecurityCopy
    {
        private Thread thread;
        private bool isRunning;

        public void StartThreadSecurityCopy()
        {
            isRunning = true;
            thread = new Thread(TaskThread);
            thread.Start();
        }

        public void StopThreadSecurityCopy()
        {
            isRunning = false;
            thread.Join();
        }

        private void TaskThread()
        {
            while (isRunning)
            {

                //UserApiService.UpdateUser();
            }
        }
    }
}
