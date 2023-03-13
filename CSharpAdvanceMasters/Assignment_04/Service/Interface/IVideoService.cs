using Assignment_04.Model;

namespace Assignment_04.Service.Interface
{
    public interface IVideoService
    {
        public IEnumerable<Video> GetAllVideos();
        public IEnumerable<Video> GetVideoByYear(int year);
        public bool AddVideo(string title, int year);
        public bool RentVideo(int? videoId, bool isRented);
        public bool DeleteVideo(int? videoId, bool isDeleted);
    }
}
