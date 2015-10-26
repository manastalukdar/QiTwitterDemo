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
            var accessToken = Constants.AccessToken;
            var accessTokenSecret = Constants.AccessTokenSecret;
            var consumerKey = Constants.ConsumerKey;
            var consumerSecret = Constants.ConsumerSecret;
            Auth.ApplicationCredentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }
        public void FetchTwitterStream(double longitide, double latitude)
        {
            var stream = Stream.CreateFilteredStream();
            var centerOfNewYork = new Location(new Coordinates(-74, 40), new Coordinates(-73, 41));
            stream.AddLocation(centerOfNewYork);
            int count = 0;
            stream.MatchingTweetReceived += (sender, args) =>
            {
                Console.WriteLine("A tweet has been found; the tweet is '" + args.Tweet + "'");
                count++;
                if (count > 10)
                {
                    stream.StopStream();
                }
            };
            stream.StartStreamMatchingAllConditions();
        }

        public void FetchTwitterStream(string searchString)
        {
            var stream = Stream.CreateFilteredStream();
            stream.AddTrack(searchString);
            int count = 0;
            stream.MatchingTweetReceived += (sender, args) =>
            {
                Console.WriteLine("A tweet has been found; the tweet is '" + args.Tweet + "'");
                count++;
                if (count > 10)
                {
                    stream.StopStream();
                }
            };
            stream.StartStreamMatchingAllConditions();
        }

        public void FetchTwitterTrends(double latitude, double longitude, long woeId)
        {
            var placeTrends = Trends.GetTrendsAt(woeId);
            var twitterTrendingData = new TwitterTrendingData { Trends = new List<TrendData>() };
            foreach (var item in placeTrends.Trends)
            {
                twitterTrendingData.TimeId = placeTrends.AsOf;
                var trendData = new TrendData { Name = item.Name };

                var searchParameter = Search.CreateTweetSearchParameter(item.Name);

                // searchParameter.MaximumNumberOfResults = 500;
                ICoordinates coord = new Coordinates(longitude, latitude);
                searchParameter.SetGeoCode(coord, 50000, DistanceMeasure.Miles);
                var tweets = Search.SearchTweets(searchParameter);

                trendData.Tweets = new List<MyTweet>();
                foreach (var tweet in tweets)
                {
                    var myTweet = new MyTweet
                    {
                        Latitude = tweet.Coordinates.Latitude,
                        Longitude = tweet.Coordinates.Longitude,
                        Text = tweet.Text
                    };
                    trendData.Tweets.Add(myTweet);
                }

                twitterTrendingData.Trends.Add(trendData);
            }
        }
    }
}
