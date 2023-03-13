using Assignment_04.Model;
using System.Data;

namespace Assignment_04.DAL
{
    public class VideoDAL : IVideoDAL
    {
        private readonly VideoContext _context;
        public VideoDAL(VideoContext context) 
        {
            _context = context;
        }
        public IEnumerable<Video> GetAllVideoUsingSQL() 
            => _context.Video
                .Where(v => !v.IsDeleted.Value && !v.IsRented.Value)
                .Select(v => new Video() { Id = v.Id, Title = v.Title, Year = v.Year }).ToList();

        public IEnumerable<Video> GetSpecificVideoByYear(int year) 
            => _context.Video
                .Where(v => v.Year == year && !v.IsDeleted.Value && !v.IsRented.Value)
                .Select(v => v)
                .ToList();

        public bool AddVideo(Video videoModel)
        {
            var video = new Video
            {
                Title = videoModel.Title,
                Year = videoModel.Year,
                IsRented = false,
                IsDeleted = false
            };

            _context.Add(video);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateVideo(int? videoId, bool isRented = false)
        {
            var video = _context.Video.Where(v => v.Id == videoId).First();
            video.IsRented = isRented;

            _context.Update(video);
            _context.SaveChanges();

            return true;
        }

        public bool DeleteVideo(int? videoId, bool isDeleted = false)
        {
            var video = _context.Video.Where(v => v.Id == videoId).First();
            video.IsDeleted = isDeleted;

            _context.Update(video);
            _context.SaveChanges();

            return true;
        }
    }
}
