using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;

namespace QiTwitterDemo.Helper
{
    public class TwitterDataManager
    {
        #region Private Fields

        private bool _isTest = false;
        private IFilteredStream _filteredStream;

        #endregion Private Fields

        #region Public Constructors

        public TwitterDataManager(bool isTest = false)
        {
            _filteredStream = Stream.CreateFilteredStream();
            var accessToken = Constants.AccessToken;
            var accessTokenSecret = Constants.AccessTokenSecret;
            var consumerKey = Constants.ConsumerKey;
            var consumerSecret = Constants.ConsumerSecret;
            Auth.ApplicationCredentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            if (isTest)
            {
                _isTest = true;
            }
        }

        #endregion Public Constructors

        #region Public Methods

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Fetches twitter stream.</summary>
        ///
        /// <param name="latitude1"> The first latitude.</param>
        /// <param name="longitide1">The first longitide.</param>
        /// <param name="latitude2"> The second latitude.</param>
        /// <param name="longitide2">The second longitide.</param>
        /// <remarks>https://github.com/linvi/tweetinvi/issues/72</remarks>
        ///-------------------------------------------------------------------------------------------------
        public void FetchTwitterStream(double latitude1, double longitide1, double latitude2, double longitide2)
        {
            var centerOfNewYork = new Location(new Coordinates(longitide1, latitude1), new Coordinates(longitide2, latitude2));
            _filteredStream.AddLocation(centerOfNewYork);
            int count = 0;
            _filteredStream.MatchingTweetReceived += (sender, args) =>
            {
                if (_isTest)
                {
                    Console.WriteLine("A tweet has been found; the tweet is '" + args.Tweet + "'");
                    count++;
                    if (count > 10)
                    {
                        _filteredStream.StopStream();
                    }
                }
            };
            _filteredStream.StartStreamMatchingAllConditions();
        }

        public void FetchTwitterStream(string searchString)
        {
            _filteredStream.AddTrack(searchString);
            int count = 0;
            _filteredStream.MatchingTweetReceived += (sender, args) =>
            {
                if (_isTest)
                {
                    Console.WriteLine("A tweet has been found; the tweet is '" + args.Tweet + "'");
                    count++;
                    if (count > 10)
                    {
                        _filteredStream.StopStream();
                    }
                }
            };
            _filteredStream.StartStreamMatchingAllConditions();
        }

        public void StopStream()
        {
            _filteredStream.StopStream();
        }

        #endregion Public Methods

        #region Private Methods

        private void FetchTwitterTrends(double latitude, double longitude, long woeId)
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

        #endregion Private Methods
    }
}