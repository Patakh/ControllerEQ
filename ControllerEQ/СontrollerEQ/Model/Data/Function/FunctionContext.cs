using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using ControllerEQ.Model;
using ControllerEQ.Model.Data.Context;
namespace Function;

public static class TicketCall
{
    public class Ticket
    {
        public long Id { get; set; }
        public string TicketNumberFull { get; set; }
    }
    public class SelectWindowResult
    {
        public long SOfficeWindowId { get; set; }
        public string WindowName { get; set; }
    }
    public static async Task<Ticket> GetNextTicketAsync(long officeWindowId)
    {
        Ticket ticket = new Ticket();
        try
        {
            using (var connection = new NpgsqlConnection(Settings.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT * FROM public.select_next_ticket(@in_s_office_window_id)", connection))
                {
                    command.Parameters.AddWithValue("in_s_office_window_id", NpgsqlDbType.Bigint, officeWindowId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            ticket.Id = (long)(reader["out_d_ticket_id"] as long?);
                            ticket.TicketNumberFull = reader["out_ticket_number_full"] as string;
                        }
                    }
                }
            }
            return ticket;
        }
        catch (Exception ex)
        {
            return ticket;
        }
    }

    public static ObservableCollection<TransferClientWindowModel> WindowResult(long id)
    {
        ObservableCollection<TransferClientWindowModel> result = new();
        try
        {
            using (var connection = new NpgsqlConnection(Settings.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM public.select_window_for_transferred(@in_d_ticket_id)", connection))
                {
                    command.Parameters.AddWithValue("in_d_ticket_id", NpgsqlDbType.Bigint, id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Selectresult = new TransferClientWindowModel
                            {
                                WindowId = (long)reader["out_s_office_window_id"],
                                WindowName = (string)reader["out_window_name"]
                            };
                            result.Add(Selectresult);
                        }
                    }
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
    }
}
public static class Prerecord
{
    public static List<PreRegistraationDate> GetDateList(long in_s_office_id, DateOnly in_date)
    {
        List<PreRegistraationDate> data = new();
        using (var connection = new NpgsqlConnection(Settings.ConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("SELECT * FROM public.select_prerecord(@in_s_office_id, @in_date)", connection))
            {
                command.Parameters.AddWithValue("in_s_office_id", in_s_office_id);
                command.Parameters.AddWithValue("in_date", in_date);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PreRegistraationDate prerecord = new();
                        prerecord.SDayWeekId = (long)reader["out_s_day_week_id"];
                        prerecord.DayName = (string)reader["out_day_name"];
                        prerecord.Date = (DateTime)reader["out_date"];
                        prerecord.StartTimePrerecord = (TimeSpan)reader["out_start_time_prerecord"];
                        prerecord.StopTimePrerecord = (TimeSpan)reader["out_stop_time_prerecord"];
                        data.Add(prerecord);
                    }
                }
            }
        }

        return data;
    }
}
public class TicketTransferred
{
    public class Ticket
    {
        public long Id { get; set; }
        public string TicketNumberFull { get; set; }
    }

    public static async Task<List<Ticket>> SelectTicketTransferredtAsync(long inSOFFICEWINDOWID)
    {
        List<Ticket> tickets = new();
        using (var connection = new NpgsqlConnection(Settings.ConnectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("SELECT * FROM public.select_ticket_transferred(@in_s_office_window_id)", connection))
            {
                command.Parameters.AddWithValue("in_s_office_window_id", inSOFFICEWINDOWID);
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var ticket = new Ticket
                        {
                            Id = Convert.ToInt64(reader["out_d_ticket_id"]),
                            TicketNumberFull = reader["out_ticket_number_full"].ToString()
                        };

                        tickets.Add(ticket);
                    }
                }
            }
        }
        return tickets;
    }
}

public class TicketPostponed
{
    public class Ticket
    {
        public long Id { get; set; }
        public string TicketNumberFull { get; set; }
    }

    public static async Task<List<Ticket>> SelectTicketPostponedAsync(long inSOFFICEWINDOWID)
    {
        List<Ticket> tickets = new();
        using (var connection = new NpgsqlConnection(Settings.ConnectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("SELECT * FROM public.select_ticket_postponed(@in_s_office_window_id)", connection))
            {
                command.Parameters.AddWithValue("in_s_office_window_id", NpgsqlDbType.Bigint, inSOFFICEWINDOWID);
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        var ticket = new Ticket
                        {
                            Id = Convert.ToInt64(reader["out_d_ticket_id"]),
                            TicketNumberFull = reader["out_ticket_number_full"].ToString()
                        };
                        tickets.Add(ticket);
                    }
                }
            }
        }
        return tickets;
    }
}

