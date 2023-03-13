using Assignment_04.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04.DAL
{
    public interface IVideoDAL
    {
        IEnumerable<Video> GetAllVideoUsingSQL();
        IEnumerable<Video> GetSpecificVideoByYear(int year);
        bool AddVideo(Video videoModel);
        bool UpdateVideo(int? videoId, bool isRented = false);
        bool DeleteVideo(int? videoId, bool isDeleted = false);
    }
}
