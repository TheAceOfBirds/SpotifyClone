using SpotifyAPI.Web;
using SpotifyClone.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyClone.Services
{
    public class QueuingTesterService
    {
        public int PlaylistLength { get; set; }
        ExcelHelper ExcelHelper { get; set; }
        SpotifyHelper SpotifyHelper { get; set; }
        List<FullTrack> Tracks { get; set; }
        public QueuingTesterService(string clientId,  string clientSecret,string playlistId, string excelPath)
        {
            ExcelHelper = new ExcelHelper(excelPath);
            SpotifyHelper = new SpotifyHelper(clientId, playlistId, clientSecret);

            ExcelHelper.ResetExcel(SpotifyHelper.GetPlayList());
        
        }
        public void ResetQueueAndLog()
        {
            ExcelHelper.SetExcelCount(SpotifyHelper.GetQueue());
        }
        public bool ShuffleQueue()
        {
            return SpotifyHelper.ShuffleQueue(); 
        }
    }
}
