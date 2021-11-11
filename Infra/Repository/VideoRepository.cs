using Data.Entities;
using Data.Interfaces;
using Infra.Repository.Generic;

namespace Infra.Repository
{
    public class VideoRepository : RepositoryGeneric<Video>, IVideoInterface
    {

    }
}
