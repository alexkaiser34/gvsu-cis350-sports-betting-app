public class HistoryController : Controller 
{

  private readonly IAPIClientService<Wager> _api;

  public HistoryController(IAPIClientService<Wager> api)
  {
    _api = api; 
  }

  public async Task<IActionResult> Index()
  {
    string endpoint = "Wager/user";
    
    // Pass in user ID or other params
    
    IEnumerable<Wager> results = await _api.GetAll(endpoint, params);
    
    return View(results);
  }

}
