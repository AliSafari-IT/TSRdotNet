using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSR
    {
    class TSrecord
        {
        private readonly DateTime date;
        private readonly DateTime time;
        private readonly double variable;


        public int Id { get; }
        public DateTime Date => date;
        public DateTime Time => time;
        public double Variable => variable;

        public TSrecord (int id, DateTime date, DateTime time, double variable)
            {
            this.Id = id;
            this.date = date;
            this.time = time;
            this.variable = variable;
            }


        }
    }
