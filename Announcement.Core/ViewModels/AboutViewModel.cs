using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Org.Json;
using System.Threading.Tasks;
using Announcement.Android;
using System.Linq;

namespace Announcement.Core
{
	public class AboutViewModel : BaseViewModel
	{
		public AboutPerson CurrentPerson { get; private set; }

		public static AboutViewModel Instance
		{
			get
			{
				return instance ?? (instance = new AboutViewModel());
			}
		}

		public AboutViewModel ()
		{
		}

		public void GetAboutPersons(Action callback)
		{			
			ProgressModule.Message(LocalizationModule.Translate("progress_trying_to_get_location"), false);

			var cityTask = LocationService.GetCityAsync();

			Task.Run<bool> (() => {				
				return cityTask.Wait (5000);
			}).ContinueWith ((resultCity) => {
				if (resultCity.Result)
				{
					var city = cityTask.Result;

					var results = OpenAboutFile();

					ProgressModule.End();

					if (results.Count == 0 || string.IsNullOrWhiteSpace(city))
					{
						AlertModule.ShowError(LocalizationModule.Translate("alert_message_empty_about_persons"), () => GetAboutPersons(callback));
					}
					else
					{
						var person = results.FirstOrDefault(f=>f.City.Equals(city));
						if(person == null)
							CurrentPerson = results.FirstOrDefault();
						else 
							CurrentPerson = person;

						if (callback != null)
						{
							DispatcherModule.Invoke(callback);
						}
					}
				}
				else 
				{
					ProgressModule.End();
					AlertModule.ShowError(LocalizationModule.Translate("alert_bad_location"), () => GetAboutPersons(callback));
				}
			});		

		}


		private List<AboutPerson> OpenAboutFile()
		{
			var linkUrl = "https://spreadsheets.google.com/tq?key=1TGlX3YMfaTViJwJCweua2ISOwvU4XklJV3u9d4B6kGk";

			var list = new List<AboutPerson> ();

			try {
				var request = HttpWebRequest.Create (linkUrl);
				request.ContentType = "application/json";
				request.Method = "GET";

				using (HttpWebResponse response = request.GetResponse () as HttpWebResponse) {

					using (StreamReader reader = new StreamReader (response.GetResponseStream ())) {
						var content = reader.ReadToEnd ();
						int start = content.IndexOf ("{", content.IndexOf ("{") + 1);
						int end = content.LastIndexOf ("}") - start;
						string jsonResponse = content.Substring (start, end);
						try {
							
							JSONObject table = new JSONObject (jsonResponse);
							JSONArray rows = table.GetJSONArray ("rows");
							int position = 0;
							for (int r = 1; r < rows.Length (); ++r) {
								JSONObject row = rows.GetJSONObject (r);
								JSONArray columns = row.GetJSONArray ("c");

								string city = columns.GetJSONObject (0).GetString ("v");
								string pib = columns.GetJSONObject (1).GetString ("v");
								string link = columns.GetJSONObject (2).GetString ("v");

								AboutPerson person = new AboutPerson () {
									Position = position,
									City = city,
									Pib = pib,
									Link = link
								};

								list.Add (person);
								position++;
							}
						} catch (JSONException e) {

						}

					}
				}
			} catch (System.Exception ex) {

			}

			return list;

		}

		private static AboutViewModel instance;

	}
}

