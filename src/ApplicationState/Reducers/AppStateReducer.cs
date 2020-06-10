using ApplicationState.Actions;
using ApplicationState.Enumerations;
using ApplicationState.States;
using Redux;

namespace ApplicationState.Reducers
{
    public static class AppStateReducer
    {
        public static AppState Execute(AppState state, IAction action)
        {
            var builder = new AppState.Builder(state);

            switch (action)
            {
                case ChangeGlobalState change:
                    builder.State = change.State;
                    break;
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
            }

            return builder.Build();
        }
    }
}
