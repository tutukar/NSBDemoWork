using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Utilities
{
    /// <summary>
    /// Tracks IDs for request response messages.
    /// </summary>
    /// <typeparam name="T">Type of data to store before sending a request for retrieval and use after getting a response.</typeparam>
    public class MessageTracker<T>
    {
        #region Fields

        private static readonly MessageTracker<T> instance = new MessageTracker<T>();
        private Dictionary<Guid, Tuple<Timer, T>> dictionary;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets an instance of singleton <see cref="MessageTracker{T}" />
        /// </summary>
        public static MessageTracker<T> Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Timeout interval in milliseconds
        /// </summary>
        public double TimeoutIntervalMs
        {
            get
            {
                return TimeoutInterval.TotalMilliseconds;
            }
            set
            {
                if (value <= Int32.MaxValue)
                {
                    TimeoutInterval = new TimeSpan((long)value * TimeSpan.TicksPerMillisecond);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("TimeoutInterval", value, string.Format("The TimeoutInterval can be a value between 0 and {0} ms.", Int32.MaxValue));
                }
            }
        }

        /// <summary>
        /// Timeout interval
        /// </summary>
        public TimeSpan TimeoutInterval { get; set; }

        #endregion

        #region Public Events

        public delegate void TimeoutEventHandler(object sender, MessageTrackerEventArgs<T> e);
        public event TimeoutEventHandler MessageTimeout;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        private MessageTracker()
        {
            dictionary = new Dictionary<Guid, Tuple<Timer, T>>();
        } 

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Track a message ID.
        /// </summary>
        /// <param name="id">ID of the message to track.</param>
        public void Track(Guid id)
        {
            var timer = new Timer(TimeoutIntervalMs);
            timer.Elapsed += new ElapsedEventHandler((sender, e) => OnMessageTimeout(sender, e, id));
            timer.Enabled = true;

            dictionary.Add(id, new Tuple<Timer, T>(timer, default(T)));
        }
        
        /// <summary>
        /// Track a message ID and hold on to associated data.
        /// </summary>
        /// <param name="id">ID of the message to track.</param>
        /// <param name="data">Data associated with the message.</param>
        public void Track(Guid id, T data)
        {
            var timer = new Timer(TimeoutIntervalMs);
            timer.Elapsed += new ElapsedEventHandler((sender, e) => OnMessageTimeout(sender, e, id));
            timer.Enabled = true;

            dictionary.Add(id, new Tuple<Timer, T>(timer, data));
        }
        
        /// <summary>
        /// Stop tracking a message ID.
        /// </summary>
        /// <param name="id">ID of the message to stop tracking.</param>
        /// <returns>True if the ID existed at the time, false otherwise.</returns>
        public bool StopTracking(Guid id)
        {
            if (dictionary.ContainsKey(id))
            {
                var timer = this.GetTimer(id);

                if (timer != null)
                {
                    timer.Close();
                }
            }

            return dictionary.Remove(id);
        }

        /// <summary>
        /// Tells if a tracked ID is found.
        /// </summary>
        /// <param name="id">ID of the message.</param>
        /// <returns>True if the ID is found, false otherwise.</returns>
        public bool Exists(Guid id)
        {
            return dictionary.ContainsKey(id);
        }
        
        /// <summary>
        /// Gets data associated with the message based on ID.
        /// </summary>
        /// <param name="id">ID of the message.</param>
        /// <returns>The associated data stored at the time of tracking.</returns>
        public T GetData(Guid id)
        {
            T retVal = default(T);

            if (dictionary.ContainsKey(id))
            {
                retVal = dictionary[id].Item2;
            }

            return retVal;
        } 

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Gets timer associated with the message based on ID.
        /// </summary>
        /// <param name="id">ID of the message.</param>
        /// <returns>The <see cref="System.Timers.Timer" /> used for tracking the message.</returns>
        private Timer GetTimer(Guid id)
        {
            Timer retVal = null;

            if (dictionary.ContainsKey(id))
            {
                retVal = dictionary[id].Item1;
            }

            return retVal;
        }
        
        /// <summary>
        /// Virtual method to override for handling the MessageTimeout event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Timer event arguments.</param>
        /// <param name="id">ID of the message.</param>
        protected virtual void OnMessageTimeout(object sender, ElapsedEventArgs e, Guid id)
        {
            var eventArgs = new MessageTrackerEventArgs<T>()
            {
                MessageId = id,
                Data = GetData(id),
            };

            if (MessageTimeout != null)
            {
                MessageTimeout(this, eventArgs);
            }

            this.StopTracking(id);
        }

        #endregion
    }
}
