using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Addy.Address.Service
{
    /// <summary>
    /// Calls Addy's Address Services
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly Lazy<IRequestClient> _client;
        private const int MinSearchCharacters = 3;
        private const int MinSearchRecords = 1;
        private const int MaxSearchRecords = 50;

        public AddressService(IRequestFactory requestFactory)
        {
            if (requestFactory == null) throw new ArgumentNullException("requestFactory");
            _client = new Lazy<IRequestClient>(requestFactory.CreateClient);
        }

        /// <summary>
        /// Search for addresses
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Address search result</returns>
        public async Task<AddressSearchResult> AddressSearchAsync(string criteria)
        {
            return await AddressSearchAsync(criteria, null, null, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for addresses
        /// </summary>
        /// <param name="criteria">Search criteria. Minimum of 3 characters.</param>
        /// <param name="excludePostBox">Exclude PO Box addresses</param>
        /// <param name="excludeRural">Exclude Rural addresses</param>
        /// <param name="excludeUndeliverable">Exclude Undeliverable mail addresses</param>
        /// <param name="maxRecords">Maximum number of records to return (Range of 1 to 50)</param>
        /// <returns>Address search result</returns>
        public async Task<AddressSearchResult> AddressSearchAsync(string criteria, bool? excludePostBox, bool? excludeRural, bool? excludeUndeliverable, int? maxRecords)
        {
            if (string.IsNullOrWhiteSpace(criteria) || criteria.Length < MinSearchCharacters) throw new ArgumentException("The search criteria must have at least 3 characters");
            if (maxRecords.HasValue && (maxRecords.Value < MinSearchRecords || maxRecords.Value < MaxSearchRecords)) throw new ArgumentException("The maxRecords value is invalid. A range of 1 to 50 is required");

            var query = new StringBuilder();
            query.Append(string.Format("search?s={0}", WebUtility.UrlEncode(criteria)));

            if (excludePostBox.HasValue)
            {
                query.Append(string.Format("&expostbox={0}", excludePostBox.Value));
            }
            if (excludeRural.HasValue)
            {
                query.Append(string.Format("&exrural={0}", excludeRural.Value));
            }
            if (excludeUndeliverable.HasValue)
            {
                query.Append(string.Format("&exundeliver={0}", excludeUndeliverable.Value));
            }
            if (maxRecords.HasValue)
            {
                query.Append(string.Format("&max={0}", maxRecords.Value));
            }

            return await _client.Value.GetAsync<AddressSearchResult>(query.ToString());
        }

        /// <summary>
        /// Retrieve full address details using the unique Id.
        /// </summary>
        /// <param name="id">Addy's unique Id</param>
        /// <returns>Address details</returns>
        public async Task<AddressDetail> GetAddressDetailByIdAsync(int id)
        {
            return await _client.Value.GetAsync<AddressDetail>(string.Format("address/{0}", id));
        }

        /// <summary>
        /// Retrieve full address details using the NZ Post DPID
        /// </summary>
        /// <param name="dpid">DPID</param>
        /// <returns>Address details</returns>
        public async Task<AddressDetail> GetAddressDetailByDpIdAsync(int dpid)
        {
            return await _client.Value.GetAsync<AddressDetail>(string.Format("address?type=dpid&id={0}", dpid));
        }

        /// <summary>
        /// Retrieve full address details using the LINZ Street ID
        /// </summary>
        /// <param name="linzid">LINZ Street ID</param>
        /// <returns>Address details</returns>
        public async Task<AddressDetail> GetAddressDetailByLinzIdAsync(int linzid)
        {
            return await _client.Value.GetAsync<AddressDetail>(string.Format("address?type=linz&id={0}", linzid));
        }

        public void Dispose()
        {
            if (_client != null && _client.Value != null)
            {
                _client.Value.Dispose();
            }
        }
    }
}
