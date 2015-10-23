using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Json;
using TwitterDataContract;

namespace QiTwitterDemo.Helper
{
    public class TwitterDataManager
    {
        public TwitterDataManager()
        {
            var accessToken = Constants.TokenAccessToken;
            var accessTokenSecter = Constants.TokenAccessTokenSecret;
            var consumerKey = Constants.TokenConsumerKey;
            var consumerSecret = Constants.TokenConsumerSecret;
            Auth.ApplicationCredentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecter);
            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecter);
        }
        public void GetTweets(double longitide, double latitude, long woeid)
        {
            var stream = Stream.CreateFilteredStream();
            var centerOfNewYork = new Location(new Coordinates(-74, 40), new Coordinates(-73, 41));
            stream.AddLocation(centerOfNewYork);
            int count = 0;
            var twitterTrendingData = new TwitterTrendingData { Trends = new List<TrendData>() };
            stream.MatchingTweetReceived += (sender, args) =>
            {
                var tweet = args.Tweet;
                var trendData = new TrendData { Name = "dummy" };
                Console.WriteLine("A tweet has been found; the tweet is '" + args.Tweet + "'");
                var myTweet = new MyTweet
                {
                    Latitude = tweet.Coordinates.Latitude,
                    Longitude = tweet.Coordinates.Longitude,
                    Text = tweet.Text
                };
                trendData.Tweets.Add(myTweet);
                count++;
                if (count > 10)
                {
                    stream.StopStream();
                }
            };
            stream.StartStreamMatchingAllConditions();


            //var placeTrends = Trends.GetTrendsAt(1);
            //var twitterTrendingData = new TwitterTrendingData {Trends = new List<TrendData>()};
            //foreach (var item in placeTrends.Trends)
            //{
            //    twitterTrendingData.TimeId = placeTrends.AsOf;
            //    var trendData = new TrendData {Name = item.Name};

            //    var searchParameter = Search.CreateTweetSearchParameter(item.Name);

            //    // searchParameter.MaximumNumberOfResults = 500;
            //    ICoordinates coord = new Coordinates(longitide, latitude);
            //    searchParameter.SetGeoCode(coord, 50000, DistanceMeasure.Miles);
            //    var tweets = Search.SearchTweets(searchParameter);

            //    trendData.Tweets = new List<MyTweet>();
            //    foreach (var tweet in tweets)
            //    {
            //        var myTweet = new MyTweet
            //        {
            //            Latitude = tweet.Coordinates.Latitude,
            //            Longitude = tweet.Coordinates.Longitude,
            //            Text = tweet.Text
            //        };
            //        trendData.Tweets.Add(myTweet);
            //    }

            //    twitterTrendingData.Trends.Add(trendData);
            //}
        }
    }
}
