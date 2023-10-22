using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
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
public static class Prerecord
{
    public class PrerecordData
    {
        public long SDayWeekId { get; set; }
        public string DayName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTimePrerecord { get; set; }
        public TimeSpan StopTimePrerecord { get; set; }
    }

    public static List<PrerecordData> GetPrerecordData(long in_s_office_id, DateOnly in_date)
    {
        List<PrerecordData> data = new List<PrerecordData>();

        // Создаем подключение к базе данных
        using (NpgsqlConnection connection = new NpgsqlConnection("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ"))
        {
            connection.Open();

            // Создаем команду для вызова функции и получения данных
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.select_prerecord(@in_s_office_id, @in_date)", connection))
            {
                // Добавляем параметры в команду
                command.Parameters.AddWithValue("in_s_office_id", in_s_office_id);
                command.Parameters.AddWithValue("in_date", in_date);

                // Выполняем команду и получаем результаты
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Создаем объект PrerecordData и заполняем его данными из результата
                        PrerecordData prerecord = new PrerecordData();
                        prerecord.SDayWeekId = (long)reader["out_s_day_week_id"];
                        prerecord.DayName = (string)reader["out_day_name"];
                        prerecord.Date = (DateTime)reader["out_date"];
                        prerecord.StartTimePrerecord = (TimeSpan)reader["out_start_time_prerecord"];
                        prerecord.StopTimePrerecord = (TimeSpan)reader["out_stop_time_prerecord"];

                        // Добавляем объект PrerecordData в список данных
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

    public static IEnumerable<Ticket> SelectTicketTransferred(long inSOFFICEWINDOWID)
    {
        List<Ticket> tickets = new List<Ticket>();

        // Создаем подключение к базе данных
        using (NpgsqlConnection connection = new NpgsqlConnection("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ"))
        {
            connection.Open();

            // Создаем команду для вызова функции и получения данных
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.select_ticket_transferred(@in_s_office_window_id)", connection))
            {

                command.Parameters.AddWithValue("in_s_office_window_id", inSOFFICEWINDOWID);
                 
                // Выполняем команду и получаем результаты
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ticket ticket = new Ticket
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

    public static IEnumerable<Ticket> SelectTicketPostponed(long inSOFFICEWINDOWID)
    {
        List<Ticket> tickets = new List<Ticket>();

        
        using (NpgsqlConnection connection = new NpgsqlConnection("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ"))
        {
            connection.Open();

            // Создаем команду для вызова функции и получения данных
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.select_ticket_postponed(@in_s_office_window_id)", connection))
            {
                command.Parameters.AddWithValue("in_s_office_window_id", NpgsqlDbType.Bigint, inSOFFICEWINDOWID); 
                command.Parameters.Add(new NpgsqlParameter("out_d_ticket_id", NpgsqlTypes.NpgsqlDbType.Bigint) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new NpgsqlParameter("out_ticket_number_full", NpgsqlTypes.NpgsqlDbType.Varchar) { Size = -1, Direction = ParameterDirection.Output });

                // Выполняем команду и получаем результаты
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ticket ticket = new Ticket
                        {
                            Id = Convert.ToInt64(command.Parameters["out_d_ticket_id"].Value),
                            TicketNumberFull = command.Parameters["out_ticket_number_full"].Value.ToString()
                        };

                        tickets.Add(ticket);
                    }
                }
            }
        }

        return tickets;
    }
}