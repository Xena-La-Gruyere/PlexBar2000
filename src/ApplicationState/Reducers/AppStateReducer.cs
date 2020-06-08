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
                    builder.MenuIndex = builder.MenuIndex == 0 ? 0 : builder.MenuIndex - 1;
                    break;
                case SelectArtist selectArtist:
                    builder.MenuIndex = 1;
                    builder.Artist = selectArtist.Artist;
                    break;
                case SelectAlbum selectAlbum:
                    builder.MenuIndex = 2;
                    builder.Album = selectAlbum.Album;
                    break;
            }

            return builder.Build();
        }
    }
}
