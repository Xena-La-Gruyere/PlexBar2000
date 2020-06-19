using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using ApplicationState;
using ApplicationState.States;
using Library.Abstractions.Models;
using Microsoft.Extensions.Hosting;

namespace MediaSession
{
    public class MediaSessionService : IHostedService
    {
        private readonly IApplicationStateService _applicationStateService;
        private IDisposable _stateSub;
        private IDisposable _playingTrack;
        private SystemMediaTransportControls _systemMediaTransportControls;

        public MediaSessionService(IApplicationStateService applicationStateService)
        {
            _applicationStateService = applicationStateService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _systemMediaTransportControls = BackgroundMediaPlayer.Current.SystemMediaTransportControls;
            _systemMediaTransportControls.IsPlayEnabled = true;
            _systemMediaTransportControls.IsPauseEnabled = true;
            _systemMediaTransportControls.IsNextEnabled = true;
            _systemMediaTransportControls.IsPreviousEnabled = true;
            _systemMediaTransportControls.IsStopEnabled = true;

            _systemMediaTransportControls.ButtonPressed += SystemControls_ButtonPressed;

            _stateSub = _applicationStateService.PlayerState
                .Select(p => p.PlayingState)
                .Subscribe(PlayingStateChanged);

            _playingTrack = _applicationStateService.PlayingTrack
                .Subscribe(PlayingTrackChanged);

            return Task.CompletedTask;
        }

        private void PlayingTrackChanged(TrackModel track)
        {
            if (track is null)
            {
                _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                return;
            }

            // Get the updater.
            var updater = _systemMediaTransportControls.DisplayUpdater;

            // Music metadata.
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = track.Artist;
            updater.MusicProperties.AlbumArtist = track.Album;
            updater.MusicProperties.Title = track.Title;

            // Set the album art thumbnail.
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(track.ThumbnailUrl);

            // Update the system media transport controls.
            updater.Update();
        }

        private void PlayingStateChanged(PlayingStateEnum state)
        {
            _systemMediaTransportControls.PlaybackStatus = state switch
            {
                PlayingStateEnum.Playing => MediaPlaybackStatus.Playing,
                PlayingStateEnum.Paused => MediaPlaybackStatus.Paused,
                PlayingStateEnum.Stopped => MediaPlaybackStatus.Stopped,
                _ => MediaPlaybackStatus.Closed
            };
        }

        private void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                case SystemMediaTransportControlsButton.Pause:
                    _applicationStateService.PauseResume();
                    break;
                case SystemMediaTransportControlsButton.Stop:
                    _applicationStateService.Stop();
                    break;
                case SystemMediaTransportControlsButton.Next:
                    _applicationStateService.Next();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    _applicationStateService.Previous();
                    break;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _stateSub?.Dispose();
            _playingTrack?.Dispose();

            if (_systemMediaTransportControls != null)
            {
                _systemMediaTransportControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                _systemMediaTransportControls.ButtonPressed -= SystemControls_ButtonPressed;
            }

            return Task.CompletedTask;
        }
    }
}
