﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Redzen.Structures;

namespace SharpNeat.Windows.App
{
    /// <summary>
    /// Logging control. The log4net subsystem is configured to patch log message through to this class'
    /// static Log() method. Log messages are held in a circular buffer and are displayed in a referenced
    /// GUI ListBox.
    /// </summary>
    public class Logger
    {
        delegate void LogDelegate(string message);
        static readonly LogDelegate logDelegate = new LogDelegate(Log);

        static ListBox __lbxLog;
        static readonly BindingList<LogItem> __bindingList;

        #region Static Constructor

        static Logger()
        {
            __bindingList = new BindingList<LogItem>(new LogBuffer(500));
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Assign the GUI ListBox to display log messages on.
        /// </summary>
        /// <param name="lbxLog"></param>
        public static void SetListBox(ListBox lbxLog)
        {
            __lbxLog = lbxLog;
            lbxLog.DataSource = __bindingList;
            lbxLog.ValueMember = "Message";
            lbxLog.DisplayMember = "Message";
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        public static void Log(string message)
        {
            // Handle calls from threads other than the GUI thread (re-invoke the method on the GUI thread, but don't wait for completion).
            if(__lbxLog.InvokeRequired)
            {
                __lbxLog.BeginInvoke(logDelegate, new object[] {message});
                return;
            }
            __bindingList.Add(new LogItem(message));
            __lbxLog.SelectedIndex = __lbxLog.Items.Count-1;
        }

        /// <summary>
        /// Clear the circular history buffer of log messages.
        /// </summary>
        public static void Clear()
        {
            __bindingList.Clear();
        }

        #endregion

        #region Inner Classes

        /// <summary>
        /// Represents a single log item.
        /// </summary>
        public class LogItem
        {
            readonly string _message;

            /// <summary>
            /// Construct with the specified log message.
            /// </summary>
            public LogItem(string message)
            {
                _message = message;
            }

            /// <summary>
            /// Gets the log message.
            /// </summary>
            public string Message
            {
                get { return _message; }
            }
        }

        class LogBuffer : CircularBuffer<LogItem>, IList<LogItem>
        {
            public LogBuffer(int capacity) : base(capacity)
            {
            }

            #region IList<LogItem>

            int IList<LogItem>.IndexOf(LogItem item)
            {
                throw new NotImplementedException();
            }

            void IList<LogItem>.Insert(int index, LogItem item)
            {
                this.Enqueue(item);
            }

            void IList<LogItem>.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            LogItem IList<LogItem>.this[int index]
            {
                get { return this[index]; }
                set { return; }
            }

            void ICollection<LogItem>.Add(LogItem item)
            {
                this.Enqueue(item);
            }

            void ICollection<LogItem>.Clear()
            {
                this.Clear();
            }

            bool ICollection<LogItem>.Contains(LogItem item)
            {
                throw new NotImplementedException();
            }

            void ICollection<LogItem>.CopyTo(LogItem[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            int ICollection<LogItem>.Count
            {
                get { return this.Length; }
            }

            bool ICollection<LogItem>.IsReadOnly
            {
                get { return false; }
            }

            bool ICollection<LogItem>.Remove(LogItem item)
            {
                throw new NotImplementedException();
            }

            IEnumerator<LogItem> IEnumerable<LogItem>.GetEnumerator()
            {
                int len = this.Length;
                for(int i=0; i < len; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                int len = this.Length;
                for(int i=0; i < len; i++)
                {
                    yield return this[i];
                }
            }

            #endregion
        }

        #endregion
    }
}
