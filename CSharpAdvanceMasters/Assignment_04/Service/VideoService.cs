using Assignment_04.DAL;
using Assignment_04.Model;
using Assignment_04.Service.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04.Service
{
    public class VideoService : IVideoService
    {
        private readonly IVideoDAL _videoDAL;
        public VideoService(IVideoDAL videoDAL)
        {
            _videoDAL = videoDAL ?? throw new ArgumentNullException(nameof(videoDAL));
        }

        public IEnumerable<Video> GetAllVideos() => _videoDAL.GetAllVideoUsingSQL().ToList();

        public IEnumerable<Video> GetVideoByYear(int year) => _videoDAL.GetSpecificVideoByYear(year);

        public bool AddVideo(string title, int year) => _videoDAL.AddVideo(new Video { Title = title, Year = year });

        public bool RentVideo(int? videoId, bool isRented) => _videoDAL.UpdateVideo(videoId, isRented);

        public bool DeleteVideo(int? videoId, bool isDeleted) => _videoDAL.DeleteVideo(videoId, isDeleted);
    }
}
