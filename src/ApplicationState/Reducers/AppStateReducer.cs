using System.Collections.Immutable;
using ApplicationState.Actions;
using ApplicationState.Enumerations;
using ApplicationState.States;
using PlexClient.Library.Models;
using Redux;
using System.Linq;

namespace ApplicationState.Reducers
{
    public static class AppStateReducer
    {
        public static AppState Execute(AppState state, IAction action)
        {
            var builder = new AppState.Builder(state);

            switch (action)
            {
                case ToggleGlobalState _:
                    builder.State = builder.State == AppStateEnum.Explorer ? AppStateEnum.Player : AppStateEnum.Explorer;
                    break;
                case PreviousMenu _:
                    builder.MenuIndex = builder.MenuIndex switch
                    {
                        MenuStateEnum.Playlist when builder.Album != null => MenuStateEnum.Album,
                        MenuStateEnum.Playlist when builder.Artist != null => MenuStateEnum.Artist,
                        MenuStateEnum.Album when builder.Artist != null => MenuStateEnum.Artist,
                        _ => MenuStateEnum.Home
                    };
                    break;
                case SelectArtist selectArtist:
                    builder.MenuIndex = MenuStateEnum.Artist;
                    builder.Artist = selectArtist.Artist;
                    break;
                case SelectAlbum selectAlbum:
                    builder.MenuIndex = MenuStateEnum.Album;
                    builder.Album = selectAlbum.Album;
                    break;
                case HomeMenu _:
                    builder.MenuIndex = MenuStateEnum.Home;
                    break;
                case PlaylistMenu _:
                    builder.MenuIndex = MenuStateEnum.Playlist;
                    break;

                // LOADING
                case AlbumLoaded loadAlbum:
                    builder.Artists = builder.Artists.Replace(loadAlbum.Artist, new ArtistModel.Builder(loadAlbum.Artist)
                    {
                        Albums = loadAlbum.Artist.Albums.Replace(loadAlbum.Album, loadAlbum.AlbumNew)
                    }.Build());
                    break;
                case ArtistLoaded artistLoaded:
                    builder.Artists = builder.Artists.Replace(artistLoaded.ArtistOld, artistLoaded.ArtistNew);
                    break;
                case ArtistsLoaded artists:
                    builder.Artists = artists.Artist;
                    break;

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
                    break;
            }

            if (builder.Artists != state.Artists)
            {
                // Update letters if artist changed
                builder.SearchLetters = builder.Artists
                    .Select(a => a.LetterSearch)
                    .Where(c => !char.IsSymbol(c) || c == '#')
                    .Where(c => !char.IsPunctuation(c))
                    .OrderBy(c => c)
                    .Distinct()
                    .ToImmutableArray();
            }

            return builder.Build();
        }
    }
}
