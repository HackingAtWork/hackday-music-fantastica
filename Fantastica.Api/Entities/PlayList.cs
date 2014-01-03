using System.Collections.Generic;

namespace Fantastica.Api.Entities {
  public class PlayList {
    public IEnumerable<string> SongIds { get; set; }
    public string Name { get; set; }
    public string CurrentSongId { get; set; }

    public PlayList() {
      SongIds = new List<string>();
    }
  }
}