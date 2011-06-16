using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucid.Base;
using System.Drawing;

namespace Lucid.Raytracing
{
    public class RaytracingMaster : JobMaster
    {
        /// <summary>
        /// List of task that have been requested but not yet joined.
        /// </summary>
        private List<Task> tasksNotCompleted;
        private bool wholeWorkSent = false;

        private int taskRectWidht = 50;
        private int taskRectHeight = 80;
        /// <summary>
        /// Width of one task rectagle.
        /// </summary>
        public int TaskRectWidth
        {
            get { return taskRectWidht; }
            set { taskRectWidht = value; }
        }
        /// <summary>
        /// Height of one task rectagle.
        /// </summary>
        public int TaskRectHeight
        {
            get { return taskRectHeight; }
            set { taskRectHeight = value; }
        }

        private int curX;
        private int curY;
        private int curTaskNo;

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public RaytracingMaster()
            : base()
        {
            init();
        }

        private int getPixelNoFromXY(int x, int y)
        {
            return (y * ImageWidth + x);
        }

        private Rectangle clipRect(Rectangle rect, int imageWidth, int imageHeight)
        {
            return new Rectangle(
                Math.Max(0, rect.Left),
                Math.Max(0, rect.Top),
                Math.Min(imageWidth - rect.Left, rect.Width),
                Math.Min(imageHeight - rect.Top, rect.Height));
        }

        private void init()
        {
            curTaskNo = 0;
            curX = curY = 0;
            ImageWidth = 0;
            ImageHeight = 0;
            tasksNotCompleted = new List<Task>();
            wholeWorkSent = false;
        }

        public override void ResetForNewJob(JobSettings job)
        {
            SceneSettings jobScene = job as SceneSettings;
            if (jobScene == null)
                throw new Exception("Must set SceneSettings to " + this.GetType().Name);

            init();
            ImageWidth = jobScene.ImageWidth;
            ImageHeight = jobScene.ImageHeight;
        }

        public override Task GetNextTask()
        {
            if (ImageWidth == 0 || ImageHeight == 0)
                throw new InvalidOperationException("ImageWidth and ImageHeight must be set.");

            // simulation of while loop
            // test at the beginning
            if (curY >= ImageHeight)
            {
                if (!Completed)
                {
                    // look if there are any pending tasks
                    return tasksNotCompleted.First();
                }
                else
                {
                    // no pending tasks
                    return null;
                }
            }

            // processing
            Rectangle taskRect = new Rectangle(curX, curY, taskRectWidht, taskRectHeight);
            taskRect = clipRect(taskRect, ImageWidth, ImageHeight);

            Task result = new Task
            {
                Start = getPixelNoFromXY(taskRect.Left, taskRect.Top),
                End = getPixelNoFromXY(taskRect.Left + taskRect.Width - 1, taskRect.Top + taskRect.Height - 1),
                Number = ++curTaskNo
            };

            // maintenance code at the end
            curX += taskRectWidht;
            if (curX >= ImageWidth)
            {
                curX = 0;
                curY += taskRectHeight;
            }
            if (curY >= ImageHeight)
            {
                // finished first loop through the image
                wholeWorkSent = true;
            }

            // result from processing
            tasksNotCompleted.Add(result);
            return result;
        }

        public override void JoinCompletedTask(Task completedTask)
        {
            // delete completed task from list of pending tasks
            tasksNotCompleted.RemoveAll(
                (Task t) => (t.Start == completedTask.Start && t.End == completedTask.End));
            onTaskComplete(completedTask);

            if (wholeWorkSent && tasksNotCompleted.Count == 0)
            {
                // everything sent and nothing remaining unsolved
                Completed = true;
                onComplete();
            }
        }
    }
}
