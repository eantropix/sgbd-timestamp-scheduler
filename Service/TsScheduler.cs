using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimestampScheduler.Model;
using TimestampScheduler.Parser;

namespace TimestampScheduler.Service
{
    public class TsScheduler
    {
        private string _scheduleId { get; set; } = "";
        private readonly Dictionary<string, DataTuple> _tuples = new();
        private readonly Dictionary<string, Transaction> _transactions = new();
        private List<Operation> _operations = new();
        

        public void Initialize(List<DataTuple> tuples, List<Transaction> transactions, Schedule schedule)
        {
            foreach (var tuple in tuples)
            {
                _tuples[tuple.Id] = tuple;
            }

            foreach (var transaction in transactions)
            {
                _transactions[transaction.Id] = transaction;
            }

            _operations = schedule.Operations;
            _scheduleId = schedule.Id;

            foreach (var tuple in _tuples.Values)
            {
                tuple.Reset();
            }
        }

        public void CheckSchedule()
        {
            var scheduleTime = 0;
            var failure = false;
            foreach (var op in _operations)
            {
                if (failure) break;
                switch (op.Type)
                {
                    case "r":
                        if (!_transactions.TryGetValue(op.TransactionId, out var transactionRead))
                        {
                            Console.WriteLine($"Transaction {op.TransactionId} not found for read operation.");
                            return;
                        }
                        if (!_tuples.TryGetValue(op.TupleId, out var tupleRead))
                        {
                            Console.WriteLine($"Tuple {op.TupleId} not found for read operation.");
                            return;
                        }

                        if (!tupleRead.CanRead(transactionRead))
                        {
                            OperationLogger.LogOutput(_scheduleId, "ROLLBACK", scheduleTime);
                            failure = true;
                            break;
                        }
                        tupleRead.UpdateTSRead(transactionRead);
                        OperationLogger.LogOperation(tupleRead.Id, _scheduleId, "read", scheduleTime);
                        break;

                    case "w":
                        if (!_transactions.TryGetValue(op.TransactionId, out var transactionWrite))
                        {
                            Console.WriteLine($"Transaction {op.TransactionId} not found for write operation.");
                            return;
                        }
                        if (!_tuples.TryGetValue(op.TupleId, out var tupleWrite))
                        {
                            Console.WriteLine($"Tuple {op.TupleId} not found for write operation.");
                            return;
                        }

                        if (!tupleWrite.CanWrite(transactionWrite))
                        {
                            OperationLogger.LogOutput(_scheduleId, "ROLLBACK", scheduleTime);
                            failure = true;
                            break;
                        }
                        tupleWrite.UpdateTSWrite(transactionWrite);
                        OperationLogger.LogOperation(tupleWrite.Id, _scheduleId, "write", scheduleTime);
                        break;

                    case "c":
                        foreach (var tuple in _tuples.Values)
                        {
                            tuple.Reset();
                        }
                        break;
                    default:
                        return;
                }
                scheduleTime++;
            }
            if (!failure)
            {
                OperationLogger.LogOutput(_scheduleId, "OK");
            }
        }
    }
}
