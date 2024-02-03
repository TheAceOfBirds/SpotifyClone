using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using SpotifyAPI.Web;

namespace SpotifyClone.Helpers
{
    internal class ExcelHelper
    {
        Application ExcelApplication;
        Workbook ExcelWorkbook;
        private int ResetRow;
        Dictionary<int, ExcelTrack> RowTrackDictionary;
        private string ExcelPath { get; set; }
        public ExcelHelper(string excelPath) 
        {
            ExcelPath= excelPath;
            RowTrackDictionary = new Dictionary<int, ExcelTrack>();
            ResetRow = 0;
        }

        public void ResetExcel(List<FullTrack> playlistTracks)
        {
            try
            {
                ExcelApplication = new Application();
                ExcelApplication.DisplayAlerts = false;
                ExcelWorkbook = (Workbook)(ExcelApplication.Workbooks._Open(ExcelPath)); // open the existing excel file

                Worksheet worksheet = ExcelWorkbook.Worksheets["Sheet1"];
                for (int i = 0; i < playlistTracks.Count; i++)
                {
                    FullTrack track = playlistTracks[i];
                    ExcelTrack excelTrack = new ExcelTrack { Row = i + 2, Name = track.Name, Album = track.Album.Name, Artist = track.Artists.First().Name };
                    RowTrackDictionary.Add(i, excelTrack);
                    SetRow(excelTrack, worksheet, i + 2);
                }
                ResetRow = playlistTracks.Count + 2;
                ExcelTrack resetTrack = new ExcelTrack { Row = ResetRow, Name = "Reset", Album = "Reset", Artist = "Reset" };
                RowTrackDictionary.Add(ResetRow, resetTrack);
                SetRow(resetTrack, worksheet, ResetRow);
                ExcelWorkbook.SaveAs(ExcelPath); // Save data in excel
                ExcelWorkbook.Close(true, ExcelPath); // close the worksheet
            }
            catch (Exception e)
            {
                Console.WriteLine($"Aborting task \'ResetExcel\', error received: {e.Message}");
            }
            finally
            {
                ExcelApplication.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApplication);
                GC.Collect();
            }
        }
        public bool SetExcelCount(List<FullTrack> Tracks)
        {
            try
            {
                ExcelApplication = new Application();
                ExcelApplication.DisplayAlerts = false; // turn off alerts
                ExcelWorkbook = (Workbook)(ExcelApplication.Workbooks._Open(ExcelPath)); // open the existing excel file
                Worksheet worksheet = ExcelWorkbook.Worksheets["Sheet1"];

                foreach (FullTrack Track in Tracks)
                {
                    //Using all criteria because there can be some songs with same name, or same artist name, or album.
                    int row = RowTrackDictionary.First(x => x.Value.Name == Track.Name && x.Value.Album == Track.Album.Name && x.Value.Artist == Track.Artists.First().Name).Key;
                    worksheet.Cells[row, 4] = Convert.ToInt32((worksheet.Cells[row, 4] as Microsoft.Office.Interop.Excel.Range).Value) + 1;
                }
                worksheet.Cells[ResetRow, 4] = Convert.ToInt32((worksheet.Cells[ResetRow, 4] as Microsoft.Office.Interop.Excel.Range).Value) + 1;

                ExcelWorkbook.SaveAs(ExcelPath); // close the worksheet
                ExcelWorkbook.Close(true, ExcelPath); // close the worksheet
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Aborting task  'SetExcelCount', error received: {e.Message}");
                return false;
            }
            finally
            {
                ExcelApplication.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelApplication);
                GC.Collect();
            }
        }
        private void SetRow(ExcelTrack track, Worksheet sheet, int rowNumber)
        {
            sheet.Cells[rowNumber, 1] = track.Name;
            sheet.Cells[rowNumber, 2] = track.Artist;
            sheet.Cells[rowNumber, 3] = track.Album;
            sheet.Cells[rowNumber, 4] = "0";
        }
    }
}
