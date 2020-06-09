using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlexClient.Library.Models;

namespace Player
{
    public interface IPlayer
    {
        IObservable<TrackModel> PlayingTrack { get; }
        IObservable<PlayerState> State { get; }
        IObservable<long> DurationAvancement { get; }

        void Play(TrackModel trackModel);
        void Stop();
        void Pause();
        void Resume();
    }
}
