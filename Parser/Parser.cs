using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimestampScheduler.Model;

namespace TimestampScheduler.Parser
{
    public class Parser
    {
        public static (List<DataTuple>, List<Transaction>, List<Schedule>) ParseInputFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            // Parse Data Tuples
            var dataTuples = lines[0].Split(',').Select(tuple => tuple.Trim().Replace(";", "")).Select(id => new DataTuple(id, 0, 0)).ToList();

            // Parse Transactions
            var transactionIds = lines[1].Split(',').Select(id => id.Trim().Replace("t", "").Replace(";", "")).ToList();

            // Replace the problematic line with the following:
            var timestamps = lines[2].Split(',').Select(ts => int.Parse(ts.Trim().Replace(";", ""))).ToList();

            var transactions = transactionIds.Zip(timestamps, (id, ts) => new Transaction(id, ts)).ToList();

            // Parse Schedules
            var schedules = new List<Schedule>();
            for (int i = 3; i < lines.Length; i++)
            {
                var scheduleParts = lines[i].Split('-');
                var scheduleId = scheduleParts[0].Trim();
                var operations = scheduleParts[1].Split(' ').Select(op => op.Trim())
                    .Where(op => !string.IsNullOrEmpty(op))
                    .Select(op => ParseOperation(op))
                    .ToList();

                schedules.Add(new Schedule(scheduleId, operations));
            }

            return (dataTuples, transactions, schedules);
        }

        private static Operation ParseOperation(string operation)
        {
            if (operation.StartsWith("r", StringComparison.OrdinalIgnoreCase))
            {
                var tupleIdStart = operation.IndexOf('(') + 1;
                var tupleIdEnd = operation.IndexOf(')');
                return new Operation(
                    "r",
                    operation.Substring(tupleIdStart, tupleIdEnd - tupleIdStart),
                    operation.Substring(1, 1)
                );
            }
            else if (operation.StartsWith("w", StringComparison.OrdinalIgnoreCase))
            {
                var tupleIdStart = operation.IndexOf('(') + 1;
                var tupleIdEnd = operation.IndexOf(')');
                return new Operation(
                    "w",
                    operation.Substring(tupleIdStart, tupleIdEnd - tupleIdStart),
                    operation.Substring(1, 1)
                );
            }
            else if (operation.Equals("c", StringComparison.OrdinalIgnoreCase))
            {
                return new Operation(
                    "c",
                    null,
                    null
                );
            }

            throw new ArgumentException($"Invalid operation format: {operation}");
        }
    }
}
