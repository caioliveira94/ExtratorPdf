using Data.Entities;
using Data.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class VideoService : IVideoService
    {
        IVideoInterface _IVideo;

        public VideoService(IVideoInterface IVideo)
        {
            _IVideo = IVideo;
        }

        public void Add(Video Entity)
        {
            _IVideo.Add(Entity);
        }

        public List<Video> GetAll()
        {
            return _IVideo.GetAll();
        }

        public Video GetById(int Id)
        {
            return _IVideo.GetById(Id);
        }

        public List<Video> GetList(string searchText = "")
        {
            var videos = _IVideo.GetAll();

            return videos.Where(v => v.Descricao.Contains(searchText) ||
                                v.Titulo.Contains(searchText) ||
                                v.Categoria.Contains(searchText)).ToList();
        }

        public void Remove(Video Entity)
        {
            _IVideo.Remove(Entity);
        }

        public void Update(Video Entity)
        {
            _IVideo.Update(Entity);
        }
    }
}
