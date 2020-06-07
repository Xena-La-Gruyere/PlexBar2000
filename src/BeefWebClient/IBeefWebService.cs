using System.Threading.Tasks;
using BeefWebClient.Models;
using BeefWebClient.Parameters;

namespace BeefWebClient
{
    public interface IBeefWebService
    {
        Task<PlayerStateResult> GetPlayerState(params TrackInfoFields[] fields);

        /// <summary>
        /// Set player state
        /// </summary>
        /// <param name="parameters">parameters</param>
        Task SetPlayerState(SetPlayerStateParameters parameters);
        Task Play();
        Task PlayRandom();
        Task Next();
        Task Previous();
        Task Stop();
        Task Pause();
        Task TogglePause();
        Task Play(string playlistId, uint index);


        Task<PlaylistInfoResult> GetPlaylists();
        Task SetCurrentPlaylist(string id);
        Task AddPlaylist(string title, uint? index = null);
        Task RemovePlaylist(string id);
        Task MovePlaylist(string id, uint index);
        Task<PlaylistItemsResult> GetPlaylistItems(string id, int offset, int count, params TrackInfoFields[] fields);
        Task RenamePlaylist(string id, string title);
        Task ClearPlaylist(string id);
        Task AddItem(string id, bool async, int? position, params string[] paths);
    }
}
