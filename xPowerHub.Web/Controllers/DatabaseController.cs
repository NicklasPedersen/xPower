using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;

namespace xPowerHub.Web.Controllers
{
    /// <summary>
    /// Controlls the database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private IDataStore _dataStore;

        public DatabaseController(IDataStore dataStore) : base()
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Goes and starts listening for first beat
        /// </summary>
        /// <returns>The new Device</returns>
        [HttpGet("reset")]
        public void Reset()
        {
            if (_dataStore is DAL dal)
            {
                dal.DropTables();
                dal.AddTables();
            }
        }
    }
}
