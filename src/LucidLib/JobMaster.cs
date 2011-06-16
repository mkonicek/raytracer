using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Common;

namespace Lucid.Base
{
    /// <summary>
    /// Master responsible for tailoring tasks for clients.
    /// </summary>
    public abstract class JobMaster
    {
        /// <summary>
        /// Reset to initial state.
        /// </summary>
        public abstract void ResetForNewJob(JobSettings job);

        /// <summary>
        /// Gets next task to be done.
        /// </summary>
        public abstract Task GetNextTask();

        /// <summary>
        /// Joins completed tasks with other completed tasks.
        /// </summary>
        public abstract void JoinCompletedTask(Task taskCompleted);

        public bool Completed { get; set; }

        /// <summary>
        /// Job has been completed. 
        /// Internal so that LucidServer can bind to it, but client code can't.
        /// </summary>
        internal event EventHandler Complete;
        protected void onComplete()
        {
            if (Complete != null)
                Complete(this, EventArgs.Empty);
        }

        /// <summary>
        /// A task has been completed. 
        /// Internal so that LucidServer can bind to it, but client code can't.
        /// </summary>
        internal event ValueEventHandler<Task> TaskComplete;
        protected void onTaskComplete(Task taskCompleted)
        {
            if (TaskComplete != null)
                TaskComplete(this, taskCompleted);
        }
    }
}
