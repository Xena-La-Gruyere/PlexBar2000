using System.Linq;
using ApplicationState.Actions;
using ApplicationState.States;
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

                case PlayAlbumAction play:
                    builder.PlayingTrack = play.Album.Tracks.First();
                    builder.PlayingState = PlayingStateEnum.Playing;
                    builder.Avancement = 0;
                    break;
                case PauseResumeAction pauseResume:
                    builder.PlayingState = builder.PlayingState == PlayingStateEnum.Playing ?
                        PlayingStateEnum.Paused : PlayingStateEnum.Playing;
                    break;
            }

            return builder.Build();
        }
    }
}
