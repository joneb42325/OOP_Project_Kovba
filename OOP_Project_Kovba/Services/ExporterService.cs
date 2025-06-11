using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Office.Interop.Word;
using OOP_Project_Kovba.Data.Repositories;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Models;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace OOP_Project_Kovba.Services
{
    public class ExporterService : IExporterService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingRepository _bookingRepository;
        public ExporterService(ITripRepository tripRepository, IBookingRepository bookingRepository)
        {
            _tripRepository = tripRepository;
            _bookingRepository = bookingRepository;
        }
        public void ExportPlannedTripsToWord(string userId, IEnumerable<Trip> driverTrips, IEnumerable<Booking> passengerBookings)
        {
            Word.Application wordApp = null!;
            Word.Document document = null!;

            try
            {
                wordApp = new Word.Application();
                document = wordApp.Documents.Add();
                wordApp.Visible = false;

                var titleParagraph = document.Content.Paragraphs.Add();
                titleParagraph.Range.Text = "Planned Trips Report";
                titleParagraph.Range.Font.Bold = 1;
                titleParagraph.Range.Font.Size = 16;
                titleParagraph.Range.InsertParagraphAfter();

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                var driverTextTitle = document.Content.Paragraphs.Add();
                driverTextTitle.Range.Text = "Driver's Trips (Text info):";
                driverTextTitle.Range.Font.Bold = 0;
                driverTextTitle.Range.Font.Size = 12;
                driverTextTitle.Range.InsertParagraphAfter();

                if (driverTrips.Any())
                {
                    foreach (var trip in driverTrips)
                    {
                        var tripPara = document.Content.Paragraphs.Add();
                        tripPara.Range.Text = trip.GetInfo();
                        tripPara.Range.Font.Bold = 0;
                        tripPara.Range.InsertParagraphAfter();

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            Marshal.ReleaseComObject(tripPara);
                        }

                    }
                }
                else
                {
                    var noTripsPara = document.Content.Paragraphs.Add();
                    noTripsPara.Range.Text = "No planned trips.";
                    noTripsPara.Range.InsertParagraphAfter();

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(noTripsPara);
                }

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                var driverTableTitle = document.Content.Paragraphs.Add();
                driverTableTitle.Range.Text = "Driver's Trips (Table):";
                driverTableTitle.Range.Font.Bold = 0;
                driverTableTitle.Range.Font.Size = 12;
                driverTableTitle.Range.InsertParagraphAfter();

                if (driverTrips.Any())
                {
                    int driverRowCount = driverTrips.Count() + 1;
                    int driverColCount = 5;

                    var driverTable = document.Tables.Add(driverTableTitle.Range, driverRowCount, driverColCount);
                    driverTable.Borders.Enable = 1;

                    driverTable.Cell(1, 1).Range.Text = "Trip ID";
                    driverTable.Cell(1, 2).Range.Text = "Departure";
                    driverTable.Cell(1, 3).Range.Text = "Arrival";
                    driverTable.Cell(1, 4).Range.Text = "Date";
                    driverTable.Cell(1, 5).Range.Text = "Seats Available";

                    driverTable.Rows[1].Range.Font.Bold = 0;

                    int driverRow = 2;
                    foreach (var trip in driverTrips)
                    {
                        driverTable.Cell(driverRow, 1).Range.Text = trip.Id.ToString();
                        driverTable.Cell(driverRow, 2).Range.Text = trip.FromCity ?? "";
                        driverTable.Cell(driverRow, 3).Range.Text = trip.ToCity ?? "";
                        driverTable.Cell(driverRow, 4).Range.Text = trip.DepartureTime.ToString("yyyy-MM-dd HH:mm");
                        driverTable.Cell(driverRow, 5).Range.Text = trip.MaxPassengers.ToString();
                        driverRow++;
                    }
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(driverTable);
                }
                else
                {
                    var noTripsPara = document.Content.Paragraphs.Add();
                    noTripsPara.Range.Text = "No planned trips.";
                    noTripsPara.Range.InsertParagraphAfter();

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(noTripsPara);
                }

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                var bookingTextTitle = document.Content.Paragraphs.Add();
                bookingTextTitle.Range.Text = "Passenger's Bookings (Text info):";
                bookingTextTitle.Range.Font.Bold = 0;
                bookingTextTitle.Range.Font.Size = 12;
                bookingTextTitle.Range.InsertParagraphAfter();

                if (passengerBookings.Any())
                {
                    foreach (var booking in passengerBookings)
                    {
                        var bookingPara = document.Content.Paragraphs.Add();
                        bookingPara.Range.Text = booking.GetInfo();
                        bookingPara.Range.Font.Bold = 0;
                        bookingPara.Range.InsertParagraphAfter();

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Marshal.ReleaseComObject(bookingPara);
                    }
                }
                else
                {
                    var noBookingsPara = document.Content.Paragraphs.Add();
                    noBookingsPara.Range.Text = "No bookings.";
                    noBookingsPara.Range.InsertParagraphAfter();

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(noBookingsPara);
                }

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                var bookingTableTitle = document.Content.Paragraphs.Add();
                bookingTableTitle.Range.Text = "Passenger's Bookings (Table):";
                bookingTableTitle.Range.Font.Bold = 0;
                bookingTableTitle.Range.Font.Size = 12;
                bookingTableTitle.Range.InsertParagraphAfter();

                if (passengerBookings.Any())
                {
                    int bookingRowCount = passengerBookings.Count() + 1;
                    int bookingColCount = 6;

                    var bookingTable = document.Tables.Add(bookingTableTitle.Range, bookingRowCount, bookingColCount);
                    bookingTable.Borders.Enable = 1;

                    bookingTable.Cell(1, 1).Range.Text = "Booking ID";
                    bookingTable.Cell(1, 2).Range.Text = "Trip ID";
                    bookingTable.Cell(1, 3).Range.Text = "Departure";
                    bookingTable.Cell(1, 4).Range.Text = "Arrival";
                    bookingTable.Cell(1, 5).Range.Text = "Date";
                    bookingTable.Cell(1, 6).Range.Text = "Status";

                    bookingTable.Rows[1].Range.Font.Bold = 0;

                    int bookingRow = 2;
                    foreach (var booking in passengerBookings)
                    {
                        bookingTable.Cell(bookingRow, 1).Range.Text = booking.Id.ToString();
                        bookingTable.Cell(bookingRow, 2).Range.Text = booking.Trip?.Id.ToString() ?? "";
                        bookingTable.Cell(bookingRow, 3).Range.Text = booking.Trip?.FromCity ?? "";
                        bookingTable.Cell(bookingRow, 4).Range.Text = booking.Trip?.ToCity ?? "";
                        bookingTable.Cell(bookingRow, 5).Range.Text = booking.Trip?.DepartureTime.ToString("yyyy-MM-dd HH:mm") ?? "";
                        bookingTable.Cell(bookingRow, 6).Range.Text = booking.IsCancelled ? "Cancelled" : "Confirmed";
                        bookingRow++;
                    }

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(bookingTable);
                }
                else
                {
                    var noBookingsPara = document.Content.Paragraphs.Add();
                    noBookingsPara.Range.Text = "No bookings.";
                    noBookingsPara.Range.InsertParagraphAfter();

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(noBookingsPara);
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Marshal.ReleaseComObject(titleParagraph);
                    Marshal.ReleaseComObject(driverTextTitle);
                    Marshal.ReleaseComObject(driverTableTitle);
                    Marshal.ReleaseComObject(bookingTextTitle);
                    Marshal.ReleaseComObject(bookingTableTitle);
                }

                var fileName = $"PlannedTrips_{DateTime.Now:yyyy_MM_dd_HH_mm}.docx";
                var filePath = Path.Combine(@"C:\studies 2 курс\Course Project\OOP_Project_Kovba\OOP_Project_Kovba\wwwroot\exports", fileName);

                document.SaveAs2(filePath);
                document.Close();

                Console.WriteLine($"Файл успішно збережено по шляху: {filePath}");
            }
            catch (Exception ex)
            {
                if (document != null)
                {
                    document.Close(false);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(document);
                }
                Console.WriteLine($"Помилка при експорті: {ex.Message}");
            }
            finally
            {
                if (wordApp != null)
                {
                    wordApp.Quit();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(wordApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void ExportPlannedTripsToExcel(string userId, IEnumerable<Trip> driverTrips, IEnumerable<Booking> passengerBookings)
        {
            Excel.Application excelApp = null!;
            Excel.Workbook workbook = null!;
            Excel.Worksheet worksheet = null!;
            Excel.Range range = null!;

            try
            {
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
                excelApp.Visible = false;

                worksheet.Cells[1, 1] = "Planned Trips Report";
                range = worksheet.Range["A1:E1"];
                range.Merge();
                range.Font.Bold = true;
                range.Font.Size = 16;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Marshal.ReleaseComObject(range);

                worksheet.Cells[3, 1] = "Driver's Trips:";
                range = worksheet.Range["A3:E3"];
                range.Merge();
                range.Font.Bold = true;
                range.Font.Size = 12;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Marshal.ReleaseComObject(range);

                if (driverTrips.Any())
                {
                    int driverRow = 5;

                    worksheet.Cells[driverRow, 1] = "Trip ID";
                    worksheet.Cells[driverRow, 2] = "Departure";
                    worksheet.Cells[driverRow, 3] = "Arrival";
                    worksheet.Cells[driverRow, 4] = "Date";
                    worksheet.Cells[driverRow, 5] = "Seats Available";

                    range = worksheet.Rows[driverRow];
                    range.Font.Bold = true;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(range);
                    driverRow++;

                    foreach (var trip in driverTrips)
                    {
                        worksheet.Cells[driverRow, 1] = trip.Id.ToString();
                        worksheet.Cells[driverRow, 2] = trip.FromCity ?? "";
                        worksheet.Cells[driverRow, 3] = trip.ToCity ?? "";
                        worksheet.Cells[driverRow, 4] = trip.DepartureTime.ToString("yyyy-MM-dd HH:mm");
                        worksheet.Cells[driverRow, 5] = trip.MaxPassengers.ToString();
                        driverRow++;
                    }
                }
                else
                {
                    worksheet.Cells[5, 1] = "No planned trips.";
                }

                int bookingsStartRow = driverTrips.Any() ? driverTrips.Count() + 8 : 8;

                worksheet.Cells[bookingsStartRow, 1] = "Passenger's Bookings:";
                range = worksheet.Range[$"A{bookingsStartRow}:F{bookingsStartRow}"];
                range.Merge();
                range.Font.Bold = true;
                range.Font.Size = 12;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Marshal.ReleaseComObject(range);

                if (passengerBookings.Any())
                {
                    int bookingRow = bookingsStartRow + 2;

                    worksheet.Cells[bookingRow, 1] = "Booking ID";
                    worksheet.Cells[bookingRow, 2] = "Trip ID";
                    worksheet.Cells[bookingRow, 3] = "Departure";
                    worksheet.Cells[bookingRow, 4] = "Arrival";
                    worksheet.Cells[bookingRow, 5] = "Date";
                    worksheet.Cells[bookingRow, 6] = "Status";

                    range = worksheet.Rows[bookingRow];
                    range.Font.Bold = true;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(range);
                    bookingRow++;

                    foreach (var booking in passengerBookings)
                    {
                        worksheet.Cells[bookingRow, 1] = booking.Id.ToString();
                        worksheet.Cells[bookingRow, 2] = booking.Trip?.Id.ToString() ?? "";
                        worksheet.Cells[bookingRow, 3] = booking.Trip?.FromCity ?? "";
                        worksheet.Cells[bookingRow, 4] = booking.Trip?.ToCity ?? "";
                        worksheet.Cells[bookingRow, 5] = booking.Trip?.DepartureTime.ToString("yyyy-MM-dd HH:mm") ?? "";
                        worksheet.Cells[bookingRow, 6] = booking.IsCancelled ? "Cancelled" : "Confirmed";
                        bookingRow++;
                    }
                }
                else
                {
                    worksheet.Cells[bookingsStartRow + 2, 1] = "No bookings.";
                }

                worksheet.Columns.AutoFit();

                var fileName = $"PlannedTrips_{DateTime.Now:yyyy_MM_dd_HH_mm}.xlsx";
                var filePath = Path.Combine(@"C:\studies 2 курс\Course Project\OOP_Project_Kovba\OOP_Project_Kovba\wwwroot\exports", fileName);

                workbook.SaveAs(filePath);
                Console.WriteLine($"Файл успішно збережено по шляху: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при експорті: {ex.Message}");
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(false);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(workbook);
                }
                if (worksheet != null)
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(worksheet);
                if (excelApp != null)
                {
                    excelApp.Quit();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Marshal.ReleaseComObject(excelApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}

