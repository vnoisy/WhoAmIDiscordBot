using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    public class Result
    {
        public string p_id { get; set; }
        public string p_name { get; set; }
        public int p_level { get; set; }
        public string p_platform { get; set; }
        public string p_user { get; set; }
        public int p_currentmmr { get; set; }
        public int p_currentrank { get; set; }
        public int verified { get; set; }
        public int kd { get; set; }
    }
    public class SearchModel
    {
        public List<Result> results { get; set; }
        public int totalresults { get; set; }
    }
}
