using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimestampScheduler.Model
{
    public class Schedule(string id, List<Operation> operations)
    {
        public string Id { get; } = id;
        public List<Operation> Operations { get; } = operations;
        public override string ToString()
        {
            return $"{Id}: [{string.Join(", ", Operations.Select(op => $"{op.Type}{op.TupleId}({op.TransactionId})"))}]";
        }
    }
}
