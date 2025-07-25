using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimestampScheduler.Model
{
    public class Transaction(string id, int timestamp)
    {
        public string Id { get; } = id;
        public int Timestamp { get; set; } = timestamp;
    }
}
