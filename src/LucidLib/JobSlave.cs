using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucid.Base
{
    /// <summary>
    /// Worker, that knows how to process given Task.
    /// </summary>
    public abstract class JobSlave
    {
        public abstract void SetJob(JobSettings job);

        public abstract void RunTask(Task task);
    }
}
