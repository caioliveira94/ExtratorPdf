using Data.Entities;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IVideoService : IServiceGeneric<Video>
    {
        List<Video> GetList(string searchText = "");
    }
}
