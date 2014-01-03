using System.Collections.Generic;
using System.Linq;

namespace Fantastica.Api.Entities {
  public class PlayList {
    public IEnumerable<string> SongIds { get; set; }
    public string Name { get; set; }
    public int CurrentSongPosition { get; set; }

    public string CurrentSongId {
      get { return SongIds.ElementAt(CurrentSongPosition); }
    }

    public PlayList() {
      SongIds = new List<string>();
    }

    public void MoveToNextSong() {
      CurrentSongPosition = (CurrentSongPosition+1) % SongIds.Count();
    }
  }
}