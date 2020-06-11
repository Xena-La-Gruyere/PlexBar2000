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
                    builder.Playlist = builder.Playlist.Remove(album.Album);
                    break;
                case ClearPlaylistAction _:
                    builder.Playlist = ImmutableArray<AlbumModel>.Empty;
                    break;

                // PLAYER
                case PlayAlbumAction play:
                    builder.Playlist = ImmutableArray<AlbumModel>.Empty.Add(play.Album);
                    builder.PlayingTrack = 0;
                    builder.PlayingState = PlayingStateEnum.Playing;
                    break;
                case PlayNextAction next:
                    var tracksLenght = builder.Playlist.SelectMany(a => a.Tracks).Count();
                    var nextTrackInd = builder.PlayingTrack + 1;
                    if (nextTrackInd == tracksLenght)
                    {
                        // No more playlist
                        builder.PlayingState = PlayingStateEnum.Paused;
                        builder.PlayingTrack = 0;
                        break;
                    }

                    builder.PlayingTrack = nextTrackInd;
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

            return builder.Build();
        }
    }
}
