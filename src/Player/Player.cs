using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PlexClient.Library.Models;

namespace Player
{
    public class Player : IPlayer
    {

        public IObservable<TrackModel> PlayingTrack { get; set; }
        private readonly BehaviorSubject<TrackModel> _playingTrack;

        public IObservable<PlayerState> State { get; set; }
        public IObservable<long> DurationAvancement { get; set; }

        public Player()
        {
            _playingTrack = new BehaviorSubject<TrackModel>(null);

            PlayingTrack = _playingTrack.DistinctUntilChanged();
        }

        public void Play(TrackModel trackModel)
        {
            _playingTrack.OnNext(trackModel);
            // TODO play
        }

        public void Stop()
        {
            _playingTrack.OnNext(null);
            // TODO stop
        }

        public void Pause()
        {
            // TODO pause
        }

        public void Resume()
        {
            // TODO resume
        }
    }
}
