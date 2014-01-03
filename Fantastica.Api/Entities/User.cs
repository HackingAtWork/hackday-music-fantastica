using System.Collections.Generic;

namespace Fantastica.Api.Entities {
  public class User {
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<PlayList> Lists { get; set; }

    public User() {
      Lists = new List<PlayList>();
    }
  }
}