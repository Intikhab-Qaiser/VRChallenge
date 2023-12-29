using System;
using VRChallenge.DB.Model;

namespace VRChallenge.Service
{
    public interface IDbService
    {
        Task Save(List<Domain.Box> data);
    }
}
