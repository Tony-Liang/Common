using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.WMI
{
    public class ProcessInfo
    {
        public ProcessInfo(int ProcessID, string ProcessName, double ProcessorTime,
                            long WorkingSet, string ProcessPath)
        {
            this.ProcessID = ProcessID;
            this.ProcessName = ProcessName;
            this.ProcessorTime = ProcessorTime;
            this.WorkingSet = WorkingSet;
            this.ProcessPath = ProcessPath;
        }

        private int m_ProcessID;
        public int ProcessID
        {
            get { return m_ProcessID; }
            set { m_ProcessID = value; }
        }

        private string m_ProcessName;
        public string ProcessName
        {
            get { return m_ProcessName; }
            set { m_ProcessName = value; }
        }

        private double m_ProcessorTime;
        public double ProcessorTime
        {
            get { return m_ProcessorTime; }
            set { m_ProcessorTime = value; }
        }

        private long m_WorkingSet;
        public long WorkingSet
        {
            get { return m_WorkingSet; }
            set { m_WorkingSet = value; }
        }

        private string m_ProcessPath;
        public string ProcessPath
        {
            get { return m_ProcessPath; }
            set { m_ProcessPath = value; }
        }
    }
}
