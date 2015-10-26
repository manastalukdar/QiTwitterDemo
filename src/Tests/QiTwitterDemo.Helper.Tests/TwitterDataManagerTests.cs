using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QiTwitterDemo.Helper.Tests
{
    [TestClass]
    public class TwitterDataManagerTests
    {
        [TestMethod]
        public void GetTweets()
        {
            var twitterDataManager = new TwitterDataManager();
            twitterDataManager.FetchTwitterStream("bihar");
            //twitterDataManager.FetchTwitterTrends(-73.47158505937502, 41.9574765444553, 2487956);
            //twitterDataManager.FetchTwitterStream(-73.47158505937502, 41.9574765444553, 23424977);
        }
    }
}
