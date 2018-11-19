namespace NetCoreJwtAuth.Entities {
  public class AppUser {
    public int ID { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
  }
}
