using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimestampScheduler.Model
{
    public class Operation
    {
        public string Type { get; set; } // "R" for read, "W" for write, "C" for commit
        public string TupleId { get; }
        public string TransactionId { get; }

        public Operation(string type, string tupleId, string transactionId)
        {
            Type = type;
            TupleId = tupleId;
            TransactionId = transactionId;
        }
    }
}
