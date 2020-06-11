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
