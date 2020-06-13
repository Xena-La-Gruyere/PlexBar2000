using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ApplicationState.Actions;
using ApplicationState.States;
using PlexClient.Library.Models;
using Redux;

namespace ApplicationState.Reducers
{
    public static class PlayerStateReducer
    {
        public static PlayerState Execute(PlayerState state, IAction action)
        {
            var builder = new PlayerState.Builder(state);

            switch (action)
            {

                // PLAYLIST
                case AddAlbumPlaylistAction album:
                    builder.Playlist = builder.Playlist.Add(album.Album);
                    break;
                case RemoveAlbumPlaylistAction album:
                    var indAlbum = builder.Playlist.IndexOf(album.Album);
                    var traksLenghtBefore = builder.Playlist.Take(indAlbum).SelectMany(a => a.Tracks).Count();
                    var indBeginAlbum = traksLenghtBefore - 1;
                    var indEndAlbum = indBeginAlbum + album.Album.Tracks.Length;

                    if (builder.PlayingTrackInd >= indBeginAlbum &&
                        builder.PlayingTrackInd <= indEndAlbum)
                    {
                        // Playing track of album removing
                        builder.PlayingTrackInd = 0;
                        builder.PlayingState = PlayingStateEnum.Stopped;
                    }
                    else if (builder.PlayingTrackInd > indEndAlbum)
                    {
                        // Playing track ind shift
                        builder.PlayingTrackInd = builder.PlayingTrackInd - album.Album.Tracks.Length;
                    }

                    builder.Playlist = builder.Playlist.Remove(album.Album);
                    break;
                case ClearPlaylistAction _:
                    builder.Playlist = ImmutableArray<AlbumModel>.Empty;
                    builder.PlayingTrackInd = 0;
                    builder.PlayingState = PlayingStateEnum.Stopped;
                    break;

                // PLAYER
                case PlayTrackAndAlbumAction play:
                    builder.Playlist = ImmutableArray<AlbumModel>.Empty.Add(play.Album);
                    builder.PlayingTrackInd = builder.Playlist.SelectMany(a => a.Tracks).ToList().IndexOf(play.Track);
                    builder.PlayingState = PlayingStateEnum.Playing;
                    break;
                case PlayTrackInPlaylistAction play:
                    builder.PlayingTrackInd = builder.Playlist.SelectMany(a => a.Tracks).ToList().IndexOf(play.Track);
                    builder.PlayingState = PlayingStateEnum.Playing;
                    break;
                case PlayAlbumAction play:
                    builder.Playlist = ImmutableArray<AlbumModel>.Empty.Add(play.Album);
                    builder.PlayingTrackInd = 0;
                    builder.PlayingState = PlayingStateEnum.Playing;
                    break;
                case PlayNextAction next:
                    var tracksLenght = builder.Playlist.SelectMany(a => a.Tracks).Count();
                    var nextTrackInd = builder.PlayingTrackInd + 1;
                    if (nextTrackInd == tracksLenght)
                    {
                        // No more playlist
                        builder.PlayingState = PlayingStateEnum.Stopped;
                        builder.PlayingTrackInd = 0;
                        break;
                    }

                    builder.PlayingState = PlayingStateEnum.Playing;
                    builder.PlayingTrackInd = nextTrackInd;
                    break;
                case PlayPreviousAction prev:
                    var prevTrackInd = builder.PlayingTrackInd - 1;
                    if (prevTrackInd < 0)
                    {
                        // No more playlist
                        builder.PlayingState = PlayingStateEnum.Stopped;
                        builder.PlayingTrackInd = 0;
                        break;
                    }

                    builder.PlayingState = PlayingStateEnum.Playing;
                    builder.PlayingTrackInd = prevTrackInd;
                    break;

                case PauseResumeAction _:
                    builder.PlayingState = builder.PlayingState == PlayingStateEnum.Playing ?
                        PlayingStateEnum.Paused : PlayingStateEnum.Playing;
                    break;
                case RefreshAvancementAction pos:
                    builder.Avancement = pos.Avancement;
                    break;
                case UpVolumeAction up:
                    builder.VolumentPercentage += up.Amount;
                    break;
                case DownVolumeAction down:
                    builder.VolumentPercentage -= down.Amount;
                    break;
            }

            if (builder.VolumentPercentage > 100)
                builder.VolumentPercentage = 100;
            if (builder.VolumentPercentage < 0)
                builder.VolumentPercentage = 0;

            // Playing track state changed
            if (builder.PlayingTrackInd != state.PlayingTrackInd ||
                builder.Playlist != state.Playlist ||
                builder.PlayingState != state.PlayingState)
            {

                // New state of new playing track
                var newPlayingTrack = 
                    builder.PlayingState == PlayingStateEnum.Stopped ? 
                        null :
                        GetPlayingTrackAndAlbum(builder.PlayingTrackInd, builder.Playlist);
                if (newPlayingTrack != null)
                    builder.Playlist = UpdateTrack(builder.Playlist, newPlayingTrack,
                        builder.PlayingState switch
                        {
                            PlayingStateEnum.Playing => TrackState.Playing,
                            PlayingStateEnum.Paused => TrackState.Paused,
                            _ => TrackState.Nothing
                        });

                // Update actual Playing track
                builder.PlayingTrack = newPlayingTrack;

                // Old playing track state set to Nothing
                var oldPlayingTrack = GetPlayingTrackAndAlbum(state.PlayingTrackInd, state.Playlist);
                if (oldPlayingTrack != null && !ReferenceEquals(newPlayingTrack, oldPlayingTrack))
                    builder.Playlist = UpdateTrack(builder.Playlist, oldPlayingTrack, TrackState.Nothing);
            }

            return builder.Build();
        }

        private static AlbumModel GetAlbum(TrackModel track, ImmutableArray<AlbumModel> playlist)
        {
            return playlist.FirstOrDefault(a => a.Tracks.Any(t => ReferenceEquals(t, track)));
        }

        private static TrackModel GetPlayingTrackAndAlbum(int index, ImmutableArray<AlbumModel> playlist)
        {
            var tracks = playlist.SelectMany(a => a.Tracks).ToArray();

            // No playing track
            if (index >= tracks.Length) return null;

            return tracks[index];
        }

        private static ImmutableArray<AlbumModel> UpdateTrack(ImmutableArray<AlbumModel> playlist, TrackModel track, TrackState trackState)
        {
            var album = playlist.FirstOrDefault(a => a.Tracks.Any(t => ReferenceEquals(t, track)));

            // Playlist no longer contains track anyway
            if (album is null) return playlist;

            // Update track
            var tracks = album.Tracks.Replace(track, new TrackModel.Builder(track)
            {
                TrackState = trackState
            }.Build());

            // Update playlist
            return playlist.Replace(album, new AlbumModel.Builder(album)
            {
                Tracks = tracks
            }.Build());
        }
    }
}
