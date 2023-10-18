using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using СontrollerEQ.Context;
using static Function.TicketCall;

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

    public static Ticket GetNextTicket(long officeWindowId)
    {
        Ticket ticket = new Ticket();
        try
        { 
            // Создаем подключение к базе данных
            using (NpgsqlConnection connection = new NpgsqlConnection("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ"))
            {
                connection.Open();
                // Создаем команду для вызова функции и получения данных
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.select_next_ticket(@in_s_office_window_id)", connection))
                { 
                    command.Parameters.AddWithValue("in_s_office_window_id", NpgsqlDbType.Bigint, officeWindowId); 
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

    public static List<SelectWindowResult> WindowResult(long id)
    {
        List<SelectWindowResult> result = new List<SelectWindowResult>();
        try
        {
            // Создаем подключение к базе данных
            using (NpgsqlConnection connection = new NpgsqlConnection("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ"))
            {
                connection.Open();
                // Создаем команду для вызова функции и получения данных
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.select_window_for_transferred(@in_d_ticket_id)", connection))
                {
                    command.Parameters.AddWithValue("in_d_ticket_id", NpgsqlDbType.Bigint, id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectWindowResult Selectresult = new SelectWindowResult
                            {
                                SOfficeWindowId = (long)reader["out_s_office_window_id"],
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