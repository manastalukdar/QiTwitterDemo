using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterDataContract
{
    public class TwitterTrendingData
    {
        [Key]
        public DateTime TimeId //CreatedAt
        {
            get;
            set;
        }

        public List<TrendData> Trends
        {
            get;
            set;
        }
    }

    public class TrendData
    {
        public string Name
        {
            get;
            set;
        }

        public List<MyTweet> Tweets
        {
            get;
            set;
        }
    }

    public class MyTweet
    {
        public string Text
        {
            get;
            set;
        }

        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }
    }
}
