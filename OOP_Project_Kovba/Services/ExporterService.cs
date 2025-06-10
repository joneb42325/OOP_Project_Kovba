using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Office.Interop.Word;
using OOP_Project_Kovba.Data.Repositories;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Models;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace OOP_Project_Kovba
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
            Word.Application wordApp = null;
            Word.Document document = null;

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

                        Marshal.ReleaseComObject(tripPara);
                    }
                }
                else
                {
                    var noTripsPara = document.Content.Paragraphs.Add();
                    noTripsPara.Range.Text = "No planned trips.";
                    noTripsPara.Range.InsertParagraphAfter();

                    Marshal.ReleaseComObject(noTripsPara);
                }

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                // --- Таблица поездок водителя ---
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

                    // Заголовки столбцов
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

                    Marshal.ReleaseComObject(driverTable);
                }
                else
                {
                    var noTripsPara = document.Content.Paragraphs.Add();
                    noTripsPara.Range.Text = "No planned trips.";
                    noTripsPara.Range.InsertParagraphAfter();

                    Marshal.ReleaseComObject(noTripsPara);
                }

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                // --- Текстовое описание бронирований пассажира ---
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

                        Marshal.ReleaseComObject(bookingPara);
                    }
                }
                else
                {
                    var noBookingsPara = document.Content.Paragraphs.Add();
                    noBookingsPara.Range.Text = "No bookings.";
                    noBookingsPara.Range.InsertParagraphAfter();

                    Marshal.ReleaseComObject(noBookingsPara);
                }

                document.Content.Paragraphs.Add().Range.InsertParagraphAfter();

                // --- Таблица бронирований пассажира ---
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

                    Marshal.ReleaseComObject(bookingTable);
                }
                else
                {
                    var noBookingsPara = document.Content.Paragraphs.Add();
                    noBookingsPara.Range.Text = "No bookings.";
                    noBookingsPara.Range.InsertParagraphAfter();

                    Marshal.ReleaseComObject(noBookingsPara);
                }

                // Очистка
                Marshal.ReleaseComObject(titleParagraph);
                Marshal.ReleaseComObject(driverTextTitle);
                Marshal.ReleaseComObject(driverTableTitle);
                Marshal.ReleaseComObject(bookingTextTitle);
                Marshal.ReleaseComObject(bookingTableTitle);

                // Сохранение и закрытие документа
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
                    Marshal.ReleaseComObject(document);
                }
                Console.WriteLine($"Помилка при експорті: {ex.Message}");
            }
            finally
            {
                if (wordApp != null)
                {
                    wordApp.Quit();
                    Marshal.ReleaseComObject(wordApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
