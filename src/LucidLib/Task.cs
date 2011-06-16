using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Lucid.Base
{
    /// <summary>
    /// Piece of job to be computed at single client.
    /// </summary>
    [DataContract(Namespace = Constants.LucidNamespace)]
    public class Task
    {
        /// <summary>
        /// Task number. Tasks should be numbered from 1.
        /// </summary>
        [DataMember]
        public long Number { get; set; }

        /// <summary>
        /// Offset in job at which this task starts.
        /// </summary>
        [DataMember]
        public long Start { get; set; }

        /// <summary>
        /// Offset in job at which this task ends.
        /// </summary>
        [DataMember]
        public long End { get; set; }

        /// <summary>
        /// Computed task result.
        /// </summary>
        [DataMember]
        public object Result { get; set; }

        public Task()
        {
            Number = 0;
            Start = 0;
            End = 0;
            Result = null;
        }
    }
}
