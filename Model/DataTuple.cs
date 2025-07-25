using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimestampScheduler.Model
{
    public class DataTuple(string id, int TSRead, int TSWrite)
    {
        public string Id { get; } = id;
        public int TSRead { get; set; } = TSRead;
        public int TSWrite { get; set; } = TSWrite;
        public override string ToString()
        {
            return $"{Id}({TSRead}, {TSWrite})";
        }

        public bool CanRead(Transaction tx)
        {
            return !(tx.Timestamp < TSWrite);
        }

        public bool CanWrite(Transaction tx)
        {             
            return !(tx.Timestamp < TSWrite ||
                    tx.Timestamp < TSRead);
        }

        public void Reset()
        {
            TSRead = 0;
            TSWrite = 0;
        }

        public void UpdateTSRead(Transaction tx)
        {
            if (TSRead < tx.Timestamp)
            {
                TSRead = tx.Timestamp;
            }
        }

        public void UpdateTSWrite(Transaction tx)
        {
            if (TSWrite < tx.Timestamp)
            {
                TSWrite = tx.Timestamp;
            }
        }
    }

}
