using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;

namespace FreshUmbraco.Controllers
{
    public class MailChimpNewsletterController : UmbracoAuthorizedJsonController
    {
        private readonly IDataTypeService _dataTypeService;

        public MailChimpNewsletterController()
        {
            _dataTypeService = ApplicationContext.Current.Services.DataTypeService;
        }

        [System.Web.Http.HttpGet]
        public object Lists()
        {
            var apiKey = GetMailChimpApiKeyValue();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://{0}.api.mailchimp.com/3.0/lists", GetMailChimpDataCenterIdentifier(apiKey)));
            request.ContentType = "application/json; charset=utf-8";
            request.Headers["Authorization"] = "apikey " + apiKey;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject(reader.ReadToEnd());
            }
        }

        private string GetMailChimpDataCenterIdentifier(string apiKey)
        {
            return apiKey.Substring(apiKey.LastIndexOf('-') + 1);
        }

        private string GetMailChimpApiKeyValue()
        {
            var dataType = _dataTypeService.GetDataTypeDefinitionByPropertyEditorAlias("SiteOcean.MailChimp.ListPicker").First();
            var prevalues = _dataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id);
            return prevalues.PreValuesAsDictionary["apiKey"].Value;
        }
    }
}