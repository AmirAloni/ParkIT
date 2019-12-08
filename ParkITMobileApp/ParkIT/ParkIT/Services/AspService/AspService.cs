using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace ParkIT.Services.AspService
{
    public class AspService
    {
        HttpClient m_HttpClient = null;
        string m_Uri = string.Empty;
        string m_Answer = string.Empty;

        public AspService()
        {
            m_HttpClient = new HttpClient();
            m_Uri = "https://parkitaspweb20191003091347.azurewebsites.net/api/values?str=";
        }

        public async void SendToServer(string i_SignStr, string i_Disable, string i_parkingNote)
        {
            string strToSend = m_Uri + i_SignStr + i_Disable + i_parkingNote + "," + DateTime.Now.ToString();
            var response = m_HttpClient.GetAsync(strToSend).Result;

            if (response.IsSuccessStatusCode)
            {
                m_Answer = await response.Content.ReadAsStringAsync();
             }
        }

        public string GetContent()
        {
            char[] charsToTrim = { '"', '\\', '[', ']' };
            if(!m_Answer.Equals(string.Empty))
                m_Answer = m_Answer.Trim(charsToTrim);
            return m_Answer;
        }
    }

}
