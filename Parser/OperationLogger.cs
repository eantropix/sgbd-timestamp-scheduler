using System.IO;
using System;
using System.Collections.Generic;

namespace TimestampScheduler.Parser
{
    public static class OperationLogger
    {
        private static readonly string DataDir = Path.Combine("Data");

        public static void LogOperation(string tupleId, string scheduleId, string operationType, int scheduleTime)
        {
            var filePath = Path.Combine(DataDir, $"{tupleId}.txt");
            //if (!File.Exists(filePath))
            //{
            //    using (File.Create(filePath)) { }
            //}
            var logEntry = $"{scheduleId},{operationType},{scheduleTime}";
            File.AppendAllText(filePath, logEntry + Environment.NewLine);
        }

        //public static void LogOperations(IEnumerable<(string tupleId, string operationType, string transactionId)> operations)
        //{
        //    foreach (var op in operations)
        //    {
        //        LogOperation(op.tupleId, op.operationType, op.transactionId);
        //    }
        //}

        public static void LogOutput(string scheduleId, string status, int? ts = null)
        {
            var filePath = Path.Combine(DataDir, "out.txt");
            //if (!File.Exists(filePath))
            //{
            //    using (File.Create(filePath)) { }
            //}
            var logEntry = $"{scheduleId}-{status}-{ts}";
            File.AppendAllText(filePath, logEntry + Environment.NewLine);
        }
    }
}
